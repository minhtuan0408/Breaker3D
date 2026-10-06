using Breaker3D.Core.Pooling;
using System.Collections;
using UnityEngine;
using VirtueSky.Audio;

public class Cow : BlockBase
{
    [SerializeField] private float upwardForce = 2.5f;
    [SerializeField] private float sidewaysForce = 0.5f;
    [SerializeField] private float despawnDelay = 0.3f;

    private int blockActiveLayer;
    private int blockBreakLayer;
    private Rigidbody rb;
    private bool hasBeenHit;

    private void Awake()
    {
        blockActiveLayer = LayerMask.NameToLayer("BlockActive");
        blockBreakLayer = LayerMask.NameToLayer("BlockBreak");
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        hasBeenHit = false;
        if (blockActiveLayer >= 0)
            gameObject.layer = blockActiveLayer;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if (gameObject.layer != blockActiveLayer)
            return;
        rb.AddForceAtPosition(.5f * Vector3.down, transform.position + transform.rotation * new Vector3(0, -1, 0), ForceMode.VelocityChange);
        rb.AddForceAtPosition(.5f * Vector3.up, transform.position, ForceMode.VelocityChange);
    }
    public override void TakeHit()
    {
        if (hasBeenHit)
            return;

        hasBeenHit = true;
        if (blockBreakLayer >= 0)
            gameObject.layer = blockBreakLayer;

        playSfxEvent.Raise(soundData);
        if (!string.IsNullOrWhiteSpace(stringParticle) && PoolManager.Instance != null)
            PoolManager.Instance.Spawn(stringParticle, transform.position, Quaternion.identity);

        StartCoroutine(BreakAtion());
    }

    private IEnumerator BreakAtion()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = new Vector3(
            sidewaysForce,
            upwardForce,
            0f
        );

        rb.isKinematic = false;
        rb.useGravity = true;

        yield return new WaitForSeconds(despawnDelay);
        base.BreakBlock();
    }
}
