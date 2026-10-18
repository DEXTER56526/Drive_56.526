#nullable enable
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

// DemoSceneBuilder: Editor utility to programmatically build a playable demo scene and prefabs.
// Use: Tools -> Drive56 -> Build Demo Scene
// This avoids committing large binary scene files and ensures the scene is wired correctly in the editor.
public static class DemoSceneBuilder
{
    [MenuItem("Tools/Drive56/Build Demo Scene")]
    public static void BuildDemoScene()
    {
        // Ensure folders exist
        Directory.CreateDirectory("Assets/Scenes");
        Directory.CreateDirectory("Assets/Prefabs");
        Directory.CreateDirectory("Assets/Art");

        // Create empty scene
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Create Main Camera
        var camGO = new GameObject("Main Camera");
        var cam = camGO.AddComponent<Camera>();
        cam.tag = "MainCamera";
        camGO.transform.position = new Vector3(0f, 5f, -10f);
        camGO.name = "Main Camera";

        // Create ground plane
        var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(20f, 1f, 20f);

        // Create InputAdapter and MobileTouchController
        var inputGO = new GameObject("InputAdapter");
        var inputAdapter = inputGO.AddComponent<Assets.Scripts.InputAdapter>();

        var mobileTouchGO = new GameObject("MobileTouch");
        var mobileTouch = mobileTouchGO.AddComponent<Assets.Scripts.MobileTouchController>();
        mobileTouch.IsTouchConnected = false; // default off until mobile UI is active
        mobileTouchGO.transform.parent = inputGO.transform;
        inputAdapter.mobileTouchController = mobileTouch;

        // Create CarRoot
        var carRoot = new GameObject("CarRoot");
        carRoot.transform.position = Vector3.zero;
        var rb = carRoot.AddComponent<Rigidbody>();
        rb.mass = 1200f;
        rb.drag = 0.1f;
        carRoot.AddComponent<Assets.Scripts.DamageController>();
        carRoot.AddComponent<Assets.Scripts.VehicleReset>();
        var wheelController = carRoot.AddComponent<Assets.Scripts.WheelVehicleController>();

        // Create WheelCollider placeholders and visuals
        Vector3[] wheelOffsets = new Vector3[] {
            new Vector3(-0.9f, -0.4f, 1.2f), // front left
            new Vector3(0.9f, -0.4f, 1.2f),  // front right
            new Vector3(-0.9f, -0.4f, -1.2f),// rear left
            new Vector3(0.9f, -0.4f, -1.2f)  // rear right
        };

        Transform[] wheelVisuals = new Transform[4];
        WheelCollider[] colliders = new WheelCollider[4];

        for (int i = 0; i < 4; i++)
        {
            var wcGO = new GameObject(i < 2 ? (i==0?"Wheel_FL_Collider":"Wheel_FR_Collider") : (i==2?"Wheel_RL_Collider":"Wheel_RR_Collider"));
            wcGO.transform.parent = carRoot.transform;
            wcGO.transform.localPosition = wheelOffsets[i];
            var wc = wcGO.AddComponent<WheelCollider>();
            colliders[i] = wc;

            // visual wheel
            var visGO = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            visGO.name = "Wheel_Vis_" + i;
            visGO.transform.parent = carRoot.transform;
            visGO.transform.localScale = new Vector3(0.4f, 0.2f, 0.4f);
            visGO.transform.position = carRoot.transform.TransformPoint(wheelOffsets[i]);
            visGO.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            wheelVisuals[i] = visGO.transform;
        }

        // Assign colliders and visuals to WheelVehicleController
        wheelController.frontLeftCollider = colliders[0];
        wheelController.frontRightCollider = colliders[1];
        wheelController.rearLeftCollider = colliders[2];
        wheelController.rearRightCollider = colliders[3];

        wheelController.frontLeftTransform = wheelVisuals[0];
        wheelController.frontRightTransform = wheelVisuals[1];
        wheelController.rearLeftTransform = wheelVisuals[2];
        wheelController.rearRightTransform = wheelVisuals[3];

        // Create camera points as children of car
        var cam3rd = new GameObject("Cam_3rd"); cam3rd.transform.parent = carRoot.transform;
        cam3rd.transform.localPosition = new Vector3(0f, 3.5f, -6f);
        cam3rd.transform.localRotation = Quaternion.Euler(10f, 0f, 0f);

        var camHood = new GameObject("Cam_Hood"); camHood.transform.parent = carRoot.transform;
        camHood.transform.localPosition = new Vector3(0f, 1.2f, 1.25f);
        camHood.transform.localRotation = Quaternion.Euler(5f, 0f, 0f);

        var camTop = new GameObject("Cam_Top"); camTop.transform.parent = carRoot.transform;
        camTop.transform.localPosition = new Vector3(0f, 12f, 0f);
        camTop.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        // Create CameraManager
        var camManagerGO = new GameObject("CameraManager");
        var camManager = camManagerGO.AddComponent<Assets.Scripts.CameraManager>();
        camManager.mainCamera = cam;
        camManager.followTarget = carRoot.transform;
        camManager.cameraPoints = new Transform[] { cam3rd.transform, camHood.transform, camTop.transform };

        // Create GasStation (trigger)
        var gasGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gasGO.name = "GasStation";
        gasGO.transform.position = new Vector3(20f, 0.5f, 0f);
        gasGO.transform.localScale = new Vector3(8f, 1f, 8f);
        var gasCol = gasGO.GetComponent<BoxCollider>();
        gasCol.isTrigger = true;
        gasGO.AddComponent<Assets.Scripts.GasStation>();

        // Create Canvas and HUD
        var canvasGO = new GameObject("Canvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Speed Text
        var speedTextGO = new GameObject("SpeedText");
        speedTextGO.transform.parent = canvasGO.transform;
        var speedText = speedTextGO.AddComponent<Text>();
        speedText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        speedText.alignment = TextAnchor.UpperRight;
        speedText.fontSize = 24;
        speedText.rectTransform.anchorMin = new Vector2(1f, 1f);
        speedText.rectTransform.anchorMax = new Vector2(1f, 1f);
        speedText.rectTransform.pivot = new Vector2(1f, 1f);
        speedText.rectTransform.anchoredPosition = new Vector2(-10f, -10f);
        speedText.rectTransform.sizeDelta = new Vector2(300f, 50f);

        // Minimap (RenderTexture)
        var minimapCamGO = new GameObject("MinimapCamera");
        var minimapCam = minimapCamGO.AddComponent<Camera>();
        minimapCam.orthographic = true;
        minimapCam.orthographicSize = 50f;
        minimapCamGO.transform.position = carRoot.transform.position + Vector3.up * 80f;
        minimapCamGO.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        var rt = new RenderTexture(256, 256, 16);
        AssetDatabase.CreateAsset(rt, "Assets/Art/Minimap.renderTexture");
        minimapCam.targetTexture = rt;

        var minimapGO = new GameObject("Minimap");
        minimapGO.transform.parent = canvasGO.transform;
        var raw = minimapGO.AddComponent<RawImage>();
        raw.texture = rt;
        raw.rectTransform.anchorMin = new Vector2(0f, 1f);
        raw.rectTransform.anchorMax = new Vector2(0f, 1f);
        raw.rectTransform.pivot = new Vector2(0f, 1f);
        raw.rectTransform.anchoredPosition = new Vector2(10f, -10f);
        raw.rectTransform.sizeDelta = new Vector2(150f, 150f);

        // HUDManager
        var hudGO = new GameObject("HUDManager");
        var hud = hudGO.AddComponent<Assets.Scripts.HUDManager>();
        hud.playerVehicle = wheelController;
        hud.speedText = speedText;
        hud.minimapCamera = minimapCam;

        // Mobile UI: create a simple Rench button (repair) and placeholders for joystick/throttle
        var mobileUIRoot = new GameObject("MobileUI");
        mobileUIRoot.transform.parent = canvasGO.transform;

        var renchGO = new GameObject("RenchButton");
        renchGO.transform.parent = mobileUIRoot.transform;
        var renchBtn = renchGO.AddComponent<Button>();
        var renchImage = renchGO.AddComponent<Image>();
        renchImage.color = new Color(0.9f, 0.6f, 0.2f, 0.9f);
        var renchTextGO = new GameObject("Text");
        renchTextGO.transform.parent = renchGO.transform;
        var renchText = renchTextGO.AddComponent<Text>();
        renchText.text = "Rench";
        renchText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        renchText.alignment = TextAnchor.MiddleCenter;
        renchText.color = Color.black;
        renchText.rectTransform.sizeDelta = new Vector2(160f, 60f);

        // Position rench button bottom-right
        renchBtn.targetGraphic = renchImage;
        renchGO.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 0f);
        renchGO.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0f);
        renchGO.GetComponent<RectTransform>().pivot = new Vector2(1f, 0f);
        renchGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(-80f, 80f);
        renchGO.GetComponent<RectTransform>().sizeDelta = new Vector2(160f, 60f);

        // Add MobileUIBinder to wire button to MobileTouchController
        var binder = mobileUIRoot.AddComponent<Assets.Scripts.MobileUIBinder>();
        binder.mobile = mobileTouch;
        binder.renchButton = renchBtn;

        // Make sure MobileTouchController is active for mobile testing
        mobileTouch.IsTouchConnected = true;

        // Place the car in the scene and make it the active player
        carRoot.transform.position = new Vector3(0f, 1f, 0f);

        // Save scene
        var scenePath = "Assets/Scenes/Demo.unity";
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), scenePath);
        AssetDatabase.Refresh();

        Debug.Log("Demo scene built and saved to " + scenePath + ". Open it in Unity and press Play to test the prototype.");
    }
}
#endif
