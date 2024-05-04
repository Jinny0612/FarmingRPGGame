
using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    /// <summary>
    /// ��Ʒ���
    /// </summary>
    [ItemCodeDescription]
    [SerializeField]
    private int _itemCode;

    /// <summary>
    /// sprite��Ⱦ��
    /// </summary>
    private SpriteRenderer spriteRenderer;

    public int ItemCode { get { return _itemCode; } set { _itemCode = value; } }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if(ItemCode != 0)
        {
            Init(ItemCode);
        }
    }

    //�����趨����Ʒ�����ʼ����Ʒ��Ϣ
    private void Init(int itemCodeParam)
    {
        if (ItemCode != 0)
        {
            ItemCode = itemCodeParam;
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(ItemCode);
            spriteRenderer.sprite = itemDetails.itemSprite;
            //�ж������Ʒ�Ƿ��ǿ��ջ���Ʒ
            if(itemDetails.itemType == ItemType.Reapable_scenary)
            {
                //��ӻζ�Ч��
                gameObject.AddComponent<ItemNudge>();
            }
        }
    }
}
