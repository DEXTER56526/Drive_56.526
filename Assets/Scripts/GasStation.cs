using UnityEngine;

// GasStation trigger that resets vehicle damage when a vehicle enters.
// Place a collider with isTrigger=true on the gas station area.
public class GasStation : MonoBehaviour
{
    [Tooltip("Reset the damage of any DamageController on incoming vehicles.")]
    public bool resetDamageOnEnter = true;

    void OnTriggerEnter(Collider other)
    {
        DamageController dc = other.GetComponentInParent<DamageController>();
        if (dc != null && resetDamageOnEnter)
        {
            dc.ResetDamage();
        }
    }
}
