using System;
using UnityEngine;

namespace Bezier
{
    /// <summary>
    /// Move an object along a path
    /// </summary>
    public class MoveAlongPath : MonoBehaviour
    {
        [SerializeField] [Range(0, 1)] private float t = 0.5f;

        private Path _path;
        
        public void Setup(Path path)
        {
            _path = path;
        }
        
        private void Update()
        {
            if (_path == null)
            {
                Debug.LogError("No Path specified for move along path!");
                return;
            }
            
            var (p, tt) = _path.LengthInterpolate(t);
            var pos = p;
            var dir = _path.InterpolateDerivative(tt);
            transform.position = pos;
            transform.right = dir;
        }
    }
}
