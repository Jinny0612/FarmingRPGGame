
/// <summary>
/// Vector3�������л�����Ϊԭʼ��Vector3�����޷����л���Ϊ�˱��ڱ�����Ϣ����Զ������л�
/// </summary>
[System.Serializable]
public class Vector3Serializable
{
    public float x,y, z;

    public Vector3Serializable(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3Serializable() { }
}
