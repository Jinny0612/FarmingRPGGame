
using UnityEngine;


/// <summary>
/// 玩家与物体碰撞触发渐隐渐显效果管理
/// 脚本添加到player上
/// </summary>
public class TriggerObscuringItemFader : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //获取与当前游戏物体碰撞的游戏物体，例如A
        //获取挂载了ObscuringItemFader组件的这些游戏物体及其（A）子物体
        //触发渐隐
        ObscuringItemFader[] obscuringItemFaders = collision.gameObject.GetComponentsInChildren<ObscuringItemFader>();

        if(obscuringItemFaders.Length > 0)
        {
            //获取到组件
            for(int i = 0; i < obscuringItemFaders.Length; i++)
            {
                //触发渐隐
                obscuringItemFaders[i].FadeOut();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //停止碰撞
        //触发渐显
        ObscuringItemFader[] obscuringItemFaders = collision.gameObject.GetComponentsInChildren<ObscuringItemFader>();

        if (obscuringItemFaders.Length > 0)
        {
            //获取到组件
            for (int i = 0; i < obscuringItemFaders.Length; i++)
            {
                //触发渐显
                obscuringItemFaders[i].FadeIn();
            }
        }
    }
}
