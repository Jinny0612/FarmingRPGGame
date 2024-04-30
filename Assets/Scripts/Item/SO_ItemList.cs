
using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 物品容器
/// ScriptableObject 是 Unity 中的一种特殊类型，它用于创建可重用的、自定义的数据容器，可以在编辑器中创建并保存数据。
/// ScriptableObject 可以像 MonoBehaviour 一样被序列化，但是不依赖于场景中的 GameObject，因此可以在项目中的多个地方共享数据。
/// 一般用于：
/// 1.配置数据：保存游戏中的配置信息，例如关卡数据、角色属性、技能设置等。
/// 2.资源管理：存储和管理游戏中使用的资源，例如纹理、音频、动画片段等。
/// 3.事件通知：充当事件系统的一部分，用于发送和接收消息。
/// 4.编辑器工具：创建自定义的编辑器工具，帮助开发人员在 Unity 编辑器中更高效地工作。
/// </summary>
/// CreateAssetMenu 在菜单中创建了一个类别为Scriptable Objects/Item/Item List的对象，默认名字是so_ItemList
[CreateAssetMenu(fileName = "so_ItemList", menuName ="Scriptable Objects/Item/Item List")]
public class SO_ItemList : ScriptableObject
{
    //[SerializeField] 可以在unity中显示和编辑
    [SerializeField]
    public List<ItemDetails> itemDetials;


}
