using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PhysicBreakout
{
    public class DownForce : MonoBehaviour
    {
        Rigidbody m_RigidBody;
        // Start is called before the first frame update
        void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            m_RigidBody.AddForceAtPosition(.5f * Vector3.down, transform.position + transform.rotation * new Vector3(0, -1, 0), ForceMode.VelocityChange);
            m_RigidBody.AddForceAtPosition(.5f * Vector3.up, transform.position, ForceMode.VelocityChange);
        }
    }
}