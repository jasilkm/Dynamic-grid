using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private CardSpwanController _spwanController;
    private List<CardView> currentSelections = new List<CardView>();

    // Start is called before the first frame update
    void Start()
    {
        _spwanController = FindAnyObjectByType<CardSpwanController>();
        StartGame();
    }

    void Update()
    {
        
    }


    IEnumerator _StartGame()
    {
        yield return new WaitForSeconds(0.1f);
        _spwanController.SpwanCards(10, (cardData) =>
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
        currentSelections.Add(card);

        if (currentSelections.Count == 2)
        {
            CardView first = currentSelections[0];
            CardView second = currentSelections[1];

            StartCoroutine(CheckMatchAsync(first, second));

            currentSelections.Clear(); 
        }

    }


    IEnumerator CheckMatchAsync(CardView card1, CardView card2)
    {
        yield return new WaitForSeconds(1);

        if (card1.cardData.cardID == card2.cardData.cardID)
        {

        }
        else
        {
            card1.BackFlipCard();
            card2.BackFlipCard();
        }

    }



}
