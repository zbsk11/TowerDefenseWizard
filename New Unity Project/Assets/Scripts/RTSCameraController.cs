using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 12f;
    public Vector2 xLimits = new Vector2(-20f, 20f);
    public Vector2 zLimits = new Vector2(-20f, 20f);

    [Header("Zoom")]
    public Transform cam;   
    public float zoomSpeed = 50f;
    public float minHeight = 10f;
    public float maxHeight = 35f;

    [Header("Rotate")]
    public float rotateSpeed = 90f; // degrees/sec

    void Awake()
    {
        if (!cam) cam = Camera.main.transform;
    }

    void Update()
    {
        // --- Pan (WASD) ---
        Vector3 input = new Vector3(
            (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0),
            0f,
            (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0)
        );

        Vector3 right = new Vector3(transform.right.x, 0f, transform.right.z).normalized;
        Vector3 fwd   = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
        Vector3 move  = (right * input.x + fwd * input.z) * moveSpeed * Time.deltaTime;

        transform.position += move;

        // Clamp to play area
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, xLimits.x, xLimits.y);
        p.z = Mathf.Clamp(p.z, zLimits.x, zLimits.y);
        transform.position = p;

        // --- Zoom (mouse wheel) ---
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.001f)
        {
            float height = cam.localPosition.y - scroll * (zoomSpeed * Time.deltaTime);
            height = Mathf.Clamp(height, minHeight, maxHeight);
            cam.localPosition = new Vector3(cam.localPosition.x, height, cam.localPosition.z);

            // keep angle pleasant as we zoom 
            float t = Mathf.InverseLerp(maxHeight, minHeight, height);
            float pitch = Mathf.Lerp(35f, 60f, t);
            cam.localEulerAngles = new Vector3(pitch, cam.localEulerAngles.y, 0f);
        }

        // --- Rotate (Q/E) ---
        float yaw = 0f;
        if (Input.GetKey(KeyCode.Q)) yaw -= 1f;
        if (Input.GetKey(KeyCode.E)) yaw += 1f;
        if (Mathf.Abs(yaw) > 0.001f)
            transform.Rotate(Vector3.up, yaw * rotateSpeed * Time.deltaTime, Space.World);
    }
}
