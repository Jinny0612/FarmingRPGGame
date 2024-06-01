using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������ͽű�
/// ��Ҫbox��ײ��
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class SceneTeleport : MonoBehaviour
{
    /// <summary>
    /// �л��ĳ�������
    /// </summary>
    [SerializeField] private SceneName sceneNameGoto = SceneName.Scene1_Farm;
    /// <summary>
    /// �л�����ʱ��λ��
    /// </summary>
    [SerializeField] private Vector3 scenePositionGoto = new Vector3();

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            //������ҵ�ǰλ��
            float xPosition = Mathf.Approximately(scenePositionGoto.x, 0f) ? 
                player.transform.position.x : scenePositionGoto.x;
            float yPosition = Mathf.Approximately(scenePositionGoto.y, 0f) ?
                player.transform.position.y : scenePositionGoto.y;
            float zPosition = 0f;

            //�л����³���
            SceneControllerManager.Instance.FadeAndLoadScene(sceneNameGoto.ToString(), new Vector3(xPosition, yPosition, zPosition));
        }
    }
}
