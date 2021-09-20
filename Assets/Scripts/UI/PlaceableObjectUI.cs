using System;
using System.Linq;
using Level;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// UI holder for a placeable object SO
    /// </summary>
    public class PlaceableObjectUI : MonoBehaviour
    {
        public static event UnityAction<PlaceableObjectSO> OnSelectPlaceableObject;

        [SerializeField] private Image iconImage;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text amountText;
        
        private PlaceableObjectSO _thisObj;
        
        public void Setup(LevelInfo.LevelObjectInfo objInfo)
        {
            _thisObj = objInfo.placeableObject;
            iconImage.sprite = objInfo.placeableObject.icon;
            button.onClick.AddListener(OnSelectThisObject);
            amountText.SetText(objInfo.amount.ToString());
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnSelectThisObject);
        }

        private void Update()
        {
            var objInfo = ObjectPlacer.Inventory.objects
                .FirstOrDefault(o => o.placeableObject == _thisObj);
            amountText.SetText(objInfo.amount.ToString());
            button.interactable = objInfo.amount > 0;
        }

        private void OnSelectThisObject()
        {
            OnSelectPlaceableObject?.Invoke(_thisObj);
        }
    }
}
