using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace PhysicBreakout
{
    public class InGameUI : MonoBehaviour
    {
        public DataStorage m_DataStorage;

        public static InGameUI m_Current;

        [SerializeField]
        private Text m_Level;
        [SerializeField]
        private Text m_Score;
        [SerializeField]
        private Text m_BlockCount;


        void Awake()
        {
            m_Current = this;
        }
        void Start()
        {

        }

        private void Update()
        {
            //m_Score.text = "Score : 0";
            m_BlockCount.text = "Blocks : " + GameControl.m_Main.m_BlockBreakCount + " / " + GameControl.m_Main.m_MaxBlockCount;
            m_Level.text = "Level " + (m_DataStorage.m_LevelNum + 1).ToString();


        }

    }
}