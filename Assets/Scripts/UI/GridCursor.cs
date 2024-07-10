using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������   ������Ʒ/�Ҿߵ�ʱ�����ڵ�������ʾ��ǰ�����Ƿ���Է���
/// </summary>
public class GridCursor : MonoBehaviour
{
    /// <summary>
    /// ui����
    /// </summary>
    private Canvas canvas;
    /// <summary>
    /// ����
    /// </summary>
    private Grid grid;
    /// <summary>
    /// �����
    /// </summary>
    private Camera mainCamera;

    /// <summary>
    /// ���ͼƬ
    /// </summary>
    [SerializeField] private Image cursorImage = null;
    /// <summary>
    /// λ�� ���ŵ���Ϣ
    /// </summary>
    [SerializeField] private RectTransform cursorRectTransform = null;
    /// <summary>
    /// ��ɫ��꾫��ͼ ����ǰλ�ÿ��Է���
    /// </summary>
    [SerializeField] private Sprite greenCursorSprite = null;
    /// <summary>
    /// ��ɫ��꾫��ͼ ����ǰλ�ò��ɷ���
    /// </summary>
    [SerializeField] private Sprite redCursorSprite = null;


    private bool _cursorPositionIsValid = false;
    /// <summary>
    /// ���λ���Ƿ����  ���Ƿ���Է�����Ʒ���Ƿ���Գ����ֵصȵ�
    /// </summary>
    public bool CursorPositionIsValid { get => _cursorPositionIsValid; set => _cursorPositionIsValid = value; }

    private int _itemUserGridRadius = 0;
    /// <summary>
    /// ��Ʒʹ������뾶
    /// </summary>
    public int ItemUserGridRadius { get => _itemUserGridRadius; set => _itemUserGridRadius = value; }

    private ItemType _selectedItemType;
    /// <summary>
    /// ѡ�����Ʒ����
    /// </summary>
    public ItemType SelectedItemType { get => _selectedItemType; set => _selectedItemType = value; }

