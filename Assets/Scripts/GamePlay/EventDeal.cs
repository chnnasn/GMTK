using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 在该类进行GamePlay相关事项的管理，该类包含玩家坐标、玩家状态、当前地图信息的汇总，每当地图切换、玩家坐标改变时，就在这里来进行统一更新数据或进行机制处理。
/// </summary>
public class EventDeal : MonoBehaviour
{
    public static EventDeal Instance { get; private set; }
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

    //设置人物和门的拉伸比例：
    public List<float> rate;



    public Dictionary<Vector3Int, List<int>> cells;
    public Vector3Int playerPosition;
    //下面的List的0号元素为高度，1号元素为宽度
    public List<int> playerState;
    public (int, int) end;
    public Dictionary<Vector3Int, List<int>> tileWallDic;
    public Grid grid;
    public Vector3Int endPosition;
    public Vector3Int startPosition;
    //由于刚走过所以被禁用的格子

    public Vector3Int bannedPosition;
    //依次是：大蒜的0到3，酒杯的0到3的精灵资源
    public TileBase daSuan0;
    public TileBase daSuan1;
    public TileBase daSuan2;
    public TileBase daSuan3;
    public TileBase jiuBei0;
    public TileBase jiuBei1;
    public TileBase jiuBei2;
    public TileBase jiuBei3;
    public TileBase tileUp;
    public TileBase tileDown;



    public (bool,int,Vector3) MoveDetermine(Vector3 worldPos)
    {
        // 将世界坐标转换为瓦片坐标
        Vector3Int clickPosition = grid.WorldToCell(worldPos);
        clickPosition.z = 0;
        Vector3 target = grid.CellToWorld(clickPosition);

        print($"本次点击的目标瓦片坐标：{clickPosition}");
        print($"本次点击的目标点坐标：{target}");


        //先判断点击位置是否在可移动的地块上，然后判断地块是否在角色身边几个格子，然后判断地块是否可抵达。三重判断达成则通知PlayConrol可进行移动。
        if (cells.ContainsKey(clickPosition)){
            Vector3Int p1 = new Vector3Int(playerPosition.x, playerPosition.y + 1, playerPosition.z);
            Vector3Int p2 = new Vector3Int(playerPosition.x+1, playerPosition.y, playerPosition.z);
            Vector3Int p3 = new Vector3Int(playerPosition.x, playerPosition.y - 1, playerPosition.z);
            Vector3Int p4 = new Vector3Int(playerPosition.x-1, playerPosition.y, playerPosition.z);

            print($"p1位置：{p1}");
            print($"p2位置：{p2}");
            print($"p3位置：{p3}");
            print($"p4位置：{p4}");

            //根据cell的值的列表的可达性位置来判断可达性
            if (cells[clickPosition][1] == 1){
                print("当前位置可达");
                //判断是否是起点
                if (clickPosition != startPosition && clickPosition != playerPosition) {
                    print("当前cell可以走，其属性：" + string.Join(",", cells[clickPosition]));
                    //接下来进行瓦片贴图的替换。
                    Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();
                    //首先是墙块替换。

                    //从墙块字典里与地板块四个方向的位置匹配一下，如果匹配到位置，就拿着位置找对应的墙块瓦片，然后改变对应贴图。
                    //为了保证替的候替换的素材是正确朝向的素材，此处采用渲染层级来判断素材朝向。
                    //规则：层级5：方向1；层级6：方向0；层级7：方向2；层级8：方向3；
                    //方向0：层级6
                    //方向1：层级5
                    //方向2：层级7
                    //方向3：层级8
                    tileChange(new Vector3Int(0, 1, 0), 2, daSuan2, jiuBei2, 7,tilemaps);
                    tileChange(new Vector3Int(1, 0, 0), 3, daSuan3, jiuBei3, 8, tilemaps);
                    tileChange(new Vector3Int(0, -1, 0), 0, daSuan0, jiuBei0, 6, tilemaps);
                    tileChange(new Vector3Int(-1, 0, 0), 1, daSuan1, jiuBei1, 5, tilemaps);

                    //然后来改地板
                    if (cells[playerPosition][2] == 1)
                    {
                        print("现在找到了要翻转的+1地毯。");
                        foreach (Tilemap tilemap in tilemaps)
                        {
                            if (tilemap.gameObject.name == "tileUp"|| tilemap.gameObject.name == "tileDown")
                            {
                                //找这个map里对应的tile并替换
                                print("替换成高度下降的地毯");
                                tilemap.SetTile(playerPosition, tileDown);
                                break;
                            }
                        }
                    }

                    if (cells[playerPosition][2] == -1)
                    {
                        print("现在找到了要翻转的-1地毯。");
                        foreach (Tilemap tilemap in tilemaps)
                        {
                            if (tilemap.gameObject.name == "tileDown"|| tilemap.gameObject.name == "tileUp")
                            {
                                //找这个map里对应的tile并替换
                                print("替换成高度上升的地毯");
                                tilemap.SetTile(playerPosition, tileUp);
                                break;
                            }
                        }
                    }



                    //把上一个地块的数值取反
                    cells[playerPosition][2] = -cells[playerPosition][2];
                    cells[playerPosition][3] = -cells[playerPosition][3];
                    print("上一个地板的数值已经取反。高的改变值为" + cells[playerPosition][2] + "宽的改变值为" + cells[playerPosition][3]);


                    //把上一个不可到达地块的可达性设置为1
                    cells[bannedPosition][1] = 1;
                    //玩家当前所处的位置变为不可到达
                    cells[playerPosition][1] = 0;
                    //把玩家当前所处位置存入不可到达的保留位置
                    bannedPosition = playerPosition;
                    //返回 移动是否成功、移动的方向、移动的目标。先不更新玩家位置，等动画播完之后由PlayControl调用本类的changeState来更新。


                    //判断方向，然后返回给PlayControl。方向的编号与wall的规则相同。见Wallsxxxx脚本。
                    if (clickPosition == p1)
                    {
                        print("p1");
                         return (true, 0, target);
                    }
                    else if (clickPosition == p2)
                    {
                        print("p2");
                        return (true, 1, target);
                    }
                    else if (clickPosition == p3)
                    {
                        print("p3");
                        return (true, 2, target);
                    }
                    else if (clickPosition == p4)
                    {
                        print("p4");
                        return (true, 3, target);
                    }
                }
            }
        }

        return (false,0, target);

    }

