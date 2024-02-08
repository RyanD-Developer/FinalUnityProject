using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public GameObject m_pauseMenu;
    public GameObject m_playerUI;
    public GameObject m_deathMenu;
    public bool m_isPaused = false;
    public bool m_isDead = false;
    [SerializeField] private AudioSource m_musicSource;

    private float m_fixedDeltaTime;
    void Awake()
    {
        Application.targetFrameRate = 60;
        m_fixedDeltaTime = Time.fixedDeltaTime;
    }

    public void PauseGame()
    {
        if(!m_isPaused)
        {
            m_isPaused = true;
            m_pauseMenu.SetActive(true);
            m_playerUI.SetActive(false);
            Time.timeScale = 0f;
            Time.fixedDeltaTime = m_fixedDeltaTime * Time.timeScale;
        }
    }

    public void UnpauseGame()
    {
        if (m_isPaused)
        {
            m_isPaused = false;
            m_pauseMenu.SetActive(false);
            m_playerUI.SetActive(true);
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = m_fixedDeltaTime * Time.timeScale;
        }
    }

    public void PlayerDead()
    {
        m_isDead = true;
        m_pauseMenu.SetActive(false);
        m_playerUI.SetActive(false);
        m_deathMenu.SetActive(true);
        m_musicSource.Stop();
        Time.timeScale = 1f;
        Time.fixedDeltaTime = m_fixedDeltaTime * Time.timeScale;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
