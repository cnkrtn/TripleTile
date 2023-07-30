using UnityEngine;

namespace Managers
{
    public class SaveLoadManager:MonoBehaviour
    { 
        public int levelIndex,totalScore,modeIndex;
        private const string LevelIndexKey = "LevelIndex"; 
        private const string ScoreIndexKey = "ScoreIndex";
        private const string ModeIndexKey = "ModeIndex";
        

        public void SaveLevelIndex()
        {
            levelIndex++;
            if (levelIndex > 4)
            {
                levelIndex = 0;}
            PlayerPrefs.SetInt(LevelIndexKey, levelIndex);
            PlayerPrefs.Save();
        }

        public void SaveTotalScore()
        {
            PlayerPrefs.SetInt(ScoreIndexKey, totalScore);
            PlayerPrefs.Save();
        }
        
        public void LoadData()
        {
            levelIndex = PlayerPrefs.GetInt(LevelIndexKey, 0);
            totalScore = PlayerPrefs.GetInt(ScoreIndexKey, 0);
            modeIndex = PlayerPrefs.GetInt(ModeIndexKey, 0);
        }
    }
}