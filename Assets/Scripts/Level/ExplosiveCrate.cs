using System;
using System.Collections;
using Audio;
using DG.Tweening;
using Simulation;
using UnityEngine;

namespace Level
{
    public class ExplosiveCrate : MonoBehaviour
    {
        private Camera _mainCamera;
        private Vector3 _originalPosition;
        
        private const float ExplosionForce = 700;
        private const float ExplosionRadius = 6;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _originalPosition = _mainCamera.transform.position;
        }

        public IEnumerator ExplodeDelayed()
        {
            yield return new WaitForSeconds(0.1f);
            Explode();
        }

        private void Explode()
        {
            // check if already exploded
            if (!gameObject.activeSelf) return;
            if (!SimulationController.Instance.isSimulationRunning) return;

            _mainCamera.DOShakePosition(0.1f, strength:1, vibrato:20)
                .OnComplete(() =>
                {
                    _mainCamera.transform.DOMove(_originalPosition, 0.2f);
                });
            
            gameObject.SetActive(false);

            var explosionOrigin = transform.position + Vector3.down*2f;
                
            var colliders = Physics.OverlapSphere(explosionOrigin, ExplosionRadius);

            foreach (var collider in colliders)
            {
                var rb = collider.attachedRigidbody;
                if (rb != null)
                {
                    rb.AddExplosionForce(ExplosionForce, explosionOrigin, ExplosionRadius);
                }

                // chain explosion if near enough
                if (collider.TryGetComponent(out ExplosiveCrate explosiveCrate) &&
                    (transform.position - collider.transform.position).magnitude < ExplosionRadius*0.7f)
                {
                    explosiveCrate.StartCoroutine(explosiveCrate.ExplodeDelayed());
                }
            }
            
            SFXManager.Instance.PlayExplosion(transform.position);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("PlaceableObjects") ||
                other.gameObject.layer == LayerMask.NameToLayer("FixedObjects"))
            {
                Explode();
            }
        }
    }
}
