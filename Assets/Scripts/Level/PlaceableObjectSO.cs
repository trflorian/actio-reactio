using UnityEngine;

namespace Level
{
    [CreateAssetMenu(menuName = "Level/Placeable Object")]
    public class PlaceableObjectSO : ScriptableObject
    {
        public enum Type
        {
            ObjectPlaceable, Dominos
        }
        
        public GameObject prefab;
        public Sprite icon;
        public float spawnYOffset;
        public Vector3 initialRotation;
        public float spawnProofRadius;
        public Type type;
    }
}
