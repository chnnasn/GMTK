using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
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

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if (transform.name == "SaveThis") {

    //    }
    //    if (transform.name == "save") {
    //        //SavePanel.instance.SaveTilemapData();
    //    }
    //    if (transform.name == "remove") {
    //        //SavePanel.instance.SaveTilemapData();
    //    }

    //}

    public void SaveThis() {
        SavePanel.instance.SaveTilemapData();
    }

    public void ReMove() {
        SavePanel.instance.ReMove(transform.parent.parent.GetSiblingIndex());

    }

    public void OldGame()
    {
        

    }

}
