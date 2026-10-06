using PhysicBreakout;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Audio;

public class Ball : MonoBehaviour
{
    public Vector3 m_MoveDirection = Vector3.forward;
    public float m_MoveSpeed = 40;
    public Transform m_Base;
    public Transform m_RollBase;

    public bool m_IsStoped = false;
    public bool m_OnSpeed = false;
    [SerializeField] protected PlaySfxEvent playSfxEvent;
    [SerializeField] protected SoundData soundDummyData;
    [SerializeField] protected SoundData soundPlayerData;
    public event Action<Ball> OnOut;
    // Start is called before the first frame update
    public static Ball m_Main;
    private int ballHitMask;

    private void Awake()
    {
        m_IsStoped = false;
        ballHitMask = Physics.DefaultRaycastLayers;
        int blockBreakLayer = LayerMask.NameToLayer("BlockBreak");
        if (blockBreakLayer >= 0)
            ballHitMask &= ~(1 << blockBreakLayer);

        //m_Main = this;
        Debug.Log("Tạo bóng");
    }
    void Start()
    {
        m_MoveDirection.Normalize();
        m_OnSpeed = false;
        Debug.Log("Sinh ra bóng");
    }
    // Update is called once per frame
    void Update()
    {
        UpdateMove();
    }
    public void UpdateMove()
    {
        if (m_IsStoped)
            return;
        //m_MoveSpeed * Time.deltaTime
        RaycastHit[] hits = Physics.SphereCastAll(
            transform.position,
            .5f,
            m_MoveDirection,
            1f,
            ballHitMask
        );
        HashSet<IDamageable> damagedTargets = new HashSet<IDamageable>();
        foreach (RaycastHit hit in hits)
        {
            Collider col = hit.collider;
            IDamageable damageable = col.GetComponentInParent<IDamageable>();
            if (damageable != null && damagedTargets.Add(damageable))
                damageable.TakeHit();
            bool doReflect = false;
            if (col.gameObject.tag == "Player")
            {
                doReflect = true;
                playSfxEvent.Raise(soundPlayerData);
            }
            else if (col.gameObject.tag == "Block")
            {
                Impulse();
                doReflect = true;
            }
            else if (col.gameObject.tag == "Explosive")
            {
                Impulse();
                doReflect = true;
            }
            else if (col.gameObject.tag == "Dummy")
            {
                Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 forceDir = 5 * m_MoveDirection;
                    rb.AddForceAtPosition(forceDir, col.gameObject.transform.position + new Vector3(0, 2, 0), ForceMode.VelocityChange);
                    rb.angularVelocity = new Vector3(0, UnityEngine.Random.Range(-20f, 20f), 0);
                }
                playSfxEvent.Raise(soundDummyData);
                doReflect = true;
            }
            else if (col.gameObject.tag == "Wall")
            {
                doReflect = true;
            }

            if (doReflect)
            {
                CameraControl.m_Main.StartShake(.3f, .3f);

                Vector3 normal = hit.normal;
                if (hit.distance <= 0)
                {
                    float delta = .25f - hit.distance;
                    Vector3 pos = transform.position - hit.collider.gameObject.transform.position;
                    pos.y = 0;
                    pos.Normalize();
                    transform.position += Time.deltaTime * pos;
                }
                else
                {
                    m_MoveDirection = Vector3.Reflect(m_MoveDirection, normal);
                    m_MoveDirection.y = 0;
                    m_MoveDirection.Normalize();
                }
                break;
            }
        }

        if (Mathf.Abs(m_MoveDirection.z) <= .1f)
        {
            m_MoveDirection.z = .3f;
            m_MoveDirection.Normalize();
        }

        m_Base.forward = m_MoveDirection;

        m_MoveSpeed = 16;
        if (m_OnSpeed)
            m_MoveSpeed = 30;

        m_RollBase.localRotation = Quaternion.Euler(Time.deltaTime * m_MoveSpeed * 62f, 0, 0) * m_RollBase.localRotation;

        transform.position += m_MoveSpeed * Time.deltaTime * m_MoveDirection;

        Vector3 pos1 = transform.position;
        pos1.x = Mathf.Clamp(pos1.x, -8f, 8f);
        pos1.z = Mathf.Clamp(pos1.z, -24f, 21f);
        transform.position = pos1;
        if (transform.position.z < Player.m_Main.transform.position.z - 2)
        {
            Debug.Log("Biến mất");
            OnOut?.Invoke(this);
            Destroy(gameObject);
        }
        
    }

    public void Impulse()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, 5);
        foreach (Collider c in colls)
        {
            Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDir = rb.transform.position - transform.position;
                forceDir = forceDir.normalized;
                rb.AddForce(4 * forceDir, ForceMode.VelocityChange);
            }
        }
    }

    public void StartSpeed()
    {
        if (m_OnSpeed)
        {
            StopAllCoroutines();
        }

        StartCoroutine(Co_StartSpeed());
    }

    IEnumerator Co_StartSpeed()
    {
        m_OnSpeed = true;
        yield return new WaitForSeconds(5);
        m_OnSpeed = false;
    }

}
