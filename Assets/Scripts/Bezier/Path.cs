using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bezier
{
    /// <summary>
    /// 2D Path for a cubic bezier curve
    /// </summary>
    public class Path
    {
        private const int SegmentSubdivision = 40;
        
        private readonly List<Vector3> _points;

        public event UnityAction OnChanged; 

        public Path()
        {
            _points = new List<Vector3>();
        }

        public Path(Vector3 center)
        {
            _points = new List<Vector3>()
            {
                center + Vector3.left,
                center + Vector3.left * 0.5f,
                center + Vector3.right * 0.5f,
                center + Vector3.right
            };
        }

        public Vector3 this[int i] => _points[i];

        public int NumPoints => _points.Count;

        public int NumSegments => (_points.Count - 4) / 3 + 1;

        public int AddSegment(Vector3 anchorPos)
        {
            if (NumPoints == 0)
            {
                _points.Add(anchorPos);
                return -1;
            }
            
            if (NumPoints == 1)
            {
                var dir = _points[0] - anchorPos;
                _points.Add(_points[0] - dir/2);
                _points.Add(anchorPos + dir/2);
                _points.Add(anchorPos);
            }
            else
            {
                _points.Add(_points[_points.Count - 1] * 2 - _points[_points.Count - 2]);
                _points.Add((_points[_points.Count - 1] + anchorPos) * 0.5f);
                _points.Add(anchorPos);
            }

            OnChanged?.Invoke();
            return NumSegments - 1;
        }

        public Vector3[] GetPointsInSegment(int index)
        {
            if (NumPoints <= 1) return _points.ToArray();
            
            var ret = new Vector3[4];
            for (var i = 0; i < 4; i++)
            {
                ret[i] = _points[index * 3 + i];
            }
            return ret;
        }

        public void MovePoint(int segIndex, int handleIndex, Vector3 newPos)
        {
            var index = segIndex * 3 + handleIndex;
            var deltaMove = newPos - _points[index];
            _points[index] = newPos;

            if (handleIndex == 0 || handleIndex == 3) // anchor point
            {
                if (index + 1 < NumPoints)
                {
                    _points[index + 1] += deltaMove;
                }
                if (index - 1 >= 0)
                {
                    _points[index - 1] += deltaMove;
                }
            }
            else
            {
                var nextPointIsAnchor = handleIndex == 2;
                var correspondingControlIndex = nextPointIsAnchor ? index + 2 : index - 2;
                var anchorIndex = nextPointIsAnchor ? index + 1 : index - 1;

                if (correspondingControlIndex >= 0 && correspondingControlIndex < NumPoints)
                {
                    var dist = (_points[anchorIndex] - _points[correspondingControlIndex]).magnitude;
                    var dir = (_points[anchorIndex] - newPos).normalized;
                    _points[correspondingControlIndex] = _points[anchorIndex] + dir * dist;
                }
            }
            
            OnChanged?.Invoke();
        }

        private (int, float) InterpolateSegment(float t)
        {
            var segIndex = (int)(t * NumSegments);
            var segT = (t - (float)segIndex / NumSegments) * NumSegments;

            if (segIndex == NumSegments)
            {
                segIndex = NumSegments - 1;
                segT = 1;
            }

            return (segIndex, segT);
        }

        public float CalculateLengthTotal()
        {
            var totalLength = 0f;
            for (int i = 0; i < NumSegments; i++)
            {
                totalLength += CalculateLengthSegment(i);
            }

            return totalLength;
        }

        public float CalculateLengthSegment(int segment)
        {
            var points = GetPointsInSegment(segment);
            var length = 0f;
            for (int i = 0; i < SegmentSubdivision-1; i++)
            {
                var t0 = (float)i / (SegmentSubdivision - 1);
                var t1 = (float)(i + 1) / (SegmentSubdivision - 1);
                var p0 = CubicCurve(points[0], points[1], points[2], points[3], t0);
                var p1 = CubicCurve(points[0], points[1], points[2], points[3], t1);
                length += Vector3.Distance(p0, p1);
            }

            return length;
        }
        
        /// <returns>Interpolated point and t for interpolation</returns>
        public (Vector3, float) LengthInterpolate(float t)
        {
            var totalLength = CalculateLengthTotal();
            var lengthToGo = totalLength * t;
            var lengthGone = 0f;
            for (int segIndex = 0; segIndex < NumSegments; segIndex++)
            {
                var nextLength = CalculateLengthSegment(segIndex);
                if ((lengthGone + nextLength) >= lengthToGo)
                {
                    var points = GetPointsInSegment(segIndex);
                    for (int segSubIndex = 0; segSubIndex < SegmentSubdivision-1; segSubIndex++)
                    {
                        var t0 = (float)segSubIndex / (SegmentSubdivision - 1);
                        var t1 = (float)(segSubIndex + 1) / (SegmentSubdivision - 1);
                        var p0 = CubicCurve(points[0], points[1], points[2], points[3], t0);
                        var p1 = CubicCurve(points[0], points[1], points[2], points[3], t1);
                        var nextSubLength = Vector3.Distance(p0, p1); 
                        if ((lengthGone + nextSubLength) >= lengthToGo)
                        {
                            return (p0, (float)(segIndex+t0)/NumSegments);
                        }

                        lengthGone += nextSubLength;
                    }
                }

                lengthGone += nextLength;
            }

            return (Interpolate(1), 1);
        }
        
        public Vector3 Interpolate(float t)
        {
            var (segIndex, segT) = InterpolateSegment(t);
            var segment = GetPointsInSegment(segIndex);
            return CubicCurve(segment[0], segment[1], segment[2], segment[3], segT);
        }

        public Vector3 InterpolateDerivative(float t)
        {
            var (segIndex, segT) = InterpolateSegment(t);
            var segment = GetPointsInSegment(segIndex);
            return CubicCurveDerivative(segment[0], segment[1], segment[2], segment[3], segT);
        }
        
        public static Vector3 QuadraticCurve(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            var p0 = Vector3.Lerp(a, b, t);
            var p1 = Vector3.Lerp(b, c, t);
            return Vector3.Lerp(p0, p1, t);
        }

        public static Vector3 CubicCurve(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            var p0 = QuadraticCurve(a, b, c, t);
            var p1 = QuadraticCurve(b, c, d, t);
            return Vector3.Lerp(p0, p1, t);
        }

        public static Vector3 CubicCurveDerivative(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            return 3 * (1 - t) * (1 - t) * (b - a) + 6 * (1 - t) * t * (c - b) + 3 * t * t * (d - c);
        }
    }
}
