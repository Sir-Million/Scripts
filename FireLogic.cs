using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLogic : MonoBehaviour
{

    public string tagToCount;
    public GameObject Player;
    public DeleteFire deletefire;
    public float firelife;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Player = GameObject.FindGameObjectWithTag(tagToCount);

        deletefire = Player.GetComponent<DeleteFire>();

    }
}
