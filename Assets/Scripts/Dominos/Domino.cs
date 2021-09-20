using System;
using UnityEngine;

namespace Dominos
{
    public class Domino : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnMouseDown()
        {
            _rigidbody.AddRelativeTorque(Vector3.forward * 100f);
        }
    }
}
