using Level;
using UnityEngine;

namespace UI
{
    public class PlaceableObjectListUI : MonoBehaviour
    {
        public void Start()
        {
            var prefab = transform.GetChild(0).gameObject;
            foreach (var plo in LevelManager.Instance.levelInfo.objects)
            {
                Instantiate(prefab, transform).GetComponent<PlaceableObjectUI>()
                    .Setup(plo);
            }
            Destroy(prefab);
        }
    }
}
