using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainScreenManager : MonoBehaviour
{
    public static MainScreenManager Instance = null;
    public GameObject m_startMenu;
    public bool m_isPaused = false;
    [SerializeField] private AudioSource m_musicSource;

    private string m_currentLevel;
    [SerializeField] private TextMeshProUGUI startText;
    void Awake()
    {
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            m_currentLevel = PlayerPrefs.GetString("CurrentLevel");
            startText.text = "Continue";
        }
        Debug.Log(m_currentLevel);
        if (!PlayerPrefs.HasKey("CurrentLevel"))
        {
            m_currentLevel = "Level1";
            startText.text = "Start";
        }
        PlayerPrefs.Save();
    }

    public void StartGame()
    {
        m_isPaused = true;
        m_startMenu.SetActive(true);
        SceneManager.LoadScene(m_currentLevel);
    }
}
