using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PhysicBreakout
{
    public class CameraControl : MonoBehaviour
    {
        public static CameraControl m_Main;
        private float m_ShakeTimer;
        private float m_ShakeArc;
        private float m_ShakeRadius = 1;

        Vector3 m_InitPosition;
        void Awake()
        {
            m_Main = this;

        }
        void Start()
        {
            m_InitPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            m_ShakeTimer -= Time.deltaTime;
            //ShakeArc += 100 * Time.deltaTime;

            if (m_ShakeTimer <= 0)
                m_ShakeTimer = 0;

            Vector3 ShakeOffset = Vector3.zero;
            float shakeSin = Mathf.Cos(30 * Time.time) * Mathf.Clamp(m_ShakeTimer, 0, 0.5f);
            float shakeCos = Mathf.Sin(50 * Time.time) * Mathf.Clamp(m_ShakeTimer, 0, 0.5f);
            ShakeOffset = new Vector3(m_ShakeRadius * shakeCos, m_ShakeRadius * shakeSin, 0);

            if (GameControl.m_Main.m_GameState == 1)
            {
                transform.position = m_InitPosition + new Vector3(.2f * Player.m_Main.transform.position.x, 0, .2f * Ball.m_Main.transform.position.z);
                transform.position += ShakeOffset;
            }
        }

        public void StartShake(float t, float r)
        {
            if (m_ShakeTimer == 0 || m_ShakeRadius < r)
                m_ShakeRadius = r;

            m_ShakeTimer = t;
        }
    }
}