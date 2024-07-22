using System;
using UnityEngine;

[Serializable]
public class ProjectileData 
{
    public float damage = 10f;
    public DamageType damageType = DamageType.Physical;
    public float speed = 10f;
    public float lifeTime = 5f;
    public bool destroyOnHit = true;
    public int hitCount = 1;
    public bool bounceOnHit;
    public LayerMask collisionLayer; // Layer mask for collision detection
    public LayerMask damageLayer;   // Layer mask for damage application
}