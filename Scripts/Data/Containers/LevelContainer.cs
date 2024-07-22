using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelContainer", menuName = "TopDownSurvivor/Level Container")]
[System.Serializable]
public class LevelContainer : ScriptableObject
{
    public List<LevelData> levels = new List<LevelData>();
}