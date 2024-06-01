
/// <summary>
/// Vector3向量序列化，因为原始的Vector3向量无法序列化，为了便于保存信息因此自定义序列化
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
