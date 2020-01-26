using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2.0f;

    public float sensitivity = 2.0f;
    public float smoothing = 2.0f;

    private float mouseLook = 0.0f;
    private float smooth = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        float h = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float v = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(h, 0, v);

        float mouseMovement = Input.GetAxisRaw("Mouse X");
        mouseMovement *= sensitivity * smoothing;
        smooth = Mathf.Lerp(smooth, mouseMovement, 1.0f / smoothing);
        mouseLook += smooth;
        transform.localRotation = Quaternion.Euler(0, mouseLook, 0);

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
