namespace Application.Core.Utility
{
    using UnityEngine;

    /// <summary>
    /// Utility methods for working with raycasts.
    /// </summary>
    public static class RaycastUtil
    {
        public static GameObject GetGameObject(this RaycastHit hit)
        {
            return hit.rigidbody != null ? hit.rigidbody.gameObject : hit.collider.gameObject;
        }
    }
}