    private bool _cursorIsEnabled = false;
    /// <summary>
    /// ����Ƿ���
    /// </summary>
    public bool CursorIsEnabled { get=> _cursorIsEnabled; set => _cursorIsEnabled = value; }


    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= SceneLoaded;
    }

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += SceneLoaded;
    }

    /// <summary>
    /// ����������Ҫ���Ĵ���
    /// </summary>
    private void SceneLoaded()
    {
        //��ȡ�������͵���Ϸ����
        //����û��ȡ����֪��Ϊʲô
        grid = GameObject.FindObjectOfType<Grid>();
    }


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        canvas = GetComponentInParent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CursorIsEnabled)
        {
            DisplayCursor();
        }
    }

    /// <summary>
    /// �����ʾ
    /// </summary>
    /// <returns></returns>
    private Vector3Int DisplayCursor()
    {
        if(grid != null)
        {
            //��ȡ������������
            Vector3Int gridPosition = GetGridPositionForCursor();
            //Debug.Log("ScreenmouseX = " + gridPosition.x + "  ScreenmouseY = " + gridPosition.y);
            //��ȡ��ҵ���������
            Vector3Int playerGridPosition = GetGridPositionForPlayer();
            //Debug.Log("playerX = " + playerGridPosition.x + "  playerY = " + playerGridPosition.y);
            //���ù�꾫��ͼ
            SetCursorValidity(gridPosition, playerGridPosition);
            //��ȡ���λ��
            cursorRectTransform.position = GetRectTransformPositionForCursor(gridPosition);

            return gridPosition;
        }
        else
        {
            return Vector3Int.zero;
        }
    }

    /// <summary>
    /// ��ȡ����RectTransfromλ��
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    private Vector2 GetRectTransformPositionForCursor(Vector3Int gridPosition)
    {
        //��������ת��Ϊ��������
        Vector3 gridWorldPosition = grid.CellToWorld(gridPosition);
        //����������ת��Ϊ���������Ļ����
        Vector2 gridScreenPosition = mainCamera.WorldToScreenPoint(gridWorldPosition);
        //����Ļ�������Ϊ���ض����Է�����Ӧ��ͬ����Ļ�ͷֱ���
        return RectTransformUtility.PixelAdjustPoint(gridScreenPosition, cursorRectTransform, canvas);
    }

    /// <summary>
    /// ��ȡ��ҵ���������
    /// ����������ת��Ϊ��Ļ�ռ��ڵ�����
    /// </summary>
    /// <returns></returns>
    public Vector3Int GetGridPositionForPlayer()
    {
        return grid.WorldToCell(Player.Instance.transform.position);
    }

    /// <summary>
    /// ��ȡ������������
    /// </summary>
    /// <returns></returns>
    public Vector3Int GetGridPositionForCursor()
    {
        // z����λ����Ϸ���������ǰ��ľ��룬��Ϊ�����z=-10��������Ʒ��z����0�����������-mainCamera.transform.position.z
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        return grid.WorldToCell(worldPosition);
    }

    /// <summary>
    /// ���ù�����Ч��
    /// </summary>
    /// <param name="cursorGridPosition"></param>
    /// <param name="playerGridPosition"></param>
    private void SetCursorValidity(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        //���ù��Ϊ��Ч
        SetCursorToValid();

        //�ж���Ʒʹ�õ�����뾶
        if(Mathf.Abs(cursorGridPosition.x - playerGridPosition.x) > ItemUserGridRadius
            || Mathf.Abs(cursorGridPosition.y - playerGridPosition.y) > ItemUserGridRadius)
        {
            //���ù��Ϊ��Ч
            SetCursorToInvalid();
            return;
        }

        //��ȡ��ѡ����Ʒ������
        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);

        if(itemDetails == null)
        {
            SetCursorToInvalid();
            return;
        }

        //��ȡ���λ�õ�����������ϸ��Ϣ
        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cursorGridPosition.x, cursorGridPosition.y);

        if(gridPropertyDetails != null)
        {
            // ����ѡ�е���Ʒ��������������ȷ��������Ч��
            switch(itemDetails.itemType)
            {
                case ItemType.Seed:
                    if (!IsCursorValidForSeed(gridPropertyDetails))
                    {
                        SetCursorToInvalid();
                        return;
                    }
                    break;

                case ItemType.Commodity:
                    if (!IsCursorValildForCommodity(gridPropertyDetails))
                    {
                        SetCursorToInvalid();
                        return;
                    }
                    break;

                case ItemType.Watering_tool:
                case ItemType.Hoeing_tool:
                case ItemType.Breaking_tool:
                case ItemType.Chopping_tool:
                case ItemType.Reaping_tool:
                case ItemType.Collecting_tool:
                    if(!IsCursorValidForTool(gridPropertyDetails, itemDetails))
                    {
                        SetCursorToInvalid();
                        return ;
                    }
                    break;

                case ItemType.none:
                    break;
                case ItemType.count:
                    break;
                default:
                    break;
            }
        }
        else
        {
            SetCursorToInvalid();
            return;
        }
    }

    /// <summary>
    /// ������Թ�������Ʒ�Ƿ����
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    /// <param name="itemDetails"></param>
    /// <returns></returns>
    private bool IsCursorValidForTool(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails)
    {
        switch (itemDetails.itemType)
        {
            case ItemType.Hoeing_tool:
                if(gridPropertyDetails.isDiggable && gridPropertyDetails.daysSinceDug == -1)
                {
                    #region ��ȡ�ڵ�ǰ���λ�õ���Ʒ��ȷ���Ƿ�����������ھ�

                    // ��ȡ������������
                    //����λ�ô��������½�����ǵģ����ǹ�����õ����ĵ㣬�����Ҫƫ��
                    Vector3 cursorWorldPosition = new Vector3(GetWorldPositionForCursor().x + 0.5f, GetWorldPositionForCursor().y + 0.5f, 0f);
                    
                    List<Item> itemList = new List<Item>();
                    //��ȡ�����ھ���Item��������壨������itemԤ����ı��壩
                    HelperMethods.GetComponentsAtBoxLocation<Item>(out itemList, cursorWorldPosition, Settings.cursorSize, 0f);


                    #endregion

                    bool foundReapable = false;

                    foreach (Item item in itemList)
                    {
                        if(InventoryManager.Instance.GetItemDetails(item.ItemCode).itemType == ItemType.Reapable_scenary)
                        {
                            foundReapable = true;
                            break;
                        }
                    }

                    if (foundReapable)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }

            case ItemType.Watering_tool:
                // ֻ�й�����ڵ������Ѿ�����ͷ�ھ���δ����ˮ����ǰ���ſ���
                if(gridPropertyDetails.daysSinceDug > -1 && gridPropertyDetails.daysSinceWatered == -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            default: return false;
        }
    }

    /// <summary>
    /// ��ȡ�����������
    /// </summary>
    /// <returns></returns>
    private Vector3 GetWorldPositionForCursor()
    {
        return grid.CellToWorld(GetGridPositionForCursor());
    }

    /// <summary>
    /// ���������Ʒ����Ʒ�Ƿ����
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    /// <returns></returns>
    private bool IsCursorValildForCommodity(GridPropertyDetails gridPropertyDetails)
    {
        return gridPropertyDetails.canDropItem;
    }

    /// <summary>
    /// ���������������Ʒ�Ƿ����
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    /// <returns></returns>
    private bool IsCursorValidForSeed(GridPropertyDetails gridPropertyDetails)
    {
        return gridPropertyDetails.canDropItem;
    }

    /// <summary>
    /// ���ý�ֹʹ��������
    /// 
    /// �ڿ���е���Ʒ���ٱ�ѡ��ʱ����
    /// </summary>
    public void DisableCursor()
    {
        cursorImage.color = Color.clear;
        CursorIsEnabled = false;
    }

    /// <summary>
    /// ���ÿ���ʹ��������
    /// ѡ�п����Ʒʱ����
    /// </summary>
    public void EnableCursor()
    {
        cursorImage.color = new Color(1f,1f,1f,1f);
        CursorIsEnabled = true;
    }

    /// <summary>
    /// ���ù��Ϊ��Ч
    /// </summary>
    private void SetCursorToValid()
    {
        //����Ϊ�̿�
        cursorImage.sprite = greenCursorSprite;
        //�����Ч�Ա��Ϊ��Ч
        CursorPositionIsValid = true;
    }

    /// <summary>
    /// ���ù��Ϊ��Ч
    /// </summary>
    private void SetCursorToInvalid()
    {
        //����Ϊ���
        cursorImage.sprite = redCursorSprite;
        //�����Ч�Ա��Ϊ��Ч
        CursorPositionIsValid = false;
    }
}
