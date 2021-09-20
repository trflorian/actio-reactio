using System;
using DG.Tweening;
using UnityEngine;

namespace Level
{
    public class GoalBall : RigidbodyLevelObject
    {
        private Vector3 _initialScale;
        
        protected override void Awake()
        {
            _initialScale = transform.localScale;
            base.Awake();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Goal"))
            {
                LevelManager.Instance.GoalReached();

                Rigidbody.isKinematic = true;
                Rigidbody.ResetInertiaTensor();
                var target = other.transform.position;
                Rigidbody.DOMove(target, 0.5f);
                transform.DOScale(0f, 0.5f);
            }
        }

        public override void ResetSimulation()
        {
            transform.DOKill();
            Rigidbody.DOKill();
            transform.localScale = _initialScale;
            base.ResetSimulation();
        }
    }
}
