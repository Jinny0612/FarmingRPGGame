using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// ��Ƭ��ͼ��������
/// ��Ϸ���кͱ༭���ж���ִ��
/// </summary>
[ExecuteAlways]
public class TilemapGridProperties : MonoBehaviour
{
    /// <summary>
    /// ��Ƭ��ͼ
    /// </summary>
    private Tilemap tilemap;
    /// <summary>
    /// �������������
    /// </summary>
    [SerializeField] private SO_GridProperties gridProperties = null;
    /// <summary>
    /// �������ԣ�Ĭ�Ͽ��ھ�
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
                //����Ϊ��ȷ���ڱ�����Ϸʱ������µ�gridproperties��Ϸ����������ģ��������ǽ����ᱣ�档
                EditorUtility.SetDirty(gridProperties);
            }
        }
    }

    /// <summary>
    /// ������������
    /// </summary>
    private void UpdateGridProperties()
    {
        //�����߽磬ʹ�����Χ����Ƭ���Ƴ�����Ŀհ������Ż��ڴ�ռ�úʹ���Ч��
        tilemap.CompressBounds();

        if (!Application.IsPlaying(gameObject))
        {
            //TODO: �����Ż�������������Ƭ���ܻ����������⣬�ر����ڴ��� Tilemap �С������Ż������߼����ں�̨�߳��н��С�
            if (gridProperties != null)
            {
                //��С�߽磬���½�
                Vector3Int startCell = tilemap.cellBounds.min;
                //���߽磬���Ͻ�
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
