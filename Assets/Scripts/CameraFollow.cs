using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [Header("Цель")]
    public Transform target;

    [Header("Настройки камеры")]
    public Vector3 offset = new Vector3(0, 12, -8);
    public float smoothTime = 0.15f;

    [Header("Смещение к прицелу")]
    public float mouseOffsetStrength = 3f;

    private Vector3 velocity = Vector3.zero;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Позиция мыши в мировых координатах
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 mouseOffset = (mouseScreen - screenCenter) / screenCenter;
        mouseOffset = Vector2.ClampMagnitude(mouseOffset, 1f);

        // Смещение камеры в сторону прицела
        Vector3 cameraOffset = new Vector3(mouseOffset.x, 0, mouseOffset.y) * mouseOffsetStrength;

        Vector3 desiredPosition = target.position + offset + cameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.LookAt(target.position + Vector3.up * 1f);
    }

    public float GetYaw() { return 0f; }
}