using UnityEngine;

namespace Breaker3D.Core.Pooling
{
    [System.Serializable]
    public class PoolEntry
    {
        [Tooltip("Stable ID used to find this pool in code, for example: particle.block.normal")]
        public string id;

        public GameObject prefab;

        [Min(0)]
        [Tooltip("Number of instances to create when the Pool Manager starts. Zero means create on demand.")]
        public int prewarmCount;
    }
}
