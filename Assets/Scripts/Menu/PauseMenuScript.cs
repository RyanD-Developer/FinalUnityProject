using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject m_pauseScreen;
    public GameObject m_settingsScreen;

    [HideInInspector] public bool m_pauseScreenIsActive = true;
    [HideInInspector] public bool m_settingsScreenIsActive = false;

    public void SwitchToSettingsScreen()
    {
        m_pauseScreen.SetActive(false);
        m_settingsScreen.SetActive(true);
        m_pauseScreenIsActive = false;
        m_settingsScreenIsActive = true;
    }
    public void SwitchToPauseScreen()
    {
        m_pauseScreen.SetActive(true);
        m_settingsScreen.SetActive(false);
        m_pauseScreenIsActive = true;
        m_settingsScreenIsActive = false;
    }

    public void SwitchToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
