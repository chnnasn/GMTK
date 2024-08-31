using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于持久化保存的数据类
[Serializable]
public class LevelData
{   
    /// <summary>
    /// 用来存储每个关卡是否已解锁的数据。
    /// </summary>
    public List<bool> level;

    /// <summary>
    /// 用来存储玩家在每一关的最低通关步数的数据。
    /// </summary>
    public List<int> step;


}
