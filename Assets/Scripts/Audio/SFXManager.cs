using System.Collections.Concurrent;
using UnityEngine;

namespace Audio
{
    public class SFXManager : MonoBehaviour
    {
        public static SFXManager Instance;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip collisionClip, explosionClip;
        [SerializeField] private GameObject explosionPrefab;

        private ConcurrentDictionary<(GameObject, GameObject), float> _collisionTimes;

        private const float MinCollisionDeltaTime = 0.1f;
        private void Awake()
        {
            Instance = this;
            _collisionTimes = new ConcurrentDictionary<(GameObject, GameObject), float>();
        }

        private void FixedUpdate()
        {
            foreach (var coll in _collisionTimes.Keys)
            {
                if (_collisionTimes.TryGetValue(coll, out var oldTime))
                {
                    _collisionTimes.TryUpdate(coll, oldTime + Time.fixedDeltaTime, oldTime);
                }
            }
        }

        public void PlayCollisionSound(float relativeVelocity, GameObject go1, GameObject go2)
        {
            if (relativeVelocity <= 0.01f) return;

            if (_collisionTimes.ContainsKey((go1, go2)))
            {
                if (_collisionTimes[(go1, go2)] <= MinCollisionDeltaTime)
                {
                    return;
                }

                _collisionTimes[(go1, go2)] = 0;
            }
            else if (_collisionTimes.ContainsKey((go2, go1)))
            {
                if (_collisionTimes[(go2, go1)] <= MinCollisionDeltaTime)
                {
                    return;
                }
                _collisionTimes[(go2, go1)] = 0;
            }
            else
            {
                _collisionTimes.TryAdd((go1, go2), 0f);
            }

            var volume = Mathf.Clamp(relativeVelocity / 30f, 0.01f, 0.8f);
            
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(collisionClip, volume);
        }

        public void PlayExplosion(Vector3 origin)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(explosionClip);

            Instantiate(explosionPrefab, origin, Quaternion.Euler(0, Random.Range(0, 360), 0));
        }
    }
}
