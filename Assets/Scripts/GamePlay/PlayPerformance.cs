using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//这个脚本根据状态信息来每帧处理动画表现。包括角色和场景。
public class PlayPerformance : MonoBehaviour
{

    public static PlayPerformance Instance { get; private set; }
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




}
