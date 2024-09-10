using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ButtonEvent : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        UIMgr.Instance.ShowPanel<BuildPanel>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SaveThis() {
        SavePanel.instance.SaveTilemapData(-1);
    }

    public void ReMove() {
        SavePanel.instance.ReMove(transform.parent.parent.GetSiblingIndex());

    }

    public void save() {
        SavePanel.instance.SaveBuildGame.GameNums[transform.parent.parent.GetSiblingIndex()].tileMaps.Clear();
        SavePanel.instance.SaveTilemapData(transform.parent.parent.GetSiblingIndex());
    }

    public void OldGame()
    {

        Tilemap[] Maps = FindObjectsOfType<Tilemap>();

        foreach (var h in Maps)
        {

            DestroyImmediate(h.gameObject); //Destroy为异步方法会在下一帧销毁，该方法在当前帧销毁

        }


        if (FindObjectsOfType<Tilemap>().Length == 0)
        {
            UIMgr.Instance.HidePanel<SavePanel>();

            ABResMgr.Instance.LoadResAsync<GameObject>("gameplay", "MainCamera", (res) =>
            {
                Instantiate(res);
            });

            TilemapEditor.instance.setOldGame(transform.parent.parent.GetSiblingIndex());
        }

    }

}
