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
    Grid grid;
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
        grid = FindAnyObjectByType<Grid>();
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

                        makeTile(2);
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
                        if (tilemap.name == selectedTile.name.Substring(selectedTile.name.IndexOf('_') + 1)) {

                            tilemap.SetTile(gridPos, null);
                        }
                        else {

                            Tilemap map = getTileMap(mousePos);

                            if (map != null)
                            {
                                Tilemap[] maps = FindObjectsOfType<Tilemap>();

                                bool found = false; // 标志变量，初始化为 false

                                TilemapRenderer oldRenderer = map.gameObject.GetComponent<TilemapRenderer>();

                                foreach (var h in maps)
                                {
                                    TilemapRenderer renderer = h.gameObject.GetComponent<TilemapRenderer>();
                                    if (renderer.sortingOrder > oldRenderer.sortingOrder &&
                                        renderer.name == selectedTile.name.Substring(selectedTile.name.IndexOf('_') + 1))
                                    {
                                        map = h; // 找到匹配的 Tilemap
                                        found = true; // 设置标志变量为 true
                                        break; // 退出循环
                                    }
                                }
                                // 如果没有找到匹配的 Tilemap，创建一个新的
                                if (!found)
                                {
                                    int newSortingOrder = map.gameObject.GetComponent<TilemapRenderer>().sortingOrder + 1;
                                    makeTile(newSortingOrder);
                                }
                                else {
                                    tilemap = map;
                                }
                            }
                        }
                        

                    }

                }
                Vector3Int tilepo = tilemap.WorldToCell(mousePos); // 转换为瓦片坐标
                // 设置选中的瓦片
                tilemap.SetTile(tilepo, selectedTile);

            }

            if (state == NewState.select)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 将鼠标屏幕坐标转换为世界坐标
                Tilemap tilemap = getTileMap(mousePos);
                if (tilemap != null) {
                    Vector3Int gridPos = tilemap.WorldToCell(mousePos); // 转换为瓦片坐标
                    Tile tile = tilemap.GetTile<Tile>(gridPos);
                    if (tile != null)
                    {
                        tilemap.SetTile(gridPos, null);
                    }
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

    public void makeTile(int sortingOrder)
    {
        GameObject tileObject = new GameObject(selectedTile.name.Substring(selectedTile.name.IndexOf('_') + 1));
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


}
