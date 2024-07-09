using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

/// <summary>
/// �������Թ���
/// </summary>
[RequireComponent(typeof(GenerateGUID))]
public class GridPropertiesManager : SingletonMonoBehvior<GridPropertiesManager>, ISaveable
{
    private Tilemap groundDecoration1;
    private Tilemap groundDecoration2;

    private Grid grid;
    /// <summary>
    /// �������������ֵ�
    /// </summary>
    private Dictionary<string, GridPropertyDetails> gridPropertyDictionary;
    /// <summary>
    /// �����������飬scriptableobjectassets�е�SO_GridProperties
    /// ��ǰ�ű����Ƕ����������й���
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
        //����ע�Ტ�����¼�
        ISaveableRegister();
        EventHandler.AfterSceneLoadEvent += AfterSceneLoaded;
        EventHandler.AdvanceGameDayEvent += AdvanceDay;
    }

    private void OnDisable()
    {
        //����ע����ȡ������
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
    /// �����ʾ������װ����Ϣ
    /// </summary>
    private void ClearDisplayGridPropertyDetails()
    {
        ClearDisplayGroundDecorations();
    }

    /// <summary>
    /// ��ʾָ�����ھ�ĵ���
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
    /// ��ʾ�Ѿ���ˮ�ĵ���
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
    /// �����Ѿ�����ˮ������
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    private void ConnectWateredGround(GridPropertyDetails gridPropertyDetails)
    {
        // ���õ�ǰ����
        Tile wateredTile0 = SetWateredTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY);
        groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY, 0), wateredTile0);

        // ���ڵ�����
        GridPropertyDetails adjacentGridPropertyDetails;
        // ��ǰ��ˮ�����Ϸ���һ������
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceWatered > -1)
        {
            Tile wateredTile1 = SetWateredTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1, 0), wateredTile1);
        }

        // ��ǰ��ˮ�����·���һ������
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceWatered > -1)
        {
            Tile wateredTile2 = SetWateredTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1, 0), wateredTile2);
        }

        // ��ǰ��ˮ������ߵ�һ������
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceWatered > -1)
        {
            Tile wateredTile3 = SetWateredTile(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY, 0), wateredTile3);
        }

        // ��ǰ��ˮ�����ұߵ�һ������
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceWatered > -1)
        {
            Tile wateredTile4 = SetWateredTile(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY, 0), wateredTile4);
        }
    }

   

    /// <summary>
    /// �����Ѿ��ھ���ĵط�
    /// �ж��������ҵ������Ƿ��ھ��������������������
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    private void ConnectDugGround(GridPropertyDetails gridPropertyDetails)
    {
        // ���õ�ǰ����
        Tile dugTile0 = SetDugTile(gridPropertyDetails.gridX,gridPropertyDetails.gridY);
        groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY, 0), dugTile0);

        // ���ڵ�����
        GridPropertyDetails adjacentGridPropertyDetails;
        // ��ǰ�ھ������Ϸ���һ������
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
        if(adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceDug > -1)
        {
            Tile dugTile1 = SetDugTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1, 0), dugTile1);
        }

        // ��ǰ�ھ������·���һ������
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceDug > -1)
        {
            Tile dugTile2 = SetDugTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1, 0), dugTile2);
        }

        // ��ǰ�ھ�������ߵ�һ������
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceDug > -1)
        {
            Tile dugTile2 = SetDugTile(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY, 0), dugTile2);
        }

        // ��ǰ�ھ������ұߵ�һ������
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceDug > -1)
        {
            Tile dugTile2 = SetDugTile(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY, 0), dugTile2);
        }
    }

    /// <summary>
    /// �����ھ����Ƭ
    /// </summary>
    /// <param name="xGrid"></param>
    /// <param name="yGrid"></param>
    /// <returns></returns>
    private Tile SetDugTile(int xGrid, int yGrid)
    {
        // ��ǰ�����������������ĸ�����ĵ����Ƿ��ھ��
        bool upDug = IsGridSquareDug(xGrid, yGrid + 1);
        bool downDug = IsGridSquareDug(xGrid, yGrid - 1);
        bool leftDug = IsGridSquareDug(xGrid - 1, yGrid);
        bool rightDug = IsGridSquareDug(xGrid + 1, yGrid);

        #region ���������Ƿ��ھ�������ò�ͬ��������Ƭ

        // ���������Ŀ�����ͬ���ھ��������16�в�ͬ�����

        if(!upDug && !downDug && !rightDug && !leftDug)
        {
            // �������Ҷ�δ�ھ�
            return dugGround[0];
        }
        else if (!upDug && downDug && rightDug && !leftDug)
        {
            // �·����ҷ��ھ��
            return dugGround[1];
        }
        else if (!upDug && downDug && rightDug && leftDug)
        {
            // ��  ��  ��
            return dugGround[2];
        }
        else if (!upDug && downDug && !rightDug && leftDug)
        {
            // ��  ��
            return dugGround[3];
        }
        else if (!upDug && downDug && !rightDug && !leftDug)
        {
            // ��
            return dugGround[4];
        }
        else if (upDug && downDug && rightDug && !leftDug)
        {
            // ��  �� ��
            return dugGround[5];
        }
        else if (upDug && downDug && rightDug && leftDug)
        {
            //  ��������
            return dugGround[6];
        }
        else if (upDug && downDug && !rightDug && leftDug)
        {
            //  �� �� ��
            return dugGround[7];
        }
        else if (upDug && downDug && !rightDug && !leftDug)
        {
            //  ��  ��
            return dugGround[8];
        }
        else if (upDug && !downDug && rightDug && !leftDug)
        {
            //  ��  ��
            return dugGround[9];
        }
        else if (upDug && !downDug && rightDug && leftDug)
        {
            //  ��  �� ��
            return dugGround[10];
        }
        else if (upDug && !downDug && !rightDug && leftDug)
        {
            //  ��  ��
            return dugGround[11];
        }
        else if (upDug && !downDug && !rightDug && !leftDug)
        {
            //  ��
            return dugGround[12];
        }
        else if (!upDug && !downDug && rightDug && !leftDug)
        {
            //  ��
            return dugGround[13];
        }
        else if (!upDug && !downDug && rightDug && leftDug)
        {
            //  ��  ��
            return dugGround[14];
        }
        else if (!upDug && !downDug && !rightDug && leftDug)
        {
            // ��
            return dugGround[15];
        }

        return null;

        #endregion
    }

    /// <summary>
    /// ���ý�ˮ����Ƭ
    /// </summary>
    /// <param name="xGrid"></param>
    /// <param name="yGrid"></param>
    /// <returns></returns>
    private Tile SetWateredTile(int xGrid, int yGrid)
    {
        // ��ǰ�����������������ĸ�����ĵ����Ƿ��ھ��
        bool upWatered = IsGridSquareWatered(xGrid, yGrid + 1);
        bool downWatered = IsGridSquareWatered(xGrid, yGrid - 1);
        bool leftWatered = IsGridSquareWatered(xGrid - 1, yGrid);
        bool rightWatered = IsGridSquareWatered(xGrid + 1, yGrid);

        #region ���������Ƿ��ھ�������ò�ͬ��������Ƭ

        // ���������Ŀ�����ͬ���ھ��������16�в�ͬ�����

        if (!upWatered && !downWatered && !rightWatered && !leftWatered)
        {
            // �������Ҷ�δ�ھ�
            return wateredGround[0];
        }
        else if (!upWatered && downWatered && rightWatered && !leftWatered)
        {
            // �·����ҷ��ھ��
            return wateredGround[1];
        }
        else if (!upWatered && downWatered && rightWatered && leftWatered)
        {
            // ��  ��  ��
            return wateredGround[2];
        }
        else if (!upWatered && downWatered && !rightWatered && leftWatered)
        {
            // ��  ��
            return wateredGround[3];
        }
        else if (!upWatered && downWatered && !rightWatered && !leftWatered)
        {
            // ��
            return wateredGround[4];
        }
        else if (upWatered && downWatered && rightWatered && !leftWatered)
        {
            // ��  �� ��
            return wateredGround[5];
        }
        else if (upWatered && downWatered && rightWatered && leftWatered)
        {
            //  ��������
            return wateredGround[6];
        }
        else if (upWatered && downWatered && !rightWatered && leftWatered)
        {
            //  �� �� ��
            return wateredGround[7];
        }
        else if (upWatered && downWatered && !rightWatered && !leftWatered)
        {
            //  ��  ��
            return wateredGround[8];
        }
        else if (upWatered && !downWatered && rightWatered && !leftWatered)
        {
            //  ��  ��
            return wateredGround[9];
        }
        else if (upWatered && !downWatered && rightWatered && leftWatered)
        {
            //  ��  �� ��
            return wateredGround[10];
        }
        else if (upWatered && !downWatered && !rightWatered && leftWatered)
        {
            //  ��  ��
            return wateredGround[11];
        }
        else if (upWatered && !downWatered && !rightWatered && !leftWatered)
        {
            //  ��
            return wateredGround[12];
        }
        else if (!upWatered && !downWatered && rightWatered && !leftWatered)
        {
            //  ��
            return wateredGround[13];
        }
        else if (!upWatered && !downWatered && rightWatered && leftWatered)
        {
            //  ��  ��
            return wateredGround[14];
        }
        else if (!upWatered && !downWatered && !rightWatered && leftWatered)
        {
            // ��
            return wateredGround[15];
        }

        return null;

        #endregion
    }

    /// <summary>
    /// �ж������Ӧ�������Ƿ񽽹�ˮ
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
    /// �ж������Ӧ�������Ƿ��ھ��
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
    /// ��ʾ������Ϣ  �ھ򡢽�ˮ����ֲ
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
    /// ��SO_GridProperties��Դ��ʵ�������������ֵ�
    /// </summary>
    private void InitialiseGridProperties()
    {
        //��ȡ����SO_GridProperties��Դ��Ϣ
        foreach (SO_GridProperties so_GridProperties in so_gridPropertiesArry)
        {
            //�������������ֵ�
            Dictionary<string, GridPropertyDetails> gridPropertyDictionary = new Dictionary<string, GridPropertyDetails>();
            //��ȡÿ�������������Լ�����Ϣ�������������ֵ���
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
                //�����������
                SetGridPropertyDetails(gridProperty.gridCoordinate.x, gridProperty.gridCoordinate.y, gridPropertyDetails, gridPropertyDictionary);
            }
            //Ϊ��ǰ��Ϸ���壨�����˵�ǰ�ű������壩���������洢��Ϣ
            SceneSave sceneSave = new SceneSave();
            sceneSave.gridPropertyDetailsDictionary = gridPropertyDictionary;

            if(so_GridProperties.sceneName.ToString().Equals(SceneControllerManager.Instance.startingSceneNmae.ToString()))
            {
                this.gridPropertyDictionary = gridPropertyDictionary;
            }
            //��ӳ�������
            GameObjectSave.sceneData.Add(so_GridProperties.sceneName.ToString(), sceneSave);
        }
    }

    private string constructKeyForCoordinate(int gridX,int gridY)
    {
        return "x" + gridX + "y" + gridY;
    }

    /// <summary>
    /// ���������������������������ֵ���   ���ڳ�ʼ��
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="gridPropertyDetails"></param>
    /// <param name="gridPropertyDictionary"></param>
    public void SetGridPropertyDetails(int x, int y, GridPropertyDetails gridPropertyDetails, Dictionary<string, GridPropertyDetails> gridPropertyDictionary)
    {
        //�������깹���ֵ��key
        string key = constructKeyForCoordinate(x, y);

        gridPropertyDetails.gridX = x;
        gridPropertyDetails.gridY = y;

        if (gridPropertyDictionary.ContainsKey(key))
        {
            //��ǰ�����Ѵ��ڣ���д����
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
    /// ������������������ֵ��ȡ�����������ϸ��Ϣ
    /// </summary>
    /// <param name="gridX">��������x</param>
    /// <param name="gridY">��������y</param>
    /// <param name="gridPropertyDictionary">������Ϣ�ֵ�</param>
    /// <returns></returns>
    public GridPropertyDetails GetGridPropertyDetails(int gridX, int gridY, Dictionary<string, GridPropertyDetails> gridPropertyDictionary)
    {
        //�������깹���ֵ��key
        string key = constructKeyForCoordinate(gridX, gridY);
        GridPropertyDetails gridPropertyDetails;
        //��ȡ�����Ӧ����������
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
    /// �������������ȡ��������
    /// </summary>
    /// <param name="gridX"></param>
    /// <param name="gridY"></param>
    /// <returns></returns>
    public GridPropertyDetails GetGridPropertyDetails(int gridX,int gridY)
    {
        return GetGridPropertyDetails(gridX,gridY,gridPropertyDictionary);
    }

    /// <summary>
    /// �ڳ���������ɺ�ִ��
    /// </summary>
    private void AfterSceneLoaded()
    {
        // ��ȡ����
        grid = GameObject.FindObjectOfType<Grid>();

        // ��ȡ��Ƭ��ͼ
        groundDecoration1 = GameObject.FindGameObjectWithTag(Tags.GroundDecoration1).GetComponent<Tilemap>();
        groundDecoration2 = GameObject.FindGameObjectWithTag(Tags.GroundDecoration2).GetComponent<Tilemap>();
    }

    /// <summary>
    /// �л�����һ��
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
        // ���������չʾ�ĵ�����Ϣ
        ClearDisplayGridPropertyDetails();

        // �������г���
        foreach(SO_GridProperties sO_GridProperties in so_gridPropertiesArry)
        {
            // ��ȡ���������������ֵ�
            if(GameObjectSave.sceneData.TryGetValue(sO_GridProperties.sceneName.ToString(), out SceneSave sceneSave))
            {
                if(sceneSave.gridPropertyDetailsDictionary != null)
                {
                    // ����ÿһ������������Ϣ
                    for(int i = sceneSave.gridPropertyDetailsDictionary.Count - 1; i >= 0; i--)
                    {
                        KeyValuePair<string, GridPropertyDetails> item = sceneSave.gridPropertyDetailsDictionary.ElementAt(i);
                        GridPropertyDetails gridPropertyDetails = item.Value;

                        #region �л�����һ��ʱ������������������Ϣ

                        if(gridPropertyDetails.daysSinceWatered > -1)
                        {
                            // ��һ��ʱ��һ���ѽ�ˮ�����������Ϊδ��ˮ״̬
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

    #region ���ݴ洢�ӿڷ���ʵ��

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
        //ʵ����������Ϣ
        if(GameObjectSave.sceneData.TryGetValue(sceneName,out SceneSave sceneSave))
        {
            // ��ȡ������������
            if(sceneSave.gridPropertyDetailsDictionary != null)
            {
                gridPropertyDictionary = sceneSave.gridPropertyDetailsDictionary;
            }
            // ������������ֵ����
            if(gridPropertyDictionary.Count > 0)
            {
                // �������װ��  �����ھ���桢��ֲ��
                ClearDisplayGridPropertyDetails();
                // ��ʾ����
                DisplayGridPropertyDetails();
            }
        }

    }

    public void ISaveableStoreScene(string sceneName)
    {
        //�洢������Ϣ
        GameObjectSave.sceneData.Remove(sceneName);

        SceneSave sceneSave = new SceneSave();

        sceneSave.gridPropertyDetailsDictionary = gridPropertyDictionary;

        GameObjectSave.sceneData.Add(sceneName, sceneSave);
    }

    #endregion
}
