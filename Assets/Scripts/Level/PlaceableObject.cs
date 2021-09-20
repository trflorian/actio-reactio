using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace Level
{
    public class PlaceableObject : MonoBehaviour
    {
        private const int RotationSteps = 8;
        
        public PlaceableObjectSO objectData;
        
        private Camera _mainCamera;

        private bool _rotating;

        private bool _startRotating;
        private float _rotationStartEuler, _rotationStartScreen;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void OnMouseDown()
        {
            _rotating = Input.GetKey(KeyCode.LeftControl);
            _startRotating = false;

            if (_rotating)
            {
                var rot = transform.rotation.eulerAngles;
                var invert = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                var step = 360f / RotationSteps;
                if (invert) step *= -1; 
                rot.y += step;
                rot.y -= rot.y % step; 
                transform.rotation = Quaternion.Euler(rot);
            }
        }

        private void OnMouseDrag()
        {
            if (Simulation.SimulationController.Instance.isSimulationRunning) return;
            if (LevelManager.Instance.IsGoalReached()) return;
            
            if (_rotating)
            {
                //RotateObject();
            }
            else
            {
                MoveObject();
            }
        }

        private void RotateObject()
        {
            var originPos = _mainCamera.WorldToScreenPoint(transform.position);

            var originPos2d = new Vector2(originPos.x, originPos.y);
            var mousePos2D = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            var delta = originPos2d - mousePos2D;
            
            var rotY = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
            var rot = transform.rotation.eulerAngles;

            if (!_startRotating)
            {
                _startRotating = true;
                _rotationStartEuler = rot.y;
                _rotationStartScreen = rotY;
            }

            rotY -= _rotationStartScreen;
            
            rot.y = _rotationStartEuler - rotY;
            transform.rotation = Quaternion.Euler(rot);
        }

        private void MoveObject()
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 1000, LayerMask.GetMask("PlaceArea")))
            {
                var pivot = hitInfo.point + objectData.spawnYOffset * Vector3.up;
                gameObject.layer = 0;
                if (!Physics.CheckSphere(pivot, objectData.spawnProofRadius,
                    LayerMask.GetMask("PlaceableObjects", "FixedObjects")))
                {
                    transform.position = pivot;
                }

                gameObject.layer = LayerMask.NameToLayer("PlaceableObjects");
            }
        }
    }
}
