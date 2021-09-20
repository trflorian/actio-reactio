using System;
using System.Collections.Generic;
using System.Linq;
using Simulation;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Level
{
    public class ObjectPlacer : MonoBehaviour
    {
        public static LevelInfo Inventory;

        [SerializeField] private AudioClip place, remove, unavailable;
        
        private Camera _mainCamera;
        private AudioSource _audioSource;
        
        private PlaceableObjectSO _selectedObjectSo;

        private Transform _previousPlacedTransform;
        private Quaternion _previousPlacedRotation;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _audioSource = GetComponent<AudioSource>();
            
            _previousPlacedRotation = Quaternion.identity;
        }

        private void Start()
        {
            Inventory = ScriptableObject.CreateInstance<LevelInfo>();
            Inventory.objects = LevelManager.Instance.levelInfo.objects.Select(p => new LevelInfo.LevelObjectInfo
            {
                placeableObject = p.placeableObject,
                amount = p.amount
            }).ToList();
            
            _selectedObjectSo = Inventory.objects[0].placeableObject;

            PlaceableObjectUI.OnSelectPlaceableObject += OnSelectObject;
        }

        private void OnDestroy()
        {
            PlaceableObjectUI.OnSelectPlaceableObject -= OnSelectObject;
        }

        private void OnSelectObject(PlaceableObjectSO po)
        {
            _selectedObjectSo = po;
        }

        private void Update()
        {
            if (SimulationController.Instance.isSimulationRunning) return;
            if (LevelManager.Instance.IsGoalReached()) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            var lmb = Input.GetMouseButtonDown(0);
            var rmb = Input.GetMouseButton(1);
            if (lmb)
            {
                if (_selectedObjectSo != null)
                {
                    if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out var raycastHit, 1000,
                        LayerMask.GetMask("PlaceArea", "PlaceableObjects")))
                    {
                        // if an object is clicked, move it around instead of doing other stuff
                        if (raycastHit.transform.gameObject.layer != LayerMask.NameToLayer("PlaceableObjects"))
                        {
                            var pivot = raycastHit.point + _selectedObjectSo.spawnYOffset * Vector3.up;
                            if (!Physics.CheckSphere(pivot, _selectedObjectSo.spawnProofRadius,
                                    LayerMask.GetMask("PlaceableObjects", "FixedObjects")) &&
                                Inventory.objects.FirstOrDefault(o => o.placeableObject == _selectedObjectSo).amount >
                                0)
                            {
                                if (_previousPlacedTransform != null)
                                {
                                    _previousPlacedRotation = _previousPlacedTransform.rotation;
                                }

                                var newRotation = _selectedObjectSo.type == PlaceableObjectSO.Type.Dominos
                                    ? AlignNewObject(pivot)
                                    : Quaternion.identity;
                                
                                var spawnedObj = Instantiate(_selectedObjectSo.prefab,
                                    pivot, Quaternion.Euler(_selectedObjectSo.initialRotation + newRotation.eulerAngles),
                                    transform);
                                spawnedObj.AddComponent<PlaceableObject>().objectData = _selectedObjectSo;
                                Inventory.objects.FirstOrDefault(o => o.placeableObject == _selectedObjectSo).amount--;

                                _previousPlacedTransform = spawnedObj.transform;
                                
                                _audioSource.pitch = Random.Range(0.8f, 1.2f);
                                _audioSource.PlayOneShot(place);
                            }
                            else
                            {
                                _audioSource.pitch = 1.2f;
                                _audioSource.PlayOneShot(unavailable);
                            }
                        }
                    }
                }
            } 
            else if (rmb)
            {
                if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out var raycastHit, 1000,
                    LayerMask.GetMask("PlaceableObjects")))
                {
                    _previousPlacedTransform = null;
                    _previousPlacedRotation = raycastHit.transform.rotation;
                    
                    var placeableObject = raycastHit.transform.GetComponentInParent<PlaceableObject>();
                    Inventory.objects.FirstOrDefault(o => o.placeableObject == placeableObject.objectData).amount++;
                    _selectedObjectSo = placeableObject.objectData;
                    Destroy(placeableObject.gameObject);

                    _audioSource.pitch = Random.Range(0.8f, 1.2f);
                    _audioSource.PlayOneShot(remove);
                }
            }
        }

        private Quaternion AlignNewObject(Vector3 pivot)
        {
            var levelObjects = FindObjectsOfType<RigidbodyLevelObject>();
            
            // Find nearest object
            var nearestObj = levelObjects
                .OrderBy(p => Vector3.Distance(p.transform.position, pivot))
                .FirstOrDefault();

            if (nearestObj == null) return _previousPlacedRotation;

            var dir = nearestObj.transform.position - pivot;

            // if nearest target still too far away
            if (dir.magnitude > 3) return _previousPlacedRotation;

            // Search in other direction for similiar distant object
            var objectInOtherDirection = levelObjects.Where(p => Vector3.Dot(p.transform.position - pivot, dir) < 0 &&
                                                               Mathf.Abs((p.transform.position - pivot).magnitude - dir.magnitude) <=
                                                               2)
                .OrderBy(p => Vector3.Distance(p.transform.position, pivot))
                .FirstOrDefault();

            Vector3 dir2;
            if (objectInOtherDirection == null)
            {
                dir2 = dir;
            }
            else
            {
                dir2 = -(objectInOtherDirection.transform.position - pivot);
            }

            // Average of the two directions
            var dirAvg = (dir + dir2) / 2f;

            var angle = -Mathf.Atan2(dirAvg.z, dirAvg.x) * Mathf.Rad2Deg;
            
            return Quaternion.Euler(0, angle, 0);
        }
    }
}
