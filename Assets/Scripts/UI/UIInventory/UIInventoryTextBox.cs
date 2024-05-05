using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �����Ʒ��������
/// </summary>
public class UIInventoryTextBox : MonoBehaviour
{
    #region �ϰ벿��
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField] private TextMeshProUGUI textmeshTop1 = null;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField] private TextMeshProUGUI textmeshTop2 = null;
    /// <summary>
    /// ��ϸ����
    /// </summary>
    [SerializeField] private TextMeshProUGUI textmeshTop3 = null;
    #endregion

    #region �°벿��
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private TextMeshProUGUI textmeshBottom1 = null;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private TextMeshProUGUI textmeshBottom2 = null;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private TextMeshProUGUI textmeshBottom3 = null;
    #endregion

    public void SetTextboxText(string textTop1,string textTop2,string textTop3,string textBottom1,string textBottom2,string textBottom3)
    {
        textmeshTop1.text = textTop1;
        textmeshTop2.text = textTop2;
        textmeshTop3.text = textTop3;
        textmeshBottom1.text = textBottom1;
        textmeshBottom2.text = textBottom2;
        textmeshBottom3.text = textBottom3;
    }
}
