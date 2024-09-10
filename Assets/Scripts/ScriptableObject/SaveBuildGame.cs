using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileData
{
    public Vector3Int position; // 瓦片位置
    public Tile tile;       // 瓦片引用
}

[System.Serializable]
public class TileMap
{
    public int layerOrder;
    public List<TileData> tileData = new List<TileData>();
}

[System.Serializable]
public class TileSort
{
    public List<TileMap> tileMaps = new List<TileMap>(); // 声明一个 TileInfo 类型的数组
}


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/keepBuilding")]
public class SaveBuildGame : ScriptableObject
{
    public List<TileSort> GameNums = new List<TileSort>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
