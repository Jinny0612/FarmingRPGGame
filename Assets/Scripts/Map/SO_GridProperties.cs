using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 网格属性菜单
/// </summary>
[CreateAssetMenu(fileName = "so_GridProperties", menuName = "Scriptable Objects/Grid Properties")]
public class SO_GridProperties : ScriptableObject
{
    /// <summary>
    /// 场景名称
    /// </summary>
    public SceneName sceneName;
    /// <summary>
    /// 网格宽度
    /// </summary>
    public int gridWidth;
    /// <summary>
    /// 网格高度
    /// </summary>
    public int gridHeight;
    /// <summary>
    /// 原x坐标
    /// </summary>
    public int originX;
    /// <summary>
    /// 原y坐标
    /// </summary>
    public int originY;

    /// <summary>
    /// 网格属性列表
    /// </summary>
    [SerializeField]
    public List<GridProperty> gridPropertyList;
}
