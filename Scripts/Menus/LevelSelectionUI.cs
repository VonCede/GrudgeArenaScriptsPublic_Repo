using System;
using UnityEngine.UI;
using UnityEngine;

public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField] private Image levelImage;
    [SerializeField] private Text levelNameText;
    [SerializeField] private Image background;
    [SerializeField] private Color selectedColor = Color.blue;
    [SerializeField] private Color unselectedColor = Color.grey;
    
    private LevelData levelData;
    private int myIndex;
    private Action<Action<bool>,int> onThisClicked;
    
    public void Initialize(LevelData levelData, int index, bool isSelected, Action<Action<bool>, int> onClicked)
    {
        this.levelData = levelData;
        myIndex = index;
        onThisClicked = onClicked;
        levelImage.sprite = levelData.levelIcon;
        levelNameText.text = levelData.levelName;
        SetSelected(isSelected);
        if(isSelected)
            onThisClicked?.Invoke(SetSelected, myIndex);
    }

    private void SetSelected(bool isSelected)
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
        if(GameState.Instance.selectedLevelIndex == myIndex)
            return;
        onThisClicked?.Invoke(SetSelected, myIndex);
        SetSelected(true);
    }
}
