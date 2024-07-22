using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDescriptionUI : MonoBehaviour
{
    [SerializeField] private Text characterNameText;
    [SerializeField] private Text characterDescriptionText;
    
    // TODO: Add character stats too

    public void CharacterChanged()
    {
        characterNameText.text = GameState.Instance.selectedCharacter.characterName;
        characterDescriptionText.text = GameState.Instance.selectedCharacter.characterDescription;
    }
}
