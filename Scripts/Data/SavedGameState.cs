[System.Serializable]
public class SavedGameState
{
    public int selectedCharacterIndex;
    public int[] unlockedCharacters;
    public long[] grudges;
    public int selectedLevelIndex;
    public int[] unlockedLevels;
    public int gold;
    public int grudgeCoins;
    // Add other data fields you want to save here...
    
    public SavedGameState()
    {
        selectedCharacterIndex = 0;
        unlockedCharacters = new int[1];
        unlockedCharacters[0] = 0;
        selectedLevelIndex = 0;
        unlockedLevels = new int[1];
        unlockedLevels[0] = 0;
        gold = 0;
        grudgeCoins = 0;
        grudges = new long[1];
        grudges[0] = 0;
    }
}