using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private CardSpwanController _spwanController;
    private HudController _scoreController;

    private List<CardView> _currentSelections = new List<CardView>();
    private int _completedPair = 0;
    private int _totalCards = 10;
    private float lastMatchTime = -10f;
    private const int BonusTime = 1;
    private const int _score = 10;
    private const int _bonus = 10;
    public static GameManager Instance { get; private set; }



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _spwanController = FindAnyObjectByType<CardSpwanController>();
        _scoreController = FindAnyObjectByType<HudController>();

    }



    void Start()
    {
      
        StartGame();
    }

    void Update()
    {
        
    }


    IEnumerator _StartGame()
    {
        yield return new WaitForSeconds(0.1f);
        _spwanController.SpwanCards(_totalCards, (cardData) =>
        {
            MatchSelectedCards(cardData);
        });
    }


    public void StartGame()
    {
        StartCoroutine(_StartGame());
    }

   

    private void MatchSelectedCards(CardView card)
    {
        _currentSelections.Add(card);

        if (_currentSelections.Count == 2)
        {
            CardView first = _currentSelections[0];
            CardView second = _currentSelections[1];
            StartCoroutine(CheckMatchAsync(first, second));
            _currentSelections.Clear(); 
        }

    }


    IEnumerator CheckMatchAsync(CardView card1, CardView card2)
    {
        yield return new WaitForSeconds(1);

        if (card1.cardData.cardID == card2.cardData.cardID)
        {
            float currentTime = Time.time;

            _scoreController.UpdateScrore(_score);
            if (currentTime - lastMatchTime <= BonusTime)
            {

                _scoreController.UpdateScrore(_bonus);
            }

            lastMatchTime = currentTime;

            _completedPair++;

            if (_completedPair == _totalCards / 2)
            {
                Debug.Log("Game Over");
            }
        }
        else
        {
            card1.BackFlipCard();
            card2.BackFlipCard();
        }

    }



}
