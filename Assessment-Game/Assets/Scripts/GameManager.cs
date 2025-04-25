using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region Private properties

    private CardSpwanController _spwanController;
    private List<CardView> _currentSelections = new List<CardView>();
    private int _completedPair = 0;
    private int _totalCards = 0;
    private float lastMatchTime = -10f;
    private float previousMatchTime = -10f;
    private const float BonusTime = 1f;
    private const int _score = 10;
    private const int _bonus = 10;
    #endregion

    #region Public properties
    public int CurrentLevel { get; private set; }
    public static GameManager Instance { get; private set; }
    #endregion


    #region Unity Methods
    void Awake()
    {
        GameEvents.OnLevelSelected += StartGame;
        GameEvents.OnQuitPressed += GameEvents_OnQuitPressed;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _spwanController = FindAnyObjectByType<CardSpwanController>();
       
    }

    void Start()
    {
      
      // StartGame();
       //LoadLevel();
    }
    #endregion

    #region Public Methods

    public GameoverInfo GetGameoverInfo() => UIManager.Instance.GetGameoverInfo();
   

    IEnumerator _StartGame()
    {
        yield return new WaitForSeconds(0.1f);
        _spwanController.SpwanCards(_totalCards, (cardView) =>
        {
            MatchSelectedCards(cardView);
        });
    }


    public void StartGame(int totalCards, int currentLevel)
    {
        UIManager.Instance.HideLevelSelection();
        UIManager.Instance.ShowHud();
        _totalCards = totalCards;
        SetCurrentLevel(currentLevel);
        _completedPair = 0;
        StartCoroutine(_StartGame());
    }


    // Load level data from Json
    public void LoadLevel()
    {
        LevelData levelData = SaveAndLoadGame.LoadLevelData();
        // Retrive all cards 
        List<CardData> cardDatas = _spwanController.cardDatas;

        //Getting card id from the data list so we can get relevent Cards
        var ids = levelData.levelDataInfos.Select(x => x.cardId).ToList();

        //Getting matched cards
        var flippedCards = levelData.levelDataInfos.Count(x => x.isFlipped);

        //Setting Previous Score
        UIManager.Instance.SetScore(levelData.score);

        //Getting how many cards alredy matched  also makesure incase one cards flipped will void it

        _completedPair = (int)Mathf.Floor(flippedCards) / 2;

        _spwanController.SpwanCards(ids, levelData, (cardData) =>
        {
            MatchSelectedCards(cardData);
        });

    }
    #endregion

    #region Private Methods

    // Matching card
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

    // checking Cards
    IEnumerator CheckMatchAsync(CardView card1, CardView card2)
    {
        yield return new WaitForSeconds(0.6f);
       
        if (card1.cardData.cardID == card2.cardData.cardID)
        {
            float currentTime = Time.time;

            AudioManager.Instance.PlaySFX(AudioManager.Instance.cardMatchedSound);

            PlayCardMoveAnimation(card1, card2);

            UIManager.Instance.SetScore(_score);

            // Checking time between card match for bonus points
            if (currentTime - lastMatchTime <= BonusTime)
            {
                UIManager.Instance.SetBonusScore(_bonus);
            }
            lastMatchTime = currentTime;

            _completedPair++;

            // Game Over when user matched all cards

            if (_completedPair == _totalCards / 2)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverSound);
                GameoverInfo gameoverInfo = GetGameoverInfo();
                UIManager.Instance.ShowGameOverScreen(gameoverInfo);
            }
        }
        else
        {

            // if it not matched back to flip 
            AudioManager.Instance.PlaySFX(AudioManager.Instance.cardMisMatchedSound);
            card1.BackFlipCard();
            card2.BackFlipCard();
        }

    }

    // Saving Data to Json
    private void SaveLevelDataToLocal()
    {
        GameoverInfo gameoverInfo = UIManager.Instance.GetGameoverInfo();
        SaveAndLoadGame.SaveLevelData(_spwanController.gameCards, gameoverInfo);
    }

    private void SetCurrentLevel(int cLevel)
    {
        CurrentLevel = cLevel;
    }


    

    // Card animation after successful selection
    private void PlayCardMoveAnimation(CardView card1, CardView card2)
    {
        // setting card position top off all other cards;
        card1.gameObject.transform.SetSiblingIndex(100);
        card2.gameObject.transform.SetSiblingIndex(100);

        Vector3 posA = card1.transform.position;
        Vector3 posB = card2.transform.position;
        Vector3 midPoint = (posA + posB) / 2;
        card1.gameObject.transform.DOMove(midPoint, .4f).SetEase(Ease.Linear).OnComplete(() => card1.gameObject.transform.DOMove(new Vector3(card1.gameObject.transform.position.x, -(Screen.height+200), 0f),.6f)); 
        card2.gameObject.transform.DOMove(midPoint, .4f).SetEase(Ease.Linear).OnComplete(() => card2.gameObject.transform.DOMove(new Vector3(card2.gameObject.transform.position.x, -(Screen.height+200), 0f), .6f)); 
    }

    // Quit Event
    private void GameEvents_OnQuitPressed()
    {
        SaveLevelDataToLocal();
        UIManager.Instance.SetScore(0);
        _spwanController.ClearLevelAssets();
        UIManager.Instance.HideHud();
        UIManager.Instance.ShowLevelSelection();
        _currentSelections.Clear();

    }
    #endregion
}
