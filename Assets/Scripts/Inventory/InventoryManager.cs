
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;


/// <summary>
/// ������
/// ����
/// </summary>
public class InventoryManager : SingletonMonoBehvior<InventoryManager> //, ISaveable
{
    /// <summary>
    /// �����ֵ�
    /// </summary>
    private Dictionary<int,ItemDetails> itemDetailsDictionary;

    /// <summary>
    /// �ڿ����ѡ�е���Ŀ
    /// �±꣺�������
    /// value��itemcode
    /// </summary>
    private int[] selectedInventoryItem;
    /// <summary>
    /// ��ɫ����������
    /// </summary>
    private UIInventoryBar inventoryBar;

    /// <summary>
    /// ����б�
    /// ����ÿ��Ԫ�ض���һ��list
    /// </summary>
    public List<InventoryItem>[] inventoryLists;

    //public Dictionary<int, InventoryItem>[] inventoryDictionaries;

    /// <summary>
    /// ��ͬ������͵�����
    /// �������±�Ϊ�������ö��ֵ����Ӧ��Ԫ��ֵΪ�������
    /// </summary>
    [HideInInspector] public int[] inventoryListCapacityIntArry;

    /// <summary>
    /// ��Ʒ����
    /// </summary>
    [SerializeField]
    private SO_ItemList itemList = null;

    /*private string _iSaveableUniqueID;
    public string ISaveableUniqueId { get => _iSaveableUniqueID; set => _iSaveableUniqueID = value; }
    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get => _gameObjectSave; set => _gameObjectSave = value; }*/

    protected override void Awake()
    {
        base.Awake();
        //���������Ϣ
        CreateInventoryLists();

        //��Ʒ��Ϣ��awake�г�ʼ����ȷ��ʹ��ʱ��awake��ʹ��
        CreateItemDetailsDictionary();

        //ʵ����ѡ�еĿ����Ŀ����
        selectedInventoryItem = new int[(int)InventoryLocation.count];
        for(int i = 0; i< selectedInventoryItem.Length; i++)
        {
            selectedInventoryItem[i] = -1;
        }

        //��ȡuid
        /*ISaveableUniqueId = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();*/
    }

    private void OnEnable()
    {
        ISaveableRegister();
    }

    private void OnDisable()
    {
        ISaveableDeregister();
    }

    private void Start()
    {
        inventoryBar = FindObjectOfType<UIInventoryBar>();
    }

    /// <summary>
    /// �������Ϳ����Ϣ
    /// </summary>
    private void CreateInventoryLists()
    {
        inventoryLists = new List<InventoryItem>[(int)InventoryLocation.count];

        for (int i = 0; i < (int)InventoryLocation.count; i++)
        {
            inventoryLists[i] = new List<InventoryItem>();
        }

        //��ʼ����ͬ������͵Ĵ�С
        inventoryListCapacityIntArry = new int[(int)InventoryLocation.count];

        //��ʼ����ұ����ռ�����
        inventoryListCapacityIntArry[(int)InventoryLocation.player] = Settings.playerInitialInventoryCapacity;

        //inventoryListCapacityIntArry[(int)InventoryLocation.chest] = Settings.
    }

    /// <summary>
    /// �����ݴ�scriptableObject ItemListд���ֵ�
    /// </summary>
    private void CreateItemDetailsDictionary()
    {
        itemDetailsDictionary  = new Dictionary<int,ItemDetails>();

        foreach(ItemDetails itemDetials in itemList.itemDetials)
        {
            itemDetailsDictionary.Add(itemDetials.itemCode, itemDetials);
        }
    }

    /// <summary>
    /// ����Ʒ��ӵ�����Ӧλ�ú�ɾ�������е������Ʒ
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <param name="item"></param>
    /// <param name="gameobjectToDelete"></param>
    public void AddItem(InventoryLocation inventoryLocation, Item item, GameObject gameobjectToDelete)
    {
        AddItem(inventoryLocation, item);
        Destroy(gameobjectToDelete);
    }

