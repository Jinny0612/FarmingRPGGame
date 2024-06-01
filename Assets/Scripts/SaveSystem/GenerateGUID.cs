
using UnityEngine;

/// <summary>
/// 生成唯一标识
/// </summary>
[ExecuteAlways]
public class GenerateGUID : MonoBehaviour
{
    [SerializeField]
    private string _gUID = "";

    public string GUID { get => _gUID; set => _gUID = value; }

    private void Awake()
    {
        //仅在编辑器中执行
        if (!Application.IsPlaying(gameObject))
        {
            if(_gUID == "")
            {
                _gUID = System.Guid.NewGuid().ToString();
            }
        }
    }
}
