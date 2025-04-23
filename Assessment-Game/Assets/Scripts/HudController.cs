using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HudController : MonoBehaviour
{

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Button _pauseBtn;

    private int _totalScore = 0;
    private int _bonus = 0;

    private void Awake()
    {
        _pauseBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPauseScreen();
        });
    }

    public void UpdateScrore(int score)
    {
        _totalScore += score;
        _scoreText.text = $"Score: {_totalScore}";
    }


    public void UpdateBonus(int bonus)
    {
        _totalScore += bonus;
        _bonus += bonus;
        _scoreText.text = $"Score: {_totalScore}";
    }


    public GameoverInfo GetGameoverInfo()
    {
        GameoverInfo gameoverInfo = new GameoverInfo();
        gameoverInfo.Score = _totalScore;
        gameoverInfo.Bonus = _bonus;
        gameoverInfo.Time = 0;
        return gameoverInfo;
    }



}
