using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerIntern : Singleton<GameManagerIntern>
{
    public bool isWin = false;

    public bool isLose = false;

    public bool isFirstTime = true;

    public bool isAuto = false;

    public AutoPlay autoPlay;

    public State state;

    public void Awake()
    {
        state = State.Menu;
    }

    public void Update()
    {

        if (state == State.Play)
        {
            CheckWin();

            if (isWin)
            {
                UIManagerIntern.Instance.winUI.SetActive(true);
                BoardManager.Instance.canClick = true;
                GameManagerIntern.Instance.isAuto = false;
            }

            if (isLose)
            {
                UIManagerIntern.Instance.loseUI.SetActive(true);
                BoardManager.Instance.canClick = true;
                GameManagerIntern.Instance.isAuto = false;
            }
        }

        if(state == State.Menu)
        {
            isLose = false;
            isWin = false;
        }

       
    }

    public void CheckWin()
    {
        if (BoardManager.Instance.cellInBoards.Count <= 0) isWin = true;

        if (BoardManager.Instance.bottomBoard.isFullValid == true) isLose = true;
    }

    public void PlayGame()
    {
        BoardManager.Instance.CreateBoard();
        BoardManager.Instance.bottom.SetActive(true);
    }

    public void AutoWin()
    {
        autoPlay.AutoPlayToWin();
    }

    public void AutoLose()
    {
        autoPlay.AutoPlayToLose();
    }

}

public enum State
{
    Menu,
    Play
}
