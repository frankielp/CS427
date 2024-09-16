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
    [SerializeField]
    private float rotationSmoothSpeed = 5.0f; // Tốc độ làm mượt khi camera xoay

    private Vector3 offset; // Khoảng cách giữa camera và đối tượng (dựa trên vị trí và hướng)

    void Start()
    {
        // Tạo khoảng cách ban đầu giữa camera và đối tượng
        offset = target.position - transform.position;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Xác định vị trí mong muốn của camera dựa trên đối tượng, khoảng cách và chiều cao
            Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;

            // Làm mượt vị trí camera bằng cách sử dụng Vector3.Lerp
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Tính toán hướng mà camera cần quay để luôn nhìn về phía đối tượng
            Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);

            // Làm mượt quá trình xoay camera
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothSpeed * Time.deltaTime);
        }
    }
}
