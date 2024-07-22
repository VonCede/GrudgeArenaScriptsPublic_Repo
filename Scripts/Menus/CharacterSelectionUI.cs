using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionUI : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    [SerializeField] private Text characterNameText;
    [SerializeField] private Image background;
    [SerializeField] private Color selectedColor = Color.blue;
    [SerializeField] private Color unselectedColor = Color.grey;
    
    private CharacterData characterData;
    private int myIndex;
    private Action<Action<bool>,int> onThisClicked;

    public void Initialize(CharacterData characterData, int index, bool isSelected, Action<Action<bool>, int> onClicked)
    {
        this.characterData = characterData;
        myIndex = index;
        onThisClicked = onClicked;
        characterImage.sprite = characterData.characterIcon;
        characterNameText.text = characterData.characterName;
        SetSelected(isSelected);
        if(isSelected)
            onThisClicked?.Invoke(SetSelected, myIndex);
    }
    
    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            background.color = selectedColor;
        }
        else
        {
            background.color = unselectedColor;
        }
    }
    
    public void OnClicked()
    {
        if(GameState.Instance.selectedCharacterIndex == myIndex)
            return;
        onThisClicked?.Invoke(SetSelected, myIndex);
        SetSelected(true);
    }

}
