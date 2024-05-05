using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 库存物品详情框管理
/// </summary>
public class UIInventoryTextBox : MonoBehaviour
{
    #region 上半部分
    /// <summary>
    /// 名称
    /// </summary>
    [SerializeField] private TextMeshProUGUI textmeshTop1 = null;
    /// <summary>
    /// 类型
    /// </summary>
    [SerializeField] private TextMeshProUGUI textmeshTop2 = null;
    /// <summary>
    /// 详细描述
    /// </summary>
    [SerializeField] private TextMeshProUGUI textmeshTop3 = null;
    #endregion

    #region 下半部分
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
