using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PhysicBreakout
{
    public class UISystem : MonoBehaviour
    {
        public GameObject m_GameUI;
        public GameObject m_WinUI;
        public GameObject m_LoseUI;
        public GameObject m_MainMenuUI;

        public static UISystem m_Main;

        private void Awake()
        {
            m_Main = this;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}