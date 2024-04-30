
using UnityEngine;


/// <summary>
/// System.Serializableʹ�����е��ֶο�����Inspector��������ʾ�ͱ༭
/// ��Ʒ��ϸ��Ϣ����
/// </summary>
[System.Serializable]
public class ItemDetails
{
    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public int itemCode;
    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public ItemType itemType;
    /// <summary>
    /// ��Ʒ���������ƣ�
    /// </summary>
    public string itemDescription;
    /// <summary>
    /// ��ƷͼƬ
    /// </summary>
    public Sprite itemSprite;
    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public string itemLongDescription;
    /// <summary>
    /// ��Ʒʹ�÷�Χ ��������
    /// </summary>
    public short itemUseGridRadius;
    /// <summary>
    /// ��Ʒʹ�÷�Χ�����ھ���
    /// </summary>
    public float itemUseRadius;
    /// <summary>
    /// �Ƿ��ǳ�ʼ��Ʒ������
    /// </summary>
    public bool isStartingItem;
    /// <summary>
    /// ��Ʒ�Ƿ��ܱ�ʰ��
    /// </summary>
    public bool canBePickedUp;
    /// <summary>
    /// ��Ʒ�Ƿ��ܱ�����
    /// </summary>
    public bool canBeDropped;
    /// <summary>
    /// ��Ʒ�Ƿ��ܱ����ʳ��
    /// </summary>
    public bool canBeEaten;
    /// <summary>
    /// ��Ʒ�Ƿ��ܱ�����
    /// </summary>
    public bool canBeCarried;
}
