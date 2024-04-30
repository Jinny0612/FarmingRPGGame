
using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    /// <summary>
    /// ŒÔ∆∑±‡∫≈
    /// </summary>
    [ItemCodeDescription]
    [SerializeField]
    private int _itemCode;

    /// <summary>
    /// sprite‰÷»æ∆˜
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
