using System;
using UnityEngine;
using Breaker3D.Core.Pooling;
using VirtueSky.Audio;

public class BlockBase : MonoBehaviour, IDamageable
{
    public string stringParticle;
    public event Action OnBreak;

    [SerializeField] protected PlaySfxEvent playSfxEvent;
    [SerializeField] protected SoundData soundData;
    public virtual void BreakBlock()
    {
        OnBreak?.Invoke();
        gameObject.SetActive(false);
    }

    public virtual void TakeHit()
    {
        if (!string.IsNullOrWhiteSpace(stringParticle) && PoolManager.Instance != null)
            PoolManager.Instance.Spawn(stringParticle, transform.position, Quaternion.identity);
        playSfxEvent.Raise(soundData);
        BreakBlock();
    }
}
