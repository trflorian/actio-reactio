using System;
using UnityEngine;

namespace Bezier
{
    /// <summary>
    /// Movable handle to modify path
    /// </summary>
    public class AnchorHandle : MonoBehaviour
    {
        private Camera _mainCamera;
        private Path _path;
        private int _segmentIndex, _handleIndex;

        [SerializeField] private Color anchorColor, controlColor;
        [SerializeField] private LineRenderer lineRenderer;

        public void Setup(Path path, int segmentIndex, int handleIndex)
        {
            _path = path;
            _path.OnChanged += OnPathChanged;
            _segmentIndex = segmentIndex;
            _handleIndex = handleIndex;

            var r = GetComponent<Renderer>();
            r.material.SetColor("_Color", _handleIndex == 0 || handleIndex == 3 ? anchorColor : controlColor);
                
            OnPathChanged();
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void OnDestroy()
        {
            _path.OnChanged -= OnPathChanged;
        }

        private void OnPathChanged()
        {
            var points = _path.GetPointsInSegment(_segmentIndex);
            transform.position = points[_handleIndex];
            
            if (_handleIndex == 1 || _handleIndex == 2)
            {
                lineRenderer.positionCount = 2;
                lineRenderer.SetPositions(new []
                {
                    transform.position, points[_handleIndex == 1 ? 0 : 3]
                });
            }
        }

        private void OnMouseDrag()
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 1000, LayerMask.GetMask("Ground")))
            {
                transform.position = hitInfo.point;
                _path.MovePoint(_segmentIndex, _handleIndex, hitInfo.point);
            }
        }
    }
}
