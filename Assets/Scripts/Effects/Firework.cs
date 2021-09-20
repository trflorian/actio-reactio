using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Effects
{
    public class Firework : MonoBehaviour
    {
        [SerializeField] private AudioClip fireworkTrail, fireworkBurst;

        private ParticleSystem _ascendingParticleSystem;
        private AudioSource _audioSource;

        private HashSet<ParticleSystem.Particle> _aliveParticles;
        private int _previousAliveParticles;

        private void Awake()
        {
            _ascendingParticleSystem = GetComponent<ParticleSystem>();
            _audioSource = GetComponent<AudioSource>();
            _aliveParticles = new HashSet<ParticleSystem.Particle>();
        }

        private void Update()
        {
            var particles = new ParticleSystem.Particle[1];
            var particlesCount = _ascendingParticleSystem.GetParticles(particles);

            if (particlesCount != _previousAliveParticles)
            {
                if (particlesCount < _previousAliveParticles)
                {
                    // particle died
                    _audioSource.Stop();
                    _audioSource.PlayOneShot(fireworkBurst);
                    Camera.main.DOShakePosition(0.1f, strength:0.5f, vibrato:20);
                }
                else
                {
                    // particle spawned
                    _audioSource.Play();
                }
                _previousAliveParticles = particlesCount;
            }
        }

        private void OnEnable()
        {
            _ascendingParticleSystem.Play();
        }

        private void OnDisable()
        {
            _ascendingParticleSystem.Stop();
        }
    }
}
