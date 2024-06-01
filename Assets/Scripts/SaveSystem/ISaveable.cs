
using UnityEngine;
/// <summary>
/// �洢�ӿ�
/// </summary>
public interface ISaveable
{
    /// <summary>
    /// �洢��Ψһ��ʶ
    /// </summary>
    string ISaveableUniqueId { get; set; }

    /// <summary>
    /// �洢����Ϣ
    /// </summary>
    GameObjectSave GameObjectSave { get; set; }

    /// <summary>
    /// ����ע��
    /// </summary>
    void ISaveableRegister();
    /// <summary>
    /// ����ע��
    /// </summary>
    void ISaveableDeregister();
    /// <summary>
    /// �洢ָ���������Ƶĳ�����Ϣ
    /// </summary>
    /// <param name="sceneName"></param>
    void ISaveableStoreScene(string sceneName);
    /// <summary>
    /// �ָ�ָ���������Ƶĳ�����Ϣ
    /// </summary>
    /// <param name="sceneName"></param>
    void ISaveableRestoreScene(string sceneName);
}
