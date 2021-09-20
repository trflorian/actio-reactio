using System;
using Bezier;
using UnityEngine;

namespace Dominos
{
    public class DominoSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject dominoPrefab;
        
        private Path _path;

        public void SetPath(Path path)
        {
            _path = path;
        }
        
        public void SpawnDominos(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var lt = (float)i / (amount - 1);

                var (p, t) = _path.LengthInterpolate(lt);
                var pos= p;
                var dir = _path.InterpolateDerivative(t);

                var go = Instantiate(dominoPrefab, pos, Quaternion.identity, transform);
                go.transform.right = dir;
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                SpawnDominos(50);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                foreach (Transform trf in transform)
                {
                    Destroy(trf.gameObject);
                }
            }
        }
    }
}
