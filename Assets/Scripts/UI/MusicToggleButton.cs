using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class MusicToggleButton : MonoBehaviour
    {
        public enum AudioType
        {
            Music, SFX
        }

        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Sprite unMutedIcon, mutedIcon;
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private AudioType type;
        
        private bool _muted;
        
        private void Awake()
        {
            button.onClick.AddListener(OnToggle);
        }

        private void Start()
        {
            ChangeMuted(PlayerPrefs.GetInt(type.ToString(), 0) == 1);
        }

        private void OnDestroy()
        {
            PlayerPrefs.SetInt(type.ToString(), _muted ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void ChangeMuted(bool newMuted)
        {
            _muted = newMuted;
            image.sprite = _muted ? mutedIcon : unMutedIcon;
            audioMixer.SetFloat(type+"Vol", _muted ? -80 : type == AudioType.SFX ? -15 : 0);
        }

        private void OnToggle()
        {
            ChangeMuted(!_muted);
        }
    }
}
