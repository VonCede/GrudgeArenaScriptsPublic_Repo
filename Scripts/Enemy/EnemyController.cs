using System;
using UnityEngine;

public class EnemyController : PlayStateDependant, IMovementController, IDamageable
{
    [SerializeField] private float moveSpeed = 5f; // Enemy movement speed
    [SerializeField] private int maxHealth = 100; 
    [Header("Enemy damage settings when touching the player")]
    [SerializeField] private float damageInterval = 1f; // Interval at which damage is applied
    [SerializeField] private int damageAmount = 10; // Amount of damage to apply
    [SerializeField] private DamageType damageType = DamageType.Physical; // Damage type
    [SerializeField] private GameObject deathModel; // Death model to instantiate on death
    [SerializeField] private float deathDestroyDelay = 1f; // Delay before destroying the enemy after death
    
    [Header("How many rows of movement in a sprite sheet")]
    [SerializeField] private int spriteSheetRows = 4; //    

    private Transform playerTransform; // Reference to the player's transform
    private float damageTimer = 0f; // Timer for damage interval
    private float currentHealth = 100f;
    private bool isTouchingPlayer = false; // Flag to indicate if the enemy is touching the player's trigger zone
    private Vector3 moveDirection = Vector3.zero; // Movement direction
    private Rigidbody rb; // Reference to the rigidbody component

    protected override void DerivedStart()
    {
        playerTransform = CharacterManager.Instance.playerBase.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (playerTransform == null || !isPlaying)
            return;

        if (isTouchingPlayer)
        {
            if (Time.time > damageTimer)
            {
                ApplyDamageToPlayer();
                damageTimer = Time.time + damageInterval;
            }
        }
        else
        {
            // Calculate the direction to the player
            Vector3 moveDirection = (playerTransform.position - transform.position).normalized;
            transform.LookAt(playerTransform.position);

            // Set the velocity to move at a constant speed
            rb.velocity = moveDirection * moveSpeed;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = true;
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
            isTouchingPlayer = false;   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isPlaying)
        {
            isTouchingPlayer = true;
        }
    }

    // OnTriggerExit is called when the enemy's trigger collider exits another collider's trigger zone.
    private void OnTriggerExit(Collider other)
    {
        isTouchingPlayer = false;
    }

    private void Die()
    {
        if(!isPlaying)
            return;
        playerTransform = null;
        moveDirection = Vector3.zero;
        if(deathModel != null)
            Instantiate(deathModel, transform.position, Quaternion.identity);
        // TODO: Enemy death visuals and sound
        // TODO: Drop loot
        Destroy(gameObject, deathDestroyDelay);
    }
    
    // Function to apply damage to the player
    private void ApplyDamageToPlayer()
    {
        if(!isPlaying)
            return;
        CharacterManager.Instance.TakeDamage(new DamageData(damageAmount, damageType));
        // TODO: Enemy attack visuals and sound
    }

    // IMovementController interface implementation
    public Direction GetMovementDirection()
    {
        return DirectionCalculations.GetDirectionFromVector(moveDirection, spriteSheetRows);
    }

    public bool IsMoving()
    {
        return moveDirection != Vector3.zero;
    }

    public void TakeDamage(DamageData damageData)
    {
        if(!isPlaying)
            return;
        currentHealth -= damageData.damageAmount;
        if(currentHealth <= 0)
        {
            Die();
        }
    }
}