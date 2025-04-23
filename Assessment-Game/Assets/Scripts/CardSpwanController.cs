using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class CardSpwanController : MonoBehaviour
{

    #region public properties

    #endregion

    #region private properties


    [SerializeField] private List<CardData> _cardDatas = new List<CardData>();
    [SerializeField] private CardView _cardView;
    private CardLayOutController _cardLayOutController;
    private Transform _gridTransform;
    private CardData _cardData;
    private int _totalCards;

    #endregion

    #region Events properties
    #endregion

    #region public Methods

    public void SpwanCards(int totalCards, Action<CardView> getCardHandler)
    {
        _totalCards = totalCards;
        
        _cardLayOutController.CreateLayout(totalCards);

        List<CardData> shuffledCards = GetShuffledCards();

        for (int i = 0; i < totalCards; i++)
        {
            GameObject obj = Instantiate(_cardView.gameObject, _gridTransform);

            CardView cardView = obj.GetComponent<CardView>();

            cardView.SetCardData(shuffledCards[i],(card)=>
            {

               // Debug.Log(card.cardData.cardID);
                getCardHandler(card);
            });
        }
    }


    #endregion

    #region private Methods

    private List<CardData> GetShuffledCards()
    {
        // Creating Uniq list for data  in each launch of the Game;
        List<CardData> tempCards = _cardDatas.OrderBy(x => UnityEngine.Random.value).ToList().Take(_totalCards / 2).ToList();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
