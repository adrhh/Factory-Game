using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float mouseSensitivity = 100f;
    private float xRotation = 0;
    private float yRotation = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        player.Rotate(Vector3.up * mouseX);
    }
}
