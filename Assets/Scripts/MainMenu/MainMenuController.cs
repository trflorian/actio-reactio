using System;
using System.Collections.Generic;
using PlayFab;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        private const string FirstTimeKey = "FirstTime";
        
        private bool isSimulationRunning;

        private void Awake()
        {
            if (PlayerPrefs.GetInt(FirstTimeKey, 0) == 0)
            {
                // TODO remove
                PlayerPrefs.SetInt(FirstTimeKey, 1);
                PlayerPrefs.Save();
                OpenProfile();
            }
        }

        public void LoadLevel(int level)
        {
            SceneManager.LoadScene(level == -1 ? SceneManager.sceneCountInBuildSettings - 1 : level + 1);
        }
        
        public void ResetLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ShowScoreboard()
        {
            SceneManager.LoadScene("Scoreboard");
        }

        public void OpenProfile()
        {
            SceneManager.LoadScene("Profile");
        }
    }
}
