using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeoplePanel : BasePanel
{
    public override void HideMe()
    {
    }

    public override void ShowMe()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void ClickBtn(string btnName)
    {
        switch (btnName)
        {
            case "ButtonBack":

                UIMgr.Instance.ShowPanel<BeginPanel>();
                UIMgr.Instance.HidePanel<PeoplePanel>();
                break;

        }
    }


}
