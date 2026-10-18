using UnityEngine;
using UnityEngine.UI;

// MobileUIBinder: wires the Rench button to the MobileTouchController.
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
