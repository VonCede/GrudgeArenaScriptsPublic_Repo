using System.Collections;
using UnityEditor;
using UnityEngine;

public class Weapon : PlayStateDependant
{
    [SerializeField] private Transform[] firePoints; // Reference to the position where projectiles are fired from.
    [SerializeField] private WeaponAimType aimType; // Type of aiming to use.
    //[SerializeField] private ProjectileData projectileData; // Reference to the projectile data to be used.
    
    private GameObject projectilePrefab; // Reference to the projectile prefab to be fired.
    private float currentFireRate = 1f; // Fire rate in shots per second.
    private int currentProjectileCount = 1; // Number of projectiles fired at once.
    private float currentDelayBetweenProjectiles = 0.1f; // Delay between firing multiple projectiles.
    private float currentDamageMultiplier = 1f; // Damage multiplier for the current shot.

    private float nextFireTime; // Time of the next allowed shot.
    private WeaponData runtimeWeaponData; // Reference to the projectile data to be used.
    
    private ObjectPool<Projectile> projectilePool; // Reference to the projectile pool.
    
    private enum WeaponAimType
    {
        Static,
        Facing,
        ClosestTarget,
        RandomRotation,
    }
    
    private void Awake()
    {
        // Check if the fire points are assigned.
        if (firePoints == null || firePoints.Length == 0)
        {
            Debug.LogError("No Fire Points are assigned in the Weapon component in weapon prefab " + gameObject.name +".");
            enabled = false; // Disable the script to prevent errors.
            return;
        }

        // Initialize the next fire time.
        nextFireTime = Time.time;
    }
    
    protected override void DerivedStart()
    {
        //runtimeProjectileData = Helpers.DeepCopy(projectileData);
    }


    // Initialize the weapon with the data from WeaponData.
    public void Init(WeaponData weaponData)
    {
        projectilePrefab = weaponData.projectilePrefab;
        currentFireRate = weaponData.fireRate;
        currentProjectileCount = weaponData.projectileCount; 
        currentDelayBetweenProjectiles = weaponData.delayBetweenProjectiles;
        
        runtimeWeaponData = Helpers.DeepCopy(weaponData);

        if (aimType == WeaponAimType.Facing)
        {
            transform.parent = CharacterManager.Instance.playerBase.transform;
            transform.rotation = CharacterManager.Instance.playerBase.transform.rotation;
        }
        else if(aimType == WeaponAimType.RandomRotation || aimType == WeaponAimType.ClosestTarget || aimType == WeaponAimType.Static)
            transform.parent = CharacterManager.Instance.playerBase.transform;
        
        // Create a new object pool for projectiles.
        projectilePool = new ObjectPool<Projectile>(projectilePrefab.GetComponent<Projectile>(), 20);                

        // Initialize the next fire time.
        nextFireTime = Time.time;
    }
    
    public void IncreaseProjectileData(ProjectileData data)
    {
        /*runtimeProjectileData.damage += data.damage;
        runtimeProjectileData.damageType = data.damageType;
        runtimeProjectileData.speed += data.speed;
        runtimeProjectileData.lifeTime += data.lifeTime;
        runtimeProjectileData.hitCount += data.hitCount;
        runtimeProjectileData.bounceOnHit = data.bounceOnHit;
        runtimeProjectileData.collisionLayer |= data.collisionLayer;
        runtimeProjectileData.damageLayer |= data.damageLayer;*/
    }

    private void Update()
    {
        if (!isPlaying)
            return;
        // Check if it's time to fire another shot.
        if (Time.time >= nextFireTime)
        {
            // Fire projectiles.
            StartCoroutine(Shoot());
            // Calculate the time of the next allowed shot based on the fire rate.
            nextFireTime = Time.time + currentFireRate;
        }
    }

    private IEnumerator Shoot()
    {
        int c = 0;
        // Loop through the number of projectiles to be fired.
        for (int i = 0; i < currentProjectileCount; i++)
        {
            // Instantiate a new projectile at the fire point position and rotation.
            Projectile newProjectile = projectilePool.GetObjectFromPool();

            if (aimType == WeaponAimType.RandomRotation)
                transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            else if (aimType == WeaponAimType.ClosestTarget)
            {
                //TODO: Try to make efficient way to find nearest enemy
            }
            else if (aimType == WeaponAimType.Static)
            {
                // make weapon face global forward
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            // Use the Init method to set the initial position and direction and pass the projectile data.
            newProjectile.Init(firePoints[c].position, firePoints[c].rotation, currentDamageMultiplier, runtimeWeaponData.projectileData);

            // Increment the current fire point index and wrap around if necessary.
            c = (c + 1) % firePoints.Length;

            // Wait for the delay between projectiles if we have more projectiles to fire.
            if (i < currentProjectileCount - 1 && currentDelayBetweenProjectiles > 0f)
                yield return new WaitForSeconds(currentDelayBetweenProjectiles);
            if (!isPlaying)
                yield break;
        }
    }
    
    private void OnDrawGizmos()
    {
        bool isSelected = false;
        
        foreach (Transform firePoint in firePoints)
        {
            if (Selection.Contains(firePoint.gameObject))
            {
                isSelected = true;
                break;
            }
        }
        if (!isSelected)
            isSelected = Selection.Contains(gameObject);
        if (!isSelected)
            return;

        if (firePoints != null)
        {
            foreach (Transform firePoint in firePoints)
            {
                if(firePoint == null)
                    continue;
                // Draw an arrow from the fire point to Vector3.forward.
                Gizmos.color = Color.green;
                Gizmos.DrawRay(firePoint.position, firePoint.forward * 3f);

                // Draw a small arrowhead at the end of the ray.
                float arrowSize = 0.1f;
                Vector3 arrowEnd = firePoint.position + firePoint.forward * 3f;
                Vector3 arrowRight = Quaternion.LookRotation(firePoint.forward) * Quaternion.Euler(0, 180 + 30, 0) * Vector3.forward * arrowSize;
                Vector3 arrowLeft = Quaternion.LookRotation(firePoint.forward) * Quaternion.Euler(0, 180 - 30, 0) * Vector3.forward * arrowSize;

                Gizmos.DrawRay(arrowEnd, arrowRight);
                Gizmos.DrawRay(arrowEnd, arrowLeft);
            }
        }
    }
}
