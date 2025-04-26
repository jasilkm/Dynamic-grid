using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLayoutController : MonoBehaviour
{

    [SerializeField] private GameObject _card;

    float containerWidth;
    float containerHeight;
    private float spacing = .06f;
    private GameObject[,] gridArray;


    Vector3 bottomLeft;
    Vector3 topRight;
    float worldWidth;
    float worldHeight;


    public GridInfo CreateGridLayOut(int totalCells)
    {
        int bestCols = 1;
        int bestRows = totalCells;

        

        if (gameObject.transform.parent != null)
        {
            Transform parentTransform = gameObject.transform.parent;
            Vector2 parentSize = parentTransform.gameObject.GetComponent<SpriteRenderer>().size;
            worldWidth = parentSize.x ;
            worldHeight = parentSize.y ;

            Debug.Log("called parent");
        }
        else
        {
             bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
             topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));
             worldWidth = (topRight.x - bottomLeft.x)-1;
             worldHeight = (topRight.y - bottomLeft.y)-1;
            Debug.Log("called Screen");

        }


        for (int i = 1; i <= totalCells; i++)
        {
            // getting divisor  so we can confirm that there wont be any empty sell
            if (totalCells % i == 0)
            {
                int cols = i;
                int rows = totalCells / i;
                // setting grid based on  landscape or portrite
           
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
        float cardWidth = (worldWidth - totalSpacingX) / bestCols;
        float cardHeight = (worldHeight - totalSpacingY) / bestRows;
        return new GridInfo(bestCols, bestRows, new Vector2(cardWidth, cardHeight), new Vector2(worldWidth, worldHeight));
    }


}

public class GridInfo
{
    public int Columns;
    public int Rows;
    public Vector2 CardSize;
    public Vector2 WidgetSize;

    public GridInfo(int cloumns,int rows, Vector2 cardSize, Vector2 widgetSize)
    {
        Columns = cloumns;
        Rows = rows;
        CardSize = cardSize;
        WidgetSize = widgetSize;
    }


}