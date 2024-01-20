using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeRandom : MonoBehaviour
{

    public GameObject planeObject;  // Objeto que representa el plano
    public Vector3 ObjPosition;
    public GameObject Player;
    public Grid grid;
    public GameObject mouseIndicator, cellIndicator;
    public GameObject gridVisualization;
    public GameObject fire;
    public Vector2 values;
    public float randomInt;
    public float count;
    public Vector3 Firepos;
    public string tagToCount = "Fire";
    public float objectCount;

    public float WaitTime;
    public bool started;

    // Start is called before the first frame update
    void Start()
    {
        started = false;
        Vector3 randomPosition = GetRandomPositionWithinPlane();
    }

    private void Update()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tagToCount);
        objectCount = objectsWithTag.Length;
        Vector3 mousePosition = mouseIndicator.transform.position;
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
        
        if (started == false)
        {
        
            WaitTime -= Time.deltaTime;
            if (WaitTime <= 0)
            {
                WaitTime = 0;
                started = true;
            }
        
        }
        
        if (started == true)
        {
            
            if (objectCount < 3)
            {
            
                if (count <= 0 )
                {
                    count = Random.Range(values.x, values.y);
                    randomInt = count;
                    FireSpawn();            
                }
                if (count > 0)
                {
                    count -= Time.deltaTime;
                }
            
            }
        
        }
    }

    public void FireSpawn()
    {
        // Obtener los límites del plano
        Vector3 planeCenter = planeObject.transform.position;
        Vector3 planeExtents = planeObject.GetComponent<Renderer>().bounds.extents;

        // Generar una posición aleatoria dentro del plano
        Vector3 randomPosition = new Vector3(
            Random.Range(planeCenter.x - planeExtents.x, planeCenter.x + planeExtents.x),
            0.05f,
            Random.Range(planeCenter.z - planeExtents.z, planeCenter.z + planeExtents.z)
        );

        Firepos = randomPosition;
        GameObject instance = Instantiate(fire, Firepos, Quaternion.identity);
    }

    private Vector3 GetRandomPositionWithinPlane()
    {
        // Obtener los límites del plano
        Vector3 planeCenter = planeObject.transform.position;
        Vector3 planeExtents = planeObject.GetComponent<Renderer>().bounds.extents;

        // Generar una posición aleatoria dentro del plano
        Vector3 randomPosition = new Vector3(
            Random.Range(planeCenter.x - planeExtents.x, planeCenter.x + planeExtents.x),
            0.5f,
            Random.Range(planeCenter.z - planeExtents.z, planeCenter.z + planeExtents.z)
        );

        ObjPosition = randomPosition;
        Player.transform.position = ObjPosition;
        return randomPosition;
    }
}
