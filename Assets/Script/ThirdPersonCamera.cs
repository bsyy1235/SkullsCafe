using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // 플레이어 캐릭터
    public Vector3 offset = new Vector3(0, 0.5f, -3);
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;
    public float minY = -30f;
    public float maxY = 60f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 고정
    }

    void LateUpdate()
    {
        if (TimeManager.isPaused) return;

        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY -= Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);
        transform.position = target.position + rotation * offset;
        transform.LookAt(target.position + Vector3.up * 1.0f); // 캐릭터 머리 높이 쯤 보기
    }

    public Quaternion GetCameraRotationY()
    {
        return Quaternion.Euler(0, rotationX, 0);
    }
}
