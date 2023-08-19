using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<Image> _healthList;
        
        private static GameManager ms_Instance;

        private int m_FlagHealth = 5;
        

        private void Awake()
        {
            ms_Instance = this;
        }


        public static void AddFlagHealth(int number)
        {
            ms_Instance.m_FlagHealth = ms_Instance.m_FlagHealth + number;

            if(ms_Instance._healthList.Count == 0) return;
            
            var lastHealth = ms_Instance._healthList[^1];

            lastHealth.color = Color.black;
            ms_Instance._healthList.Remove(lastHealth);
        }
    }
}
