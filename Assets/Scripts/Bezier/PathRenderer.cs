using System;
using UnityEngine;

namespace Bezier
{
    /// <summary>
    /// Renders the path
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class PathRenderer : MonoBehaviour
    {
        private const int PointsPerSegment = 30;
        
        private LineRenderer _lineRenderer;
        
        private Path _path;

        public void SetPath(Path path)
        {
            _path = path;
            _path.OnChanged += OnPathChanged;
            OnPathChanged();
        }

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void OnDestroy()
        {
            _path.OnChanged -= OnPathChanged;
        }

        public void OnPathChanged()
        {
            var linePoints = new Vector3[_path.NumSegments * PointsPerSegment];
            for (int i = 0; i < _path.NumSegments; i++)
            {
                var segment = _path.GetPointsInSegment(i);
                for (var pi = 0; pi < PointsPerSegment; pi++)
                {
                    var t = (float)pi / (PointsPerSegment - 1);
                    var p = Path.CubicCurve(segment[0], segment[1], segment[2], segment[3], t);
                    linePoints[i * PointsPerSegment + pi] = p + Vector3.up * 0.01f;
                }
            }

            _lineRenderer.positionCount = linePoints.Length;
            _lineRenderer.SetPositions(linePoints);
        }
    }
}
