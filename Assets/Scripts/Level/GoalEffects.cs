using System;
using Effects;
using Simulation;
using UnityEngine;

namespace Level
{
    public class GoalEffects : MonoBehaviour
    {
        [SerializeField] private Firework firework;

        private void Start()
        {
            SimulationController.Instance.OnSimulationStopped += ResetFireworks;
            LevelManager.Instance.OnGoalReached += GoalReached;
        }

        private void OnDestroy()
        {
            SimulationController.Instance.OnSimulationStopped -= ResetFireworks;
            LevelManager.Instance.OnGoalReached -= GoalReached;
        }

        private void GoalReached()
        {
            firework.gameObject.SetActive(true);
        }
        
        private void ResetFireworks()
        {
            firework.gameObject.SetActive(false);
        }
    }
}
