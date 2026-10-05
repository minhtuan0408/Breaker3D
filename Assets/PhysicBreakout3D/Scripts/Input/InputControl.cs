using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PhysicBreakout
{
    public class InputControl : MonoBehaviour
    {

        //--inputs
        [HideInInspector]
        public Vector3 m_Movement;

        [HideInInspector]
        public Vector3 m_LastTouchPos;
        [HideInInspector]
        public bool m_LastTouch = false;

        public bool m_mobileControl = false;

        public static InputControl m_Main;

        void Awake()
        {
            m_Main = this;
        }
        // Start is called before the first frame update
        void Start()
        {

            Vector3 m_LastTouchPos = Vector3.zero;

        }

        // Update is called once per frame
        void Update()
        {
            m_Movement = Vector3.zero;

            if (m_mobileControl)
            {
                if (!m_LastTouch)
                {
                    if (Input.touchCount == 1)
                    {
                        m_LastTouchPos = new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, 0);
                        m_LastTouch = true;
                    }
                }
                else
                {
                    m_Movement = new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, 0) - m_LastTouchPos;
                    m_Movement = .06f * m_Movement;
                    m_Movement = Vector3.ClampMagnitude(m_Movement, 1);
                    m_LastTouchPos = Vector3.Lerp(m_LastTouchPos, new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, 0), 10 * Time.deltaTime);
                    if (Input.touchCount == 0)
                        m_LastTouch = false;
                }
            }
            else
            {
                if (!m_LastTouch)
                {
                    if (Input.GetMouseButton(0))
                    {
                        m_LastTouchPos = Input.mousePosition;
                        m_LastTouch = true;
                    }
                }
                else
                {
                    m_Movement = Input.mousePosition - m_LastTouchPos;
                    m_Movement = .06f * m_Movement;
                    m_Movement = Vector3.ClampMagnitude(m_Movement, 1);
                    m_LastTouchPos = Vector3.Lerp(m_LastTouchPos, Input.mousePosition, 10 * Time.deltaTime);
                    if (!Input.GetMouseButton(0))
                        m_LastTouch = false;
                }

            }

            m_Movement = Vector3.ClampMagnitude(m_Movement, 1.0f);
        }
    }
}