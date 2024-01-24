using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChangerOnClick : MonoBehaviour
{
    public Texture2D standard;
    public Texture2D clicked;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(this.standard, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) { Cursor.SetCursor(this.clicked, Vector2.zero, CursorMode.Auto); }
        else if (Input.GetMouseButtonUp(0)) { Cursor.SetCursor(this.standard, Vector2.zero, CursorMode.Auto); }
    }
}
