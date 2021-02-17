using UnityEngine;
public class Orbit : MonoBehaviour
{
    public Transform center;
    public float DistanceX = 0.0f;
    public float DistanceY = 0.0f;
    public float DistanceZ = 0.0f;
    private Vector3 distance;
    public float degreesPerSecond = -65.0f;
    public bool AxisX;
    public bool AxisY;
    public bool AxisZ;
    void Start()
    {
        distance = transform.position - center.position;

        distance.x = DistanceX;
        distance.y = DistanceY;
        distance.z = DistanceZ;
    }
    void Update()
    {
        transform.LookAt(center);
        if (AxisX)
        {
            distance = Quaternion.AngleAxis(degreesPerSecond * Time.deltaTime, Vector3.right) * distance;
            transform.position = center.position + distance;
        }
        else if (AxisY)
        {
            distance = Quaternion.AngleAxis(degreesPerSecond * Time.deltaTime, Vector3.up) * distance;
            transform.position = center.position + distance;
        }
        else if (AxisZ)
        {
            distance = Quaternion.AngleAxis(degreesPerSecond * Time.deltaTime, Vector3.forward) * distance;
            transform.position = center.position + distance;
        }
    }
}