    /// <summary>
    /// �����Ʒ�������ȥ
    /// </summary>
    /// <param name="itemLocation"></param>
    /// <param name="item"></param>
    public void AddItem(InventoryLocation itemLocation, Item item)
    {
        int itemCode = item.ItemCode;
        List<InventoryItem> inventoryList = inventoryLists[(int)itemLocation];

        //�жϿ�����Ƿ��Ѿ��������Ʒ
        int itemPosition = FindItemInInventory(itemLocation, item);

        if(itemPosition != -1)
        {
            AddItemAtPosition(inventoryList, itemCode, itemPosition);
        }
        else
        {
            AddItemAtPosition(inventoryList, itemCode);
        }
        //���ÿ������¼�
        EventHandler.CallInventoryUpdatedEvent(itemLocation, inventoryLists[(int)itemLocation]);
    }

    /// <summary>
    /// ������Ʒ�ڿ���е��������Ѵ��������Ʒ��
    /// </summary>
    /// <param name="inventoryList"></param>
    /// <param name="itemCode"></param>
    /// <param name="itemPosition"></param>
    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int itemPosition)
    {
        InventoryItem item = new InventoryItem();

        int quantity = inventoryList[itemPosition].itemQuantity + 1;
        item.itemQuantity = quantity;
        item.itemCode = itemCode;
        inventoryList[itemPosition] = item;
        //��ʾ��ǰ���
        //Debug.ClearDeveloperConsole();
        //DebugPrintInventoryList(inventoryList);
    }

    /// <summary>
    /// ������Ʒ�ڿ���е������������������Ʒ��
    /// </summary>
    /// <param name="inventoryList"></param>
    /// <param name="itemCode"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode)
    {
        InventoryItem inventoryItem = new InventoryItem();

        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = 1;
        inventoryList.Add(inventoryItem);
        //��ʾ��ǰ���
        //DebugPrintInventoryList(inventoryList);

    }

    /*private void DebugPrintInventoryList(List<InventoryItem> inventoryList)
    {
        foreach(InventoryItem item in inventoryList)
        {
            Debug.Log("Item Description:" + InventoryManager.Instance.GetItemDetails(item.itemCode).itemDescription + "    Item Quantity: " + item.itemQuantity);
        }
        Debug.Log("*************************************************************");
    }*/

    /// <summary>
    /// ���ҿ�����Ƿ����������Ʒ
    /// </summary>
    /// <param name="itemLocation"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public int FindItemInInventory(InventoryLocation itemLocation, Item item)
    {
        //��ǰ�Ŀ���б�
        List<InventoryItem> inventoryList = inventoryLists[(int)itemLocation];

        for(int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].itemCode == item.ItemCode)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// ���ҿ�����Ƿ����������Ʒ
    /// </summary>
    /// <param name="itemLocation"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public int FindItemInInventory(InventoryLocation itemLocation, int itemCode)
    {
        //��ǰ�Ŀ���б�
        List<InventoryItem> inventoryList = inventoryLists[(int)itemLocation];
        ItemDetails item = GetItemDetails(itemCode);
        if (item != null)
        {
            for (int i = 0; i < inventoryList.Count; i++)
            {
                if (inventoryList[i].itemCode == item.itemCode)
                {
                    return i;
                }
            }
        }
        
        return -1;
    }

    /// <summary>
    /// ��ȡ��Ʒ����
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public ItemDetails GetItemDetails(int itemCode)
    {
        ItemDetails itemDetials;
        if(itemDetailsDictionary.TryGetValue(itemCode, out itemDetials)) 
        { 
            return itemDetials; 
        }
        return null;
    }

    /// <summary>
    /// ��ȡѡ�еĿ����Ʒ����Ʒ����
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <returns></returns>
    private int GetSelectedInventoryItem(InventoryLocation inventoryLocation)
    {
        return selectedInventoryItem[(int)inventoryLocation];
    }

    /// <summary>
    /// ��ȡѡ�е���Ʒ����Ʒ����
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <returns></returns>
    public ItemDetails GetSelectedInventoryItemDetails(InventoryLocation inventoryLocation)
    {
        int itemCode = GetSelectedInventoryItem(inventoryLocation);
        if (itemCode == -1)
        {
            return null;
        }
        else
        {
            return GetItemDetails(itemCode);
        }
    }

    /// <summary>
    /// �Ƴ�����е���Ʒ
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemCode"></param>
    public void RemoveItem(InventoryLocation inventoryLocation, int itemCode)
    {
        //��ȡ��ǰ�����Ʒ�б�
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];
        //�����Ʒ�Ƿ��ڿ���д���
        int itemPosition = FindItemInInventory(inventoryLocation,itemCode);

        if(itemPosition != -1)
        {
            RemoveItemAtPosition(inventoryList, itemCode, itemPosition);
        }

        //�����������¼�
        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }

    /// <summary>
    /// �Ƴ�����е�ǰλ�õ���Ʒ
    /// </summary>
    /// <param name="inventoryList"></param>
    /// <param name="itemCode"></param>
    /// <param name="itemPosition"></param>
    private void RemoveItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int itemPosition)
    {
        InventoryItem inventoryItem = new InventoryItem();
        //һ��ֻ�Ƴ�һ����Ʒ
        int quantity = inventoryList[itemPosition].itemQuantity - 1;

        if(quantity > 0)
        {
            //����ʣ����Ʒ���޸�����
            inventoryItem.itemQuantity = quantity;
            inventoryItem.itemCode = itemCode;
            inventoryList[itemPosition] = inventoryItem;
        }
        else
        {
            //û��ʣ����Ʒ��ɾ�����λ�õ�����
            inventoryList.RemoveAt(itemPosition);
        }
    }

    /// <summary>
    /// ���������������λ����Ʒ
    /// </summary>
    /// <param name="fromItem">����ק��Ʒ�ĳ�ʼλ��</param>
    /// <param name="toItem">����ק��Ʒ��Ŀ��λ��</param>
    public void SwapInventoryItems(InventoryLocation inventoryLocation, int fromItem, int toItem)
    {
        //��ʼλ�ú�Ŀ��λ�ö��ڿ�淶Χ�ڣ��Ҳ���ȣ��Ҷ���С��0
        //����Ƚϵ��ǿ����Ʒ�����������Ҫ���Էŵ��յĲ�ۣ�Ӧ��Ҫ����ɫ����������
        if(fromItem < inventoryLists[(int)inventoryLocation].Count && toItem < inventoryLists[(int)inventoryLocation].Count
            && fromItem != toItem && fromItem >= 0 && toItem >= 0)
        {
            //��ȡ����λ����Ʒ��Ϣ
            InventoryItem fromInventoryItem = inventoryLists[(int)inventoryLocation][fromItem];
            InventoryItem toInventoryItem = inventoryLists[(int)inventoryLocation][toItem];
            //����
            inventoryLists[(int)inventoryLocation][toItem] = fromInventoryItem;
            inventoryLists[(int)inventoryLocation][fromItem] = toInventoryItem;
            //�����������¼�
            EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
        }
    }

    /// <summary>
    /// ��ȡ��Ʒ����
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    public string GetItemTypeDescription(ItemType itemType)
    {
        string itemTypeDescription;
        switch(itemType)
        {
            case ItemType.Breaking_tool:
                itemTypeDescription = Settings.BreakingTool;
                break;
            case ItemType.Chopping_tool:
                itemTypeDescription = Settings.ChoppingTool;
                break;
            case ItemType.Hoeing_tool:
                itemTypeDescription = Settings.HoeingTool;
                break;
            case ItemType.Reaping_tool:
                itemTypeDescription = Settings.ReapingTool;
                break;
            case ItemType.Watering_tool:
                itemTypeDescription = Settings.WateringTool;
                break;
            case ItemType.Collecting_tool:
                itemTypeDescription = Settings.CollectingTool;
                break;
            default:
                itemTypeDescription = itemType.ToString();
                break;
        }
        return itemTypeDescription;
    }

    /// <summary>
    /// ���ÿ���б�ѡ�е���Ʒ
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <param name="itemCode"></param>
    public void SetSelectedInventoryItem(InventoryLocation inventoryLocation,int itemCode)
    {
        selectedInventoryItem[(int)inventoryLocation] = itemCode;
    }

    /// <summary>
    /// ȡ��ѡ��
    /// </summary>
    /// <param name="inventoryLocation"></param>
    public void ClearSelectedInventoryItem(InventoryLocation inventoryLocation)
    {
        selectedInventoryItem[(int)inventoryLocation] = -1;
    }

    public void ISaveableRegister()
    {
        
    }

    public void ISaveableDeregister()
    {
        
    }

    public void ISaveableStoreScene(string sceneName)
    {
        
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        
    }
}
