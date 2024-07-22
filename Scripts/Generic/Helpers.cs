using UnityEngine;

public class Helpers
{
    public static T DeepCopy<T>(T original) where T : ScriptableObject
    {
        if (original == null)
        {
            Debug.LogError("Cannot copy a null ScriptableObject.");
            return null;
        }

        T copy = Object.Instantiate(original);

        return copy;
    }
}
