
using UnityEngine;

/// <summary>
/// ����Ψһ��ʶ
/// </summary>
[ExecuteAlways]
public class GenerateGUID : MonoBehaviour
{
    [SerializeField]
    private string _gUID = "";

    public string GUID { get => _gUID; set => _gUID = value; }

    private void Awake()
    {
        //���ڱ༭����ִ��
        if (!Application.IsPlaying(gameObject))
        {
            if(_gUID == "")
            {
                _gUID = System.Guid.NewGuid().ToString();
            }
        }
    }
}
