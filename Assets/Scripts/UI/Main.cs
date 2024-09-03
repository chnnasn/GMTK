using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    public Texture2D cursorDefault;
    public Texture2D cursorPress;
    public Texture2D cursorError;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
        UIMgr.Instance.ShowPanel<PassPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        //设置鼠标指针
        if (Input.GetMouseButtonDown(0))
        {  // 左键按下
            Cursor.SetCursor(cursorPress, Vector2.zero, CursorMode.Auto);
        }
        if (Input.GetMouseButtonUp(0))
        {  // 左键松开
            Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
        }
    }
}
