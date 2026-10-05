using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PhysicBreakout
{
    public class Block : MonoBehaviour
    {
        public GameObject m_ParticlePrefab1;

        // Start is called before the first frame update
        void Start()
        {
            GameControl.m_Main.m_MaxBlockCount++;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BreakBlock()
        {
            GameControl.m_Main.m_BlockBreakCount++;

            GameObject obj = Instantiate(m_ParticlePrefab1);
            obj.transform.position = transform.position;
            Destroy(obj, 3);

            Destroy(gameObject);
        }
    }
}
