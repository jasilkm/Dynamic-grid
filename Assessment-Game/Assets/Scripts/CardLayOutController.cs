
using UnityEngine;
using UnityEngine.UI;
//[ExecuteAlways]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GridLayoutGroup))]
public class CardLayOutController : MonoBehaviour
{

    #region public properties
   [SerializeField] private float spacing = 20f;
   [SerializeField] private Transform GridTransform;
   [SerializeField] private RectTransform containerRect;
   [SerializeField] private RectOffset rectOffset;
   [SerializeField] private float xOffset;
   [SerializeField] private float yOffset;

    #endregion

    #region Private properties
    public GridLayoutGroup gridLayout;
    #endregion


    #region Unity Methods
    private void Awake()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        containerRect.anchoredPosition = new Vector2(0, -150);
    }

    #endregion


    #region Public Methods
    
    public void CreateLayout(int totalCells)
    {
      

        gridLayout.enabled = true;
        if (totalCells <= 0) {

            Debug.LogError($"[CardLayOutController] Invalid total cell count: {totalCells}. Must be greater than 0 to create a layout.");

            return;
        } 

        int bestCols = 1;
        int bestRows = totalCells;

        float containerWidth = containerRect.rect.width - (rectOffset.left + rectOffset.right + xOffset);
        float containerHeight = containerRect.rect.height - (rectOffset.top + rectOffset.bottom + yOffset);

        
        for (int i = 1; i <= totalCells; i++)
        {
            // getting divisor  so we can confirm that there wont be any empty sell
            if (totalCells % i == 0)
            {
                int cols = i;
                int rows = totalCells / i;
                // setting grid based on  landscape or portrite
                float aspect = (float)cols / rows;
                float targetAspect = containerWidth / containerHeight;

                if (Mathf.Abs(cols - rows) < Mathf.Abs(bestCols - bestRows))
                {
                    bestCols = cols;
                    bestRows = rows;
                }

            }
        }

        // Calculating total spacing  using for all cards
        float totalSpacingX = spacing * (bestCols - 1);
        float totalSpacingY = spacing * (bestRows - 1);

        // Calculating card sizes  this will assign to cell width and height
        float cardWidth = (containerWidth - totalSpacingX) / bestCols;
        float cardHeight = (containerHeight - totalSpacingY) / bestRows;

        // Setting grid properties
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = bestCols;
        gridLayout.cellSize = new Vector2(cardWidth, cardHeight);
        gridLayout.spacing = new Vector2(spacing, spacing);
        gridLayout.padding = new RectOffset(rectOffset.left, rectOffset.right, rectOffset.top, rectOffset.bottom);
    }

    //Getting Grid Transform
    public Transform GetGridTransform() => GridTransform;

    #endregion








    #region Editor Methods

    // Resetting Scale properites 
    void OnValidate()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            RectTransform rect = GetComponent<RectTransform>();
            if (rect != null)
            {
                this.transform.localScale = Vector3.one;
                this.transform.localPosition = Vector3.one;
                rect.localScale = Vector3.one;
                rect.localRotation = Quaternion.identity;

            }
        }
    }
    #endregion




}
