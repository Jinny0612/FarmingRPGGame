
using UnityEngine;


/// <summary>
/// System.Serializableʹ�����е��ֶο�����Inspector��������ʾ�ͱ༭
/// ��Ʒ��ϸ��Ϣ����
/// </summary>
[System.Serializable]
public class ItemDetails
{
    /// <summary>
    /// ��Ʒ���������ƣ�
    /// </summary>
    public string itemDescription;//����ֶη��ڵ�һλ����ôunity�п��Ӵ��ڻ�ֱ����ʾ������������element 0
    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public int itemCode;
    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public ItemType itemType;
    
    /// <summary>
    /// ��ƷͼƬ
    /// </summary>
    public Sprite itemSprite;
    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public string itemLongDescription;
    /// <summary>
    /// ��Ʒʹ�÷�Χ ��������  ������İ뾶
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
