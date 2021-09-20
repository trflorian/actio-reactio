using System;
using Level;
using UnityEngine;

namespace Audio
{
    public class GoalAudio : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            LevelManager.Instance.OnGoalReached += PlayApplause;
        }

        private void OnDestroy()
        {
            LevelManager.Instance.OnGoalReached -= PlayApplause;
        }

        private void PlayApplause()
        {
            _audioSource.Play();
        }
    }
}
