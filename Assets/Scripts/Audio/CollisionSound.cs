using System;
using UnityEngine;

namespace Audio
{
    public class CollisionSound : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            var totalAngularVelocity = 0f;
            if (other.rigidbody) totalAngularVelocity += other.rigidbody.angularVelocity.magnitude;
            if (TryGetComponent(out Rigidbody r))
            {
                totalAngularVelocity += r.angularVelocity.magnitude;
            }
            SFXManager.Instance.PlayCollisionSound(other.relativeVelocity.magnitude + totalAngularVelocity/10f, 
                gameObject, other.gameObject);
        }
    }
}
