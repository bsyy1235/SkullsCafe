using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 8f;
    public float rotateSpeed = 10;
    public float jumpForce = 3f; 

    private bool isJumping = false;
    Animator playerAni;

    float h, v;

    public Transform cameraTransform;

    void Start()
    {
       rb = GetComponent<Rigidbody>();
       playerAni = GetComponent<Animator>();
       jumpForce = 3f;
    }
    private void Update()
    {
        Jump();
    }

    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v);

        if (inputDir.magnitude > 0.1f)
        {
            //카메라의 Y 방향을 기준으로 이동 방향 계산
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * v + camRight * h;

            if (moveDir.sqrMagnitude > 0.01f)
            {
                // 캐릭터 회전 (위치는 고정)
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                Quaternion smoothedRotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * rotateSpeed);
                rb.MoveRotation(smoothedRotation);
            }

            // 이동
            Vector3 newVelocity = new Vector3(moveDir.x * speed, rb.linearVelocity.y, moveDir.z * speed);
            rb.linearVelocity = newVelocity;

            // 애니메이션
            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerAni.SetBool("isRunning", true);
                playerAni.SetBool("isWalking", false);
                speed = 14f;
            }
            else
            {
                playerAni.SetBool("isWalking", true);
                playerAni.SetBool("isRunning", false);
                speed = 8f;
            }
        }
        else
        {
            // 정지 상태에서 자동회전 방지
            rb.angularVelocity = Vector3.zero;

            Vector3 newVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            rb.linearVelocity = newVelocity;

            playerAni.SetBool("isWalking", false);
            playerAni.SetBool("isRunning", false);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&!isJumping)
        {
            isJumping = true;
            playerAni.SetBool("isJumping", true);

            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            rb.AddForce(Vector3.up * jumpForce * rb.mass, ForceMode.Impulse);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            playerAni.SetBool("isJumping", false);
        }
    }
}
