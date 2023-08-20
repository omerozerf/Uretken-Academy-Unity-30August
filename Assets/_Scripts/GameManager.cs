using System.Collections.Generic;
using EmreBeratKR.LazyCoroutines;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _tryAgainButton;
        [SerializeField] private List<Image> _healthList;
        [SerializeField] private TMP_Text _aiDeadCountText;
        [SerializeField] private TMP_Text _maxScoreText;
        
        private static GameManager ms_Instance;

        private int m_FlagHealth = 5;
        private int m_AiDeadCount;
        private bool m_IsGameStart;

        private const string AiDeadCountKey = "AiDeadCount";
        

        private void Awake()
        {
            ms_Instance = this;

            Time.timeScale = 0;
        }

        private void Start()
        {
            // LoadAiDeadCount();
            UpdateAiDeadCountText();

            _maxScoreText.text = $"Max Score: {PlayerPrefs.GetInt(AiDeadCountKey).ToString()}";
            
            _startButton.onClick.AddListener((() =>
            {
                Time.timeScale = 1;
                m_IsGameStart = true;
                
                _mainMenu.SetActive(false);
            }));
            
            _exitButton.onClick.AddListener((() =>
            {
                Application.Quit();
            }));
            
            _tryAgainButton.onClick.AddListener((() =>
            {
                SceneManager.LoadScene(0);
            }));
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

        // ReSharper disable Unity.PerformanceAnalysis
        public static void GameOver()
        {
            Time.timeScale = 0;
            
            ms_Instance._mainMenu.SetActive(true);
            ms_Instance._startButton.gameObject.SetActive(false);
            ms_Instance._tryAgainButton.gameObject.SetActive(true);
            
            ms_Instance._maxScoreText.text = $"Max Score: {PlayerPrefs.GetInt(AiDeadCountKey).ToString()}";
            
            print(PlayerPrefs.GetInt(AiDeadCountKey));
        }

        public static bool GetIsGameStart()
        {
            return ms_Instance.m_IsGameStart;
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
