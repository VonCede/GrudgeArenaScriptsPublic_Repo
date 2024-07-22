using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private new Animation animation;
    private IMovementController movementController;
    private bool isMoving = false;

    [SerializeField] private string movementAnimation = "Walk";
    [SerializeField] private string idleAnimation = "Idle";

    private void Start()
    {
        animation = GetComponent<Animation>();
        movementController = GetComponent<IMovementController>();

        if (animation == null)
        {
            Debug.LogError( gameObject.name + " couldn't find Animator component.");
        }

        // Find the MovementController in parent GameObject
        movementController = transform.parent.gameObject.GetComponent<IMovementController>();
        // if not found, try to one step up
        if (movementController == null)
        {
            movementController = transform.parent.parent.gameObject.GetComponent<IMovementController>();
        }
        // Still not found, give error
        if (movementController == null)
        {
            Debug.LogError( gameObject.name + " couldn't find MovementController in parent or grandparent GameObject.");
        }
        
        animation.Play(idleAnimation);
    }

    private void Update()
    {

        // get isMoving from MovementController and if state changed, play animation
        if (isMoving != movementController.IsMoving())
        {
            isMoving = movementController.IsMoving();
            if (isMoving)
            {
                animation.Play(movementAnimation);
            }
            else
            {
                animation.Play(idleAnimation);
            }
        }
    }
}
