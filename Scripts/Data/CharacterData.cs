using System;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public string characterName;
    public Sprite characterIcon;
    public GameObject characterPrefab;
    public string characterDescription;
    public int maxHealth;
    public float moveSpeed;
    public float attackSpeedMultiplier;
    public float damageMultiplier;
    public ResistanceSystem resistances;
    public WeaponData startingWeapon;
    public WeaponData[] availableWeapons;

    // Constructor
    public CharacterData()
    {
        resistances = new ResistanceSystem();
    }
}