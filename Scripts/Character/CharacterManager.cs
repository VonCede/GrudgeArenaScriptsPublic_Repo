using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to load selected character and set up player stats for the run
// and to keep track of player stats during the run

public class CharacterManager : PlayStateDependant, IDamageable
{
    
    [SerializeField] private GameObject playerBasePrefab;
    
    public int playerMaxHealth { get; private set; }
    
    public ResistanceSystem resistanceSystem { get; private set; }
    public float playerCurrentHealth{ get; private set; }
    
    public ResistanceSystem playerCurrentResistances { get; private set; }
    
    public float playerMoveSpeed{ get; private set; }
    public float playerAttackSpeedMultiplier { get; private set; }
    public float playerDamageMultiplier { get; private set; }
    public GameObject playerBase { get; private set; }
    
    private GameObject startingWeapon;
    
    
    public static CharacterManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of CharacterManager in " + Instance.gameObject.name + " and in " + gameObject.name + "!");
            return;
        }
        Instance = this;
        // Spawn player base
        playerBase = Instantiate(playerBasePrefab, Vector3.zero, Quaternion.identity);
    }

    protected override void DerivedStart()
    {
        // Load selected character data
        if (GameState.Instance.selectedCharacter != null)
        {
            playerMaxHealth = GameState.Instance.selectedCharacter.maxHealth;
            playerCurrentHealth = GameState.Instance.selectedCharacter.maxHealth;
            playerCurrentResistances = new ResistanceSystem();
            // Make deep copy of resistances
            foreach (var resistanceData in GameState.Instance.selectedCharacter.resistances.resistances)
            {
                var newResistanceData = new ResistanceData
                {
                    damageType = resistanceData.damageType,
                    resistanceValue = resistanceData.resistanceValue
                };
    
                playerCurrentResistances.resistances.Add(newResistanceData);
            }
            playerCurrentResistances.resistances.Sort(new ResistanceDataComparer());            
            playerMoveSpeed = GameState.Instance.selectedCharacter.moveSpeed;
            playerAttackSpeedMultiplier = GameState.Instance.selectedCharacter.attackSpeedMultiplier;
            playerDamageMultiplier = GameState.Instance.selectedCharacter.damageMultiplier;
            startingWeapon = Instantiate(GameState.Instance.selectedCharacter.startingWeapon.weaponPrefab, playerBase.transform.position, playerBase.transform.rotation);
            startingWeapon.transform.parent = playerBase.transform;
            startingWeapon.GetComponent<Weapon>().Init(GameState.Instance.selectedCharacter.startingWeapon);
            
            // Load player visuals
            GameObject playerVisuals = Instantiate(GameState.Instance.selectedCharacter.characterPrefab, playerBase.transform.position, playerBase.transform.rotation);
            playerVisuals.transform.parent = playerBase.transform;
        }
        else
        {
            Debug.LogError("No character selected!");
        }
    }

    public void TakeDamage(DamageData damageData)
    {
        if(!isPlaying)
            return;
        if(playerCurrentHealth <= 0)
            return;
        // TODO: Fix this
        playerCurrentHealth -= damageData.damageAmount; // * (1f - playerCurrentResistances.GetResistance(damageData.damageType));
        if (playerCurrentHealth <= 0)
        {
            OnPlayerDeath();
        }
    }
    
    public void OnPlayerDeath()
    {
        PlayState.Instance.OnGameOver();
        playerBase = null;
        // TODO: Player death visuals and sound
    }

    public void SelectRandomWeapon()
    {
        // TODO: Select random weapon from available weapons from character data, and instantiate it, avoid duplicates on WeaponType
        
    }
}
