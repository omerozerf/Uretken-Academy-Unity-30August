using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<Image> _healthList;
        [SerializeField] private TMP_Text _aiDeadCountText;
        
        private static GameManager ms_Instance;

        private int m_FlagHealth = 5;
        private int m_AiDeadCount;
        

        private void Awake()
        {
            ms_Instance = this;
        }

        private void Start()
        {
            _aiDeadCountText.text = $"Enemy: {m_AiDeadCount}";
        }


        public static void AddFlagHealth(int number)
        {
            ms_Instance.m_FlagHealth = ms_Instance.m_FlagHealth + number;

            if(ms_Instance._healthList.Count == 0) return;
            
            var lastHealth = ms_Instance._healthList[^1];

            lastHealth.color = Color.black;
            ms_Instance._healthList.Remove(lastHealth);
        }

        public static void AddAiCount(int addNumber)
        {
            ms_Instance.m_AiDeadCount = ms_Instance.m_AiDeadCount + addNumber;
            
            ms_Instance._aiDeadCountText.text = $"Enemy: {ms_Instance.m_AiDeadCount}";
        }
    }
}
