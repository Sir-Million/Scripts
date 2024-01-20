using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem : MonoBehaviour
{

    public float movementSpeed;

    public new Rigidbody rigidbody;
    public Vector3 direction;
    public Vector3 LastDirection;
    public GameObject Player;
    public GameObject Camera;

    public float acceleration = 0.0f;
    public bool ismoving;

    public float timedelta;

    public Vector3 DefCamAngle;
    public float maxangle;
    public Vector3 CamAngle;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        timedelta = Time.deltaTime * 5f;

        if (hor != 0 || ver != 0) {
            ismoving = true;
            acceleration = Mathf.Clamp01(acceleration + timedelta);
        } else {
            ismoving = false;
            acceleration = Mathf.Clamp01(acceleration - timedelta);
        }

        float xAngle = DefCamAngle.x + (-LastDirection.z * acceleration * maxangle);
        float zAngle = DefCamAngle.z + (LastDirection.x * acceleration * maxangle);

        CamAngle = new Vector3(xAngle, 0f, zAngle);

        Camera.transform.rotation = Quaternion.Euler(CamAngle);

        if (acceleration != 0 && (hor != 0 || ver != 0))
        {
            LastDirection = direction;
        }

        if (LastDirection != Vector3.zero)
        {
            Player.transform.rotation = Quaternion.LookRotation(LastDirection);
        }

        direction = (transform.forward * ver + transform.right * hor).normalized;

        rigidbody.velocity = LastDirection * movementSpeed * acceleration;

        if (ismoving)
        {
            if (acceleration <= 0)
            {
                acceleration = Mathf.Lerp(0, 1, Time.deltaTime);
            }
        }
        else
        {
            if (acceleration >= 1)
            {
                acceleration = Mathf.Lerp(1, 0, Time.deltaTime);
            }
        }
    }
}
