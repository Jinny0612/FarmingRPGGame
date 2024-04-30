
using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ��Ʒ����
/// ScriptableObject �� Unity �е�һ���������ͣ������ڴ��������õġ��Զ�������������������ڱ༭���д������������ݡ�
/// ScriptableObject ������ MonoBehaviour һ�������л������ǲ������ڳ����е� GameObject����˿�������Ŀ�еĶ���ط��������ݡ�
/// һ�����ڣ�
/// 1.�������ݣ�������Ϸ�е�������Ϣ������ؿ����ݡ���ɫ���ԡ��������õȡ�
/// 2.��Դ�����洢�͹�����Ϸ��ʹ�õ���Դ������������Ƶ������Ƭ�εȡ�
/// 3.�¼�֪ͨ���䵱�¼�ϵͳ��һ���֣����ڷ��ͺͽ�����Ϣ��
/// 4.�༭�����ߣ������Զ���ı༭�����ߣ�����������Ա�� Unity �༭���и���Ч�ع�����
/// </summary>
/// CreateAssetMenu �ڲ˵��д�����һ�����ΪScriptable Objects/Item/Item List�Ķ���Ĭ��������so_ItemList
[CreateAssetMenu(fileName = "so_ItemList", menuName ="Scriptable Objects/Item/Item List")]
public class SO_ItemList : ScriptableObject
{
    //[SerializeField] ������unity����ʾ�ͱ༭
    [SerializeField]
    public List<ItemDetails> itemDetials;


}
