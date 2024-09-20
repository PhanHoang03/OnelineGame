using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] protected int columns = 0;
    [SerializeField] protected int rows = 0;
    [SerializeField] protected GameObject gridSquare;
    [SerializeField] protected Vector2 startPosition = new Vector2(0f, 0f);
    [SerializeField] protected float squareScale = 0.5f;
    [SerializeField] protected float squareOffset = 0.0f;

    private Vector2 _offset = new Vector2(0f, 0f);
    private List<GameObject> _gridSquares = new List<GameObject>();

    void Start()
    {
        this.CreateGrid();
    }

    public void LoadLevel (LevelSO level)
    {   
        int squareIndex = 0;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                bool state = level.board[row]._row[col];
                //this._gridSquares[squareIndex].GetComponent<GridSquare>().SetUp();
                //this._gridSquares[squareIndex].GetComponent<GridSquare>().gameObject.SetActive(state);
                squareIndex++;
            }
        }
    }

    public void ResetGrid()
    {
        int squareIndex = 0;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                //this._gridSquares[squareIndex].GetComponent<GridSquare>().SetUp();
                squareIndex++;
            }
        }
    }

    protected virtual void CreateGrid()
    {
        this.SqpawnGridSquares();
        this.SetGridSquaresPosition();
    }

    protected virtual void SqpawnGridSquares()
    {
        int squareIndex = 0;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Debug.Log(row);
                Debug.Log(rows);
                Debug.Log(col);
                Debug.Log(columns);
                this._gridSquares.Add(Instantiate(gridSquare));
                this._gridSquares[squareIndex].transform.SetParent(this.transform);
                this._gridSquares[squareIndex].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                //this._gridSquares[squareIndex].GetComponent<GridSquare>().SetUp();
                squareIndex++;
            }
        }
    }

    protected virtual void SetGridSquaresPosition()
    {
        int rowNum = 0;
        int colNum = 0;

        var squareRect = _gridSquares[0].GetComponent<RectTransform>();

        this._offset.x = squareRect.rect.width * squareRect.transform.localScale.x + this.squareOffset; 
        this._offset.y = squareRect.rect.height * squareRect.transform.localScale.y + this.squareOffset; 

        foreach (GameObject square in _gridSquares)
        {
            if (colNum == columns)
            {
                colNum = 0;
                rowNum++;
            }

            var posX = this.startPosition.x + this._offset.x * colNum;
            var posY = this.startPosition.y + this._offset.y * rowNum;

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, -posY);
            square.GetComponent<RectTransform>().localPosition = new Vector3(posX, -posY, 0f);
            colNum++;
        }
    }

    public int GetSquareIndex(int row, int col)
    {
        int _index = row * columns + col;
        return _index;
    }
}
