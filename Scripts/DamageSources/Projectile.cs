using UnityEngine;

public class Projectile : DamageGiver, IPoolable
{
    private float damageMultiplier = 1f;
    private Rigidbody rb;
    private int shotHitCount = 0;

    private ProjectileData projectileData; // Reference to the projectile data.

    float originalVelocityMagnitude;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(Vector3 position, Quaternion rotation, float paramDamageMultiplier, ProjectileData data)
    {
        transform.position = position;
        transform.rotation = rotation;
        damageMultiplier = paramDamageMultiplier;
        rb.velocity = transform.forward * data.speed;
        shotHitCount = data.hitCount;
        originalVelocityMagnitude = data.speed;
        ChangeDamage(new DamageData(data.damage, data.damageType));

        // Schedule the projectile to be destroyed after a certain lifetime
        Invoke(nameof(destroy), data.lifeTime);

        projectileData = data; // Set the projectile data.
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a layer in either the collisionLayer or damageLayer mask
        bool isCollisionLayer = ((1 << collision.gameObject.layer) & projectileData.collisionLayer) != 0;
        bool isDamageLayer = ((1 << collision.gameObject.layer) & projectileData.damageLayer) != 0;

        
        if (isCollisionLayer || isDamageLayer)
        {
            HandleCollisionOrTrigger(collision.collider, isCollisionLayer, collision.contacts[0].normal);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the trigger is with a layer in the damageLayer mask
        if (((1 << other.gameObject.layer) & projectileData.damageLayer) != 0)
        {
            HandleCollisionOrTrigger(other);
        }
    }
    private void HandleCollisionOrTrigger(Collider other, bool isCollision = false, Vector3 normal = default)
    {
        bool isDamageLayer = ((1 << other.gameObject.layer) & projectileData.damageLayer) != 0;

        shotHitCount--;

        // Check if there are remaining hits, and if so, bounce the projectile
        if (shotHitCount > 1 && projectileData.bounceOnHit && isCollision)
        {
            Vector3 reflectionDirection = Vector3.Reflect(transform.forward,normal);
            //Rotate bullet to new direction
            transform.rotation = Quaternion.LookRotation(reflectionDirection);
            // Set the velocity to the reflection direction with the same magnitude.
            rb.velocity = reflectionDirection * originalVelocityMagnitude;
        }
        else
        {
            // No more hits remaining, destroy the projectile if destroyOnHit is true.
            if (projectileData.destroyOnHit)
            {
                destroy();
            }
        }

        // Handle damage to objects in the damageLayer here
        if (isDamageLayer)
        {
            // Attempt to get a reference to the IDamageable component on the collided object.
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

            // If the object has an IDamageable component, apply damage.
            if (damageable != null)
            {
                damageable.TakeDamage(GiveDamage(damageMultiplier, projectileData.damage));
            }
        }
    }


    public void InitializePoolable()
    {
        gameObject.SetActive(true);
    }
    
    private void destroy()
    {
        gameObject.SetActive(false);
    }
}
