using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace PhysicBreakout
{
    public class ExplosiveBlock : MonoBehaviour
    {
        public GameObject m_ParticlePrefab1;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BreakBlock()
        {
            GameObject obj = Instantiate(m_ParticlePrefab1);
            obj.transform.position = transform.position;
            Destroy(obj, 3);

            Explode();

            Destroy(gameObject);
        }

        public void Explode()
        {
            Collider[] colls = Physics.OverlapSphere(transform.position, 3);
            foreach (Collider c in colls)
            {
                if (c.gameObject == gameObject)
                    continue;
                Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 forceDir = rb.transform.position - transform.position;
                    forceDir = forceDir.normalized;
                    rb.AddForce(4 * forceDir, ForceMode.VelocityChange);
                }

                if (c.gameObject.tag == "Block")
                {
                    Block block = c.gameObject.GetComponent<Block>();
                    block.BreakBlock();
                }
                else if (c.gameObject.tag == "Explosive")
                {
                    ExplosiveBlock block = c.gameObject.GetComponent<ExplosiveBlock>();
                    block.BreakBlock();
                }
            }
        }
    }
}
