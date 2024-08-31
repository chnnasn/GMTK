using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : BasePanel
{

    private string levelName = "";
    public override void HideMe()
    {

    }

    public override void ShowMe()
    {
        Grid levelGrid = FindObjectOfType<Grid>();

        GameObject levelNow = levelGrid.gameObject;
        levelName = levelNow.name;
        int lengthToKeep = levelName.Length - 7;//此处的7是为了截掉(clone)字样
        levelName = levelName.Substring(0, lengthToKeep);
        print($"当前关卡名字是111111：{levelName}");
        PlayControl player = FindObjectOfType<PlayControl>();
        GameObject playerNow = player.gameObject;

        CameraBindle mainCamera = FindObjectOfType<CameraBindle>();
        GameObject cameraNow = mainCamera.gameObject;



        if (levelGrid != null)
        {
            Destroy(levelNow);
        }
        if (playerNow != null)
        {
            Destroy(playerNow);
        }
        if (cameraNow != null)
        {
            Destroy(cameraNow);
        }
        if (Level.Instance.spriteObject != null)
        {
            Destroy(Level.Instance.spriteObject);
        }

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

                UIMgr.Instance.ShowPanel<ChoosePanel>();
                UIMgr.Instance.HidePanel<WinPanel>();
                break;
            case "ButtonAgain":
                //还没写完
                print("你点了BUttonAgain!");
                UIMgr.Instance.HidePanel<WinPanel>();
                ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", levelName, (res) =>
                {
                    Instantiate(res);
                    EventDeal.Instance.grid = res.GetComponent<Grid>();
                });
                Level.Instance.setPO(levelName);
                UIMgr.Instance.ShowPanel<PlayPanel>();
                
                break;
            case "ButtonNext":
                //找一下本关的总索引
                int num =   LevelDataMgr.Instance.levels.IndexOf(levelName);

                print($"当前关卡名字是：{levelName}");
                UIMgr.Instance.HidePanel<WinPanel>();
                print($"当前关卡索引值是：{num}");
                //找下一个位置的关卡，然后加载。
                ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", LevelDataMgr.Instance.levels[++num], (res) =>
                {
                    Instantiate(res);
                    EventDeal.Instance.grid = res.GetComponent<Grid>();
                });
                Level.Instance.setPO(LevelDataMgr.Instance.levels[++num]);

                UIMgr.Instance.ShowPanel<PlayPanel>();
                break;

        }
    }
}
