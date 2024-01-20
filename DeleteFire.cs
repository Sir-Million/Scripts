using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteFire : MonoBehaviour
{
    public SphereCollider trigger;
    public GameObject Fire;
    public bool interaction;
    public bool candeletefire;

    public float firelife;
    public FireLogic firelogic;

    public void Start()
    {
    
        interaction = false;
        candeletefire = false;
    
    }

    public void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // Comenzar la acción de disparo
            interaction = true;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            // Detener la acción de disparo
            interaction = false;
        }

        if (interaction == true && Fire != null)
        {
            Destroy(Fire);
            Fire = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject != null && other.gameObject.CompareTag("Fire"))
        {
            Fire = other.gameObject;

            firelogic = Fire.GetComponent<FireLogic>();

            candeletefire = true;

            firelife = firelogic.firelife;
        }
    
    }

    private void OnTriggerExit(Collider other)
    {
        
        Fire = null;
        
        firelogic = null;

        candeletefire = false;
    }
}
