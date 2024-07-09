# FarmingRPGGame
 A farming RPG 

 <H1>2024.7.9</H1>

 1. 实现了锄地时地面的变化
 2. 实现了浇水时地面的变化，并且在切换到下一天后会重置浇水的状态


 <H1>2024.6.1</H1>

 1. 实现了锄地动画   但是地面变化暂未实现

  <H1>2024.5.30</H1>

  1. 修复预加载场景时网格数据只有非初始场景列表最后一个场景的网格数据的问题
  2. 优化游戏内物体管理so_ItemList显示问题，不再显示Element 0、Element 1等，而是直接显示物体名称

 <H1>2024.5.29</H1>

1. 实现放置物品时，在地面显示放置在哪个网格位置（暂未实现一个网格只放一个物品，而且物品是放在鼠标位置的，希望能放在网格中间）


 <H1>2024.5.16</H1>

1. 修复场景切换黑屏时人物还可以移动的问题
2. 实现物品只能放到地图指定地点的功能（但是目前无法做到同一个网格允许放置物品和放置家具，需要调整网格属性初始化逻辑）（目前无法做到一个网格里只放一个物品，我希望能实现这个功能）

  <H1>2024.5.15</H1>

1. 完成农场小屋场景绘制
2. 保存场景的状态
3. 在瓦片地图网格中存储bool值，判断是否能放置物品、是否能种地等等
4. 在地图上绘制能否放置物品、放置家具，能否挖掘等区域


OnTriggerStay2D 是 Unity 引擎中的一个事件回调函数，用于检测两个 Collider2D 组件在触发器（trigger）状态下保持接触的情况。只要两个碰撞器保持接触，该方法就会在每一帧被调用。可以用来检测角色是否在某个区域内，例如检测角色是否在伤害区域内，是否进入特定区域以触发某些事件等。




  <H1>2024.5.13</H1>

  1. 完成第二个场景野外的绘制
  2. 实现了农场和野外两个场景的切换，走到农场右侧路口会进入野外，在野外走到左侧路口会进入农场。需要指定传送的坐标



 <H1>2024.5.10</H1>

 实现场景切换管理：

 player所在的持久化场景一直存在无需卸载，其他场景需要进行加载和卸载。

 所有在需要加载卸载场景中存在的游戏物体，获取时都需要在场景加载完成后再加载，因此使用到这些物体的挂载在游戏物体上的脚本，都需要订阅场景加载卸载事件以便于获取需要的游戏物体。

 ![场景管理图1](./READMEIMAGE/SceneControllerManager01.PNG)

  ![场景管理图2](./READMEIMAGE/SceneControllerManager02.PNG)
 
  <H1>2024.5.10</H1>

  1. 实现游戏内时间系统
  2. 实现时钟UI
   
   时间系统并不复杂，需要设定好游戏单位时间对应的实际时间，注意边界值的处理。年份季节日期时分秒，这些的变化通过发布订阅实现，时间系统发布时间的变化，时间UI订阅时间的变化

  <H1>2024.5.8</H1>
  
  1. 实现了在物品栏中选中物品角色可以直接举起显示的功能

  <H1>2024.5.5</H1>

  1. 实现在在背包工具栏拖拽物体与工具栏其他位置交换的功能 (UIInventorySlot下的所有子物体都要取消选中Raycast Target，否则无法用鼠标进行交互)  此时只能与放置了物品的插槽交换
  2. 实现鼠标悬停在库存物品上显示物品详情的功能
  3. 实现在背包工具栏选中物品高亮的功能

 <H1>2024.5.4</H1>

 1. 绘制角色背包工具栏ui，实现拾取物品在ui中显示的功能
 2. 实现从背包工具栏拖拽物体放到地图上的功能


```c#
    /// <summary>
    /// 将拖拽物体放置到鼠标当前位置
    /// </summary>
    private void DropSelectedItemAtMousePosition()
    {
        if(itemDetails != null)
        {
            //这里将坐标转换为世界坐标，否则后面实例化预制体会导致位置错误，不在相机范围内显示
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

            //从预制体中实例化一个物体到鼠标当前位置
            GameObject itemGameObject = Instantiate(itemPerfab, worldPosition, Quaternion.identity, parentItem);
            Item item = itemGameObject.GetComponent<Item>();
            item.ItemCode = itemDetails.itemCode;

            //从库存中移除物品
            InventoryManager.Instance.RemoveItem(InventoryLocation.player,item.ItemCode);
        }
    }
```
ScreenToWorldPoint:用于将屏幕上的点坐标转换为世界空间中的点坐标。通常情况下，这个方法用于将鼠标点击位置转换为在世界空间中的位置，以便在游戏中执行相应的操作。

