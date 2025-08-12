using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Cell : MonoBehaviour
{
    public Data _data;
    public bool isChoice;

    public void Awake()
    {
        isChoice = false;
    }
    public void Init(Data data)
    {
        _data = data;

        if (_data != null && _data.image != null)
        {
            GameObject obj = Instantiate(_data.image, transform.position, Quaternion.identity);
            obj.transform.SetParent(gameObject.transform, false);
            obj.transform.position = gameObject.transform.position;
        }
    }

    public void OnMouseDown()
    {
        if(BoardManager.Instance.canClick && !GameManagerIntern.Instance.isAuto && isChoice == false)
            BoardManager.Instance.MoveCellToBottom(this);
        
    }


}
