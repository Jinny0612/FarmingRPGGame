using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 瓦片地图网格属性
/// 游戏运行和编辑器中都会执行
/// </summary>
[ExecuteAlways]
public class TilemapGridProperties : MonoBehaviour
{
    /// <summary>
    /// 瓦片地图
    /// </summary>
    private Tilemap tilemap;
    /// <summary>
    /// 管理网格的属性
    /// </summary>
    [SerializeField] private SO_GridProperties gridProperties = null;
    /// <summary>
    /// 网格属性，默认可挖掘
    /// </summary>
    [SerializeField] private GridBoolProperty gridBoolProperty = GridBoolProperty.diggable;

    private void OnEnable()
    {
        if (!Application.IsPlaying(gameObject))
        {
            tilemap = GetComponent<Tilemap>();

            if(gridProperties != null)
            {
                gridProperties.gridPropertyList.Clear();
            }
        }
    }

    private void OnDisable()
    {
        if(!Application.IsPlaying(gameObject))
        {
            UpdateGridProperties();

            if(gridProperties != null)
            {
                //这是为了确保在保存游戏时保存更新的gridproperties游戏对象所必需的，否则它们将不会保存。
                EditorUtility.SetDirty(gridProperties);
            }
        }
    }

    /// <summary>
    /// 更新网格属性
    /// </summary>
    private void UpdateGridProperties()
    {
        //调整边界，使其紧密围绕瓦片，移除多余的空白区域，优化内存占用和处理效率
        tilemap.CompressBounds();

        if (!Application.IsPlaying(gameObject))
        {
            //TODO: 性能优化：遍历所有瓦片可能会有性能问题，特别是在大型 Tilemap 中。考虑优化遍历逻辑或在后台线程中进行。
            if (gridProperties != null)
            {
                //最小边界，左下角
                Vector3Int startCell = tilemap.cellBounds.min;
                //最大边界，右上角
                Vector3Int endCell = tilemap.cellBounds.max;

                for(int x = startCell.x; x < endCell.x; x++)
                {
                    for(int y = startCell.y; y < endCell.y; y++)
                    {
                        TileBase tile = tilemap.GetTile(new Vector3Int(x, y,0));

                        if(tile != null)
                        {
                            gridProperties.gridPropertyList.Add(new GridProperty(new GridCoordinate(x, y), gridBoolProperty, true));
                        }
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (!Application.IsPlaying(gameObject))
        {
            Debug.Log("Disable property tilemaps");
        }
    }
}
