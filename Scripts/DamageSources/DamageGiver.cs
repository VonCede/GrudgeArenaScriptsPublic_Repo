using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Physical,
    Fire,
    Ice,
    Electric,
    Poison,
    // Add more damage types as needed
}

public struct DamageData
{
    public float damageAmount;
    public DamageType damageType;
    
    public DamageData(float amount, DamageType type)
    {
        damageAmount = amount;
        damageType = type;
    }
}

public class DamageGiver : PlayStateDependant
{
    [SerializeField] private float myDamage = 10f;
    [SerializeField] private DamageType damageType = DamageType.Physical;
    
    
    public DamageData GiveDamage(float runtimeDamageMultiplier = 1f, float additionalDamage = 0f )
    {
        if (isPlaying)
        {
            return new DamageData((myDamage + additionalDamage) * runtimeDamageMultiplier, damageType);
        }
        return new DamageData(0f, damageType);
    }
    
    public void ChangeDamage(DamageData newDamage)
    {
        myDamage = newDamage.damageAmount;
        damageType = newDamage.damageType;
    }

    protected override void DerivedStart()
    {
    }
}
