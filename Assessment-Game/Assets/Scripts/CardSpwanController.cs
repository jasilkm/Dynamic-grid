using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DG.Tweening;
public class CardSpwanController : MonoBehaviour
{

    #region public properties
    [HideInInspector]
    public List<CardView> gameCards = new List<CardView>();
    public List<CardData> cardDatas = new List<CardData>();

    #endregion

    #region private properties



    [SerializeField] private CardView _cardView;
  
    private CardLayOutController _cardLayOutController;
    private Transform _gridTransform;
    private CardData _cardData;
    private int _totalCards;
    private Action<CardView> _getCardHandler;
    #endregion

    #region Events properties
    #endregion

    #region public Methods



    /// <summary>
    /// 
    /// </summary>
    /// <param name="totalCards">the total cards need to spawn on gridLayout</param>
    /// <param name="getCardHandler">Registering callback event to the each card</param>
    public void SpwanCards(int totalCards, Action<CardView> getCardHandler)
    {
        _totalCards = totalCards;
        ClearLevelAssets();
        _cardLayOutController.CreateLayout(totalCards);

        _getCardHandler = getCardHandler;
        // genrating cards
        StartCoroutine(_genrateCards(totalCards)) ;
    }



    IEnumerator _genrateCards(int totalCards)
    {

       
        List<CardData> shuffledCards = GetShuffledCards();

        for (int i = 0; i < totalCards; i++)
        {
            yield return new WaitForEndOfFrame();
            GameObject obj = Instantiate(_cardView.gameObject, _gridTransform);

            CardView cardView = obj.GetComponent<CardView>();

            cardView.gameObject.GetComponent<RectTransform>().sizeDelta = _cardLayOutController.gridLayout.cellSize;

            cardView.gameObject.transform.localScale = new Vector3(1, 1, 1);

            cardView.gameObject.SetActive(false);
            gameCards.Add(cardView);
            // Registering call back
            cardView.SetCardData(shuffledCards[i], (card) =>
            {
                _getCardHandler(card);
            });
        }

        foreach (var item in gameCards)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.cardGenerate);
            item.gameObject.SetActive(true);
            item.FlipCards();
        }
 
        yield return new WaitForSeconds(3f);
        gameCards.ForEach(x => x.BackFlipCard());

        yield return new WaitForSeconds(1);
        DisbaleGrid();
    }

    /// <summary>
    /// Method is using to spawan card if user trying play from persistance
    /// </summary>
    /// <param name="cards">Cards ids based on this id  rretring matching data from card list</param>
    /// <param name="levelData"></param>
    /// <param name="getCardHandler"></param>
    public void SpwanCards(List<int> cards, LevelData levelData, Action<CardView> getCardHandler)
    {
        ClearLevelAssets();
        _totalCards = cards.Count;
        _cardLayOutController.CreateLayout(_totalCards);
        _getCardHandler = getCardHandler;
        StartCoroutine(_genrateCards(cards, levelData));
    }


    IEnumerator _genrateCards(List<int> cards,LevelData levelData)
    {
        _cardLayOutController.CreateLayout(_totalCards);
        CardView cardView = new CardView();

        for (int i = 0; i < _totalCards; i++)
        {


            yield return new WaitForEndOfFrame();
            GameObject obj = Instantiate(_cardView.gameObject, _gridTransform);
            cardView = obj.GetComponent<CardView>();
            cardView.gameObject.transform.localScale = new Vector3(1, 1, 1);

            cardView.gameObject.GetComponent<RectTransform>().sizeDelta = _cardLayOutController.gridLayout.cellSize;
            cardView.gameObject.transform.localScale = Vector3.one;
            cardView.gameObject.SetActive(true);
            //getting matching Card data 
            var cardData = cardDatas
                .FirstOrDefault(card => card.cardID == cards[i]);

          
            // Registering call back
            cardView.SetCardData(cardData, (card) =>
            {
                _getCardHandler(card);
            });

            gameCards.Add(cardView);
        }
        yield return new WaitForEndOfFrame();
        DisbaleGrid();

        for (int i = 0; i < _totalCards; i++)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.cardGenerate);

            if (levelData.levelDataInfos[i].isSelected)
            {
                gameCards[i].isMatched = true;

                PlayCardMoveAnimation(gameCards[i]);


            }
            else

            {
                gameCards[i].FlipCards();

            }

          


        }


        // delay set for intial view card to memorise

        yield return new WaitForSeconds(3f);
        gameCards.Where(x => !x.isMatched).ToList().ForEach(x => x.BackFlipCard());


      
    }

    /// <summary>
    /// Clearing all level assetes from Gridlayout
    /// </summary>
    public void ClearLevelAssets()
    {
        if (gameCards == null || gameCards.Count == 0) return;

        foreach (var item in gameCards)
        {
            Destroy(item.gameObject);
        }
        gameCards.Clear();
       
    }
    #endregion

    #region private Methods
    /// <summary>
    /// Retriving required uniq cards for the level and multiplying with 2 for pair and reshuflled to make random
    /// </summary>
    /// <returns>Final list of cards</returns>
    private List<CardData> GetShuffledCards()
    {
        // Creating Uniq list for data  in each launch of the Game;
        List<CardData> tempCards = cardDatas.OrderBy(x => UnityEngine.Random.value).ToList().Take(_totalCards / 2).ToList();
        // duplicating same data to spawn
        List<CardData> duplicated = tempCards.Concat(tempCards).ToList();
        //Making Random order 
        List<CardData> shuffledCards = duplicated.OrderBy(x => UnityEngine.Random.value).ToList();
        return shuffledCards;
    }

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _cardLayOutController = FindAnyObjectByType<CardLayOutController>();

        if (_cardLayOutController != null)
        {
            _gridTransform = _cardLayOutController.GetGridTransform();
        }

    }

    // Disabling grid so we can animate the its childs
    public void DisbaleGrid()
    {
        _cardLayOutController.gridLayout.enabled = false;
    }

    // Enable grid for spwan assets
    public void EnableGrid()
    {
        _cardLayOutController.gridLayout.enabled = true;
    }



    // Card animation after successful selection
    private void PlayCardMoveAnimation(CardView card1)
    {
        // setting card position top off all other cards;
        card1.gameObject.transform.SetSiblingIndex(100);

        Vector3 posA = card1.transform.position;
        card1.gameObject.transform.DOMove(new Vector3(card1.gameObject.transform.position.x, -(Screen.height + 200), 0f), .6f);
    }


    public void HideGameLayout() => _gridTransform.gameObject.SetActive(false);
    private void ShowGameLayout() => _gridTransform.gameObject.SetActive(true);





}


