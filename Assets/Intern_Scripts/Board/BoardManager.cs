using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    [Header("Trên Trái")]
    public int rowsTopLeft  ;
    public int colsTopLeft ;

    [Header("Trên Phải")]
    public int rowsTopRight ;
    public int colsTopRight ;

    [Header("Dưới Trái")]
    public int rowsBottomLeft ;
    public int colsBottomLeft ;

    [Header("Dưới Phải")]
    public int rowsBottomRight ;
    public int colsBottomRight ;

    public GameObject cellPrefab;
    public GameObject board;
    public GameObject bottom;
    public BottomBoard bottomBoard;
    private float cellSize = 1f;
    public bool canClick = true;

    public List<Data> datas = new List<Data>();
    

    public List<_Cell> cellInBoards = new List<_Cell>();

    public List<GameObject> panelCell = new List<GameObject>();

  

    public void CreateBoard()
    {
        if (cellPrefab == null)
        {
            Debug.LogError("Chưa gán Cell Prefab!");
            return;
        }

       
        int totalCells =
            rowsTopLeft * colsTopLeft +
            rowsTopRight * colsTopRight +
            rowsBottomLeft * colsBottomLeft +
            rowsBottomRight * colsBottomRight;

        
        int perData = (totalCells / datas.Count);
        perData = (perData / 3) * 3;

       
        List<Data> dataPool = new List<Data>();
        foreach (var d in datas)
        {
            for (int i = 0; i < perData; i++)
                dataPool.Add(d);
        }

        
        for (int i = 0; i < dataPool.Count; i++)
        {
            int rand = Random.Range(i, dataPool.Count);
            var temp = dataPool[i];
            dataPool[i] = dataPool[rand];
            dataPool[rand] = temp;
        }

        
        int poolIndex = 0;
        CreateQuadrant(-1, 1, rowsTopLeft, colsTopLeft, dataPool, ref poolIndex);
        CreateQuadrant(1, 1, rowsTopRight, colsTopRight, dataPool, ref poolIndex);
        CreateQuadrant(-1, -1, rowsBottomLeft, colsBottomLeft, dataPool, ref poolIndex);
        CreateQuadrant(1, -1, rowsBottomRight, colsBottomRight, dataPool, ref poolIndex);

        DOVirtual.DelayedCall(0.7f, () =>
        {
            GameManagerIntern.Instance.state = State.Play;
        });
        
    }

    private void CreateQuadrant(int xDir, int yDir, int rows, int cols, List<Data> pool, ref int poolIndex)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                float x = (col + 0.5f) * cellSize * xDir;
                float y = (row + 0.5f) * cellSize * yDir;
                Vector3 position = new Vector3(x, y, 0f);

                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                //cell.transform.SetParent(transform, false);
                panelCell.Add(cell);
                _Cell cellScript = cell.GetComponent<_Cell>();
                if (cellScript != null)
                {
                    Data d = pool[poolIndex++];
                    cellScript._data = d;
                    cellScript.Init(d);
                    cellInBoards.Add(cellScript);
                }

                Vector3 scale = cell.transform.localScale;
                cell.transform.localScale = Vector3.zero;
                cell.transform.DOScale(scale, 0.5f).SetEase(Ease.InBack);
            }
        }
    }


    public void MoveCellToBottom(_Cell cell)
    {
        canClick = false;

        foreach (var _cell in bottomBoard.answer)
        {
            if (_cell.isValid == false)
            {
                cell.isChoice = true;

                Vector3 originalScale = cell.transform.localScale;
                Vector3 biggerScale = originalScale * 1.2f;
                Sequence seq = DOTween.Sequence();

                seq.Append(cell.transform.DOScale(biggerScale, 0.2f).SetEase(Ease.OutQuad)) 
                   .Append(cell.transform.DOMove(_cell.transform.position, 0.3f).SetEase(Ease.InOutQuad)) 
                   .Join(cell.transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutQuad)) 
                   .OnComplete(() =>
                   {
                       _cell.cellIn = cell;
                       bottomBoard.answerIsValid.Add(cell);
                       cellInBoards.Remove(cell);
                       _cell.isValid = true;
                   });

                break;
            }
        }

        DOVirtual.DelayedCall(0.5f, () =>
        {
            canClick = true;
        });
    }

    public void AutoMoveCellToBottom(_Cell cell)
    {
        

        foreach (var _cell in bottomBoard.answer)
        {
            if (_cell.isValid == false)
            {
                cell.isChoice = true;

                Vector3 originalScale = cell.transform.localScale;
                Vector3 biggerScale = originalScale * 1.2f;
                Sequence seq = DOTween.Sequence();

                seq.Append(cell.transform.DOScale(biggerScale, 0.2f).SetEase(Ease.OutQuad))
                   .Append(cell.transform.DOMove(_cell.transform.position, 0.3f).SetEase(Ease.InOutQuad))
                   .Join(cell.transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutQuad))
                   .OnComplete(() =>
                   {
                       _cell.cellIn = cell;
                       bottomBoard.answerIsValid.Add(cell);
                       cellInBoards.Remove(cell);
                       _cell.isValid = true;
                   });

                break;
            }
        }

        
    }


    public void Clear()
    {
        
        foreach (var cell in panelCell)
        {
            if (cell != null)
                Destroy(cell);
        }
        panelCell.Clear();
        cellInBoards.Clear();

        
        foreach (var _cell in bottomBoard.answer)
        {
            if (_cell.cellIn != null)
            {
                Destroy(_cell.cellIn.gameObject);
                _cell.cellIn = null;
            }
            _cell.isValid = false;
        }
        bottomBoard.answerIsValid.Clear();

        bottomBoard.isFullValid = false;
        bottom.SetActive(false);
    }



}
