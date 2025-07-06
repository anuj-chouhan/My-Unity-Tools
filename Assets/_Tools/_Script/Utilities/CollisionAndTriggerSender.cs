using System;
using UnityEngine;

namespace Anuj.Utility.Physics
{
    public class CollisionAndTriggerSender : MonoBehaviour
    {
        public event Action<Collider> _OnTriggerEnter;
        public event Action<Collider> _OnTriggerStay;
        public event Action<Collider> _OnTriggerExit;
        public event Action<Collision> _OnCollisionEnter;
        public event Action<Collision> _OnCollisionStay;
        public event Action<Collision> _OnCollisionExit;
        private void OnTriggerEnter(Collider other)
        {
            _OnTriggerEnter?.Invoke(other);
        }
        private void OnTriggerStay(Collider other)
        {
            _OnTriggerStay?.Invoke(other);
        }
        private void OnTriggerExit(Collider other)
        {
            _OnTriggerExit?.Invoke(other);
        }
        private void OnCollisionEnter(Collision collision)
        {
            _OnCollisionEnter?.Invoke(collision);
        }
        private void OnCollisionStay(Collision collision)
        {
            _OnCollisionStay?.Invoke(collision);
        }
        private void OnCollisionExit(Collision collision)
        {
            _OnCollisionExit?.Invoke(collision);
        }
    }

}