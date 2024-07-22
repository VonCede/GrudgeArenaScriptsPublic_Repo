using UnityEngine;

public class AOEDamageGiver : DamageGiver
{
    public float damageInterval = 1f;
    public LayerMask damageableLayers; // Specify which layers can be damaged by the AOE.

    private float damageTimer = 0f;

    private void OnTriggerStay(Collider other)
    {
        // Check if the collider's layer is included in the damageableLayers.
        if ((damageableLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            // Increment the damage timer.
            damageTimer += Time.deltaTime;

            // Check if it's time to apply damage again.
            if (damageTimer >= damageInterval)
            {
                // Apply damage to the object.
                IDamageable damageable = other.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(GiveDamage());
                }

                // Reset the damage timer.
                damageTimer = 0f;
            }
        }
    }
}