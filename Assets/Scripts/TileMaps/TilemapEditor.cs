using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public enum NewState
{
    nothing,
    select,
    build
}

public class TilemapEditor : MonoBehaviour
{
    public NewState state = NewState.nothing;

    public static TilemapEditor instance;

    Tilemap tilemap; // 当前 Tilemap 的引用

    [HideInInspector]
    public Tile[] availableTiles; // 可用的瓦片数组
    Tile selectedTile; // 当前选中的瓦片

    [HideInInspector]
    public int index = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        availableTiles = Resources.LoadAll<Tile>("Tiles");
        selectedTile = availableTiles[index];
    }

    void Update()
    {
        selectedTile = availableTiles[index];

        if (Input.GetMouseButtonDown(0))
        {
            if (state == NewState.build)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return; // 如果鼠标在UI上，停止执行后续的地图编辑代码
                }

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


                Tilemap currentTilemap = getTileMap(mousePos);

                if (currentTilemap == null)
                {

                    GameObject startAndEnd = GameObject.Find(selectedTile.name.Substring(selectedTile.name.IndexOf('_') + 1));

                    if (startAndEnd == null)
                    {

                        makeTile(2,selectedTile.name.Substring(selectedTile.name.IndexOf('_') + 1));
                    }
                    else
                    {

                        tilemap = startAndEnd.GetComponent<Tilemap>();

                        if (index == 0 || index == 1)
                        {

                            tilemap.ClearAllTiles();

                        }

                    }
                }
                else
                {

                    Vector3Int gridPos = tilemap.WorldToCell(mousePos); // 转换为瓦片坐标

                    Tile tile = tilemap.GetTile<Tile>(gridPos);

                    if (tile != null)
                    {
                        if (tilemap.name == selectedTile.name.Substring(selectedTile.name.IndexOf('_') + 1))
                        {

                            tilemap.SetTile(gridPos, null);
                        }
                        else
                        {

                            getNewMap(mousePos);

                        }

                    }
                    else
                    {

                        getNewMap(mousePos);

                    }

                }

                Vector3Int tilepo = tilemap.WorldToCell(mousePos); // 转换为瓦片坐标
                // 设置选中的瓦片
                tilemap.SetTile(tilepo, selectedTile);

            }

            if (state == NewState.select)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 将鼠标屏幕坐标转换为世界坐标
                Tilemap map = getTileMap(mousePos);
                if (map != null) {
                    Vector3Int gridPos = map.WorldToCell(mousePos); // 转换为瓦片坐标
                    Tile tile = map.GetTile<Tile>(gridPos);
                    if (tile != null)
                    {
                        map.SetTile(gridPos, null);
                    }
                }

                if (map != null && IsTilemapEmpty(map))
                {

                    Destroy(map.gameObject);
                }

            }
        }
    }

    // 瓦片选择方法
    public void SelectTile(int tileIndex)
    {
        if (tileIndex >= 0 && tileIndex < availableTiles.Length)
        {
            selectedTile = availableTiles[tileIndex];
        }
    }

    public void makeTile(int sortingOrder, string tileName)
    {
        Grid grid = grid = FindAnyObjectByType<Grid>();
        GameObject tileObject = new GameObject(tileName);
        Tilemap newTilemap = tileObject.AddComponent<Tilemap>();
        TilemapRenderer newRenderer = tileObject.AddComponent<TilemapRenderer>();

        newRenderer.sortOrder = TilemapRenderer.SortOrder.TopRight;
        newRenderer.mode = TilemapRenderer.Mode.Individual;
        newRenderer.sortingOrder = sortingOrder; // 设置新的排序顺序

        tileObject.transform.SetParent(grid.transform);
        tilemap = newTilemap;
    }


    Tilemap getTileMap(Vector3 po)
    {
        Tilemap[] allMaps = FindObjectsOfType<Tilemap>();

        foreach (var map in allMaps)
        {
            Vector3Int gridPos = map.WorldToCell(po); // 转换为 Tilemap 的网格坐标
            TileBase clickedTile = map.GetTile(gridPos); // 获取点击位置的 Tile

            if (clickedTile != null) // 如果有 Tile 存在
            {
                return map; // 返回包含点击位置的 Tilemap
            }
        }
        return null; // 没有找到对应的 Tilemap
    }

    void getNewMap(Vector3 mousePos) {

        Tilemap map = getTileMap(mousePos);

        Tilemap[] maps = FindObjectsOfType<Tilemap>();

        bool found = false; // 标志变量，初始化为 false

        TilemapRenderer oldRenderer = map.gameObject.GetComponent<TilemapRenderer>();

        foreach (var h in maps)
        {
            TilemapRenderer renderer = h.gameObject.GetComponent<TilemapRenderer>();

            if (renderer.name == selectedTile.name.Substring(selectedTile.name.IndexOf('_') + 1) && renderer.sortingOrder > oldRenderer.sortingOrder)
            {
                map = h; // 找到匹配的 Tilemap
                found = true; // 设置标志变量为 true
                break; // 退出循环
            }
        }

        if (!found)
        {
            int newSortingOrder = map.gameObject.GetComponent<TilemapRenderer>().sortingOrder + 1;
            makeTile(newSortingOrder, selectedTile.name.Substring(selectedTile.name.IndexOf('_') + 1));
        }
        else
        {
            tilemap = map;
        }
    }

    bool IsTilemapEmpty(Tilemap map)
    {
        // 获取 Tilemap 的边界范围
        BoundsInt bounds = map.cellBounds;

        // 遍历所有单元格位置
        foreach (var position in bounds.allPositionsWithin)
        {
            // 如果在此位置有瓦片，则返回 false
            if (map.HasTile(position))
            {
                return false;
            }
        }

        // 如果遍历完所有位置后仍未找到瓦片，返回 true
        return true;
    }

    public void setOldGame(int index) {


        List<TileMap> maps = SavePanel.instance.SaveBuildGame.GameNums[index].tileMaps;

        for (int i = 0; i < maps.Count; i++)
        {
            Tilemap map = null; 

            for (int x = 0; x < maps[i].tileData.Count; x++)
            {

                Tilemap[] Maps = FindObjectsOfType<Tilemap>();

                bool found = false; // 标志变量，初始化为 false

                foreach (var h in Maps) {

                    TilemapRenderer renderer = h.gameObject.GetComponent<TilemapRenderer>();

                    if (renderer.name == maps[i].tileData[x].tile.name.Substring(maps[i].tileData[x].tile.name.IndexOf('_') + 1) 
                        && renderer.sortingOrder == maps[i].layerOrder)
                    { 
                        map = h; // 找到匹配的 Tilemap
                        found = true; // 设置标志变量为 true
                        break; // 退出循环
                    }
                } 

                if (!found) { 

                    int newSortingOrder = maps[i].layerOrder;
                    makeTile(newSortingOrder, maps[i].tileData[x].tile.name.Substring(maps[i].tileData[x].tile.name.IndexOf('_') + 1));

                } 
                else 
                { 
                    tilemap = map; 
                }

                tilemap.SetTile(maps[i].tileData[x].position, maps[i].tileData[x].tile);

            }

        }

    }

}
