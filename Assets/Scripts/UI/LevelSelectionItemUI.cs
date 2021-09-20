using System;
using Level;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelectionItemUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelNameText;
        [SerializeField] private Button selectButton;

        private LevelInfo _levelInfo;
        public event UnityAction<int> OnSelectThisItem;
        
        public void SetLevel(LevelInfo levelInfo)
        {
            _levelInfo = levelInfo;
            levelNameText.SetText(levelInfo.DisplayText());
            selectButton.onClick.AddListener(OnButtonClicked);

            levelNameText.color = levelInfo.IsLevelAvailable() ? selectButton.colors.normalColor : selectButton.colors.disabledColor;
        }

        private void OnDestroy()
        {
            selectButton.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            OnSelectThisItem?.Invoke(_levelInfo.levelNumber);
        }
    }
}
