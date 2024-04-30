
using UnityEngine;


/// <summary>
/// �����������ײ������������Ч������
/// �ű���ӵ�player��
/// </summary>
public class TriggerObscuringItemFader : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��ȡ�뵱ǰ��Ϸ������ײ����Ϸ���壬����A
        //��ȡ������ObscuringItemFader�������Щ��Ϸ���弰�䣨A��������
        //��������
        ObscuringItemFader[] obscuringItemFaders = collision.gameObject.GetComponentsInChildren<ObscuringItemFader>();

        if(obscuringItemFaders.Length > 0)
        {
            //��ȡ�����
            for(int i = 0; i < obscuringItemFaders.Length; i++)
            {
                //��������
                obscuringItemFaders[i].FadeOut();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //ֹͣ��ײ
        //��������
        ObscuringItemFader[] obscuringItemFaders = collision.gameObject.GetComponentsInChildren<ObscuringItemFader>();

        if (obscuringItemFaders.Length > 0)
        {
            //��ȡ�����
            for (int i = 0; i < obscuringItemFaders.Length; i++)
            {
                //��������
                obscuringItemFaders[i].FadeIn();
            }
        }
    }
}
