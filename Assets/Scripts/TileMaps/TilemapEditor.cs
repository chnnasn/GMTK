using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    public Image ui;
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
        ui.sprite = selectedTile.sprite;

        if (Input.GetMouseButtonDown(0))
        {
            if (state == NewState.build)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return; // 如果鼠标在UI上，停止执行后续的地图编辑代码
                }

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = tilemap.WorldToCell(mousePos); // 转换为瓦片坐标

                Tile tile = tilemap.GetTile<Tile>(gridPos);

                if (tile != null)
                {
                    int newSortingOrder = GetMaxSortingOrder() + 1;
                    makeTile(newSortingOrder);
                }

                // 设置选中的瓦片
                tilemap.SetTile(gridPos, selectedTile);
            }

            if (state == NewState.select)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = tilemap.WorldToCell(mousePos); // 转换为瓦片坐标
                // 选择瓦片逻辑
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
        GameObject tileObject = new GameObject(selectedTile.name);
        Tilemap newTilemap = tileObject.AddComponent<Tilemap>();
        TilemapRenderer newRenderer = tileObject.AddComponent<TilemapRenderer>();

        newRenderer.sortOrder = TilemapRenderer.SortOrder.TopRight;
        newRenderer.mode = TilemapRenderer.Mode.Individual;
        newRenderer.sortingOrder = sortingOrder; // 设置新的排序顺序

        tileObject.transform.SetParent(grid.transform);
        tilemap = newTilemap;
    }

    public int GetMaxSortingOrder()
    {
        int maxSortingOrder = 1;
        TilemapRenderer[] renderers = FindObjectsOfType<TilemapRenderer>();

        foreach (TilemapRenderer renderer in renderers)
        {
            if (renderer.sortingOrder > maxSortingOrder)
            {
                maxSortingOrder = renderer.sortingOrder;
            }
        }

        return maxSortingOrder;
    }
}