    public void settle()
    {

    }

    //返回的分别是玩家当前的高和宽
    public (int,int) changeState(Vector3Int p)
    {
        playerPosition = p;
        playerState[0] += cells[p][2];
        playerState[1] += cells[p][3];

        int height = playerState[0];
        int width = playerState[1];

        print($"以下分别是玩家当前的高和宽：{height},{width}");
        if (height < 1)
        {
            print("GameOver!!!!!You are so little!!!");
            PlayControl.Instance.isChanging = false;
            UIMgr.Instance.HidePanel<PlayPanel>();
            UIMgr.Instance.ShowPanel<LosePanel>();
            
        }
        if(height > 5)
        {
            print("GameOver!!!!!You are so high!!!");
            PlayControl.Instance.isChanging = false;
            UIMgr.Instance.HidePanel<PlayPanel>();
            UIMgr.Instance.ShowPanel<LosePanel>();
        }
        if(width < 1)
        {
            print("GameOver!!!!!You are so small");
            PlayControl.Instance.isChanging = false;
            UIMgr.Instance.HidePanel<PlayPanel>();
            UIMgr.Instance.ShowPanel<LosePanel>();
        }
        if (width > 5)
        {
            print("GameOver!!!!!You are so wide");
            PlayControl.Instance.isChanging = false;
            UIMgr.Instance.HidePanel<PlayPanel>();
            UIMgr.Instance.ShowPanel<LosePanel>();
        }

        if (playerPosition == endPosition)
        {
            PlayControl.Instance.isChanging = false;
            UIMgr.Instance.HidePanel<PlayPanel>();

            print("yes,this is end");
            if(height == end.Item1 && width == end.Item2)
            {
                print("You win!!!!!You are the best!");
                UIMgr.Instance.ShowPanel<WinPanel>();
                return (0,0);
            }
            print("You lose!!!!!Your statement is not satisfied the pass requerement.");
            UIMgr.Instance.ShowPanel<LosePanel>();
            return (0,0);
            
        }

        return (height, width);

    }

