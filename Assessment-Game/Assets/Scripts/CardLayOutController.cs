
using UnityEngine;
using UnityEngine.UI;
//[ExecuteAlways]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GridLayoutGroup))]
public class CardLayOutController : MonoBehaviour
{

    #region public properties
    public float spacing = 40f;
    public Transform GridTransform;
    public RectTransform containerRect;
    public float paddingX = 120f;
    public float paddingY = 120f;
    #endregion

    #region Private properties
    private GridLayoutGroup gridLayout;
    #endregion


    #region Unity Methods
    private void Awake()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        Debug.Log("CardLayOutController");
    }

    #endregion


    #region Public Methods
    public void CreateLayout(int totalCells)
    {

        int bestCols = 1;
        int bestRows = totalCells;

        float containerWidth = containerRect.rect.width - paddingX;
        float containerHeight = containerRect.rect.height - paddingY;

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
