using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginPanel : BasePanel
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
            case "ButtonPlay":
                UIMgr.Instance.HidePanel<BeginPanel>();
                UIMgr.Instance.ShowPanel<ChoosePanel>();
                break;
            case "ButtonPeople":
                UIMgr.Instance.HidePanel<BeginPanel>();
                UIMgr.Instance.ShowPanel<PeoplePanel>();
                break;

            case "ButtonHelp":
                UIMgr.Instance.HidePanel<BeginPanel>();
                //加载地图即可。人物是在Level脚本里顺着地图初始化逻辑加载出来的。
                ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", "0101", (res) =>
                {
                    Instantiate(res);
                    EventDeal.Instance.grid = res.GetComponent<Grid>();
                });
                Level.Instance.setPO("0101");
                UIMgr.Instance.ShowPanel<PlayPanel>();
                break;

            case "ButtonQuit":
                Application.Quit();
                break;


        }
    }
}
