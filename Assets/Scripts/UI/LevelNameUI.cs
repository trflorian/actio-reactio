using System;
using Level;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LevelNameUI : MonoBehaviour
    {
        private TMP_Text _levelNameText;

        private void Awake()
        {
            _levelNameText = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            _levelNameText.SetText(LevelManager.Instance.levelInfo.DisplayText());
        }
    }
}
