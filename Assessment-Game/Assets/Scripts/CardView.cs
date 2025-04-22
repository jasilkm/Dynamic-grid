using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardView : MonoBehaviour
{

    #region public properties
    private CardData cardData;
    public Image carImage;

    #endregion

    #region private properties
    #endregion

    #region Events properties
    #endregion

    #region public Methods

    public void SetCardData(CardData cardData )
    {
        carImage.sprite = cardData.cardImage;

    }


    #endregion

    #region private properties
    #endregion





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
