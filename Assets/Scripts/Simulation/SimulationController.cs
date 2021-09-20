using System.Collections.Generic;
using Level;
using UnityEngine;
using UnityEngine.Events;

namespace Simulation
{
    public class SimulationController : MonoBehaviour
    {
        public event UnityAction OnSimulationStarted, OnSimulationStopped; 

        public static SimulationController Instance;
    
        private List<ILevelObject> _allLevelObjects;
        
        public bool isSimulationRunning;
        public float simulationTime;

        public void RegisterLevelObject(ILevelObject lo) => _allLevelObjects.Add(lo);
        public void UnRegisterLevelObject(ILevelObject lo) => _allLevelObjects.Remove(lo);

        private void Awake()
        {
            Instance = this;
            _allLevelObjects = new List<ILevelObject>();
        }

        private void FixedUpdate()
        {
            if (isSimulationRunning)
            {
                simulationTime += Time.fixedDeltaTime;
            }
        }
        
        public void StartLevelSimulation()
        {
            if (isSimulationRunning)
            {
                Debug.LogWarning("Trying to start simulation, but it's already running!");
                return;
            }
            isSimulationRunning = true;
            _allLevelObjects.ForEach(lo => lo.StartSimulation());
            
            simulationTime = 0;
            
            OnSimulationStarted?.Invoke();
        }

        public void ResetLevelSimulation()
        {
            isSimulationRunning = false;
            _allLevelObjects.ForEach(lo => lo.ResetSimulation());
            
            simulationTime = 0;
            
            OnSimulationStopped?.Invoke();
        }

        public void StopSimulation()
        {
            isSimulationRunning = false;
        }
    }
}
