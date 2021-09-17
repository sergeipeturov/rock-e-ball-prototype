using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml.Serialization;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public Player Player;
    public Camera MainCamera;
    public Camera PlayerCamera;
    public TextMeshProUGUI MediatorsCountText;
    public GameObject LevelUpMenu;
    public GameObject PauseMenu;
    public GameObject SettingsMenu;
    public GameObject Aim;
    public GameObject DamagePopup;

    public List<Feature> AllFeatures = new List<Feature>();
    public int CurrentWorld { get; set; }

    public bool IsNewGameStarted { get; set; }
    public bool IsLevelUpMenuShowed { get { return LevelUpMenu != null ? LevelUpMenu.activeSelf : false; } }
    public bool IsPauseMenuShowed { get { return PauseMenu != null ? PauseMenu.activeSelf : false; } }

    public delegate void LevelUpMenuShowedHandler();
    public event LevelUpMenuShowedHandler LevelUpMenuShowedNotify;

    private MediatorsRandomGenerator mrg;

    private GameState gameState;
    private SettingsMenuScript gameSettings;

    public GameState GameState
    {
        get { return gameState; }
        set
        {
            gameState = value;
            if (gameState == GameState.play)
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (gameSettings != null && gameSettings.ShowAim) Aim.SetActive(true);
            }    
            else if (gameState == GameState.paused)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                Aim.SetActive(false);
            }
        }
    }

    public void SetPause(bool pause)
    {
        GameState = pause ? GameState.paused : GameState.play;
    }

    public void PauseMenuShow(bool show)
    {
        if (show)
        {
            SetPause(true);
            PauseMenu.SetActive(true);
        }
        else
        {
            SetPause(false);
            PauseMenu.SetActive(false);
        }
    }

    public void LevelUpMenuShow(bool show)
    {
        if (show)
        {
            SetPause(true);
            LevelUpMenu.SetActive(true);
            LevelUpMenuShowedNotify?.Invoke();
        }
        else
        {
            SetPause(false);
            LevelUpMenu.SetActive(false);
        }
    }

    public void PlayerBuyFeature(Feature feature)
    {
        Player.Features.Add(feature);
        Player.MediatorsCount = Player.MediatorsCount - feature.Cost;
    }

    private void UpdateMediatorsCountText()
    {
        MediatorsCountText.text = "Mediators: " + Player.MediatorsCount.ToString();
    }

    private void OnSettingsMenuShow()
    {
        PauseMenu.SetActive(!PauseMenu.activeSelf);
        SettingsMenu.SetActive(!SettingsMenu.activeSelf);
    }

    private void OnSettingsApply()
    {
        PlayerCamera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = gameSettings.MouseSensitivityXValue;
        PlayerCamera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = gameSettings.MouseSensitivityYValue;
        OnSettingsMenuShow();
    }


    // Start is called before the first frame update
    void Start()
    {
        UpdateMediatorsCountText();

        AllFeatures = Feature.GetFeaturesList();

        LevelUpMenu.SetActive(false);
        PauseMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        PauseMenu.GetComponent<PauseMenuScript>().SettingsMenuShowNotify += OnSettingsMenuShow;
        gameSettings = SettingsMenu.GetComponent<SettingsMenuScript>();
        gameSettings.SettingsMenuShowNotify += OnSettingsMenuShow;
        gameSettings.SettingsApplyNotify += OnSettingsApply;

        IsNewGameStarted = true;

        if (IsNewGameStarted)
        {
            CurrentWorld = 1;

            Player.MediatorsCountChangedNotify += UpdateMediatorsCountText;

            MainCamera.enabled = false;
            PlayerCamera.enabled = true;

            GameState = GameState.play;
            mrg = GameObject.Find("MediatorsRandomGenerator").GetComponent<MediatorsRandomGenerator>();
            mrg.Generate(12);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Player.CurrentGameState = gameState;
    }
}

public enum GameState
{
    play, paused
}
