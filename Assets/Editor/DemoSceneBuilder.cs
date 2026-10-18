#nullable enable
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public static class DemoSceneBuilder
{
    [MenuItem("Tools/Drive56/Build Demo Scene")]
    public static void BuildDemoScene()
    {
        // Ensure folders exist
        Directory.CreateDirectory("Assets/Scenes");
        Directory.CreateDirectory("Assets/Prefabs");
        Directory.CreateDirectory("Assets/Art");
        Directory.CreateDirectory("Assets/Editor");

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Main Camera
        var camGO = new GameObject("Main Camera");
        var cam = camGO.AddComponent<Camera>();
        cam.tag = "MainCamera";
        camGO.transform.position = new Vector3(0f, 5f, -10f);

        // Ground
        var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(20f, 1f, 20f);

        // InputAdapter + MobileTouchController
        var inputGO = new GameObject("InputAdapter");
        var inputAdapter = inputGO.AddComponent<Assets.Scripts.InputAdapter>();

        var touchGO = new GameObject("MobileTouch");
        touchGO.transform.parent = inputGO.transform;
        var touch = touchGO.AddComponent<Assets.Scripts.MobileTouchController>();
        touch.IsTouchConnected = false;
        inputAdapter.mobileTouchController = touch;

        // CarRoot
        var carRoot = new GameObject("CarRoot");
        carRoot.transform.position = new Vector3(0f, 1f, 0f);
        var rb = carRoot.AddComponent<Rigidbody>();
        rb.mass = 1200f;
        rb.drag = 0.1f;

        carRoot.AddComponent<Assets.Scripts.DamageController>();
        carRoot.AddComponent<Assets.Scripts.VehicleReset>();
        var wheelController = carRoot.AddComponent<Assets.Scripts.WheelVehicleController>();

        // wheel positions and visuals
        Vector3[] offsets = new Vector3[] {
            new Vector3(-0.9f, -0.4f, 1.2f),
            new Vector3(0.9f, -0.4f, 1.2f),
            new Vector3(-0.9f, -0.4f, -1.2f),
            new Vector3(0.9f, -0.4f, -1.2f)
        };

        Transform[] wheelVisuals = new Transform[4];
        WheelCollider[] colliders = new WheelCollider[4];

        for (int i = 0; i < 4; i++)
        {
            var wcGO = new GameObject(i < 2 ? (i == 0 ? "Wheel_FL_Collider" : "Wheel_FR_Collider") : (i == 2 ? "Wheel_RL_Collider" : "Wheel_RR_Collider"));
            wcGO.transform.parent = carRoot.transform;
            wcGO.transform.localPosition = offsets[i];
            var wc = wcGO.AddComponent<WheelCollider>();
            colliders[i] = wc;

            var visGO = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            visGO.transform.parent = carRoot.transform;
            visGO.transform.localScale = new Vector3(0.4f, 0.2f, 0.4f);
            visGO.transform.position = carRoot.transform.TransformPoint(offsets[i]);
            visGO.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            wheelVisuals[i] = visGO.transform;
        }

        wheelController.frontLeftCollider = colliders[0];
        wheelController.frontRightCollider = colliders[1];
        wheelController.rearLeftCollider = colliders[2];
        wheelController.rearRightCollider = colliders[3];

        wheelController.frontLeftTransform = wheelVisuals[0];
        wheelController.frontRightTransform = wheelVisuals[1];
        wheelController.rearLeftTransform = wheelVisuals[2];
        wheelController.rearRightTransform = wheelVisuals[3];

        // Camera points on the car
        var cam3rd = new GameObject("Cam_3rd"); cam3rd.transform.parent = carRoot.transform; cam3rd.transform.localPosition = new Vector3(0f, 3.5f, -6f); cam3rd.transform.localRotation = Quaternion.Euler(10f, 0f, 0f);
        var camHood = new GameObject("Cam_Hood"); camHood.transform.parent = carRoot.transform; camHood.transform.localPosition = new Vector3(0f, 1.2f, 1.25f); camHood.transform.localRotation = Quaternion.Euler(5f, 0f, 0f);
        var camTop = new GameObject("Cam_Top"); camTop.transform.parent = carRoot.transform; camTop.transform.localPosition = new Vector3(0f, 12f, 0f); camTop.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        var camManagerGO = new GameObject("CameraManager");
        var camManager = camManagerGO.AddComponent<Assets.Scripts.CameraManager>();
        camManager.mainCamera = cam;
        camManager.followTarget = carRoot.transform;
        camManager.cameraPoints = new Transform[] { cam3rd.transform, camHood.transform, camTop.transform };

        // Gas station trigger + light visual
        var gasGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gasGO.name = "GasStation";
        gasGO.transform.position = new Vector3(20f, 0.5f, 0f);
        gasGO.transform.localScale = new Vector3(8f, 1f, 8f);
        var box = gasGO.GetComponent<BoxCollider>();
        box.isTrigger = true;
        gasGO.AddComponent<Assets.Scripts.GasStation>();

        var sign = GameObject.CreatePrimitive(PrimitiveType.Cube);
        sign.name = "GasStationSign";
        sign.transform.parent = gasGO.transform;
        sign.transform.localScale = new Vector3(0.3f, 2f, 0.3f);
        sign.transform.localPosition = new Vector3(0f, 1.5f, 0f);

        // Canvas + HUD
        var canvasGO = new GameObject("Canvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        var speedTextGO = new GameObject("SpeedText");
        speedTextGO.transform.SetParent(canvasGO.transform, false);
        var speedText = speedTextGO.AddComponent<Text>();
        speedText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        speedText.fontSize = 26;
        speedText.alignment = TextAnchor.UpperRight;
        speedText.color = Color.white;
        speedText.text = "0 km/h";
        var speedRect = speedText.GetComponent<RectTransform>();
        speedRect.anchorMin = new Vector2(1f, 1f);
        speedRect.anchorMax = new Vector2(1f, 1f);
        speedRect.pivot = new Vector2(1f, 1f);
        speedRect.anchoredPosition = new Vector2(-20f, -20f);
        speedRect.sizeDelta = new Vector2(180f, 40f);

        var minimapGO = new GameObject("Minimap");
        minimapGO.transform.SetParent(canvasGO.transform, false);
        var raw = minimapGO.AddComponent<RawImage>();
        raw.rectTransform.anchorMin = new Vector2(0f, 1f);
        raw.rectTransform.anchorMax = new Vector2(0f, 1f);
        raw.rectTransform.pivot = new Vector2(0f, 1f);
        raw.rectTransform.anchoredPosition = new Vector2(20f, -20f);
        raw.rectTransform.sizeDelta = new Vector2(150f, 150f);

        // create minimap camera
        var minimapCamGO = new GameObject("MinimapCamera");
        var minimapCam = minimapCamGO.AddComponent<Camera>();
        minimapCam.orthographic = true;
        minimapCam.orthographicSize = 40f;
        minimapCamGO.transform.position = carRoot.transform.position + new Vector3(0f, 50f, 0f);
        minimapCamGO.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        var rt = new RenderTexture(256, 256, 16);
        rt.name = "MinimapRT";
        minimapCam.targetTexture = rt;
        raw.texture = rt;

        var hudGO = new GameObject("HUDManager");
        var hud = hudGO.AddComponent<Assets.Scripts.HUDManager>();
        hud.playerVehicle = wheelController;
        hud.speedText = speedText;
        hud.minimapCamera = minimapCam;

        // MobilePanel + UI controls
        var mobilePanelGO = new GameObject("MobilePanel");
        mobilePanelGO.transform.SetParent(canvasGO.transform, false);
        var mobilePanel = mobilePanelGO.AddComponent<RectTransform>();
        mobilePanel.anchorMin = Vector2.zero;
        mobilePanel.anchorMax = Vector2.one;
        mobilePanel.offsetMin = Vector2.zero;
        mobilePanel.offsetMax = Vector2.zero;

        // Joystick base + handle
        var joystickBaseGO = new GameObject("SteeringJoystick");
        var joystickBase = joystickBaseGO.AddComponent<RectTransform>();
        joystickBase.SetParent(mobilePanelGO.transform, false);
        joystickBase.anchorMin = new Vector2(0f, 0f);
        joystickBase.anchorMax = new Vector2(0f, 0f);
        joystickBase.pivot = new Vector2(0.5f, 0.5f);
        joystickBase.sizeDelta = new Vector2(140f, 140f);
        joystickBase.anchoredPosition = new Vector2(110f, 110f);

        var joystickBg = joystickBaseGO.AddComponent<Image>();
        joystickBg.color = new Color(1f, 1f, 1f, 0.35f);

        var handleGO = new GameObject("Handle");
        var handleRect = handleGO.AddComponent<RectTransform>();
        handleRect.SetParent(joystickBase, false);
        handleRect.sizeDelta = new Vector2(55f, 55f);
        handleRect.anchoredPosition = Vector2.zero;

        var handleImg = handleGO.AddComponent<Image>();
        handleImg.color = new Color(1f, 1f, 1f, 0.8f);

        var joystick = joystickBaseGO.AddComponent<Assets.Scripts.SimpleJoystick>();
        joystick.mobile = touch;
        joystick.handle = handleRect;
        joystick.centerArea = joystickBase;
        joystick.radius = 45f;

        // Throttle area
        var throttleGO = new GameObject("ThrottleArea");
        throttleGO.transform.SetParent(canvasGO.transform, false);
        var throttleRect = throttleGO.AddComponent<RectTransform>();
        throttleRect.anchorMin = new Vector2(1f, 0f);
        throttleRect.anchorMax = new Vector2(1f, 0f);
        throttleRect.pivot = new Vector2(1f, 0f);
        throttleRect.sizeDelta = new Vector2(120f, 180f);
        throttleRect.anchoredPosition = new Vector2(-150f, 120f);
        var throttleImg = throttleGO.AddComponent<Image>();
        throttleImg.color = new Color(0.2f, 1f, 0.2f, 0.35f);

        var throttleArea = throttleGO.AddComponent<Assets.Scripts.ThrottleArea>();
        throttleArea.mobile = touch;
        throttleArea.area = throttleRect;

        // Rench button
        var renchGO = new GameObject("RenchButton");
        renchGO.transform.SetParent(canvasGO.transform, false);
        var renchRect = renchGO.AddComponent<RectTransform>();
        renchRect.anchorMin = new Vector2(1f, 0f);
        renchRect.anchorMax = new Vector2(1f, 0f);
        renchRect.pivot = new Vector2(1f, 0f);
        renchRect.sizeDelta = new Vector2(160f, 60f);
        renchRect.anchoredPosition = new Vector2(-20f, 50f);
        var renchImage = renchGO.AddComponent<Image>();
        renchImage.color = new Color(1f, 0.7f, 0.2f, 0.9f);

        var renchBtn = renchGO.AddComponent<Button>();
        renchBtn.targetGraphic = renchImage;

        var renchTextGO = new GameObject("Text");
        renchTextGO.transform.SetParent(renchGO.transform, false);
        var renchText = renchTextGO.AddComponent<Text>();
        renchText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        renchText.text = "Rench";
        renchText.fontSize = 24;
        renchText.alignment = TextAnchor.MiddleCenter;
        renchText.color = Color.black;
        renchText.resizeTextForBestFit = true;
        var renchTextRect = renchText.GetComponent<RectTransform>();
        renchTextRect.anchorMin = Vector2.zero; renchTextRect.anchorMax = Vector2.one; renchTextRect.offsetMin = Vector2.zero; renchTextRect.offsetMax = Vector2.zero;

        var uiBinder = canvasGO.AddComponent<Assets.Scripts.MobileUIBinder>();
        uiBinder.mobile = touch;
        uiBinder.renchButton = renchBtn;

        touch.IsTouchConnected = true;

        // Save scene
        string scenePath = "Assets/Scenes/Demo.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Drive56 demo scene built at " + scenePath + ".");
    }
}
#endif
