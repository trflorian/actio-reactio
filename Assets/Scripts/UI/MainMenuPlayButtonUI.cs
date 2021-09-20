using System;
using DG.Tweening;
using Level;
using MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuPlayButtonUI : MonoBehaviour
    {
        [SerializeField] private MainMenuController controller;
        
        private Button _button;
        private int _selectedLevel;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => controller.LoadLevel(_selectedLevel));

            LevelSelectionContainerUI.OnLevelSelected += OnLevelSelected;
        }

        private void Start()
        {
            var sequence = DOTween.Sequence()
                .Append(transform.DOPunchScale(Vector3.one*0.2f, 2f, 5, 0.2f))
                .AppendInterval(2f);
            sequence.SetLoops(-1, LoopType.Restart);
        }

        private void OnDestroy()
        {
            LevelSelectionContainerUI.OnLevelSelected -= OnLevelSelected;
        }

        private void OnLevelSelected(LevelInfo selectedLevel)
        {
            _selectedLevel = selectedLevel.levelNumber;
            _button.interactable = selectedLevel.IsLevelAvailable();
        }
    }
}
