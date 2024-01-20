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
        // Aceleración progresiva solo cuando se presiona la barra espaciadora
        if (Input.GetKey(KeyCode.Space))
        {
            velocidadInicial += aceleracion * Time.deltaTime;
            velocidadInicial = Mathf.Clamp(velocidadInicial, 0f, velocidadMaxima);
        }
        else
        {
            // Deceleración gradual cuando no se presiona el espacio
            velocidadInicial = Mathf.Lerp(velocidadInicial, 0f, decrementoVelocidad * Time.deltaTime);
        }

        // Rotación con las flechas
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontal * rotacionSpeed * Time.deltaTime);

        // Rotación del objeto BOTE para simular derrape
        float inerciaRotacion = -horizontal * inerciaRotacionFactor * velocidadInicial;

        // Calcular el ángulo objetivo afectado por direccionDerrape
        float anguloObjetivo = Mathf.Clamp(anguloActual + direccionDerrape, -limiteAnguloRotacion, limiteAnguloRotacion);

        // Ajustar la velocidad de rotación del bote
        float velocidadRotacionBote = 10f; // Puedes ajustar este valor según sea necesario

        // Aplicar la rotación de manera suave en el espacio local del padre
        BOTE.transform.Rotate(Vector3.forward, (anguloObjetivo - anguloActual) * velocidadRotacionBote * Time.deltaTime, Space.Self);
        anguloActual = Mathf.Lerp(anguloActual, anguloObjetivo, 0.1f); // Ajustado el valor de interpolación para hacerlo más suave

        // Cambio gradual en la dirección del derrape
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

        // Aplicar la rotación basada en la línea proporcionada
        anguloDerrape = anguloActual * direccionDerrape;
        BOTE.transform.localRotation = Quaternion.Euler(90f, 0f, anguloDerrape);

        // Volver la rotación del BOTE gradualmente a 0 cuando no se está yendo a la derecha o izquierda
        if (horizontal == 0f)
        {
            anguloActual = Mathf.Lerp(anguloActual, 0f, 0.1f);
            BOTE.transform.localRotation = Quaternion.Euler(90f, 0f, anguloActual);
        }
    }
}