ScreenToViewportPoint:用于将屏幕上的点坐标转换为视口坐标，它会将屏幕上的点坐标转换为相对于摄像机视野的一个点的坐标，范围在 (0, 0) 到 (1, 1) 之间。

<H1>2024.5.3</H1>

1. 实现经过场景中可收获的场景物体时，场景物体轻轻晃动的效果
2. 实现简单库存管理，与可拾取物体碰撞时捡起物体放入player背包库存中


<H1>2024.4.30</H1>

PropertyDrawer是Unity中的一个功能强大的类，它允许你自定义Inspector中的属性显示方式。通过自定义PropertyDrawer，你可以为特定类型的属性创建自定义的Inspector显示方式，使其更加直观、易于理解，或者根据你的需求进行定制化。

ScriptableObject 是 Unity 中的一种特殊类型，它用于创建可重用的、自定义的数据容器，可以在编辑器中创建并保存数据。
ScriptableObject 可以像 MonoBehaviour 一样被序列化，但是不依赖于场景中的 GameObject，因此可以在项目中的多个地方共享数据。
一般用于：

1. 配置数据：保存游戏中的配置信息，例如关卡数据、角色属性、技能设置等。
2. 资源管理：存储和管理游戏中使用的资源，例如纹理、音频、动画片段等。
3. 事件通知：充当事件系统的一部分，用于发送和接收消息。
4. 编辑器工具：创建自定义的编辑器工具，帮助开发人员在 Unity 编辑器中更高效地工作。

```c#
//CreateAssetMenu 在菜单中创建了一个类别为Scriptable Objects/Item/Item List的对象，默认名字是so_ItemList：
[CreateAssetMenu(fileName = "so_ItemList", menuName ="Scriptable Objects/Item/Item List")]
public class SO_ItemList : ScriptableObject
{
    //[SerializeField] 可以在unity中显示和编辑
    [SerializeField]
    public List<ItemDetials> itemDetials;


}
```
可以在Unity编辑器中直接将物品添加到so_ItemList对象的itemDetails中。

当物体数量过多时，可以将信息写入json文件中，通过脚本将json文件中的信息写入itemDetails。如果有需要存储的资源信息，则可以记录资源的路径，将这些资源放在统一的文件夹中方便读取


今日进度：

 1. 实现游戏物体遮挡玩家时的渐隐渐显效果
 2. 统一物品相关的信息设置
 3. 自定义属性绘制器，无需运行即可在Inspector上显示游戏物体代码对应的物体描述，避免绑定的物品代码数字错误

<H1>2024.4.29</H1>
创建场景 

使用tilemap层级来创建场景，不同的层级优先级不同，利用这个层级来实现场景视觉的深度

例如：要实现玩家站在房子前面时房子在玩家身后，玩家走到房子后面时房子在玩家前面，那么需要将房子下半部分绘制在玩家角色更下层的层级中，将房子上半部分绘制在玩家角色更上层的层级中

需要进行碰撞检测的，将碰撞瓦片统一放在一个层级中处理，这些瓦片属性中的collider type一定不能选择none，否则不会触发碰撞

1. 完成了农场场景的绘制
2. 对农场场景设置多边形碰撞器，框住整个场景，避免即将移动到场景边缘时相机超出场景显示范围

 <H1>2024.4.28</H1>
 Awake方法和Start方法的区别：
  
    Awake在创建实例时被调用，一般用于进行初始化，即使脚本组件被禁用也会调用Awake

    Start在所有Awake都被调用后的第一帧更新之前被调用，一些依赖于其他组件初始化状态的操作可以在Start中进行

1. 实现了player的移动与步行跑步切换
2. 创建了农场地图的结构

 
 出现的问题：

 1. player移动碰到死区边缘时角色会抖动(目前降低了相机跟随速度，抖动降低了)
 2. tilemap相关需要去仔细看一下

 <H1>2024.4.26</H1>

1. 创建单例抽象类，所有场景中只允许存在一个的游戏物体的脚本都需要继承这个类
2. 为player骨骼绑定动画
3. 为事件新增静态事件处理类
4. 新增player移动控制脚本，用于控制player的相关动画触发逻辑，并测试player动画是否正常执行



 <H1>2024.4.25</H1>

 1. 角色身体部件拆分，方便制作骨骼动画
 2. 角色身体骨骼有不同的排序序号，要对角色父物体添加Sorting Group组件，将骨骼视为一个整体，避免角色前后站立的时候，后面角色身体的骨骼会穿过前面角色的身体显示出来
 3. 要实现角色移动camera也跟随，需要再创建一个VirtualCamera（要先在package manager中安装cinemathine），follow player
