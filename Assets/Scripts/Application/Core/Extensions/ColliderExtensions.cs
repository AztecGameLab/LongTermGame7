using UnityEngine;

namespace Application.Core
{
    public static class ColliderExtensions
    {
        public static GameObject GetRoot(this Collider collider)
        {
            if (collider.attachedRigidbody != null)
                return collider.attachedRigidbody.gameObject;

            return collider.gameObject;
        }
    }
}
