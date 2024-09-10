using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SavePanel : BasePanel
{
    public static SavePanel instance;
    public SaveBuildGame SaveBuildGame;
    public override void HideMe()
    {

    }

    public override void ShowMe()
    {

    }
    private void Awake()
    {
        if (instance != null) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    protected override void ClickBtn(string btnName)
    {
        TilemapEditor TilemapEditor = FindFirstObjectByType<TilemapEditor>();
        Destroy(TilemapEditor.gameObject);

        switch (btnName)
        {
            case "ButtonBack":

                UIMgr.Instance.HidePanel<SavePanel>();
                UIMgr.Instance.HidePanel<BuildPanel>();
                UIMgr.Instance.ShowPanel<BeginPanel>();
                break;
        }
    }
    public void SaveTilemapData(int index)
    {

        TilemapEditor TilemapEditor = FindFirstObjectByType<TilemapEditor>();

        Tilemap[] maps = FindObjectsOfType<Tilemap>();

        if (maps.Length != 0)
        {
            TileSort tileSort = new TileSort();

            foreach (var h in maps)
            {

                BoundsInt bounds = h.cellBounds;

                TileMap tileInfo = new TileMap(); //其中一个map
                // 遍历所有瓦片并保存它们的信息
                foreach (Vector3Int pos in bounds.allPositionsWithin)
                {

                    TileBase tile = h.GetTile(pos);

                    if (tile != null) // 如果该位置存在瓦片
                    {
                        TileData tileData = new TileData
                        {
                            position = pos,
                            tile = tile as Tile
                        };

                        tileInfo.tileData.Add(tileData); //map里的tile数据
                    }

                    tileInfo.layerOrder = h.gameObject.GetComponent<TilemapRenderer>().sortingOrder;

                }

                tileSort.tileMaps.Add(tileInfo);

            }

            if (index >= 0 ) {

                for(int i = 0; i< tileSort.tileMaps.Count;i++) {

                    SaveBuildGame.GameNums[index].tileMaps.Add(tileSort.tileMaps[i]);
                }
                
            }
            else {

                SaveBuildGame.GameNums.Add(tileSort);

            }
        }

        Destroy(TilemapEditor.gameObject);
        UIMgr.Instance.HidePanel<SavePanel>();
        UIMgr.Instance.HidePanel<BuildPanel>();
        UIMgr.Instance.ShowPanel<BeginPanel>();
    }


    public void ReMove(int index) {
        SaveBuildGame.GameNums.RemoveAt(index);
        Save();
    }


    public void Save() {

        for (int i = 0; i < transform.GetChild(0).GetChild(0).childCount; i++) {

            Destroy(transform.GetChild(0).GetChild(0).GetChild(i).gameObject);

        }


        for (int i = 0; i < SaveBuildGame.GameNums.Count; i++)
        {
            ABResMgr.Instance.LoadResAsync<GameObject>("ui", "GameNum", (res) =>
            {
                GameObject h = Instantiate(res, transform.GetChild(0).GetChild(0).position, Quaternion.identity);
                h.transform.SetParent(transform.GetChild(0).GetChild(0));
                h.transform.localScale = new Vector3(9, 1.2f, 1.2f);
                Text text = h.gameObject.GetComponentInChildren<Text>();
                text.text = text.text == "关卡" + (i + 1) ? "New Text" : "关卡" + (i + 1);
            });
        }

        ABResMgr.Instance.LoadResAsync<GameObject>("ui", "SaveThis", (res) =>
        {
            GameObject h = Instantiate(res, transform.GetChild(0).GetChild(0).position, Quaternion.identity);
            h.transform.SetParent(transform.GetChild(0).GetChild(0));
            h.transform.localScale = new Vector3(12, 1.2f, 1.2f);
        });
    }

}
