using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

/// <summary>
/// 网格属性管理
/// </summary>
[RequireComponent(typeof(GenerateGUID))]
public class GridPropertiesManager : SingletonMonoBehvior<GridPropertiesManager>, ISaveable
{
    private Tilemap groundDecoration1;
    private Tilemap groundDecoration2;

    private Grid grid;
    /// <summary>
    /// 网格属性详情字典
    /// </summary>
    private Dictionary<string, GridPropertyDetails> gridPropertyDictionary;
    /// <summary>
    /// 网格属性数组，scriptableobjectassets中的SO_GridProperties
    /// 当前脚本就是对这个数组进行管理
    /// </summary>
    [SerializeField] private SO_GridProperties[] so_gridPropertiesArry = null;
    [SerializeField] private Tile[] dugGround = null;
    [SerializeField] private Tile[] wateredGround = null;

    private string _iSaveableUniqueID;
    public string ISaveableUniqueId { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }
    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    protected override void Awake()
    {
        base.Awake();

        ISaveableUniqueId = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
    }

    private void OnEnable()
    {
        //内容注册并订阅事件
        ISaveableRegister();
        EventHandler.AfterSceneLoadEvent += AfterSceneLoaded;
        EventHandler.AdvanceGameDayEvent += AdvanceDay;
    }

    private void OnDisable()
    {
        //内容注销并取消订阅
        ISaveableDeregister();
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoaded;
        EventHandler.AdvanceGameDayEvent -= AdvanceDay;
    }

    private void Start()
    {
        InitialiseGridProperties();
    }

    private void ClearDisplayGroundDecorations()
    {
        groundDecoration1.ClearAllTiles();
        groundDecoration2.ClearAllTiles();
    }

    /// <summary>
    /// 清除显示的网格装饰信息
    /// </summary>
    private void ClearDisplayGridPropertyDetails()
    {
        ClearDisplayGroundDecorations();
    }

    /// <summary>
    /// 显示指定的挖掘的地面
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    public void DisplayDugGround(GridPropertyDetails gridPropertyDetails)
    {
        if(gridPropertyDetails.daysSinceDug > -1)
        {
            ConnectDugGround(gridPropertyDetails);
        }
    }

    /// <summary>
    /// 显示已经浇水的地面
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    public void DisplayWateredGround(GridPropertyDetails gridPropertyDetails)
    {
        if(gridPropertyDetails.daysSinceWatered > -1)
        {
            ConnectWateredGround(gridPropertyDetails);
        }
    }

