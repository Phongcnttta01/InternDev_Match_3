using DG.Tweening;
using System.Linq;
using UnityEngine;

public class AutoPlay : MonoBehaviour
{
    public void AutoPlayToWin()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            
            var cells = BoardManager.Instance.cellInBoards;
            if (cells.Count == 0) return;

            var groups = cells
                .GroupBy(c => c._data.id)
                .OrderBy(g => g.Key)
                .ToList();

            float delay = 0f;

            foreach (var group in groups)
            {
                foreach (var cell in group)
                {
                    var targetCell = cell;
                    DOVirtual.DelayedCall(delay, () =>
                    {
                        BoardManager.Instance.AutoMoveCellToBottom(targetCell);
                    });

                    delay += 1f;
                }
            }
            
        });

        
    }




    public void AutoPlayToLose()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            

            Sequence seq = DOTween.Sequence();

            for (int i = 0; i < BoardManager.Instance.cellInBoards.Count; i++)
            {
                int index = i;
                seq.AppendCallback(() =>
                {
                    if (index < BoardManager.Instance.cellInBoards.Count && BoardManager.Instance.canClick)
                    {
                        var cell = BoardManager.Instance.cellInBoards[index];
                        if (cell != null)
                        {
                            BoardManager.Instance.AutoMoveCellToBottom(cell);
                        }
                    }
                });

                seq.AppendInterval(0.1f);


                seq.AppendCallback(() =>
                {
                    if (BoardManager.Instance.bottomBoard.isFullValid)
                    {
                        Debug.Log("AutoPlayToLose: Đã thua!");
                        seq.Kill();
                    }
                });
            }
            
        });
       
    }
}
