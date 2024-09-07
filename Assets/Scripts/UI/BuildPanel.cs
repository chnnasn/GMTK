using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPanel : BasePanel
{

    private string levelName = "";
    public static BuildPanel Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 如果希望这个实例在场景切换时保持存在
        }
        else
        {
            Destroy(gameObject); // 如果已经有一个实例存在，销毁当前对象
        }

        base.Awake();
    }
    public GameObject imageImp; //门
    public GameObject imageCha; //角色
    public List<float> rate = new List<float>() { 0.4f, 0.55f, 0.7f, 0.85f, 1f };

    public override void HideMe()
    {

    }

    public override void ShowMe()
    {
        //显示PlayPanel时直接设定门的尺寸
        Vector3 end = new Vector3(rate[EventDeal.Instance.end.Item2 - 1], rate[EventDeal.Instance.end.Item1 - 1], 1.0f);
        imageImp.transform.localScale = end;
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
        Grid levelGrid = FindObjectOfType<Grid>();

        GameObject levelNow = levelGrid.gameObject;
        levelName = levelNow.name;
        int lengthToKeep = levelName.Length - 7;//此处的7是为了截掉(clone)字样
        levelName = levelName.Substring(0, lengthToKeep);

        PlayControl player = FindObjectOfType<PlayControl>();
        GameObject playerNow = player.gameObject;

        CameraBindle mainCamera = FindObjectOfType<CameraBindle>();
        GameObject cameraNow = mainCamera.gameObject;

        PlayControl.Instance.isChanging = false;
        UIMgr.Instance.HidePanel<PlayPanel>();

        if (levelGrid != null)
        {
            Destroy(levelNow);
        }
        if (playerNow != null)
        {
            Destroy(playerNow);
            PlayControl.Instance = null;
        }
        if (cameraNow != null)
        {
            Destroy(cameraNow);
        }
        if (Level.Instance.spriteObject != null)
        {
            Destroy(Level.Instance.spriteObject);
        }

        switch (btnName)
        {
            case "ButtonBack":

                UIMgr.Instance.ShowPanel<ChoosePanel>();
                UIMgr.Instance.HidePanel<LosePanel>();
                UIMgr.Instance.HidePanel<PlayPanel>();

                break;
            case "ButtonAgain":
                //print("你点了BUttonAgain!");
                UIMgr.Instance.HidePanel<LosePanel>();
                ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", levelName, (res) =>
                {
                    Instantiate(res);
                    EventDeal.Instance.grid = res.GetComponent<Grid>();
                    Level.Instance.setPO(levelName);

                    transform.GetChild(4).localScale = new Vector3(1, 1, 1);
                    UIMgr.Instance.ShowPanel<PlayPanel>();
                });

                break;

        }
    }


}
