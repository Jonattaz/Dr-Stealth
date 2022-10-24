using UnityEngine;

namespace Mz.App
{
    public static class UnityExtensions
    {
        public static Transform Clear(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            return transform;
        }
    }
}
