using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class ChoosePanel : BasePanel
{

    public GameObject PlayControl;

    public override void HideMe()
    {
    }

    public override void ShowMe()
    {
        UIMgr.Instance.HidePanel<PlayPanel>();
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
            case "Button0101":
                UIMgr.Instance.HidePanel<ChoosePanel>();
                //加载地图即可。人物是在Level脚本里顺着地图初始化逻辑加载出来的。
                ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", "0101", (res) => 
                {   
                    Instantiate(res);
                    EventDeal.Instance.grid = res.GetComponent<Grid>();
                });
                Level.Instance.setPO("0101");
                UIMgr.Instance.ShowPanel<PlayPanel>();
                break;
            case "Button0201":
                UIMgr.Instance.ShowPanel<PlayPanel>();
                UIMgr.Instance.HidePanel<ChoosePanel>();
                ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", "0201", (res) => 
                {   
                    Instantiate(res);
                    EventDeal.Instance.grid = res.GetComponent<Grid>();
                }); 
                Level.Instance.setPO("0201");
                break;

            case "Button0202":
                UIMgr.Instance.ShowPanel<PlayPanel>();
                UIMgr.Instance.HidePanel<ChoosePanel>();
                ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", "0202", (res) =>
                {
                    Instantiate(res);
                    EventDeal.Instance.grid = res.GetComponent<Grid>();
                });
                Level.Instance.setPO("0202");
                break;
            case "Button0203":
                UIMgr.Instance.ShowPanel<PlayPanel>();
                UIMgr.Instance.HidePanel<ChoosePanel>();
                ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", "0203", (res) =>
                {
                    Instantiate(res);
                    EventDeal.Instance.grid = res.GetComponent<Grid>();
                });
                Level.Instance.setPO("0203");
                break;
            case "Button0204":
                UIMgr.Instance.ShowPanel<PlayPanel>();
                UIMgr.Instance.HidePanel<ChoosePanel>();
                ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", "0204", (res) =>
                {
                    Instantiate(res);
                    EventDeal.Instance.grid = res.GetComponent<Grid>();
                });
                Level.Instance.setPO("0204");
                break;
            case "Button0301":
                UIMgr.Instance.ShowPanel<PlayPanel>();
                UIMgr.Instance.HidePanel<ChoosePanel>();
                ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", "0301", (res) =>
                {
                    Instantiate(res);
                    EventDeal.Instance.grid = res.GetComponent<Grid>();
                });
                Level.Instance.setPO("0301");
                break;
            case "Button0302":
                UIMgr.Instance.ShowPanel<PlayPanel>();
                UIMgr.Instance.HidePanel<ChoosePanel>();
                ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", "0302", (res) =>
                {
                    Instantiate(res);
                    EventDeal.Instance.grid = res.GetComponent<Grid>();
                });
                Level.Instance.setPO("0302");
                break;
            case "Button0303":
                UIMgr.Instance.ShowPanel<PlayPanel>();
                UIMgr.Instance.HidePanel<ChoosePanel>();
                ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", "0303", (res) =>
                {
                    Instantiate(res);
                    EventDeal.Instance.grid = res.GetComponent<Grid>();
                });
                Level.Instance.setPO("0303");
                break;
            case "Button0304":
                UIMgr.Instance.ShowPanel<PlayPanel>();
                UIMgr.Instance.HidePanel<ChoosePanel>();
                ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", "0304", (res) =>
                {
                    Instantiate(res);
                    EventDeal.Instance.grid = res.GetComponent<Grid>();
                });
                Level.Instance.setPO("0304");
                break;
            case "Button0305":
                UIMgr.Instance.ShowPanel<PlayPanel>();
                UIMgr.Instance.HidePanel<ChoosePanel>();
                ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", "0305", (res) =>
                {
                    Instantiate(res);
                    EventDeal.Instance.grid = res.GetComponent<Grid>();
                });
                Level.Instance.setPO("0305");
                break;
            case "Button0401":
                UIMgr.Instance.ShowPanel<PlayPanel>();
                UIMgr.Instance.HidePanel<ChoosePanel>();
                ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", "0401", (res) =>
                {
                    Instantiate(res);
                    EventDeal.Instance.grid = res.GetComponent<Grid>();
                });
                Level.Instance.setPO("0401");
                break;

            case "Button0403":
                UIMgr.Instance.ShowPanel<PlayPanel>();
                UIMgr.Instance.HidePanel<ChoosePanel>();
                ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", "0403", (res) =>
                {
                    Instantiate(res);
                    EventDeal.Instance.grid = res.GetComponent<Grid>();
                });
                Level.Instance.setPO("0403");
                break;

            case "ButtonBack":
                UIMgr.Instance.ShowPanel<BeginPanel>();
                UIMgr.Instance.HidePanel<ChoosePanel>();
                break;

        }
    }

}
