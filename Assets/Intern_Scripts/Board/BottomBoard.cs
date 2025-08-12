using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BottomBoard : MonoBehaviour
{
    public List<_CellInBottom> answer = new List<_CellInBottom>();

    public List<_Cell> answerIsValid = new List<_Cell>();

    public bool isFullValid = false;

    public void Update()
    {
        ThreeMerge();
    }

    public void ThreeMerge()
    {
        var groups = answer
            .Where(c => c.cellIn != null)
            .GroupBy(c => c.cellIn._data.id);

        bool foundMerge = false; 

        foreach (var group in groups)
        {
            if (group.Count() >= 3)
            {
                foundMerge = true;
                BoardManager.Instance.canClick = false;
                int count = 0;

                foreach (var cell in group)
                {
                    if (cell.cellIn != null)
                    {
                        cell.cellIn.transform
                            .DOScale(Vector3.zero, 0.3f)
                            .SetEase(Ease.OutBack)
                            .OnComplete(() =>
                            {
                                RemoveCell(cell);
                            });
                    }

                    count++;
                    if (count >= 3)
                    {
                        DOVirtual.DelayedCall(0.35f, () =>
                        {
                            BoardManager.Instance.canClick = true;
                        });
                        break;
                    }
                }
            }
        }

      
        if (!foundMerge && answer.All(c => c.isValid))
        {
            isFullValid = true;
        }
        else
        {
            isFullValid = false;
        }
    }



    public void RemoveCell(_CellInBottom cell)
    {
        answerIsValid.Remove(cell.cellIn);
        Destroy(cell.cellIn.gameObject);
        cell.isValid = false;
    }

    

}
