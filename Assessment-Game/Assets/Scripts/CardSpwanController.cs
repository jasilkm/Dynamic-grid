using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
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




    public void SpwanCards(int totalCards, Action<CardView> getCardHandler)
    {
        _totalCards = totalCards;
        
        _cardLayOutController.CreateLayout(totalCards);

        _getCardHandler = getCardHandler;

        StartCoroutine(_genrateCards(totalCards)) ;
    }


    IEnumerator _genrateCards(int totalCards)
    {
       

        List<CardData> shuffledCards = GetShuffledCards();

        for (int i = 0; i < totalCards; i++)
        {
           
            GameObject obj = Instantiate(_cardView.gameObject, _gridTransform);

            CardView cardView = obj.GetComponent<CardView>();
            cardView.gameObject.SetActive(false);
            gameCards.Add(cardView);

            cardView.SetCardData(shuffledCards[i], (card) =>
            {
                _getCardHandler(card);
            });
        }

        foreach (var item in gameCards)
        {
            yield return new WaitForSeconds(0.03f);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.cardGenerate);
            item.gameObject.SetActive(true);
        }


    }


    public void SpwanCards(List<int> cards,LevelData levelData,  Action<CardView> getCardHandler)
    {
        _totalCards = cards.Count;

        _cardLayOutController.CreateLayout(_totalCards);


        for (int i = 0; i < _totalCards; i++)
        {
            GameObject obj = Instantiate(_cardView.gameObject, _gridTransform);

            CardView cardView = obj.GetComponent<CardView>();


            //getting matching Card data 
            var cardData = cardDatas
                .FirstOrDefault(card => card.cardID == cards[i]);


            if (levelData.levelDataInfos[i].isFlipped)
            {
                cardView.FlipCards();

            }

            gameCards.Add(cardView);

            cardView.SetCardData(cardData, (card) =>
            {
                getCardHandler(card);
            });
        }
    }


    public void ClearLevelAssets()
    {
        foreach (var item in gameCards)
        {
            Destroy(item.gameObject);
        }
        gameCards.Clear();

        Debug.Log("gameCards :"+ gameCards.Count);
       
    }
    #endregion

    #region private Methods

    private List<CardData> GetShuffledCards()
    {
        // Creating Uniq list for data  in each launch of the Game;
        List<CardData> tempCards = cardDatas.OrderBy(x => UnityEngine.Random.value).ToList().Take(_totalCards / 2).ToList();
        // duplicating same data to spawn
        List<CardData> duplicated = tempCards.Concat(tempCards).ToList();
        //Making Random order 
      //  List<CardData> shuffledCards = duplicated.OrderBy(x => UnityEngine.Random.value).ToList();
        return duplicated;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}


