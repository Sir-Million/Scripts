using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float _speed;
    public float _rotationSpeed;
    public Rigidbody _rigidbody;
    public float ver;
    public float hor;
    public GameObject PlayerAvatar;
    public float AvataryAngle;

    [Header("Camara Orbital")]
    public GameObject camara;
    public GameObject objetivo;
    public GameObject MirarA;
    public float distanciaOrbita;
    public float velocidadOrbita;
    public float alturaCamara;
    public float CamerayAngle;

    [Header("Control del Mouse")]
    public float sensibilidadMouseX;

    private float rotacionY;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movimiento del jugador
        ver = Input.GetAxisRaw("Vertical");
        hor = Input.GetAxisRaw("Horizontal");

        // Control del mouse para cambiar la orientación de la cámara
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouseX;
        rotacionY += mouseX;

        // Obtener el ángulo Y de la cámara
        CamerayAngle = camara.transform.eulerAngles.y;

        // Rotar el PlayerAvatar de acuerdo a la dirección de movimiento
        if (ver != 0f || hor != 0f)
        {
            Vector3 moveDirection = new Vector3(hor, 0f, ver).normalized;
            float moveAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            AvataryAngle = CamerayAngle + moveAngle;

            // Suavemente rotar el PlayerAvatar en función del ángulo Y de la cámara
            AvataryAngle = Mathf.LerpAngle(PlayerAvatar.transform.eulerAngles.y, AvataryAngle, Time.deltaTime * _rotationSpeed);
            PlayerAvatar.transform.rotation = Quaternion.Euler(0, AvataryAngle, 0);

            // Calcular la dirección del movimiento con respecto a la rotación del PlayerAvatar
            Vector3 movementDirection = PlayerAvatar.transform.TransformDirection(Vector3.forward);

            // Aplicar la fuerza al Rigidbody en la dirección del movimiento
            _rigidbody.AddForce(movementDirection * _speed);
        }

        // Controlar la órbita de la cámara
        OrbitCamera();
    }

    // Función para controlar la órbita de la cámara
    void OrbitCamera()
    {
        Quaternion rotacionCircunferencial = Quaternion.Euler(0, rotacionY, 0);
        Vector3 direccionCircunferencial = rotacionCircunferencial * Vector3.forward;

        Vector3 posicionOrbita = objetivo.transform.position - direccionCircunferencial * distanciaOrbita;
        posicionOrbita.y = objetivo.transform.position.y + alturaCamara;

        camara.transform.position = Vector3.Lerp(camara.transform.position, posicionOrbita, Time.deltaTime * velocidadOrbita);

        camara.transform.LookAt(MirarA.transform);
    }
}
