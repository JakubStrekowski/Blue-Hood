using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PauseRestartUIManager : MonoBehaviour
{
    public GameObject pausePanel;

    public GameObject restartPanel;

    public GameObject victoryPanel;

    [SerializeField]
    private CharacterHealth playerHealthInfo;

    private void Awake()
    {
        playerHealthInfo.OnIsDeathChange += ShowDeathPanel;
    }

    public void Update()
    {
        if (Input.GetButtonDown("Pause") && restartPanel.activeSelf == false)
        {
            if(pausePanel.activeSelf == false)
            {
                pausePanel.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
    private void ShowDeathPanel()
    {
        restartPanel.SetActive(true);
    }

    public void ShowVictoryPanel()
    {
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
