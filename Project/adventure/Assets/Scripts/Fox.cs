using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Fox : MonoBehaviour
{
    // Biến tham chiếu đến Animator của cáo
    private Animator animator;

    // Tốc độ di chuyển của cáo
    public float moveSpeed = 10f;
    public float jumpForce = 10f;

    // Kiểm tra xem cáo có đang ở trên mặt đất không
    private bool isGrounded=false;

    // Tham chiếu đến Rigidbody để xử lý nhảy
    private Rigidbody rb;

    [SerializeField]
    private LayerMask groundLayer;

    // Gravity force
    private const float gravityForce = -1f;


    // Bắt đầu
    void Start()
    {
        // Gán Animator và Rigidbody
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Cập nhật trạng thái di chuyển trong mỗi khung hình
    void Update()
    {
        // Kiểm tra xem cáo có chạm đất không
        //isGrounded = Physics.Raycast(transform.position, -Vector3.up, k_GroundRayLength);

        // Lấy input từ người chơi
        float moveX = CrossPlatformInputManager.GetAxis("Horizontal");
        float moveZ = CrossPlatformInputManager.GetAxis("Vertical");
        bool jump = CrossPlatformInputManager.GetButtonDown("Jump");


        // Ghi lại giá trị input vào console
        Debug.Log($"Jump: {jump}, isGrounded: {isGrounded}, rb.velocity.y {rb.velocity.y}");
        // Di chuyển cáo theo hướng
        float fallSpeed = gravityForce;
        if (isGrounded)
        {
            fallSpeed = 0.0f;
        }
        else if (jump)
        {
            fallSpeed = 10f;
        }
        Vector3 movement = new Vector3(moveX, 0f, moveZ) + new Vector3(0, fallSpeed, 0).normalized;
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.Self);

        // Gọi các hàm để điều khiển animation
        HandleJump(jump);
        HandleMovement(moveX, moveZ);
        
    }

    // Hàm xử lý di chuyển và animation
    void HandleMovement(float moveX, float moveZ)
    {
        bool isRunning = false;
        bool isWalkingLeft = false;
        bool isWalkingRight = false;
        bool isWalkingBack = false;
        bool modify=false;
        // Nếu không có input di chuyển, chuyển sang trạng thái idle
        if (moveX == 0 && moveZ == 0)
        {
            modify = true;
        }
        else
        {
            // Di chuyển về phía trước (run)
            if (moveZ > 0)
            {
                isRunning = true;
                modify = true;
            }
            // Nếu di chuyển ngược lại
            else if (moveZ < 0)
            {
                isWalkingBack = true;
                modify = true;
            }
            // Di chuyển sang trái (walk left)
            if (moveX < 0)
            {
                isWalkingLeft = true;
                modify = true;
            }
            // Di chuyển sang phải (walk right)
            else if (moveX > 0)
            {
                isWalkingRight = true;
                modify = true;
            }

        }
        if (modify)
        {
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isWalkingLeft", isWalkingLeft);
            animator.SetBool("isWalkingRight", isWalkingRight);
            animator.SetBool("isWalkingBack", isWalkingBack);
            //Debug.Log($"isRunning: {isRunning}, isWalkingLeft: {isWalkingLeft}, isWalkingRight: {isWalkingRight}, isWalkingBack: {isWalkingBack}");
        }
        
    }

    // Hàm xử lý nhảy
    void HandleJump(bool jump)
    {
        // Nếu người chơi nhấn nút nhảy và cáo đang trên mặt đất
        if (jump && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("isJumping", true);
            isGrounded = false;
            //Debug.Log("Jumping!");
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }
    void OnCollisionStay(Collision collision)
    {
        // Check if the object is on the ground layer
        if (IsGroundedLayer(collision.gameObject.layer))
        {
            isGrounded = true;
        }
    }

    // OnCollisionExit: Called when the character stops touching another collider
    void OnCollisionExit(Collision collision)
    {
        // When the character stops touching the ground layer
        if (IsGroundedLayer(collision.gameObject.layer))
        {
            isGrounded = false;
        }
    }

    // Helper function to check if the layer is part of the ground layer
    bool IsGroundedLayer(int layer)
    {
        return groundLayer == (groundLayer | (1 << layer));
    }
}
