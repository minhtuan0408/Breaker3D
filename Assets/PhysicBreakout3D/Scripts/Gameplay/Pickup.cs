using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PhysicBreakout
{
    public class Pickup : MonoBehaviour
    {
        public GameObject m_ParticlePrefab1;
        public int m_ParticleType = 0;
        //public bool m_StartedMove;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (GameControl.m_Main.m_MainBall != null)
            {
                if (Vector3.Distance(transform.position, Ball.m_Main.transform.position) <= 1.5f)
                {
                    GameObject obj = Instantiate(m_ParticlePrefab1);
                    obj.transform.position = transform.position;
                    Destroy(obj, 3);

                    switch (m_ParticleType)
                    {
                        case 0:
                            Ball.m_Main.StartSpeed();
                            break;

                        case 1:
                            GameControl.m_Main.GenerateBalls();
                            break;
                    }

                    Destroy(gameObject);
                }
            }
        }
    }
}
