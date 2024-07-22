using UnityEngine;
using UnityEngine.UI;

public class LevelDescriptionUI : MonoBehaviour
{
    [SerializeField] private Text levelNameText;
    [SerializeField] private Text levelDescriptionText;
    
    public void LevelChanged()
    {
        levelNameText.text = GameState.Instance.GetSelectedLevel().levelName;
        levelDescriptionText.text = GameState.Instance.GetSelectedLevel().levelDescription;
    }
}
