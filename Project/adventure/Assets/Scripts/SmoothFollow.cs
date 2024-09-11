using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target; // Đối tượng mà camera sẽ theo dõi
    [SerializeField]
    private float distance = 8.0f; // Khoảng cách giữa camera và đối tượng
    [SerializeField]
    private float height = 7.0f; // Chiều cao của camera so với đối tượng
    [SerializeField]
    private float smoothSpeed = 0.125f; // Tốc độ làm mượt di chuyển của camera

    void LateUpdate()
    {
        if (target != null)
        {
            // Xác định vị trí mong muốn của camera
            Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;
            // Làm mượt vị trí của camera
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            // Cập nhật vị trí của camera
            transform.position = smoothedPosition;

            // Camera nhìn về phía đối tượng
            transform.LookAt(target);
        }
    }
}
