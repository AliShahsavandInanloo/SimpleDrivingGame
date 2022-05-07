using System.Globalization;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private float scoreMultiplier = 1f;

    private float    _score;
    private TMP_Text _scoreText;

    private void Awake()
    {
        this._scoreText = GetComponentInChildren<TMP_Text>();
    }


    private void Update()
    {
        this._score          += Time.deltaTime * this.scoreMultiplier;
        this._scoreText.text = Mathf.FloorToInt(this._score).ToString(CultureInfo.InvariantCulture);
    }
}