using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;

/// <summary>
/// 通用的关卡脚本工具，挂在空物体上，在游戏运行时始终唯一存在，由每一关的Walls调用，来分别获取这一关的所有类型的地块以及墙壁，以及刷新EventDeal数值。
/// </summary>
public class Level : MonoBehaviour
{
    Dictionary<string, Dictionary<Vector3Int, List<int>>> nameValueMap; 
    Dictionary<string, List<int>> startAndEndMap;
    public static Level Instance { get; private set; }
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
    }

    private void Start()
    {
        StartCoroutine(ReadJsonFile());
    }

    //设置人物和门的拉伸比例：
    public List<float> rate = new List<float>() { 0.4f, 0.55f ,0.7f,0.85f,1f};



    public Sprite sprite; // 要放置的 Sprite
    public GameObject spriteObject;//门的gameobject
    //分别存储所有类型地板的坐标
    public List<Vector3Int> tileStartP;
    public List<Vector3Int> tileEndP;
    public List<Vector3Int> tileUpP;
    public List<Vector3Int> tileDownP;
    public List<Vector3Int> tileNormalP;

    //分别存储着墙壁Cell位置以及四面状态。这些内容在额外的与关卡绑定的Walls脚本内进行编写，最终传入这里。
    public Dictionary<Vector3Int, List<int>> tileWallDic;

    string tilemapName1 = "tileStart";
    string tilemapName2 = "tileEnd";
    string tilemapName3 = "tileUp";
    string tilemapName4 = "tileDown";
    string tilemapName5 = "tileNormal";

    //终极整合：把各种效果统一到Cell上。
    //List有五个元素，从索引0到5，分别代表：类型、可达性、高度、宽度。
    //类型：值为0，1或2，0代表起点，1代表终点，2代表中间格子；
    //可达性：值为0或1，0代表不可达，1代表可达；
    //高度：值为0，1或-1，分别代表高度不变、高度+1，高度-1；
    //宽度：值为0,1或-1，分别代表宽度不变、宽度+1,，宽度-1；
    public Dictionary<Vector3Int, List<int>> cells = new Dictionary<Vector3Int, List<int>>();

    GameObject player;


    //该函数由每一关的wallsx.x执行完之后传参，获得每一关的地板和墙。
    public void init(Dictionary<Vector3Int, List<int>> walls, (int, int) start, (int, int) end,Grid grid)
    {
        print("level is running");
        clean();
        //获得地板：
        getCells(tilemapName1, tileStartP);
        getCells(tilemapName2, tileEndP);
        getCells(tilemapName3, tileUpP);
        getCells(tilemapName4, tileDownP);
        getCells(tilemapName5, tileNormalP);

        //获得墙：
        tileWallDic = walls;

        //获得了地板和墙，然后开始把格子融合在一起：
        setCells();

        //输出调试信息：
        int num = 0;
        cells.ToList().ForEach(i =>
        {
            print($"地板{i.Key}:{string.Join(",", i.Value)}");
            num++;
        });
        print($"本地图地板数量为：{num}");

        //然后把处理后的关卡信息传给EventDeal，供游戏过程中进行gameplay

        EventDeal.Instance.cells = cells;
        EventDeal.Instance.playerState = new List<int>(){ start.Item1,start.Item2};
        EventDeal.Instance.end = end;
        EventDeal.Instance.tileWallDic = tileWallDic;
        EventDeal.Instance.grid = grid;

        ABResMgr.Instance.LoadResAsync<GameObject>("player", "PlayerCharacter", (res) => {
            player = Instantiate(res);

            EventDeal.Instance.setInit(player);

            CameraControl.Instance.target = player.transform;
        });
        ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", "MainCamera", (res) => {
            Instantiate(res);
        });

        //在生成完关卡之后，该类的各数据清空，防止影响再次生成关卡

    }


    void getCells(string tilemapName,List<Vector3Int> tilePs)
    {
        print("get!");
        // 获取Tilemap对象
        Tilemap targetTilemap = null;
        Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();
        foreach (var tilemap in tilemaps)
        {
            if (tilemap.gameObject.name == tilemapName)
            {
                print(tilemapName);
                targetTilemap = tilemap;
                break;
            }
        }

        if (targetTilemap == null)
        {
            print("未找到名称为 " + tilemapName + " 的地板.");
            return;
        }

        // 遍历Tilemap的所有cell，得到坐标
        BoundsInt bounds = targetTilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int z = bounds.zMin; z < bounds.zMax; z++)
                {
                    Vector3Int cellPosition = new Vector3Int(x, y, z);
                    TileBase tile = targetTilemap.GetTile(cellPosition);

                    if (tile != null)
                    {
                        //不知道为什么，终点会被找重。所以这里加一个终点的判断。有待优化！！！
                        if ((tileEndP.Count !=0&&cellPosition != tileEndP[0]) || tileEndP.Count == 0)
                        {
                            // 这里把cell的Position保存到一个列表
                            tilePs.Add(cellPosition);
                            print("找到地板了！: " + cellPosition + $"现在是{tilePs[0]}");
                        }

                    }
                }
            }
        }

    }

    //提示：写移动的时候先检测点击位置是否在格子字典，然后检测是否可达
    void setCells()
    {
        foreach(var t in tileStartP)
        {
            cells[t] = new List<int> { 0, 1, 0, 0 };
        }
        foreach (var t in tileEndP)
        {
            print("找到终点了！！！！！！！！！！！！");
            cells[t] = new List<int> { 1, 1, 0, 0 };
        }
        foreach (var t in tileUpP)
        {
            cells[t] = new List<int> { 2, 1, 1, 0 };
        }
        foreach (var t in tileDownP)
        {
            cells[t] = new List<int> { 2, 1, -1, 0 };
        }
        foreach (var t in tileNormalP)
        {
            cells[t] = new List<int> { 2, 1, 0, 0 };
        }
        foreach (var t in tileWallDic)
        {
            //墙面不加到cell里，直接在eventdeal里判断cell外的点击无效即可不到达墙面。
            //分别找到墙体四个面的邻近格子是否为地板，有地板就为地板cell的list修改索引3处的内容。
            Vector3Int position = new Vector3Int(t.Key.x, t.Key.y + 1, t.Key.z);
            if (cells.ContainsKey(position))
            {
                cells[position][3] += t.Value[0];

            }
            position = new Vector3Int(t.Key.x + 1, t.Key.y, t.Key.z);
            if (cells.ContainsKey(position))
            {
                cells[position][3] += t.Value[1];

            }

            position = new Vector3Int(t.Key.x, t.Key.y - 1, t.Key.z);
            if (cells.ContainsKey(position))
            {
                cells[position][3] += t.Value[2];
            }

            position = new Vector3Int(t.Key.x - 1, t.Key.y, t.Key.z);
            if (cells.ContainsKey(position))
            {
                cells[position][3] += t.Value[3];
            }
        }


    }

    public void clean()
    {
        tileStartP?.Clear();
        tileEndP?.Clear();
        tileUpP?.Clear();
        tileDownP?.Clear();
        tileNormalP?.Clear();
        tileWallDic?.Clear();
        cells?.Clear();
    }

    public void makeDoor(Grid grid , Vector3Int h, (int, int) end) {
        //拿到门的cellposition,然后做门的初始化：
        Vector3 position = grid.CellToWorld(h);
        spriteObject = new GameObject("Gate");
        Vector3 scale = new Vector3(rate[end.Item2 - 1], rate[end.Item1 - 1], 1.0f);
        // 将其位置设置为指定的位置
        spriteObject.transform.position = new Vector3(position.x, position.y + 1, position.z);
        spriteObject.transform.localScale = scale;
        // 添加 SpriteRenderer 组件，并设置 Sprite以及渲染层级（跟所有预制体的门统一，为5）
        SpriteRenderer renderer = spriteObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = 5;
        print($"门的世界坐标：{position}，瓦片坐标：{tileEndP[0]}，门的缩放：{scale}");
    }

        private IEnumerator ReadJsonFile()
        {
        string uri = Path.Combine(Application.streamingAssetsPath, "levels.json");

        UnityWebRequest request = UnityWebRequest.Get(uri);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            TableDataList dataList = JsonUtility.FromJson<TableDataList>(json);
            nameValueMap = new Dictionary<string, Dictionary<Vector3Int, List<int>>>();
            startAndEndMap = new Dictionary<string, List<int>>();

            foreach (var table in dataList.tables)
            {
                Dictionary<Vector3Int, List<int>> tableMap = new Dictionary<Vector3Int, List<int>>();

                foreach (var sample in table.sampleList.samples)
                {
                        Vector3Int key = new Vector3Int(sample.po[0], sample.po[1], sample.po[2]);
                        tableMap[key] = sample.value;
                }
                nameValueMap[table.work] = tableMap;
                startAndEndMap[table.work] = table.startAndEnd;
            }
        }

        yield break;
        }

    public void setPO(string key) {
        init(nameValueMap[key],(startAndEndMap[key][0], startAndEndMap[key][1]), (startAndEndMap[key][2], startAndEndMap[key][3]), GameObject.FindFirstObjectByType<Grid>());
    }
}

