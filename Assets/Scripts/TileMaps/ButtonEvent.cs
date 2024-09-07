using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEvent : MonoBehaviour, IPointerClickHandler
{


    // Start is called before the first frame update
    void Start()
    {
        UIMgr.Instance.ShowPanel<BuildPanel>();
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        

    }
}
