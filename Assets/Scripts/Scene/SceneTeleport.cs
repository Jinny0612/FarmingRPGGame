using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景传送脚本
/// 需要box碰撞器
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class SceneTeleport : MonoBehaviour
{
    /// <summary>
    /// 切换的场景名称
    /// </summary>
    [SerializeField] private SceneName sceneNameGoto = SceneName.Scene1_Farm;
    /// <summary>
    /// 切换场景时的位置
    /// </summary>
    [SerializeField] private Vector3 scenePositionGoto = new Vector3();

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            //计算玩家当前位置
            float xPosition = Mathf.Approximately(scenePositionGoto.x, 0f) ? 
                player.transform.position.x : scenePositionGoto.x;
            float yPosition = Mathf.Approximately(scenePositionGoto.y, 0f) ?
                player.transform.position.y : scenePositionGoto.y;
            float zPosition = 0f;

            //切换到新场景
            SceneControllerManager.Instance.FadeAndLoadScene(sceneNameGoto.ToString(), new Vector3(xPosition, yPosition, zPosition));
        }
    }
}
