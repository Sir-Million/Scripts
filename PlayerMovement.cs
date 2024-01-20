using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private new Rigidbody rigidbody;

    public float movementSpeed;
    public float sensitivity;

    public float horizontalSpeed;
    public float verticalSpeed;
    public float mouseHorizontal;
    public float mouseVertical;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UpdateMovement() {

        float hor = Input.GetAxisRaw("Horizontal");
        horizontalSpeed = hor;
        float ver = Input.GetAxisRaw("Vertical");
        verticalSpeed = ver;

        if (hor != 0 || ver != 0) {
            Vector3 direction = (transform.forward * ver + transform.right * hor).normalized;
        
            rigidbody.velocity = direction * movementSpeed;
        }
        
    }

    private void UpdateMouseLook() {
        float hor = Input.GetAxis("Mouse X");
        mouseHorizontal = hor;
        float ver = Input.GetAxis("Mouse Y");
        mouseVertical = ver;

        if (hor != 0) {
            transform.Rotate(0, hor * sensitivity, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateMouseLook();
    }
}