using System;
using Level;
using Simulation;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HighscoreTextUI : MonoBehaviour
    {
        private TMP_Text _highscoreText;

        private void Awake()
        {
            _highscoreText = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            LevelManager.Instance.OnGoalReached += CheckHighscore;
            var currentHighscore = LoadHighscore();
            SetHighscoreText(currentHighscore);
        }

        private void OnDestroy()
        {
            LevelManager.Instance.OnGoalReached -= CheckHighscore;
        }

        private void SetHighscoreText(int highscore)
        {
            var ts = new TimeSpan(0, 0, 0, 0, highscore);
            _highscoreText.SetText("Best Time: "+ts.ToString(@"mm\:ss\.fff"));
        }

        private int LoadHighscore()
        {
            return PlayerPrefs.GetInt(LevelManager.Instance.levelInfo.LevelHighscoreKey, int.MaxValue);
        }

        private void CheckHighscore()
        {
            var currentTime = (int)(SimulationController.Instance.simulationTime * 1000);
            if (currentTime <= LoadHighscore())
            {
                PlayerPrefs.SetInt(LevelManager.Instance.levelInfo.LevelHighscoreKey, currentTime);
                SetHighscoreText(currentTime);
            }
        }
    }
}
