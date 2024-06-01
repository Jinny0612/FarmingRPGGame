using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �������Բ˵�
/// </summary>
[CreateAssetMenu(fileName = "so_GridProperties", menuName = "Scriptable Objects/Grid Properties")]
public class SO_GridProperties : ScriptableObject
{
    /// <summary>
    /// ��������
    /// </summary>
    public SceneName sceneName;
    /// <summary>
    /// ������
    /// </summary>
    public int gridWidth;
    /// <summary>
    /// ����߶�
    /// </summary>
    public int gridHeight;
    /// <summary>
    /// ԭx����
    /// </summary>
    public int originX;
    /// <summary>
    /// ԭy����
    /// </summary>
    public int originY;

    /// <summary>
    /// ���������б�
    /// </summary>
    [SerializeField]
    public List<GridProperty> gridPropertyList;
}
