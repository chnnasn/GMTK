
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Walls0101 : MonoBehaviour
{
    //存储着墙壁Cell位置以及其代表的效果。
    public Dictionary<Vector3Int, List<int>> tileWallDic;
    public (int,int) start;
    public (int, int) end;

    // Start is called before the first frame update
    void Start()
    {
        //改一下，此处直接生成字典，键是坐标，值是四维影响，然后直接形成墙体的容器。不用tilebase了。相应的，level和eventdeal也改一下。
        //然后在level获取walls里的墙体字典。level的start里还要定玩家属性，也就是把playcontrol变成单例模式，修改里面的数值。
        //**然后直接在level的start函数里把这些地图数据获取然后传给eventdeal。接下来开始写eventdeal的方法，也就是拿着玩家坐标来分别遍历各种地板或者墙的坐标（或者换转一下墙的效果，直接把墙的效果生效格子对应坐标列出来）。
        //按效果来分函数，参数是角色坐标，回传bool值，每次调用函数外分别根据函数的情况套一层判断，决定是否要给玩家属性加/减或不变。然后改变playcontrol的属性，或者判定通关与否。起点格子离开后永远设为不可达到，刚离开的格子也设为不可达到。在playcontrol里每一帧判断属性是否过高或过低，不合适就进入失败动画。合适就不变。
        //另外，eventdeal里写一个方法，接收来自playcontrol的tile坐标，实现在玩家点击地板的时候判断地板是否可走（是否在五种地板的tilemap列表里）。然后把bool结果传到playcontrol里。由playcontrol来控制玩家行走失败或成功行走。成功行走时，就为eventdeal更新坐标，否则不改变eventdeal。
        //过关之后显示过关界面，选择下一关就删当前关卡，加载下一个关卡。加载时新的关卡的level脚本和对应walls脚本会出现。此时就进入**的步骤了。这个闭环就实现了。
        //

        //初始化墙块字典
        tileWallDic = new Dictionary<Vector3Int, List<int>>();

        //获得grid
        Grid grid = this.GetComponent<Grid>();
        ///以下内容在每个地图都要修改，配置关卡信息使用：
        ///--------------------

        //调用这个函数来配置有效果的墙体。先用瓦片画特殊地板，然后从Unity里找到特殊地板的位置，然后根据位置一个个填效果。
        //List里从0到4索引位置分别是墙的左上、右上、右下、左下面的状态。
        //状态值为0则对该面紧邻的砖块无效果。1为宽度+1, -1为宽度-1
        addPositions(
            (new Vector3Int(0, -3, 0), new List<int> { 0, 0, 0, 1 }),
            (new Vector3Int(0, -5, 0), new List<int> { 0, 0, 0, 1 }),
            (new Vector3Int(0, -7, 0), new List<int> { -1, 0, 0, 0 }),
            (new Vector3Int(2, -5, 0), new List<int> { 0, 0, -1, 0 }),
            (new Vector3Int(4, -5, 0), new List<int> { 0, 0, -1, 0 })
            );

        //设定该地图起点和终点属性数据，每个元组内按顺序分别是高度和宽度
        start = (3, 3);
        end = (1, 2);

        ///_______________________
        ///
        //关卡信息传给Level统一处理
        if (tileWallDic == null)
        {
            print("dic is null");
        }

        if (grid == null)
        {
            print("grid is null");
        }
        Level.Instance.init(tileWallDic,start,end,grid);




    }


    void addPositions(params (Vector3Int key, List<int> value)[] walls)
    {
        foreach (var p in walls)
        {
            tileWallDic[p.key] = p.value;
        }
    }

    //找到符合“cellPosition”键的坐标的格子就略过，不在cellPosition键值的就是无效果墙，直接把值List全赋为0即可。
    //把效果映射到格子上其实根本不需要考虑无效果的墙面，本方法弃用
    //void getCells(string tilemapName)
    //{
    //    print("get!");
    //    // 获取Tilemap对象
    //    Tilemap targetTilemap = null;
    //    Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();
    //    foreach (var tilemap in tilemaps)
    //    {
    //        if (tilemap.gameObject.name == tilemapName)
    //        {
    //            print(tilemapName);
    //            targetTilemap = tilemap;
    //            break;
    //        }
    //    }

    //    if (targetTilemap == null)
    //    {
    //        Debug.LogError("未找到名称为 " + tilemapName + " 的Tilemap.");
    //        return;
    //    }

    //    // 创建一个列表来存储待添加的元素
    //    List<(Vector3Int key, List<int> value)> positionsToAdd = new List<(Vector3Int, List<int>)>();


    //    // 遍历Tilemap的所有cell
    //    BoundsInt bounds = targetTilemap.cellBounds;
    //    for (int x = bounds.xMin; x < bounds.xMax; x++)
    //    {
    //        for (int y = bounds.yMin; y < bounds.yMax; y++)
    //        {
    //            for (int z = bounds.zMin; z < bounds.zMax; z++)
    //            {
    //                Vector3Int cellPosition = new Vector3Int(x, y, z);
    //                TileBase tile = targetTilemap.GetTile(cellPosition);
    //                if (tile != null && !tileWallDic.ContainsKey(cellPosition))
    //                {
    //                    addPositions((cellPosition, new List<int> { 0, 0, 0, 0 }));
    //                }
    //            }
    //        }
    //    }

    //}

    // Update is called once per frame
    void Update()
    {

    }
}
