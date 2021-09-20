using System;
using Dominos;
using UnityEngine;

namespace Bezier
{
    /// <summary>
    /// Creates the path object for a bezier curve at its origin
    /// </summary>
    public class PathCreator : MonoBehaviour
    {
        [SerializeField] private PathRenderer pathRenderer;
        [SerializeField] private DominoSpawner dominoSpawner;
        [SerializeField] private GameObject anchorHandlePrefab;

        private Path _path;

        private void Awake()
        {
            CreatePath();
        }

        private void Start()
        {
            pathRenderer.SetPath(_path);
            dominoSpawner.SetPath(_path);
        }

        public void CreatePath()
        {
            _path = new Path();
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hitInfo))
                {
                    if (hitInfo.transform.CompareTag("Ground"))
                    {
                        var seg = _path.AddSegment(hitInfo.point);
                        if (seg == -1)
                        {
                            Instantiate(anchorHandlePrefab).GetComponent<AnchorHandle>().Setup(_path, 0, 0);
                        }
                        else
                        {
                            Instantiate(anchorHandlePrefab).GetComponent<AnchorHandle>().Setup(_path, seg, 1);
                            Instantiate(anchorHandlePrefab).GetComponent<AnchorHandle>().Setup(_path, seg, 2);
                            Instantiate(anchorHandlePrefab).GetComponent<AnchorHandle>().Setup(_path, seg, 3);
                        }
                    }
                }
            }
        }
    }
}
