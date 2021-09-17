using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;

public class PauseMenuScript : MonoBehaviour
{
    public TextMeshProUGUI txtPause;

    private GameManager gm;

    public delegate void SettingsMenuShow();
    public event SettingsMenuShow SettingsMenuShowNotify;

    public void OnResumeClick()
    {
        gm.PauseMenuShow(false);
    }

    public void OnSettingsClick()
    {
        SettingsMenuShowNotify?.Invoke();
    }

    public void OnResetLevelClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnExitGameClick()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        txtPause.text = "Game Paused";
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
