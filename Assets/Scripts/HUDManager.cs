using UnityEngine;
using UnityEngine.UI;

// HUD manager: updates speed text/needle and provides hooks for a minimap camera.
public class HUDManager : MonoBehaviour
{
    public WheelVehicleController playerVehicle; // can also accept other vehicle controller types
    public Text speedText; // e.g. "000 km/h"
    [Header("Needle")]
    public RectTransform speedNeedle; // optional: rotate based on speed
    public float maxNeedleAngle = -220f; // angle at top speed

    [Header("Minimap")]
    public Camera minimapCamera; // assign a top-down camera that renders to a RenderTexture

    void Update()
    {
        if (playerVehicle != null && speedText != null)
        {
            float kph = playerVehicle.GetSpeedKPH();
            speedText.text = Mathf.RoundToInt(kph) + " km/h";

            if (speedNeedle != null)
            {
                float t = Mathf.Clamp01(kph / playerVehicle.topSpeedKPH);
                float angle = Mathf.Lerp(0f, maxNeedleAngle, t);
                speedNeedle.localEulerAngles = new Vector3(0f, 0f, angle);
            }
        }

        // minimap camera should already be positioned above the player and set to orthographic for best results.
    }
}
