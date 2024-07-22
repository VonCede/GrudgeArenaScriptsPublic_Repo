using UnityEngine;

public class SpriteAnimationController : MonoBehaviour
{
    [SerializeField] private Texture2D spriteSheet;    // Reference to the sprite sheet.
    [SerializeField] private int rowCount = 4;         // Number of rows in the sprite sheet (for each direction).
    [SerializeField] private int colCount = 4;         // Number of columns in the sprite sheet (frames per direction).
    [SerializeField] private int frameRate = 12;       // Frames per second for the animation.

    private IMovementController movementController;
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer.
    private int currentFrame;
    private int directionFrameOffset;
    private float frameTimer;
    private int totalFrames;
    private int frameWidth;
    private int frameHeight;
    private bool isMoving = false;

    private Direction lastKnownDirection = Direction.Down; // Default direction for idle state.

    private void Start()
    {
        // Find the MovementController in parent GameObject
        movementController = transform.parent.gameObject.GetComponent<IMovementController>();
        // if not found, try to one step up
        if (movementController == null)
            movementController = transform.parent.parent.gameObject.GetComponent<IMovementController>();
        // Still not found, give an error
        if (movementController == null)
            Debug.LogError(gameObject.name + " couldn't find MovementController in parent or grandparent GameObject.");

        if (spriteSheet == null)
            Debug.LogError(gameObject.name + " has no SpriteSheet not assigned in the inspector.");

        
        if (rowCount < 1)
            Debug.LogError(gameObject.name + " has an invalid row count of " + rowCount + ". Row count must be at least 1.");
        if(colCount < 1)
            Debug.LogError(gameObject.name + " has an invalid column count of " + colCount + ". Column count must be at least 1.");

        // Get the SpriteRenderer component on this GameObject.
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        totalFrames = rowCount * colCount;
        frameWidth = spriteSheet.width / colCount;
        frameHeight = spriteSheet.height / rowCount;
        directionFrameOffset = (int)lastKnownDirection;

        currentFrame = 0;
        frameTimer = 1f / frameRate;

        // Set the initial sprite for idle (first frame of the row for the last known direction)
        SetIdleSprite();
    }

    private void Update()
    {
        if (movementController != null && spriteRenderer != null)
        {
            // Get the character's movement direction and state
            Direction direction = movementController.GetMovementDirection();
            isMoving = movementController.IsMoving();

            // Update character's animations and sprite based on direction and movement state
            UpdateCharacterAnimations(direction, isMoving);
        }
    }

    private void UpdateCharacterAnimations(Direction direction, bool isMoving)
    {
        // If the direction has changed, update the frame offset
        if (direction != lastKnownDirection)
        {
            lastKnownDirection = direction;
            directionFrameOffset = (int)direction;
            currentFrame = 0;
        }
        if (isMoving)
        {
            // Calculate the frame index based on the direction and current frame
            int spriteIndex = directionFrameOffset * colCount + currentFrame;

            // Calculate the position of the frame on the sprite sheet
            int row = Mathf.Clamp(spriteIndex / colCount, 0, rowCount - 1);
            int col = spriteIndex % colCount;
            Rect spriteRect = new Rect(col * frameWidth, (rowCount - 1 - row) * frameHeight, frameWidth, frameHeight);

            // Create a sprite using the calculated rect
            Sprite sprite = Sprite.Create(spriteSheet, spriteRect, Vector2.one * 0.5f);
            spriteRenderer.sprite = sprite;
        }
        else
        {
            // Set the idle sprite based on the last known direction
            SetIdleSprite();
        }
    }

    private void SetIdleSprite()
    {
        // Calculate the frame index based on the direction and current frame for idle animation
        int idleFrame = (int)Direction.Idle * colCount + currentFrame;

        // Calculate the position of the frame on the sprite sheet for idle animation
        int row = Mathf.Clamp(idleFrame / colCount, 0, rowCount - 1);
        int col = idleFrame % colCount;
        Rect spriteRect = new Rect(col * frameWidth, (rowCount - 1 - row) * frameHeight, frameWidth, frameHeight);

        // Create a sprite using the calculated rect for idle animation
        Sprite sprite = Sprite.Create(spriteSheet, spriteRect, Vector2.one * 0.5f);
        spriteRenderer.sprite = sprite;
    }

    // Update is called once per frame.
    private void LateUpdate()
    {
        if (isMoving)
        {
            // Advance to the next frame based on the frame rate
            frameTimer -= Time.deltaTime;
            if (frameTimer <= 0f)
            {
                currentFrame = (currentFrame + 1) % colCount;
                frameTimer = 1f / frameRate;
            }
        }
    }
}
