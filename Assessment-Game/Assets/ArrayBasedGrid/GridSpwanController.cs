
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class GridSpwanController : MonoBehaviour
{

    #region public properties
    [HideInInspector]
    public List<CardViewItem> gameCards = new List<CardViewItem>();
    public List<CardData> cardDatas = new List<CardData>();
       

    #endregion

    #region private properties



    [SerializeField] private CardViewItem _cardView;

    private GridLayoutController _cardLayOutController;
    private Transform _gridTransform;
    private CardData _cardData;
    private int _totalCards;
    private Action<CardViewItem> _getCardHandler;
    private GameObject[,] gridArray;
    private float spacing = .06f;
    private int i = 0;
    #endregion

    #region Events properties
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        _cardLayOutController = FindAnyObjectByType<GridLayoutController>();
        GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
       // GetComponent<SpriteRenderer>().size = new Vector2(4, 6);

    }


    #region public Methods



    /// <summary>
    /// 
    /// </summary>
    /// <param name="totalCards">the total cards need to spawn on gridLayout</param>
    /// <param name="getCardHandler">Registering callback event to the each card</param>
    public void SpwanCards(int totalCards, Action<CardViewItem> getCardHandler)
    {
        i = 0;
        _totalCards = totalCards;
        ClearLevelAssets();
      GridInfo gridInfo =  _cardLayOutController.CreateGridLayOut(totalCards);

        _getCardHandler = getCardHandler;
        // genrating cards
        StartCoroutine(_genrateCards(totalCards, gridInfo));
    }

    

    IEnumerator _genrateCards(int totalCards, GridInfo gridInfo)
    {


        List<CardData> shuffledCards = GetShuffledCards();

        int columns = gridInfo.Columns;
        int rows = gridInfo.Rows;
        Vector2 cardSize = gridInfo.CardSize;

        gridArray = new GameObject[gridInfo.Columns, gridInfo.Rows];

        float startX = -gridInfo.WidgetSize.x / 2 + gridInfo.CardSize.x / 2;
        float startY = -gridInfo.WidgetSize.y / 2 + gridInfo.CardSize.y / 2;


        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 position = new Vector3(
                startX + x * (cardSize.x + spacing),
                startY + y * (cardSize.y + spacing),
                0
            );

                GameObject card = Instantiate(_cardView.gameObject, position, Quaternion.identity, _cardLayOutController.transform);

                CardViewItem cardView = card.GetComponent<CardViewItem>();

                gameCards.Add(cardView);

                card.GetComponent<SpriteRenderer>().size = cardSize;

                card.name = $"Card1_{x}_{y}";

                // Registering call back

                cardView.SetCardData(shuffledCards[i], (card) =>
                {
                    _getCardHandler(card);
                });

                gridArray[x, y] = card;


                i++;

            }
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
    }

    /// <summary>
    /// Method is using to spawan card if user trying play from persistance
    /// </summary>
    /// <param name="cards">Cards ids based on this id  rretring matching data from card list</param>
    /// <param name="levelData"></param>
    /// <param name="getCardHandler"></param>
    public void SpwanCards(List<int> cards, LevelData levelData, Action<CardViewItem> getCardHandler)
    {
        ClearLevelAssets();
        _totalCards = cards.Count;
        _cardLayOutController.CreateGridLayOut(_totalCards);
        _getCardHandler = getCardHandler;
        StartCoroutine(_genrateCards(cards, levelData));
    }


    IEnumerator _genrateCards(List<int> cards, LevelData levelData)
    {
        CardViewItem cardView = new CardViewItem();

        for (int i = 0; i < _totalCards; i++)
        {


            yield return new WaitForEndOfFrame();
            GameObject obj = Instantiate(_cardView.gameObject, _gridTransform);
            cardView = obj.GetComponent<CardViewItem>();
            cardView.gameObject.transform.localScale = new Vector3(1, 1, 1);

          //  cardView.gameObject.GetComponent<RectTransform>().sizeDelta = _cardLayOutController.gridLayout.cellSize;
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


  



    // Card animation after successful selection
    private void PlayCardMoveAnimation(CardViewItem card1)
    {
        // setting card position top off all other cards;
        card1.gameObject.transform.SetSiblingIndex(100);

        Vector3 posA = card1.transform.position;
        card1.gameObject.transform.DOMove(new Vector3(card1.gameObject.transform.position.x, -(Screen.height + 200), 0f), .6f);
    }


    public void HideGameLayout() => _gridTransform.gameObject.SetActive(false);
    private void ShowGameLayout() => _gridTransform.gameObject.SetActive(true);





}


