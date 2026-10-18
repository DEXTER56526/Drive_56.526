using UnityEngine;
using UnityEngine.UI;

// MobileUIBinder: wires mobile UI buttons to the MobileTouchController at runtime.
// This script sets up the onClick listener in Awake so the linkage persists when the scene is saved.
public class MobileUIBinder : MonoBehaviour
{
    public MobileTouchController mobile;
    public Button renchButton;

    void Awake()
    {
        if (mobile != null && renchButton != null)
        {
            renchButton.onClick.RemoveAllListeners();
            renchButton.onClick.AddListener(mobile.PressResetDamage);
        }
    }
}
