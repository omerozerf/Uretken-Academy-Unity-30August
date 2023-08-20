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

        private const string AiDeadCountKey = "AiDeadCount";
        

        private void Awake()
        {
            ms_Instance = this;
        }

        private void Start()
        {
            // LoadAiDeadCount();
            UpdateAiDeadCountText();
        }

        public static void AddFlagHealth(int number)
        {
            ms_Instance.m_FlagHealth = ms_Instance.m_FlagHealth + number;

            if(ms_Instance._healthList.Count == 0) return;
            
            var lastHealth = ms_Instance._healthList[^1];

            lastHealth.color = Color.black;
            ms_Instance._healthList.Remove(lastHealth);

            if (ms_Instance._healthList.Count == 0)
            {
                GameOver();
            }
        }

        public static void AddAiCount(int addNumber)
        {
            ms_Instance.m_AiDeadCount = ms_Instance.m_AiDeadCount + addNumber;
            ms_Instance.UpdateAiDeadCountText();

            // Check if the current AI dead count is higher than the saved value
            int savedAiDeadCount = PlayerPrefs.GetInt(AiDeadCountKey, 0);
            if (ms_Instance.m_AiDeadCount > savedAiDeadCount)
            {
                ms_Instance.SaveAiDeadCount();
            }
        }

        public static void GameOver()
        {
            Time.timeScale = 0;
            
            print(PlayerPrefs.GetInt(AiDeadCountKey));
        }

        private void UpdateAiDeadCountText()
        {
            _aiDeadCountText.text = $"Enemy: {m_AiDeadCount}";
        }

        private void SaveAiDeadCount()
        {
            PlayerPrefs.SetInt(AiDeadCountKey, ms_Instance.m_AiDeadCount);
            PlayerPrefs.Save();
        }

        private void LoadAiDeadCount()
        {
            m_AiDeadCount = PlayerPrefs.GetInt(AiDeadCountKey, 0);
        }
    }
}
