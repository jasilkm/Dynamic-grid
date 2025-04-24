
using UnityEngine;
using UnityEngine.UI;
//[ExecuteAlways]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GridLayoutGroup))]
public class CardLayOutController : MonoBehaviour
{

    #region public properties
   [SerializeField] private float spacing = 40f;
   [SerializeField] private Transform GridTransform;
   [SerializeField] private RectTransform containerRect;
   [SerializeField] private RectOffset rectOffset;
   [SerializeField] private float xOffset;
   [SerializeField] private float yOffset;

    #endregion

    #region Private properties
    private GridLayoutGroup gridLayout;
    #endregion


    #region Unity Methods
    private void Awake()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        containerRect.anchoredPosition = new Vector2(0, -150);
    }

    private void Start()
    {
        
    }

    #endregion


    #region Public Methods
    public void CreateLayout(int totalCells)
    {

        int bestCols = 1;
        int bestRows = totalCells;

        float containerWidth = containerRect.rect.width - (rectOffset.left+rectOffset.right+ xOffset);
        float containerHeight = containerRect.rect.height - (rectOffset.top + rectOffset.bottom + yOffset);

        for (int i = 1; i <= totalCells; i++)
        {
            if (totalCells % i == 0)
            {
                int cols = i;

                int rows = totalCells / i;

                if (Mathf.Abs(cols - rows) < Mathf.Abs(bestCols - bestRows))
                {

                    bestCols = cols;
                    bestRows = rows;
                }
            }

        }

        // Calculating total spaces 
        float totalSpacingX = spacing * (bestCols - 1);
        float totalSpacingY = spacing * (bestRows - 1);

        // Calculating Card Sizes 
        float cardWidth = (containerWidth - totalSpacingX) / bestCols;
        float cardheight = (containerHeight - totalSpacingY) / bestRows;

        // Setting Grid Properties
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = bestCols;
        gridLayout.cellSize = new Vector2(cardWidth, cardheight);
        gridLayout.spacing = new Vector2(spacing, spacing);
        gridLayout.padding = new RectOffset(rectOffset.left, rectOffset.right, rectOffset.top, rectOffset.bottom);
    }





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
