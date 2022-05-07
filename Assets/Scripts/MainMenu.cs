using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int                        maxEnergy;
    [SerializeField] private int                        energyRechargeDuration;
    [SerializeField] private AndroidNotificationHandler androidNotificationHandler;

    private int      _energy;
    private TMP_Text _energyText;
    private TMP_Text _highScoreText;
    private Button   _playButton;

    private void Awake()
    {
        this._highScoreText = GameObject.Find("HighScoreText").GetComponent<TMP_Text>();
        this._energyText    = GameObject.Find("PlayButton").GetComponentInChildren<TMP_Text>();
        this._playButton    = GetComponentInChildren<Button>();

        this._energy = PlayerPrefs.GetInt(TagManager.EnergyKey, this.maxEnergy);

        this._playButton.onClick.AddListener(Play);
    }

    private void Start()
    {
        this._highScoreText.text = $"High Score: {PlayerPrefs.GetInt(TagManager.HighScoreKey, 0)}";

        CheckForEnergy();
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
        }

        this._energyText.text = $"Play ({this._energy})";
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

        SceneManager.LoadScene(TagManager.Sandbox);
    }
}