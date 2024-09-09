using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPanel : BasePanel
{

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
        TilemapEditor levelGrid = FindObjectOfType<TilemapEditor>();

        GameObject levelNow = levelGrid.gameObject;

        CameraBindle mainCamera = FindObjectOfType<CameraBindle>();
        GameObject cameraNow = mainCamera.gameObject;

        switch (btnName)
        {
            case "ButtonBack":
                TilemapEditor.instance.state = NewState.nothing;
                UIMgr.Instance.ShowPanel<BeginPanel>();
                Destroy(cameraNow);
                Destroy(levelNow);
                UIMgr.Instance.HidePanel<BuildPanel>();

                break;
            case "ButtonSave":
                TilemapEditor.instance.state = NewState.nothing;
                UIMgr.Instance.ShowPanel<SavePanel>();
                SavePanel.instance.Save();

                Destroy(cameraNow);

                UIMgr.Instance.HidePanel<BuildPanel>();

                break;
            case "start":
                TilemapEditor.instance.state = NewState.build;
                TilemapEditor.instance.index = 0;
                break;
            case "end":
                TilemapEditor.instance.state = NewState.build;
                TilemapEditor.instance.index = 1;
                break;
            case "floor":
                TilemapEditor.instance.state = NewState.build;
                TilemapEditor.instance.index = 2;
                break;
            case "blueCarpet":
                TilemapEditor.instance.state = NewState.build;
                TilemapEditor.instance.index = 3;
                break;
            case "orangeCarpet":
                TilemapEditor.instance.state = NewState.build;
                TilemapEditor.instance.index = 4;
                break;
            case "deskBu":
                TilemapEditor.instance.state = NewState.build;
                TilemapEditor.instance.index = 5;
                break;
            case "redWine":
                TilemapEditor.instance.state = NewState.build;
                TilemapEditor.instance.index = 6;
                break;
            case "garlic":
                TilemapEditor.instance.state = NewState.build;
                TilemapEditor.instance.index = 7;
                break;
            case "remove":
                TilemapEditor.instance.state = NewState.select;
                break;

        }
    }


}
