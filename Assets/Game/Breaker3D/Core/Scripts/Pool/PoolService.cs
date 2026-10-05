using UnityEngine;
using VirtueSky.ObjectPooling;

namespace Breaker3D.Core.Pooling
{
    /// <summary>
    /// Shared entry point for pooling project prefabs such as particles, balls, and blocks.
    /// </summary>
    public static class PoolService
    {
        private static bool isInitialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeBeforeSceneLoad()
        {
            EnsureInitialized();
        }

        public static GameObject Spawn(
            GameObject prefab,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null,
            bool initialize = true)
        {
            if (prefab == null)
            {
                Debug.LogError("Cannot spawn a null prefab from the pool.");
                return null;
            }

            EnsureInitialized();
            return prefab.Spawn(position, rotation, parent, initialize: initialize);
        }

        public static T Spawn<T>(
            T prefab,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null,
            bool initialize = true) where T : Component
        {
            if (prefab == null)
            {
                Debug.LogError("Cannot spawn a null component prefab from the pool.");
                return null;
            }

            EnsureInitialized();
            return prefab.Spawn(position, rotation, parent, initialize: initialize);
        }

        public static void Return(GameObject instance, bool destroy = false)
        {
            if (instance == null)
                return;

            EnsureInitialized();
            instance.DeSpawn(destroy);
        }

        public static void Return(Component instance, bool destroy = false)
        {
            if (instance == null)
                return;

            Return(instance.gameObject, destroy);
        }

        public static void Prewarm(GameObject prefab, int count)
        {
            if (prefab == null)
            {
                Debug.LogError("Cannot prewarm a null prefab.");
                return;
            }

            if (count <= 0)
                return;

            EnsureInitialized();
            new PoolData { prefab = prefab, count = count }.PreSpawn();
        }

        public static void ReturnAll()
        {
            EnsureInitialized();
            Pool.DeSpawnAll();
        }

        private static void EnsureInitialized()
        {
            if (isInitialized)
                return;

            Pool.InitPool();
            isInitialized = true;
        }
    }
}
