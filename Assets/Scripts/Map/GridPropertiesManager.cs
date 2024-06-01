using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������Թ���
/// </summary>
[RequireComponent(typeof(GenerateGUID))]
public class GridPropertiesManager : SingletonMonoBehvior<GridPropertiesManager>, ISaveable
{
    public Grid grid;
    /// <summary>
    /// �������������ֵ�
    /// </summary>
    private Dictionary<string, GridPropertyDetails> gridPropertyDictionary;
    /// <summary>
    /// �����������飬scriptableobjectassets�е�SO_GridProperties
    /// ��ǰ�ű����Ƕ����������й���
    /// </summary>
    [SerializeField] private SO_GridProperties[] so_gridPropertiesArry = null;

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
    }

    private void OnDisable()
    {
        //����ע����ȡ������
        ISaveableDeregister();
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoaded;
    }

    private void Start()
    {
        InitialiseGridProperties();
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
        grid = GameObject.FindObjectOfType<Grid>();
    }

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
            if(sceneSave.gridPropertyDetailsDictionary != null)
            {
                gridPropertyDictionary = sceneSave.gridPropertyDetailsDictionary;
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
}
