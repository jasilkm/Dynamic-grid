using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HudController : MonoBehaviour
{

    [SerializeField] private TMP_Text _scoreText;
    private int _totalScore = 0;
    private int _bonus = 0;



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



}
