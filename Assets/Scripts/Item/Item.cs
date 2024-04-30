
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

    private void Init(int itemCodeParam)
    {

    }
}
