using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterContainer", menuName = "TopDownSurvivor/Character Container")]
[System.Serializable]
public class CharacterContainer : ScriptableObject
{
    public List<CharacterData> characters = new List<CharacterData>();
}