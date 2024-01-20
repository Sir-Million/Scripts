using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMe : MonoBehaviour
{
    public float velocidadInicial;
    public float aceleracion;
    public float velocidadMaxima;
    public float rotacionSpeed;
    public float derrapeFactor;
    public float decrementoVelocidad;
    public float inerciaRotacionFactor;
    public float fuerzaAdicionalDerrape;
    public float limiteAnguloRotacion;
    public float direccionDerrape = 0f;
    public float anguloDerrape;

    public bool mostrarAnguloRotacion;

    public GameObject BOTE;

    private Rigidbody rb;
    public float anguloActual = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Aceleraci�n progresiva solo cuando se presiona la barra espaciadora
        if (Input.GetKey(KeyCode.Space))
        {
            velocidadInicial += aceleracion * Time.deltaTime;
            velocidadInicial = Mathf.Clamp(velocidadInicial, 0f, velocidadMaxima);
        }
        else
        {
            // Deceleraci�n gradual cuando no se presiona el espacio
            velocidadInicial = Mathf.Lerp(velocidadInicial, 0f, decrementoVelocidad * Time.deltaTime);
        }

        // Rotaci�n con las flechas
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontal * rotacionSpeed * Time.deltaTime);

        // Rotaci�n del objeto BOTE para simular derrape
        float inerciaRotacion = -horizontal * inerciaRotacionFactor * velocidadInicial;

        // Calcular el �ngulo objetivo afectado por direccionDerrape
        float anguloObjetivo = Mathf.Clamp(anguloActual + direccionDerrape, -limiteAnguloRotacion, limiteAnguloRotacion);

        // Ajustar la velocidad de rotaci�n del bote
        float velocidadRotacionBote = 10f; // Puedes ajustar este valor seg�n sea necesario

        // Aplicar la rotaci�n de manera suave en el espacio local del padre
        BOTE.transform.Rotate(Vector3.forward, (anguloObjetivo - anguloActual) * velocidadRotacionBote * Time.deltaTime, Space.Self);
        anguloActual = Mathf.Lerp(anguloActual, anguloObjetivo, 0.1f); // Ajustado el valor de interpolaci�n para hacerlo m�s suave

        // Cambio gradual en la direcci�n del derrape
        direccionDerrape = Mathf.Lerp(direccionDerrape, horizontal, Time.deltaTime);

        // Movimiento hacia adelante solo cuando se presiona la barra espaciadora
        Vector3 forwardForce = transform.forward * velocidadInicial;
        rb.velocity = new Vector3(forwardForce.x, rb.velocity.y, forwardForce.z);

        // Derrape
        float derrape = direccionDerrape * derrapeFactor;

        // Agrega una fuerza adicional de derrape si estamos cerca de -1 o 1
        if (Mathf.Abs(direccionDerrape) > 0.1f)
        {
            float fuerzaLateralAdicional = -fuerzaAdicionalDerrape * direccionDerrape;
            Vector3 fuerzaAdicional = BOTE.transform.right * fuerzaLateralAdicional;
            rb.AddForce(fuerzaAdicional, ForceMode.Impulse);
        }

        Vector3 lateralForce = BOTE.transform.right * derrape;
        rb.AddForce(lateralForce);

        // Aplicar la rotaci�n basada en la l�nea proporcionada
        anguloDerrape = anguloActual * direccionDerrape;
        BOTE.transform.localRotation = Quaternion.Euler(90f, 0f, anguloDerrape);

        // Volver la rotaci�n del BOTE gradualmente a 0 cuando no se est� yendo a la derecha o izquierda
        if (horizontal == 0f)
        {
            anguloActual = Mathf.Lerp(anguloActual, 0f, 0.1f);
            BOTE.transform.localRotation = Quaternion.Euler(90f, 0f, anguloActual);
        }
    }
}
