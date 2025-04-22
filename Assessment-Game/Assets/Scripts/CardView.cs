using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardView : MonoBehaviour
{

    #region private properties
    private CardData cardData;
    [SerializeField]private Image _frontImage;
    [SerializeField]private Button _selectBtn;
    [SerializeField]private Image _backImage;
    private bool isFlipped = false;

    #endregion

    #region public properties
    #endregion

    #region Events properties
    #endregion

    #region Private properties
    public void FlipImage()
    {
        StartCoroutine(FlipCard(false));
        StartCoroutine(FlipCardToNormal(true));
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

        yield return new WaitForSeconds(2);
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
        isFlipped = showFront;

    }





    #endregion



    #region public Methods

    public void SetCardData(CardData cardData )
    {
        _frontImage.sprite = cardData.cardImage;

    }


    #endregion

    #region Unity Methods
    private void Awake()
    {
       _selectBtn.onClick.AddListener(FlipImage);
    }



    void Start()
    {
     
    }

    void Update()
    {

    }


    #endregion






}
