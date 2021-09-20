using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Simulation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        public event UnityAction OnGoalReached;
        private bool _goalReached;

        public static LevelManager Instance;
        
        public LevelInfo levelInfo;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SimulationController.Instance.OnSimulationStarted += ResetGoalReached;
            SimulationController.Instance.OnSimulationStopped += ResetGoalReached;
        }

        private void OnDestroy()
        {
            SimulationController.Instance.OnSimulationStarted -= ResetGoalReached;
            SimulationController.Instance.OnSimulationStopped -= ResetGoalReached;
        }

        private void ResetGoalReached()
        {
            _goalReached = false;
        }

        public void ResetLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void GoalReached()
        {
            if(_goalReached) return;

            _goalReached = true;
            
            Camera.main.DOShakePosition(0.5f, strength:1, vibrato:10);
            levelInfo.CompleteLevel();
            
            OnGoalReached?.Invoke();

            SimulationController.Instance.StopSimulation();
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene(0);
        }

        public bool IsGoalReached()
        {
            return _goalReached;
        }
    }
}
