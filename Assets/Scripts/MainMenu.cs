using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int                        maxEnergy;
    [SerializeField] private int                        energyRechargeDuration;
    [SerializeField] private AndroidNotificationHandler androidNotificationHandler;
    [SerializeField] private GameObject                 mapPanel;

    private Button   _backToMainMenuButton;
    private Button   _changeMapButton;
    private TMP_Text _currentMapText;
    private int      _energy;
    private Image    _energyImage;
    private TMP_Text _energyText;
    private TMP_Text _highScoreText;
    private Button   _playButton;
    private string   _sceneToLoad;

    private void Awake()
    {
        this._highScoreText = GameObject.Find("HighScoreText").GetComponent<TMP_Text>();
        this._energyText = GameObject.Find("EnergyText").GetComponentInChildren<TMP_Text>();
        this._currentMapText = GameObject.Find("CurrentMap").GetComponentInChildren<TMP_Text>();
        this._playButton = GameObject.Find("PlayButton").GetComponentInChildren<Button>();
        this._energyImage = GameObject.Find("Energy").GetComponent<Image>();
        this._changeMapButton = GameObject.Find("ChangeMapButton").GetComponentInChildren<Button>();
        this._backToMainMenuButton = GameObject.Find("BackButton").GetComponentInChildren<Button>();

        this._energy = PlayerPrefs.GetInt(TagManager.EnergyKey, this.maxEnergy);

        this._sceneToLoad         = TagManager.SunnyDay;
        this._currentMapText.text = this._sceneToLoad;

        this.mapPanel.SetActive(false);

        this._playButton.onClick.AddListener(Play);
        this._changeMapButton.onClick.AddListener(ChangeMap);
        this._backToMainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    private void Start()
    {
        this._highScoreText.text = $"High Score: {PlayerPrefs.GetInt(TagManager.HighScoreKey, 0)}";

        CheckForEnergy();
    }

    private void Update()
    {
        if (this.mapPanel.activeInHierarchy) SelectMap();
    }

    private void CheckForEnergy()
    {
        if (this._energy == 0)
        {
            var energyReadyString =
                PlayerPrefs.GetString(TagManager.EnergyReadyKey, string.Empty);

            if (energyReadyString == string.Empty) return;

            var energyReady = DateTime.Parse(energyReadyString);

            if (DateTime.Now > energyReady)
            {
                this._energy = this.maxEnergy;
                PlayerPrefs.SetInt(TagManager.EnergyKey, this._energy);
            }
            else
                Invoke(nameof(EnergyRecharged), (energyReady - DateTime.Now).Seconds);
        }

        this._energyText.text = $"X{this._energy}";
    }

    private void EnergyRecharged()
    {
        this._energy = this.maxEnergy;
        PlayerPrefs.SetInt(TagManager.EnergyKey, this._energy);

        this._energyText.text = $"X{this._energy}";
    }

    public void ChangeMap()
    {
        this.mapPanel.SetActive(true);
        this._changeMapButton.gameObject.SetActive(false);
        this._playButton.gameObject.SetActive(false);
        this._highScoreText.gameObject.SetActive(false);
        this._energyImage.gameObject.SetActive(false);
    }

    private void SelectMap()
    {
        var selectedGameObjectName = EventSystem.current.currentSelectedGameObject.name;

        if (selectedGameObjectName == TagManager.SunnyDay   ||
            selectedGameObjectName == TagManager.FoggyNight ||
            selectedGameObjectName == TagManager.Night      ||
            selectedGameObjectName == TagManager.RainyDay   ||
            selectedGameObjectName == TagManager.RainyNight)
        {
            this._sceneToLoad         = selectedGameObjectName;
            this._currentMapText.text = this._sceneToLoad;
        }
    }

    public void BackToMainMenu()
    {
        this.mapPanel.SetActive(false);
        this._changeMapButton.gameObject.SetActive(true);
        this._playButton.gameObject.SetActive(true);
        this._highScoreText.gameObject.SetActive(true);
        this._energyImage.gameObject.SetActive(true);
    }

    public void Play()
    {
        if (this._energy < 1) return;

        this._energy--;

        PlayerPrefs.SetInt(TagManager.EnergyKey, this._energy);

        if (this._energy == 0)
        {
            var energyReady = DateTime.Now.AddMinutes(this.energyRechargeDuration);
            PlayerPrefs.SetString(TagManager.EnergyReadyKey, energyReady.ToString());

#if UNITY_ANDROID

            this.androidNotificationHandler.ScheduleNotification(energyReady);

#endif
        }

        SceneManager.LoadScene(this._sceneToLoad);
    }
}