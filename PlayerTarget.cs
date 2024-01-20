using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    public SphereCollider trigger;
    public float Radius = 0.5f;
    public float Center = 0.75f;
    public Rigidbody Dummy;
    public CharacterJoint Joint;

    public Rigidbody pickedObj; // Variable para almacenar el objeto seleccionado como Rigidbody

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Se ha presionado la tecla H.");
            trigger.radius = 2;
        }

        if (Input.GetKeyUp(KeyCode.H))
        {
            Debug.Log("Se soltó la tecla H.");
            trigger.radius = Radius;
        }

        if (pickedObj != null)
        {
            Joint.autoConfigureConnectedAnchor = true;
            Joint.connectedBody = pickedObj;
        }
        else
        {
            Joint.autoConfigureConnectedAnchor = false;
            Joint.connectedBody = Dummy;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
        if (otherRigidbody != null && other.gameObject.CompareTag("Fire") && pickedObj == null)
        {
            pickedObj = otherRigidbody; // Almacena el Rigidbody del objeto en la variable pickedObj
            Debug.Log("Objeto entró en el trigger: " + other.gameObject.name);
            Debug.Log("El jugador ha colisionado con un enemigo!");

            // Aquí puedes realizar acciones adicionales con el objeto seleccionado
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
        if (otherRigidbody == pickedObj)
        {
            pickedObj = null; // Reinicia la variable pickedObj
            Debug.Log("Objeto salió del trigger: " + other.gameObject.name);
        }
    }
}
