using System;
using UnityEngine;

namespace _Scripts
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager ms_Instance;

        private int m_FlagHealth = 5;
        

        private void Awake()
        {
            ms_Instance = this;
        }


        public static void AddFlagHealth(int number)
        {
            ms_Instance.m_FlagHealth = ms_Instance.m_FlagHealth + number;
        }
    }
}
