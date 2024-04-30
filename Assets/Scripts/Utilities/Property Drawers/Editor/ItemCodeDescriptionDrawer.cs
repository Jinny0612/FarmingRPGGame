
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// �Զ������Ի���������Inspector����ʾ�ض�����,����Ŀ�Ӽ�飬����¼����Ʒ�������
/// </summary>
[CustomPropertyDrawer(typeof(ItemCodeDescriptionAttribute))]
public class ItemCodeDescriptionDrawer : PropertyDrawer
{

    /// <summary>
    /// ��Ҫչʾ������Ʒ�������Ʒ������������Ҫ�����ĸ߶�
    /// </summary>
    /// <param name="property"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property) * 2;
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="position"></param>
    /// <param name="property"></param>
    /// <param name="label"></param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        
        //ʹ��beginproperty / endproperty ȷ�������߼���������������
        EditorGUI.BeginProperty(position, label,property);//��ʼ��������
        //������Ʒ����
        var newValue = EditorGUI.IntField(new Rect(position.x,position.y,position.width,position.height / 2), label,property.intValue);
        //������Ʒ����
        EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "Item Description",
            GetItemDescription(property.intValue));

        if (EditorGUI.EndChangeCheck())
        {
            property.intValue = newValue;
        }

        EditorGUI.EndProperty();//��������
    }

    /// <summary>
    /// �����Ʒ���������ƣ�
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    private string GetItemDescription(int itemCode)
    {
        SO_ItemList so_itemList;
        so_itemList = AssetDatabase.LoadAssetAtPath("Assets/Scriptable Object Assets/Item/so_ItemList.asset",typeof(SO_ItemList)) as SO_ItemList;
        List<ItemDetails> itemdetailsList = so_itemList.itemDetials;
        ItemDetails itemDetails = itemdetailsList.Find(i => itemCode == i.itemCode);
        if (itemDetails != null)
        {
            return itemDetails.itemDescription;
        }
        return null;

    }
}
