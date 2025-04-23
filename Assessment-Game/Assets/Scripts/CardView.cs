using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class CardView : MonoBehaviour
{

    #region private properties
    [SerializeField]private Image _frontImage;
    [SerializeField]private Button _selectBtn;
    [SerializeField]private Image _backImage;
    private Action<CardView> _selectedCardHandler;

    #endregion

    #region public properties
    [HideInInspector]public CardData cardData;
    public bool isFlipped = false;

    #endregion

    #region Events properties
    #endregion

    #region Private properties
    public void OnCardSelected()
    {

        if (!isFlipped)
        {
            isFlipped = true;
            _selectedCardHandler(this);
            StartCoroutine(FlipCard(false));
        }
    }


    public void FlipCards()
    {
        isFlipped = true;
        StartCoroutine(InitialFlipCard(false));

    }


    public void BackFlipCard()
    {
        
        StartCoroutine(FlipCardToNormal(true));
        
    }


    IEnumerator InitialFlipCard(bool showFront)
    {
        yield return new WaitForSeconds(1f);
        float duration = 0.2f;
        float time = 0f;
        Vector3 start = transform.localScale;
        Vector3 end = new Vector3(-start.x, start.y, start.z);

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(start, end, time / duration);
            time += Time.deltaTime;

            if (transform.localScale.x < 0)
            {
                _backImage.gameObject.SetActive(showFront);
            }
            yield return null;
        }
        transform.localScale = end;
    }



    IEnumerator FlipCard(bool showFront)
    {
        float duration = 0.2f;
        float time = 0f;
        Vector3 start = transform.localScale;
        Vector3 end = new Vector3(-start.x, start.y, start.z); 

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(start, end, time / duration);
            time += Time.deltaTime;

            if (transform.localScale.x < 0)
            {
                _backImage.gameObject.SetActive(showFront);
            }
            yield return null;
        }
        transform.localScale = end;
    }



    IEnumerator FlipCardToNormal(bool showFront)
    {
        float duration = 0.2f;
        float time = 0f;
        Vector3 start = transform.localScale;
        Vector3 end = new Vector3(1, start.y, start.z); // flip X

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(start, end, time / duration);
            time += Time.deltaTime;

            if (transform.localScale.x> 0)
            {
                _backImage.gameObject.SetActive(showFront);
            }
            yield return null;
        }

        transform.localScale = end;
        isFlipped = false;
    }

    #endregion



    #region public Methods

    public void SetCardData(CardData cardDataArg, Action<CardView> selectedCardHandler)
    {
        this.cardData = cardDataArg;
        _selectedCardHandler = selectedCardHandler;
        _frontImage.sprite = cardData.cardImage;

    }


    #endregion

    #region Unity Methods
    private void Awake()
    {
       _selectBtn.onClick.AddListener(OnCardSelected);
    }



    void Start()
    {
     
    }

    void Update()
    {

    }


    #endregion






}
