using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class align : MonoBehaviour
{

    public Grid grid;
    public bool placed = false;
    public Vector3 OldPosition;
    public Vector3 PositionFix;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (placed == false)
        {
            grid = GameObject.FindObjectOfType<Grid>();
            if (grid != null)
            {
                Vector3 objPos = gameObject.transform.position;

                if (objPos.x < 0)
                    PositionFix.x = 0.5f;
                if (objPos.z < 0)
                    PositionFix.z = 0.5f;
                if (objPos.x > 0)
                    PositionFix.x = -0.5f;
                if (objPos.z > 0)
                    PositionFix.z = -0.5f;

                Vector3Int gridPosition = grid.WorldToCell(objPos);

                OldPosition = gridPosition;

                Vector3 adjustedPosition = new Vector3(
                    (float)gridPosition.x + PositionFix.x,
                    (float)gridPosition.y,
                    (float)gridPosition.z + PositionFix.z
                );

                gameObject.transform.position = adjustedPosition;

                placed = true;
            }
        }
    }

}
