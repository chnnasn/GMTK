# GMTK — The Shape-Shifting Girl

一款网格移动解谜游戏（Unity 2022.3 / URP 2D），2024 年 Game Jam 项目，玩法对应 GMTK 2024 主题「Built to Scale」。

主角女孩可以在 **5 档体型**之间缩放，每面墙带有多个「维度」数值 — 当前体型与墙体维度的关系决定能否通过。玩家要在网格关卡中规划路线、切换体型，抵达终点，并尽量用更少的步数完成。

## 核心玩法

- **体型缩放**：缩放比例共 5 档（0.4 / 0.55 / 0.7 / 0.85 / 1.0），由 `PlayControl` 驱动移动与变形，变形后可通过的墙体随之变化
- **规则裁决**：`EventDeal` 是独立的移动规则模块 — 每次移动前根据玩家体型与目标格墙体维度（`Dictionary<Vector3Int, List<int>>`）判定可否通行，并反馈成功 / 失败表现
- **步数计分**：`LevelData` 持久化每关的解锁状态与完成步数，鼓励最优路线

## 关卡与编辑器

- **12 个关卡**（`Assets/Scripts/GamePlay/Levels/`：Walls0101 – Walls0402，共四章），每关以代码定义墙体维度数据、起点与终点
- **内置关卡编辑器**（`Assets/Scripts/TileMaps/TilemapEditor.cs`）：select / build 两态，玩家可自己铺设关卡，并通过存档面板与 ScriptableObject（`SaveBuildGame`）保存、读取自制关卡

## 流程与界面

基于自研框架 BasePanel 的面板式 UI，共 11 个面板，构成完整闭环：

开始（Begin）→ 选关（Choose）→ 游玩（Play）→ 过关（Pass）/ 失败（Lose）/ 胜利（Win），以及帮助（Help）、设置（Setting）、存档（Save）等面板。

## 自研框架（`Assets/Scripts/Framework`）

本项目是这套框架的首次成型，后被 [Light](https://github.com/chnnasn/Light) 复用：

- **EventCenter** 事件中心、**PoolMgr** 对象池、**TimerMgr** 计时器
- **ResMgr / ABMgr** 资源与 AssetBundle 管理
- **UIMgr + BasePanel** 面板式 UI 框架、**InputMgr** 输入封装、**MusicMgr** 音频、**SceneMgr** 场景切换
- 单例基类与工具类（加密、数学、文本）；编辑器工具 xlsx 转 JSON（`Assets/Editor/EditorScripts/xlsxToJson.cs`）

## 技术栈

| 项 | 内容 |
|---|---|
| 引擎 | Unity 2022.3.26f1c1 |
| 渲染 | URP 14.0.11（2D） |
| 代码规模 | 玩法约 3,500 行（`GamePlay` / `TileMaps` / `UI`）+ 框架约 5,000 行 |
| 时间 | 2024 年 8 – 9 月，27 次提交 |

## 项目结构

```text
Assets/
├── Scripts/
│   ├── GamePlay/
│   │   ├── PlayControl.cs   # 玩家移动与体型缩放
│   │   ├── EventDeal.cs     # 移动合法性规则裁决
│   │   ├── Level.cs / LevelData.cs / LevelDataMgr.cs  # 关卡与进度
│   │   └── Levels/          # 12 个关卡的墙体数据（Walls0101–0402）
│   ├── TileMaps/
│   │   └── TilemapEditor.cs # 内置关卡编辑器
│   ├── UI/                  # 11 个流程面板
│   ├── ScriptableObject/    # 自制关卡存档数据
│   └── Framework/           # 自研框架（后被 Light 复用）
├── Editor/EditorScripts/    # xlsx 转 JSON 工具
└── AssetBundles-Browser-master/  # 第三方：AssetBundle 查看工具
```

## 运行方式

1. 用 Unity **2022.3.26f1** 打开本项目；
2. 运行主场景，从开始面板进入选关 / 游玩；
3. 建造模式下可使用内置编辑器铺设并保存自制关卡。

## 现状说明

- 官方关卡数据以 C# 类硬编码在 `Levels/` 下（未走数据文件），增删关卡需要改代码；
- `PeoplePanel` 为预留面板，未实现功能；
- 部分源码注释为 GBK 编码，在 UTF-8 环境下显示为乱码，不影响编译。

## 贡献者

- [chnnasn](https://github.com/chnnasn)（主导，25 / 27 次提交）
- [wyc-ci101](https://github.com/wyc-ci101)

## 第三方说明

**AssetBundle Browser**（`Assets/AssetBundles-Browser-master/`）为第三方工具，版权归其作者所有；本仓库的游戏代码与关卡为原创。
