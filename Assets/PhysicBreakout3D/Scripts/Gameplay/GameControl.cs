using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace PhysicBreakout
{
    public class GameControl : MonoBehaviour
    {
        public static GameControl m_Main;
        public DataStorage m_DataStorage;

        [HideInInspector]
        public int m_MaxBlockCount;
        [HideInInspector]
        public int m_BlockBreakCount;
        [HideInInspector]
        public int m_GameState = 0;

        [HideInInspector]
        public Ball m_MainBall;

        public GameObject m_BallPrefab;
        public GameObject m_BallPrefab2;

        public GameObject[] m_LevelPrefabs;
        void Awake()
        {
            m_Main = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_MaxBlockCount = 0;
            m_BlockBreakCount = 0;

            int levelNum = m_DataStorage.m_LevelNum;
            if (levelNum > m_LevelPrefabs.Length - 1)
            {
                levelNum = m_LevelPrefabs.Length - 1;
                m_DataStorage.m_LevelNum = levelNum;
            }
            else if (levelNum<0)
            {
                levelNum = 0;
                m_DataStorage.m_LevelNum = 0;
            }

            if (m_LevelPrefabs[levelNum] != null)
            {
                GameObject level = Instantiate(m_LevelPrefabs[levelNum]);
                level.transform.position = Vector3.zero;
            }

            m_GameState = 0;

            StartCoroutine(Co_StartGame());
        }

        IEnumerator Co_StartGame()
        {
            m_GameState = 0;

            yield return new WaitForSeconds(1);

            GameObject ball = Instantiate(m_BallPrefab);
            m_MainBall = ball.GetComponent<Ball>();
            Ball.m_Main = m_MainBall;
            m_MainBall.transform.position = Player.m_Main.transform.position + new Vector3(0, 0, 3);
            m_MainBall.m_MoveDirection = Quaternion.Euler(0, Random.Range(-20f, 20f), 0) * Vector3.forward;
            m_MainBall.m_IsStoped = true;

            yield return new WaitForSeconds(.6f);

            m_GameState = 1;
            m_MainBall.m_IsStoped = false;
        }
        // Update is called once per frame
        void Update()
        {
            switch (m_GameState)
            {
                case 1:
                    if (m_BlockBreakCount >= m_MaxBlockCount)
                    {
                        m_GameState = 2;
                        Ball.m_Main.m_IsStoped = true;
                        UISystem.m_Main.m_GameUI.SetActive(false);
                        UISystem.m_Main.m_WinUI.SetActive(true);
                        break;
                    }

                    if (Ball.m_Main.transform.position.z <= Player.m_Main.transform.position.z - 3)
                    {
                        m_GameState = 3; //lose
                        Ball.m_Main.m_IsStoped = true;
                        UISystem.m_Main.m_GameUI.SetActive(false);
                        UISystem.m_Main.m_LoseUI.SetActive(true);
                        break;
                    }
                    break;
            }
        }

        public void GenerateBalls()
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject ballObj = Instantiate(m_BallPrefab2);
                Ball ball = ballObj.GetComponent<Ball>();
                ball.transform.position = m_MainBall.transform.position;
                ball.m_MoveDirection = Quaternion.Euler(0, i * 60, 0) * Vector3.forward;
                ball.m_IsStoped = false;
            }
        }

    }
}
