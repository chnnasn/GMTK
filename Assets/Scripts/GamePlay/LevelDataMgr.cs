using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    

    public static LevelDataMgr Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //在这里编辑关卡序号
    public List<string> levels = new List<string>() { "0101", "0201", "0202", "0203","0204", "0301", "0302", "0303", "0304", "0305", "0401", "0403"};
    //读取玩家关卡解锁数量以及玩家的步数
    public LevelData level;
    public LevelData step;


    //



}

