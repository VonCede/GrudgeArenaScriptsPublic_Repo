using UnityEngine;

public class MouseFollowCharacterController : PlayStateDependant, IMovementController
{
    enum MouseButtons
    {
        MouseLeft = 0,
        MouseRight = 1,
        MouseMiddle = 2
    }

    [SerializeField] private MouseButtons mouseButton = MouseButtons.MouseLeft;
    [SerializeField] private float defaultMoveSpeed = 5f;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private int amountOfDirections = 4;

    private bool isMoving = false;
    private Vector3 targetPosition;
    private CharacterController controller;
    private float horizontalInput;
    private float verticalInput;
    private Direction lastDirection = Direction.Idle;
    private float moveSpeed = 5f;
    private float targetRotationY;
    private Vector3 lookDirection;

    protected override void DerivedStart()
    {
        moveSpeed = defaultMoveSpeed;
        
        targetPosition = transform.position;
        controller = GetComponent<CharacterController>();
        if (!mainCamera)
            mainCamera = Camera.main;
        if (!mainCamera)
            Debug.LogError("No main camera found!");
        if (!controller)
            Debug.LogError("No CharacterController found in" + gameObject.name + "!");
    }

    private void Update()
    {
        if (!isPlaying)
            return;     
        if (Input.GetMouseButton((int)mouseButton) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetMouseButton((int)mouseButton))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
                {
                    targetPosition = hit.point;
                    isMoving = true;
                    Vector3 moveDirection = (targetPosition - transform.position).normalized;
                    ChangeDirection(DirectionCalculations.GetDirectionFromVector(moveDirection, amountOfDirections));
                    
                    // Calculate rotation around y-axis based on mouse position
                    lookDirection = targetPosition - transform.position;
                    targetRotationY = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
                    // Lock the character's rotation to only y-axis
                    transform.rotation = Quaternion.Euler(0, targetRotationY, 0);
                }
                else
                {
                    isMoving = false;
                }
            }
            else
            {
                isMoving = false;
                horizontalInput = Input.GetAxis("Horizontal");
                verticalInput = Input.GetAxis("Vertical");

                if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
                {
                    Vector3 inputDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
                    targetPosition = transform.position + inputDirection * (moveSpeed * Time.deltaTime);
                    isMoving = true;
                    ChangeDirection(DirectionCalculations.GetDirectionFromVector(inputDirection, amountOfDirections));
                    
                    // Lock the character's rotation to only y-axis
                    lookDirection = targetPosition - transform.position;
                    targetRotationY = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, targetRotationY, 0);
                }
            }
        }
        else
        {
            isMoving = false;
        }

        if (!isMoving)
        {
            StopMoving();
        }

        // Move the character using the CharacterController
        if (isMoving)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            controller.Move(moveDirection * (moveSpeed * Time.deltaTime));
        }
    }
    
    public Direction GetMovementDirection()
    {
        return lastDirection;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    private void ChangeDirection(Direction direction)
    {
        lastDirection = direction;
        // Handle direction change logic here
    }

    private void StopMoving()
    {
        // Stop movement logic here
    }
}