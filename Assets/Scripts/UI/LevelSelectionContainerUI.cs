using System;
using System.Collections.Generic;
using DG.Tweening;
using Level;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class LevelSelectionContainerUI : MonoBehaviour
    {
        public static event UnityAction<LevelInfo> OnLevelSelected;
        
        [SerializeField] private List<LevelInfo> levels;
        
        private List<LevelSelectionItemUI> _items;

        private RectTransform _rectTransform;
        private float _initialY;
        
        private int _selectedLevel = 0;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _initialY = _rectTransform.anchoredPosition.y;
            
            var prefab = transform.GetChild(0);

            for (var index = 0; index < levels.Count; index++)
            {
                var levelInfo = levels[index];
                
                var lui = Instantiate(prefab, transform).GetComponent<LevelSelectionItemUI>();
                lui.SetLevel(levelInfo);
                lui.OnSelectThisItem += OnSelectLevel;
            }

            Destroy(prefab.gameObject);
        }

        private void Update()
        {
            var dy = Input.mouseScrollDelta.y;
            if (Mathf.Abs(dy) >= 0.01f)
            {
                OnSelectLevel(_selectedLevel - (int)dy);
            }
        }

        private void OnSelectLevel(int level)
        {
            _selectedLevel = Mathf.Clamp(level, 0, levels.Count-1);

            _rectTransform.DOAnchorPosY(_initialY + _selectedLevel * 17, 0.3f);

            OnLevelSelected?.Invoke(levels[_selectedLevel]);
        }
    }
}
