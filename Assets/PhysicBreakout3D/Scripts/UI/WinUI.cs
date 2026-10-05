using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace PhysicBreakout
{
    public class WinUI : MonoBehaviour
    {

        public DataStorage m_DataStorage;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BtnContinue()
        {
            m_DataStorage.m_LevelNum++;
            if (m_DataStorage.m_LevelNum > 6)
            {
                m_DataStorage.m_LevelNum = 0;
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}