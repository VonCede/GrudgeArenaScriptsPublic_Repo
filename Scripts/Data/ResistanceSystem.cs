using System;
using System.Collections.Generic;

[System.Serializable]
public class ResistanceData
{
    public DamageType damageType;
    public float resistanceValue;
}

// Define a custom comparer for ResistanceData
public class ResistanceDataComparer : IComparer<ResistanceData>
{
    public int Compare(ResistanceData x, ResistanceData y)
    {
        return x.damageType.CompareTo(y.damageType);
    }
}

[System.Serializable]
public class ResistanceSystem
{
    // Define resistances based on DamageTypes enum
    public List<ResistanceData> resistances = new List<ResistanceData>();

    // Constructor to initialize resistances
    public ResistanceSystem()
    {
        foreach (DamageType type in Enum.GetValues(typeof(DamageType)))
        {
            // Create a new ResistanceData object for each damage type
            ResistanceData resistance = new ResistanceData();
            resistance.damageType = type;
            resistance.resistanceValue = 0f;

            resistances.Add(resistance);
        }

        // Sort the resistances list based on DamageType
        resistances.Sort(new ResistanceDataComparer());
    }
    
    // Get the resistance value for a specific DamageType
    public float GetResistance(DamageType type)
    {
        // Perform a binary search to find the desired resistance value
        int index = resistances.BinarySearch(new ResistanceData { damageType = type }, new ResistanceDataComparer());

        if (index >= 0)
        {
            // Found a matching DamageType, return the resistance value
            return resistances[index].resistanceValue;
        }

        // If the specified damage type is not found, return a default value (e.g., 0)
        return 0f;
    }
}
