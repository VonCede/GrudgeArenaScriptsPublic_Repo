using UnityEngine;

public enum WeaponType
{
    Dagger,
    Sword,
    Axe,
    BattleAxe,
    Staff,
    Spear,
    ThrowingKnife,
    ThrowingAxe,
    Bow,
    Crossbow,
    Wand,
    MagicStaff,
    // Add more weapon types as needed
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "TopDownSurvivor/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite weaponIcon;
    public WeaponType weaponType;
    public GameObject weaponPrefab;
    public GameObject projectilePrefab;
    public float fireRate = 1f;
    public int projectileCount = 1;
    public float delayBetweenProjectiles = 0.1f;
    
    public ProjectileData projectileData;
}