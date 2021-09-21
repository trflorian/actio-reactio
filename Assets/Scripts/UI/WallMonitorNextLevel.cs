using System;
using DG.Tweening;
using Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class WallMonitorNextLevel : MonoBehaviour
    {
        [SerializeField] private Button nextLevelButton;
        
        public bool showOnlyWhenNextLevelExists;
        
        private float _endPositionZ;

        private bool _nextLevelExists;

        private void Awake()
        {
            _endPositionZ = transform.position.z;
            _nextLevelExists = SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings-1;
        }

        private void Start()
        {
            if (!LevelManager.Instance.levelInfo.IsLevelCompleted() || (showOnlyWhenNextLevelExists && !_nextLevelExists))
            {
                // hide panel
                transform.position += new Vector3(0, 0, 1);
                nextLevelButton.interactable = false;
            }

            LevelManager.Instance.OnGoalReached += GoalReached;
        }

        private void OnDestroy()
        {
            LevelManager.Instance.OnGoalReached -= GoalReached;
        }

        private void GoalReached()
        {
            if (_nextLevelExists || !showOnlyWhenNextLevelExists)
            {
                nextLevelButton.interactable = true;
                transform.DOMoveZ(_endPositionZ, 0.5f);
            }
        }
    }
}
