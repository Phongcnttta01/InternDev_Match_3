using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerIntern : Singleton<UIManagerIntern>
{
    public GameObject mainMenu;

    public GameObject winUI;

    public GameObject loseUI;

    public void Play()
    {
        mainMenu.SetActive(false);
        
        GameManagerIntern.Instance.PlayGame();
    }

    public void WinMainMenu()
    {
        GameManagerIntern.Instance.state = State.Menu;
        winUI.SetActive(false);
        BoardManager.Instance.Clear();
        mainMenu.SetActive(true);
    }

    public void LoseMainMenu()
    {
        GameManagerIntern.Instance.state = State.Menu;
        loseUI.SetActive(false);
        BoardManager.Instance.Clear();
        mainMenu.SetActive(true);
    }

    public void AutoWinGame()
    {
        mainMenu.SetActive(false);

        GameManagerIntern.Instance.PlayGame();

        BoardManager.Instance.canClick = false;

        GameManagerIntern.Instance.isAuto = true;

        GameManagerIntern.Instance.AutoWin();
    }

    public void AutoLoseGame()
    {
        mainMenu.SetActive(false);

        GameManagerIntern.Instance.PlayGame();

        BoardManager.Instance.canClick = false;

        GameManagerIntern.Instance.isAuto = true;

        GameManagerIntern.Instance.AutoLose();

    }
}
