using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEvent : MonoBehaviour, IPointerClickHandler
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.name == "left") {

            TilemapEditor.instance.index = (TilemapEditor.instance.index == 0) ? TilemapEditor.instance.availableTiles.Length - 1 : TilemapEditor.instance.index - 1;

        }
        if (transform.name == "right") {

            TilemapEditor.instance.index = (TilemapEditor.instance.index == TilemapEditor.instance.availableTiles.Length - 1) ? 0 : TilemapEditor.instance.index + 1;
        }
        if (transform.name == "start")
        {
            TilemapEditor.instance.state = NewState.build;
            TilemapEditor.instance.makeTile(TilemapEditor.instance.GetMaxSortingOrder()+1);

        }
        if (transform.name == "end")
        {
            TilemapEditor.instance.state = NewState.nothing;

        }
        if (transform.name == "resize") {
            TilemapEditor.instance.state = NewState.select;
        }

    }
}
