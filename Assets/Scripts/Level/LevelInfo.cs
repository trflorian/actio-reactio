using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(menuName = "Level/Level Info")]
    public class LevelInfo : ScriptableObject
    {
        [System.Serializable]
        public class LevelObjectInfo
        {
            public PlaceableObjectSO placeableObject;
            public int amount;
        }

        public int levelNumber;
        public string levelName;
        public List<LevelObjectInfo> objects;

        public string LevelKey => "LEVEL_" + levelNumber;
        public string PreviousLevelKey => "LEVEL_" + (levelNumber-1);
        public string LevelHighscoreKey => "LEVEL_HS_" + levelNumber;

        public string DisplayText() => (levelNumber == -1 ? levelName : 
            $"Level {levelNumber} - {levelName}");

        public bool IsLevelAvailable()
        {
            if (levelNumber <= 0) return true;
            return PlayerPrefs.GetInt(PreviousLevelKey, 0) == 1;
        }
        
        public bool IsLevelCompleted()
        {
            return PlayerPrefs.GetInt(LevelKey, 0) == 1;
        }

        public void CompleteLevel()
        {
            PlayerPrefs.SetInt(LevelKey, 1);
            PlayerPrefs.Save();
        }
    }
}
