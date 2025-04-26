using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class CardViewItem : MonoBehaviour
{

    #region private properties
    [SerializeField] private SpriteRenderer frontImage;
    [SerializeField] private SpriteRenderer backImage;
    private Action<CardViewItem> _selectedCardHandler;
    [SerializeField] private BoxCollider2D _collider;

    #endregion

    #region public properties
    [HideInInspector]public CardData cardData;
    public bool isFlipped = false;
    public bool isMatched = false;

    #endregion

    #region Events properties
    #endregion

    #region Private properties
    public void OnCardSelected()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.cardSelectedSound);


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
               backImage.gameObject.SetActive(showFront);
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
        _collider.enabled = false;
        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(start, end, time / duration);
            time += Time.deltaTime;

            if (transform.localScale.x < 0)
            {
                backImage.gameObject.SetActive(showFront);
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
                backImage.gameObject.SetActive(showFront);
            }
            yield return null;
        }

        transform.localScale = end;
        isFlipped = false;
        _collider.enabled = true;
    }

    #endregion



    #region public Methods

    public void SetCardData(CardData cardDataArg, Action<CardViewItem> selectedCardHandler)
    {
        this.cardData = cardDataArg;
        _selectedCardHandler = selectedCardHandler;
        frontImage.sprite = cardData.cardImage;
        backImage.size = this.GetComponent<SpriteRenderer>().size;
        SetAspectRatio( this.GetComponent<SpriteRenderer>().size);
        _collider.size = this.GetComponent<SpriteRenderer>().size;
    }

    public void SetScale()
    {
        this.gameObject.transform.DOScale(new Vector3(.001f, .001f, .001f),.1f);
    }



    #endregion

    #region Unity Methods
   

    private void OnMouseDown()
    {
        OnCardSelected();
    }


    #endregion



    void SetAspectRatio(Vector2 size)
    {

        if (size.x < size.y)
        {

            float targetWidth = size.x; // Desired width in world units
            float aspectRatio = frontImage.sprite.bounds.size.y / frontImage.sprite.bounds.size.x;
            float targetHeight = targetWidth * aspectRatio;
            frontImage.size = new Vector2(targetWidth, targetHeight);

        }
        else
        {
            float targetHeight = size.y; // Desired height in world units
            float aspectRatio = frontImage.sprite.bounds.size.x / frontImage.sprite.bounds.size.y;
            float targetWidth = targetHeight * aspectRatio;
            frontImage.size = new Vector2(targetWidth, targetHeight);
        }

    }



}
