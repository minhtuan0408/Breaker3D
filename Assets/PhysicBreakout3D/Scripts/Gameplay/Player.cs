using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PhysicBreakout
{
    public class Player : MonoBehaviour
    {
        private Vector3 m_MoveVector;
        private bool m_ControlEnabled;
        public static Player m_Main;

        void Awake()
        {
            m_Main = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_ControlEnabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            m_MoveVector = Vector3.zero;
            if (m_ControlEnabled)
            {


                    m_MoveVector += InputControl.m_Main.m_Movement.x * Vector3.right;
              
                //m_MoveVector.Normalize();

            }

            float speed = 20;
            Vector3 pos = transform.position;
            pos += speed * Time.deltaTime * m_MoveVector;
            pos.x = Mathf.Clamp(pos.x, -6.3f, 6.3f);
            transform.position = pos;
        }
    }
}