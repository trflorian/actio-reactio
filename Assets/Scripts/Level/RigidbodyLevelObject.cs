using System;
using Simulation;
using UnityEngine;

namespace Level
{
    public class RigidbodyLevelObject : MonoBehaviour, ILevelObject
    {
        protected Rigidbody Rigidbody;

        private Vector3 _originalPosition;
        private Quaternion _originalRotation;

        public bool isStatic;

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;
        }

        private void Start()
        {
            SimulationController.Instance.RegisterLevelObject(this);
            ResetSimulation();
        }

        private void OnDestroy()
        {
            SimulationController.Instance.UnRegisterLevelObject(this);
        }

        public virtual void StartSimulation()
        {
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;
            Rigidbody.isKinematic = isStatic;
        }

        public virtual void ResetSimulation()
        {
            gameObject.SetActive(true);
            StopAllCoroutines();
            Rigidbody.isKinematic = true;
            Rigidbody.ResetInertiaTensor();
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
            transform.position = _originalPosition;
            transform.rotation = _originalRotation;
        }
    }
}
