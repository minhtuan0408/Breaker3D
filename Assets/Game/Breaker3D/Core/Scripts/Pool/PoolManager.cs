using System;
using System.Collections.Generic;
using UnityEngine;

namespace Breaker3D.Core.Pooling
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        [SerializeField] private List<PoolEntry> entries = new List<PoolEntry>();

        private readonly Dictionary<string, PoolEntry> entriesById =
            new Dictionary<string, PoolEntry>(StringComparer.OrdinalIgnoreCase);

        private bool lookupBuilt;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            BuildLookup();

            foreach (PoolEntry entry in entries)
            {
                if (entry != null && entry.prefab != null && entry.prewarmCount > 0)
                    PoolService.Prewarm(entry.prefab, entry.prewarmCount);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        public GameObject Spawn(
            string id,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null)
        {
            if (!TryGetEntry(id, out PoolEntry entry))
            {
                Debug.LogError($"Pool ID '{id}' was not found on {name}.", this);
                return null;
            }

            return PoolService.Spawn(entry.prefab, position, rotation, parent);
        }

        public T Spawn<T>(
            string id,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null) where T : Component
        {
            GameObject instance = Spawn(id, position, rotation, parent);
            if (instance == null)
                return null;

            T component = instance.GetComponent<T>();
            if (component == null)
                Debug.LogError($"Pool '{id}' prefab does not contain {typeof(T).Name}.", instance);

            return component;
        }

        public bool TrySpawn(
            string id,
            Vector3 position,
            Quaternion rotation,
            out GameObject instance,
            Transform parent = null)
        {
            instance = null;
            if (!TryGetEntry(id, out PoolEntry entry))
                return false;

            instance = PoolService.Spawn(entry.prefab, position, rotation, parent);
            return instance != null;
        }

        public void Return(GameObject instance, bool destroy = false)
        {
            PoolService.Return(instance, destroy);
        }

        private bool TryGetEntry(string id, out PoolEntry entry)
        {
            if (!lookupBuilt)
                BuildLookup();

            entry = null;
            if (string.IsNullOrWhiteSpace(id))
                return false;

            return entriesById.TryGetValue(id.Trim(), out entry);
        }

        private void BuildLookup()
        {
            entriesById.Clear();

            foreach (PoolEntry entry in entries)
            {
                if (entry == null || string.IsNullOrWhiteSpace(entry.id))
                    continue;

                string key = entry.id.Trim();
                if (entriesById.ContainsKey(key))
                {
                    Debug.LogError($"Duplicate pool ID '{key}' on {name}; IDs must be unique.", this);
                    continue;
                }

                if (entry.prefab == null)
                {
                    Debug.LogError($"Pool entry '{key}' has no prefab assigned on {name}.", this);
                    continue;
                }

                entriesById.Add(key, entry);
            }

            lookupBuilt = true;
        }
    }
}
