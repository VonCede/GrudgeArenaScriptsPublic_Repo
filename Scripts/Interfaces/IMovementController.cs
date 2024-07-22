
using UnityEngine;

public enum Direction
{
    Idle,
    Up,
    Down,
    Left,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight
}

public class DirectionCalculations
{
    public static Vector3 GetVectorFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector3.forward;
            case Direction.Down:
                return Vector3.back;
            case Direction.Left:
                return Vector3.left;
            case Direction.Right:
                return Vector3.right;
            default:
                return Vector3.zero;
        }
    }

    public static Direction GetDirectionFromVector(Vector3 inputVector, int amountOfDirections)
    {
        if(inputVector == Vector3.zero)
            return Direction.Idle;
        
        float angle = Mathf.Atan2(inputVector.x, inputVector.z) * Mathf.Rad2Deg;

        if (amountOfDirections == 1)
            return Direction.Up;        
        if(amountOfDirections == 4)
            return GetDirectionFromAngle4(angle);
        if (amountOfDirections == 8)
            return GetDirectionFromAngle8(angle);
        return Direction.Idle;
    }

    private static Direction GetDirectionFromAngle8(float angle)
    {
        // Calculate 8 directions from an angle
        if (angle >= -22.5f && angle < 22.5f)
        {
            return Direction.Right;
        }
        if (angle >= 22.5f && angle < 67.5f)
        {
            return Direction.UpRight;
        }
        if (angle >= 67.5f && angle < 112.5f)
        {
            return Direction.Up;
        }
        if (angle >= 112.5f && angle < 157.5f)
        {
            return Direction.UpLeft;
        }
        if (angle >= 157.5f || angle < -157.5f)
        {
            return Direction.Left;
        }
        if (angle >= -157.5f && angle < -112.5f)
        {
            return Direction.DownLeft;
        }
        if (angle >= -112.5f && angle < -67.5f)
        {
            return Direction.Down;
        }
        if (angle >= -67.5f && angle < -22.5f)
        {
            return Direction.DownRight;
        }

        // Handle the case when the angle doesn't match any of the defined ranges
        return Direction.Idle;
    }


    private static Direction GetDirectionFromAngle4(float angle)
    {
        if (angle >= -45f && angle < 45f)
        {
            return Direction.Right;
        }
        else if (angle >= 45f && angle < 135f)
        {
            return Direction.Up;
        }
        else if (angle >= -135f && angle < -45f)
        {
            return Direction.Down;
        }
        else
        {
            return Direction.Left;
        }
    }
}

public interface IMovementController
{
    Direction GetMovementDirection();
    bool IsMoving();
}