    public void setInit(GameObject player)
    {
        Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();
        foreach (var tilemap in tilemaps)
        {
            if (tilemap.gameObject.name == "tileStart" )
            {
                player.transform.position = getCellPo(tilemap,false);
                break;
            }

            if (tilemap.gameObject.name == "tileEnd")
            {
                endPosition = Vector3Int.CeilToInt(getCellPo(tilemap, true));
            }
        }
        int num = 0;
        foreach(var i in cells)
        {
            num++;
            string list = "";
            foreach(var j in i.Value)
            {
                list += j.ToString();
                list += ",";
            }
            print("这是地图中的第"+num+"个cell："+i +list);
        }



        //foreach (var c in cells)
        //{
        //    if (c.Value[0] == 0)
        //    {
        //        Vector3 statrWorldPosition = grid.CellToWorld(c.Key);
        //        player.transform.position = new Vector3(statrWorldPosition.x, statrWorldPosition.y + 2, statrWorldPosition.z);

        //        startPosition = c.Key;
        //        playerPosition = startPosition;
        //        bannedPosition = startPosition;
        //        print(player.transform.position);
        //        print("这是人物的起始网格坐标：");
        //        print(c.Key);
        //        print($"这是人物的世界坐标：{player.transform.position}");
        //    }
        //    if(c.Value[0] == 1)
        //    {
        //        endPosition = c.Key;
        //        print($"终点位置是：“{endPosition}”!!!!!!!!");
        //    }
        //}
    }

    Vector3 getCellPo(Tilemap targetTilemap,bool isEndOf)
    {
        // 检查目标Tilemap是否为空
        if (targetTilemap != null)
        {
            BoundsInt bounds = targetTilemap.cellBounds;

            // 遍历目标Tilemap的所有单元格
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    for (int z = bounds.zMin; z < bounds.zMax; z++)
                    {
                        Vector3Int cellPosition = new Vector3Int(x, y, z);

                        // 获取指定位置的瓦片
                        TileBase tile = targetTilemap.GetTile(cellPosition);

                        // 检查是否是目标瓦片
                        if (tile != null)
                        {
                            if (!isEndOf)
                            {
                                // 获取瓦片在世界坐标中的位置
                                Vector3 startWorldPosition = grid.CellToWorld(cellPosition);

                                startPosition = cellPosition;
                                playerPosition = startPosition;
                                bannedPosition = startPosition;

                                // 返回转换后的世界坐标（上移2单位）
                                return new Vector3(startWorldPosition.x, startWorldPosition.y + 2, startWorldPosition.z);
                            }
                            else {
                                //Debug.LogError("lllll");
                                Level.Instance.makeDoor(grid,cellPosition, end);
                                return cellPosition;
                            }
                        }
                    }
                }
            }
        }

        // 如果未找到目标瓦片或Tilemap为空，返回默认值
        return Vector3.zero;  // 使用Vector3.zero返回一个(0,0,0)的默认向量
    }

    void tileChange(Vector3Int offset,int index,TileBase daSuan,TileBase jiuBei,int sortingOrder, Tilemap[] tilemaps)
    {
        Vector3Int position = new Vector3Int(playerPosition.x + offset.x, playerPosition.y + offset.y, playerPosition.z + offset.z);
        if (tileWallDic.ContainsKey(position))
        {
            foreach (Tilemap tilemap in tilemaps)
            {
                int order = tilemap.GetComponent<TilemapRenderer>().sortingOrder;
                if (order == sortingOrder)
                {
                    print("上个地板附近的砖块满足需求，应该被替换。");
                    if (tileWallDic[position][index] == 1)
                    {
                        print("这个砖块是酒杯，要被换成大蒜");
                        tileWallDic[position][index] = -1;
                        tilemap.SetTile(position, daSuan);
                    }
                    else if (tileWallDic[position][index] == -1)
                    {

                        print("这个砖块是大蒜，要被换成酒杯");
                        tileWallDic[position][index] = 1;
                        tilemap.SetTile(position, jiuBei);
                    }
                }
            }
        }
    }

}