    /// <summary>
    /// 连接已经浇过水的网格
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    private void ConnectWateredGround(GridPropertyDetails gridPropertyDetails)
    {
        // 设置当前网格
        Tile wateredTile0 = SetWateredTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY);
        groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY, 0), wateredTile0);

        // 相邻的网格
        GridPropertyDetails adjacentGridPropertyDetails;
        // 当前浇水网格上方的一格网格
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceWatered > -1)
        {
            Tile wateredTile1 = SetWateredTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1, 0), wateredTile1);
        }

        // 当前浇水网格下方的一格网格
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceWatered > -1)
        {
            Tile wateredTile2 = SetWateredTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1, 0), wateredTile2);
        }

        // 当前浇水网格左边的一格网格
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceWatered > -1)
        {
            Tile wateredTile3 = SetWateredTile(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY, 0), wateredTile3);
        }

        // 当前浇水网格右边的一格网格
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceWatered > -1)
        {
            Tile wateredTile4 = SetWateredTile(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY, 0), wateredTile4);
        }
    }

   

    /// <summary>
    /// 连接已经挖掘过的地方
    /// 判断上下左右的网格是否挖掘过，是则连接两块土地
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    private void ConnectDugGround(GridPropertyDetails gridPropertyDetails)
    {
        // 设置当前网格
        Tile dugTile0 = SetDugTile(gridPropertyDetails.gridX,gridPropertyDetails.gridY);
        groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY, 0), dugTile0);

        // 相邻的网格
        GridPropertyDetails adjacentGridPropertyDetails;
        // 当前挖掘网格上方的一格网格
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
        if(adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceDug > -1)
        {
            Tile dugTile1 = SetDugTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1, 0), dugTile1);
        }

        // 当前挖掘网格下方的一格网格
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceDug > -1)
        {
            Tile dugTile2 = SetDugTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1, 0), dugTile2);
        }

        // 当前挖掘网格左边的一格网格
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceDug > -1)
        {
            Tile dugTile2 = SetDugTile(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY, 0), dugTile2);
        }

        // 当前挖掘网格右边的一格网格
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceDug > -1)
        {
            Tile dugTile2 = SetDugTile(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY, 0), dugTile2);
        }
    }

    /// <summary>
    /// 设置挖掘的瓦片
    /// </summary>
    /// <param name="xGrid"></param>
    /// <param name="yGrid"></param>
    /// <returns></returns>
    private Tile SetDugTile(int xGrid, int yGrid)
    {
        // 当前网格坐标上下左右四个方向的地面是否被挖掘过
        bool upDug = IsGridSquareDug(xGrid, yGrid + 1);
        bool downDug = IsGridSquareDug(xGrid, yGrid - 1);
        bool leftDug = IsGridSquareDug(xGrid - 1, yGrid);
        bool rightDug = IsGridSquareDug(xGrid + 1, yGrid);

        #region 根据四周是否被挖掘过来设置不同的网格瓦片

        // 根据四周四块网格不同的挖掘情况存在16中不同的组合

        if(!upDug && !downDug && !rightDug && !leftDug)
        {
            // 上下左右都未挖掘
            return dugGround[0];
        }
        else if (!upDug && downDug && rightDug && !leftDug)
        {
            // 下方和右方挖掘过
            return dugGround[1];
        }
        else if (!upDug && downDug && rightDug && leftDug)
        {
            // 下  右  左
            return dugGround[2];
        }
        else if (!upDug && downDug && !rightDug && leftDug)
        {
            // 下  左
            return dugGround[3];
        }
        else if (!upDug && downDug && !rightDug && !leftDug)
        {
            // 下
            return dugGround[4];
        }
        else if (upDug && downDug && rightDug && !leftDug)
        {
            // 上  下 右
            return dugGround[5];
        }
        else if (upDug && downDug && rightDug && leftDug)
        {
            //  上下左右
            return dugGround[6];
        }
        else if (upDug && downDug && !rightDug && leftDug)
        {
            //  上 下 左
            return dugGround[7];
        }
        else if (upDug && downDug && !rightDug && !leftDug)
        {
            //  上  下
            return dugGround[8];
        }
        else if (upDug && !downDug && rightDug && !leftDug)
        {
            //  上  右
            return dugGround[9];
        }
        else if (upDug && !downDug && rightDug && leftDug)
        {
            //  上  右 左
            return dugGround[10];
        }
        else if (upDug && !downDug && !rightDug && leftDug)
        {
            //  上  左
            return dugGround[11];
        }
        else if (upDug && !downDug && !rightDug && !leftDug)
        {
            //  上
            return dugGround[12];
        }
        else if (!upDug && !downDug && rightDug && !leftDug)
        {
            //  右
            return dugGround[13];
        }
        else if (!upDug && !downDug && rightDug && leftDug)
        {
            //  右  左
            return dugGround[14];
        }
        else if (!upDug && !downDug && !rightDug && leftDug)
        {
            // 左
            return dugGround[15];
        }

        return null;

        #endregion
    }

    /// <summary>
    /// 设置浇水的瓦片
    /// </summary>
    /// <param name="xGrid"></param>
    /// <param name="yGrid"></param>
    /// <returns></returns>
    private Tile SetWateredTile(int xGrid, int yGrid)
    {
        // 当前网格坐标上下左右四个方向的地面是否被挖掘过
        bool upWatered = IsGridSquareWatered(xGrid, yGrid + 1);
        bool downWatered = IsGridSquareWatered(xGrid, yGrid - 1);
        bool leftWatered = IsGridSquareWatered(xGrid - 1, yGrid);
        bool rightWatered = IsGridSquareWatered(xGrid + 1, yGrid);

        #region 根据四周是否被挖掘过来设置不同的网格瓦片

        // 根据四周四块网格不同的挖掘情况存在16中不同的组合

        if (!upWatered && !downWatered && !rightWatered && !leftWatered)
        {
            // 上下左右都未挖掘
            return wateredGround[0];
        }
        else if (!upWatered && downWatered && rightWatered && !leftWatered)
        {
            // 下方和右方挖掘过
            return wateredGround[1];
        }
        else if (!upWatered && downWatered && rightWatered && leftWatered)
        {
            // 下  右  左
            return wateredGround[2];
        }
        else if (!upWatered && downWatered && !rightWatered && leftWatered)
        {
            // 下  左
            return wateredGround[3];
        }
        else if (!upWatered && downWatered && !rightWatered && !leftWatered)
        {
            // 下
            return wateredGround[4];
        }
        else if (upWatered && downWatered && rightWatered && !leftWatered)
        {
            // 上  下 右
            return wateredGround[5];
        }
        else if (upWatered && downWatered && rightWatered && leftWatered)
        {
            //  上下左右
            return wateredGround[6];
        }
        else if (upWatered && downWatered && !rightWatered && leftWatered)
        {
            //  上 下 左
            return wateredGround[7];
        }
        else if (upWatered && downWatered && !rightWatered && !leftWatered)
        {
            //  上  下
            return wateredGround[8];
        }
        else if (upWatered && !downWatered && rightWatered && !leftWatered)
        {
            //  上  右
            return wateredGround[9];
        }
        else if (upWatered && !downWatered && rightWatered && leftWatered)
        {
            //  上  右 左
            return wateredGround[10];
        }
        else if (upWatered && !downWatered && !rightWatered && leftWatered)
        {
            //  上  左
            return wateredGround[11];
        }
        else if (upWatered && !downWatered && !rightWatered && !leftWatered)
        {
            //  上
            return wateredGround[12];
        }
        else if (!upWatered && !downWatered && rightWatered && !leftWatered)
        {
            //  右
            return wateredGround[13];
        }
        else if (!upWatered && !downWatered && rightWatered && leftWatered)
        {
            //  右  左
            return wateredGround[14];
        }
        else if (!upWatered && !downWatered && !rightWatered && leftWatered)
        {
            // 左
            return wateredGround[15];
        }

        return null;

        #endregion
    }

    /// <summary>
    /// 判断坐标对应的网格是否浇过水
    /// </summary>
    /// <param name="xGrid"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    private bool IsGridSquareWatered(int xGrid, int yGrid)
    {
        GridPropertyDetails gridPropertyDetails = GetGridPropertyDetails(xGrid, yGrid);

        if (gridPropertyDetails == null)
        {
            return false;
        }
        else if (gridPropertyDetails.daysSinceWatered > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 判断坐标对应的网格是否被挖掘过
    /// </summary>
    /// <param name="xGrid"></param>
    /// <param name="yGrid"></param>
    /// <returns></returns>
    private bool IsGridSquareDug(int xGrid, int yGrid)
    {
        GridPropertyDetails gridPropertyDetails = GetGridPropertyDetails(xGrid, yGrid);

        if(gridPropertyDetails == null)
        {
            return false;
        }
        else if(gridPropertyDetails.daysSinceDug > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 显示地面信息  挖掘、浇水、种植
    /// </summary>
    private void DisplayGridPropertyDetails()
    {
        foreach(KeyValuePair<string, GridPropertyDetails> item in gridPropertyDictionary)
        {
            GridPropertyDetails gridPropertyDetails = item.Value;

            DisplayDugGround(gridPropertyDetails);

            DisplayWateredGround(gridPropertyDetails);
        }
    }

    /// <summary>
    /// 从SO_GridProperties资源中实例化网格属性字典
    /// </summary>
    private void InitialiseGridProperties()
    {
        //读取所有SO_GridProperties资源信息
        foreach (SO_GridProperties so_GridProperties in so_gridPropertiesArry)
        {
            //网格属性详情字典
            Dictionary<string, GridPropertyDetails> gridPropertyDictionary = new Dictionary<string, GridPropertyDetails>();
            //读取每个场景网格属性集合信息并填充进入上面字典中
            foreach(GridProperty gridProperty in so_GridProperties.gridPropertyList)
            {
                GridPropertyDetails gridPropertyDetails;
                gridPropertyDetails = GetGridPropertyDetails(gridProperty.gridCoordinate.x, gridProperty.gridCoordinate.y, gridPropertyDictionary);
                if (gridPropertyDetails == null)
                {
                    gridPropertyDetails = new GridPropertyDetails();
                }
                switch(gridProperty.gridBoolProperty)
                {
                    case GridBoolProperty.diggable:
                        gridPropertyDetails.isDiggable = gridProperty.gridBoolValue; break;
                    case GridBoolProperty.canDropItem:
                        gridPropertyDetails.canDropItem = gridProperty.gridBoolValue; break;
                    case GridBoolProperty.canPlaceFurniture:
                        gridPropertyDetails.canPlaceFurniture = gridProperty.gridBoolValue; break;
                    case GridBoolProperty.isPath:
                        gridPropertyDetails.isPath = gridProperty.gridBoolValue; break;
                    case GridBoolProperty.isNPCObstacle:
                        gridPropertyDetails.isNPCObstacle = gridProperty.gridBoolValue; break;
                    default: break;
                }
                //填充网格详情
                SetGridPropertyDetails(gridProperty.gridCoordinate.x, gridProperty.gridCoordinate.y, gridPropertyDetails, gridPropertyDictionary);
            }
            //为当前游戏物体（挂载了当前脚本的物体）创建场景存储信息
            SceneSave sceneSave = new SceneSave();
            sceneSave.gridPropertyDetailsDictionary = gridPropertyDictionary;

            if(so_GridProperties.sceneName.ToString().Equals(SceneControllerManager.Instance.startingSceneNmae.ToString()))
            {
                this.gridPropertyDictionary = gridPropertyDictionary;
            }
            //添加场景数据
            GameObjectSave.sceneData.Add(so_GridProperties.sceneName.ToString(), sceneSave);
        }
    }

    private string constructKeyForCoordinate(int gridX,int gridY)
    {
        return "x" + gridX + "y" + gridY;
    }

    /// <summary>
    /// 根据坐标和网格属性详情添加至字典中   用于初始化
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="gridPropertyDetails"></param>
    /// <param name="gridPropertyDictionary"></param>
    public void SetGridPropertyDetails(int x, int y, GridPropertyDetails gridPropertyDetails, Dictionary<string, GridPropertyDetails> gridPropertyDictionary)
    {
        //根据坐标构建字典的key
        string key = constructKeyForCoordinate(x, y);

        gridPropertyDetails.gridX = x;
        gridPropertyDetails.gridY = y;

        if (gridPropertyDictionary.ContainsKey(key))
        {
            //当前网格已存在，重写属性
            gridPropertyDetails.isDiggable = gridPropertyDictionary[key].isDiggable || gridPropertyDetails.isDiggable;
            gridPropertyDetails.canPlaceFurniture = gridPropertyDictionary[key].canPlaceFurniture || gridPropertyDetails.canPlaceFurniture;
            gridPropertyDetails.canDropItem = gridPropertyDictionary[key].canDropItem || gridPropertyDetails.canDropItem;
            gridPropertyDetails.isPath = gridPropertyDictionary[key].isPath || gridPropertyDetails.isPath;
            gridPropertyDetails.isNPCObstacle = gridPropertyDictionary[key].isNPCObstacle || gridPropertyDetails.isNPCObstacle;

        }
        gridPropertyDictionary[key] = gridPropertyDetails;
    }

    public void SetGridPropertyDetails(int gridX,int gridY, GridPropertyDetails gridPropertyDetails)
    {
        SetGridPropertyDetails(gridX, gridY, gridPropertyDetails, gridPropertyDictionary);
    }

    /// <summary>
    /// 根据坐标和网格详情字典获取具体的网格详细信息
    /// </summary>
    /// <param name="gridX">网格坐标x</param>
    /// <param name="gridY">网格坐标y</param>
    /// <param name="gridPropertyDictionary">网格信息字典</param>
    /// <returns></returns>
    public GridPropertyDetails GetGridPropertyDetails(int gridX, int gridY, Dictionary<string, GridPropertyDetails> gridPropertyDictionary)
    {
        //根据坐标构建字典的key
        string key = constructKeyForCoordinate(gridX, gridY);
        GridPropertyDetails gridPropertyDetails;
        //获取坐标对应的网格详情
        if (!gridPropertyDictionary.TryGetValue(key, out gridPropertyDetails))
        {
            return null;
        }
        else
        {
            return gridPropertyDetails;
        }
    }

    /// <summary>
    /// 根据网格坐标获取网格详情
    /// </summary>
    /// <param name="gridX"></param>
    /// <param name="gridY"></param>
    /// <returns></returns>
    public GridPropertyDetails GetGridPropertyDetails(int gridX,int gridY)
    {
        return GetGridPropertyDetails(gridX,gridY,gridPropertyDictionary);
    }

    /// <summary>
    /// 在场景加载完成后执行
    /// </summary>
    private void AfterSceneLoaded()
    {
        // 获取网格
        grid = GameObject.FindObjectOfType<Grid>();

        // 获取瓦片地图
        groundDecoration1 = GameObject.FindGameObjectWithTag(Tags.GroundDecoration1).GetComponent<Tilemap>();
        groundDecoration2 = GameObject.FindGameObjectWithTag(Tags.GroundDecoration2).GetComponent<Tilemap>();
    }

    /// <summary>
    /// 切换到下一日
    /// </summary>
    /// <param name="gameYear"></param>
    /// <param name="gameSeason"></param>
    /// <param name="gameDay"></param>
    /// <param name="gameDayOfWeek"></param>
    /// <param name="gameHour"></param>
    /// <param name="gameMinute"></param>
    /// <param name="gameSecond"></param>
    private void AdvanceDay(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek,
        int gameHour, int gameMinute, int gameSecond)
    {
        // 清除所有已展示的地面信息
        ClearDisplayGridPropertyDetails();

        // 遍历所有场景
        foreach(SO_GridProperties sO_GridProperties in so_gridPropertiesArry)
        {
            // 获取场景的网格属性字典
            if(GameObjectSave.sceneData.TryGetValue(sO_GridProperties.sceneName.ToString(), out SceneSave sceneSave))
            {
                if(sceneSave.gridPropertyDetailsDictionary != null)
                {
                    // 遍历每一格网格属性信息
                    for(int i = sceneSave.gridPropertyDetailsDictionary.Count - 1; i >= 0; i--)
                    {
                        KeyValuePair<string, GridPropertyDetails> item = sceneSave.gridPropertyDetailsDictionary.ElementAt(i);
                        GridPropertyDetails gridPropertyDetails = item.Value;

                        #region 切换到下一日时更新所有网格属性信息

                        if(gridPropertyDetails.daysSinceWatered > -1)
                        {
                            // 下一日时上一日已浇水的网格会重置为未浇水状态
                            gridPropertyDetails.daysSinceWatered = -1;
                        }
                        SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails, sceneSave.gridPropertyDetailsDictionary);

                        #endregion
                    }
                }
            }
        }

        DisplayGridPropertyDetails();
    }

    #region 数据存储接口方法实现

    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        //实例化场景信息
        if(GameObjectSave.sceneData.TryGetValue(sceneName,out SceneSave sceneSave))
        {
            // 获取网格属性详情
            if(sceneSave.gridPropertyDetailsDictionary != null)
            {
                gridPropertyDictionary = sceneSave.gridPropertyDetailsDictionary;
            }
            // 如果网格属性字典存在
            if(gridPropertyDictionary.Count > 0)
            {
                // 清除地面装饰  例如挖掘地面、种植等
                ClearDisplayGridPropertyDetails();
                // 显示地面
                DisplayGridPropertyDetails();
            }
        }

    }

    public void ISaveableStoreScene(string sceneName)
    {
        //存储场景信息
        GameObjectSave.sceneData.Remove(sceneName);

        SceneSave sceneSave = new SceneSave();

        sceneSave.gridPropertyDetailsDictionary = gridPropertyDictionary;

        GameObjectSave.sceneData.Add(sceneName, sceneSave);
    }

    #endregion
}
