using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PhysicBreakout
{
    [CreateAssetMenu(fileName = "DataStorage", menuName = "CustomObjects/DataStorage", order = 1)]
    public class DataStorage : ScriptableObject
    {
        public int m_LevelNum;
        //public int m_UnlockedLevelNum;
        public void SaveData()
        {
            PlayerPrefs.SetInt("m_LevelNum", m_LevelNum);
            //PlayerPrefs.SetInt("m_UnlockedLevelNum", m_UnlockedLevelNum);

            PlayerPrefs.Save();
        }

        public void LoadData()
        {
            //m_TotalScore = PlayerPrefs.GetInt("m_TotalScore", 0);
            m_LevelNum = PlayerPrefs.GetInt("m_LevelNum", 0);
            //m_UnlockedLevelNum = PlayerPrefs.GetInt("m_UnlockedLevelNum", 0);

        }
    }

}