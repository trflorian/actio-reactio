using System;
using Level;
using Simulation;
using TMPro;
using UnityEngine;

namespace UI
{
    public class SimulationTimerUI : MonoBehaviour
    {
        private TMP_Text _timerText;

        private void Awake()
        {
            _timerText = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            var ts = new TimeSpan(0, 0, 0, 0, (int)(SimulationController.Instance.simulationTime*1000));
            _timerText.SetText(ts.ToString(@"mm\:ss\.fff"));
        }
    }
}
