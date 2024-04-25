# FarmingRPGGame
 A farming RPG game

 2024.4.25

 1. 角色身体部件拆分，方便制作骨骼动画
 2. 角色身体骨骼有不同的排序序号，要对角色父物体添加Sorting Group组件，将骨骼视为一个整体，避免角色前后站立的时候，后面角色身体的骨骼会穿过前面角色的身体显示出来
 3. 要实现角色移动camera也跟随，需要再创建一个VirtualCamera（要先在package manager中安装cinemathine），follow player
