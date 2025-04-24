using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameManager : MonoBehaviour
{

    private CardSpwanController _spwanController;
    private List<CardView> _currentSelections = new List<CardView>();
    private int _completedPair = 0;
    private int _totalCards = 16;
    private float lastMatchTime = -10f;
    private const int BonusTime = 1;
    private const int _score = 10;
    private const int _bonus = 10;
    public static GameManager Instance { get; private set; }

  

    private void Awake()
    {
        GameEvents.OnLevelSelected += StartGame;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        GameEvents.OnQuitPressed += GameEvents_OnQuitPressed;
        _spwanController = FindAnyObjectByType<CardSpwanController>();
       
    }

    void Start()
    {
      
      // StartGame();
      // LoadLevel();
    }

    IEnumerator _StartGame()
    {
        yield return new WaitForSeconds(0.1f);
        _spwanController.SpwanCards(_totalCards, (cardData) =>
        {
            MatchSelectedCards(cardData);
        });
    }


    public void StartGame(int totalCards)
    {
        UIManager.Instance.HideLevelSelection();
        _totalCards = totalCards;
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
            AudioManager.Instance.PlaySFX(AudioManager.Instance.cardMatchedSound);
            UIManager.Instance.SetScore(_score);
            if (currentTime - lastMatchTime <= BonusTime)
            {
                UIManager.Instance.SetBonusScore(_bonus);
            }

            lastMatchTime = currentTime;

            _completedPair++;

           

            if (_completedPair == _totalCards / 2)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverSound);
                GameoverInfo gameoverInfo = UIManager.Instance.GetGameoverInfo();

                UIManager.Instance.ShowGameOverScreen(gameoverInfo);


            }
        }
        else
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.cardMisMatchedSound);
            card1.BackFlipCard();
            card2.BackFlipCard();
        }

    }


    private void SaveLevelDataToLocal()
    {

        Debug.Log("Game data saved");
        GameoverInfo gameoverInfo = UIManager.Instance.GetGameoverInfo();
        SaveAndLoadGame.SaveLevelData(_spwanController.gameCards, gameoverInfo);
    }

    public void LoadLevel()
    {
        LevelData levelData =  SaveAndLoadGame.LoadLevelData();
        List<CardData> cardDatas = _spwanController.cardDatas;

        //Getting card id from the data list so we can get relevent Cards
        var ids  =  levelData.levelDataInfos.Select(x => x.cardId).ToList();
        var flippedCards = levelData.levelDataInfos.Count(x => x.isFlipped);
        UIManager.Instance.SetScore(levelData.score);
        _completedPair = (int)Mathf.Floor(flippedCards)/2;

        _spwanController.SpwanCards(ids, levelData,(cardData) =>
        {
            MatchSelectedCards(cardData);
        });

    }


    private void GameEvents_OnQuitPressed()
    {
        SaveLevelDataToLocal();
        _spwanController.ClearLevelAssets();
        UIManager.Instance.ShowLevelSelection();
        UIManager.Instance.SetScore(0);
    }

}
