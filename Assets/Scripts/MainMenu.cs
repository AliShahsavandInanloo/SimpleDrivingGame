using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private TMP_Text _highScoreText;
    private Button   _playButton;

    private void Awake()
    {
        this._highScoreText = GameObject.Find("HighScoreText").GetComponent<TMP_Text>();
        this._playButton    = GetComponentInChildren<Button>();

        this._playButton.onClick.AddListener(Play);
    }

    private void Start()
    {
        this._highScoreText.text = $"High Score: {PlayerPrefs.GetInt(TagManager.HighScoreKey, 0)}";
    }

    public void Play()
    {
        SceneManager.LoadScene(TagManager.Sandbox);
    }
}