using UnityEngine;

public class ExplosionDamage : DamageGiver, IPoolable
{
    public float explosionRadius = 5f;
    public LayerMask damageableLayers;
    public GameObject explosionVisuals; // Reference to the explosion visuals object.
    public float explosionDuration = 1f; // How long the explosion should last.
    public float explosionDelay = 0.5f; // How long the explosion takes to fade out.

    private Collider[] colliders;
    private int numColliders;

    protected override void DerivedStart()
    {
        base.DerivedStart();
        // Create an array to store colliders (adjust the size as needed).
        colliders = new Collider[100];
    }

    private void Explode()
    {
        // Perform the overlap check and store the result in the 'colliders' array.
        numColliders = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, colliders, damageableLayers);

        explosionVisuals.SetActive(true);

        // Iterate through the colliders found.
        for (int i = 0; i < numColliders; i++)
        {
            // Check if the collider has a component that implements the IDamageable interface.
            IDamageable damageable = colliders[i].GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(GiveDamage());
            }
        }

        // Optionally, you can also destroy or deactivate this game object after the explosion.
        Destroy(gameObject, explosionDuration);
    }

    public void InitializePoolable()
    {
        gameObject.SetActive(true);
        explosionVisuals.SetActive(false);
        Invoke(nameof(Explode), explosionDelay);
    }
}