namespace Application.Core.Utility
{
    using UnityEngine;

    public static class GetComponentUtils
    {
        public static bool TryGetComponentParents<T>(this Component component, out T result)
            where T : class
        {
            result = component.GetComponentInParent<T>();
            return result != null;
        }
    }
}
