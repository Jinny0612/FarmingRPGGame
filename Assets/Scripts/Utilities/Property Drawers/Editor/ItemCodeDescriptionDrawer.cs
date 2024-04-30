
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// 自定义属性绘制器，在Inspector上显示特定属性,便于目视检查，避免录入物品代码错误
/// </summary>
[CustomPropertyDrawer(typeof(ItemCodeDescriptionAttribute))]
public class ItemCodeDescriptionDrawer : PropertyDrawer
{

    /// <summary>
    /// 需要展示的是物品代码和物品描述，所以需要两倍的高度
    /// </summary>
    /// <param name="property"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property) * 2;
    }

    /// <summary>
    /// 绘制属性
    /// </summary>
    /// <param name="position"></param>
    /// <param name="property"></param>
    /// <param name="label"></param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        
        //使用beginproperty / endproperty 确保绘制逻辑适用于整个属性
        EditorGUI.BeginProperty(position, label,property);//开始绘制属性
        //绘制物品代码
        var newValue = EditorGUI.IntField(new Rect(position.x,position.y,position.width,position.height / 2), label,property.intValue);
        //绘制物品描述
        EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "Item Description",
            GetItemDescription(property.intValue));

        if (EditorGUI.EndChangeCheck())
        {
            property.intValue = newValue;
        }

        EditorGUI.EndProperty();//结束绘制
    }

    /// <summary>
    /// 获得物品描述（名称）
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
