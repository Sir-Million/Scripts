using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public List<Vector2Int> coordinatesList = new List<Vector2Int>();
    public List<Vector2> bridgeCoordinatesList = new List<Vector2>();
    public List<Vector2Int> adjacentCoordinatesList = new List<Vector2Int>();
    public List<Vector2Int> blockedCoordinatesList = new List<Vector2Int>();
    public Dictionary<Vector2Int, bool> instantiatedBuilds = new Dictionary<Vector2Int, bool>();
    public Dictionary<Vector2Int, bool> instantiatedAdjacentBuilds = new Dictionary<Vector2Int, bool>();
    public GameObject build;
    public GameObject adjacentBuild;
    public GameObject bridge;
    public GameObject NoWay;
    public float boss;
    public float exit;

    public int maxCoordinates;
    public int bridgeCount;
    public bool bridgesplaced;
    public int confirmedCoordinates;

    void Start()
    {
        bridgesplaced = false;

        // Agregar (0,0) a las coordenadas bloqueadas
        blockedCoordinatesList.Add(Vector2Int.zero);

        // Generar las cuatro coordenadas adyacentes a (0,0)
        Vector2Int right = new Vector2Int(1, 0);

        // Agregar las coordenadas adyacentes a sus respectivas listas
        adjacentCoordinatesList.Add(right);
    }

    void Update()
    {
        bridgeCount = bridgeCoordinatesList.Count;

        if (confirmedCoordinates < maxCoordinates)
        {
            // Generar una nueva coordenada aleatoria de la lista de coordenadas adyacentes no bloqueadas
            List<Vector2Int> availableAdjacentCoordinates = GetAvailableAdjacentCoordinates();
            int randomIndex = Random.Range(0, availableAdjacentCoordinates.Count);
            Vector2Int newCoordinate = availableAdjacentCoordinates[randomIndex];

            // Agregar la nueva coordenada a coordinatesList
            coordinatesList.Add(newCoordinate);

            // Eliminar la nueva coordenada de la lista de coordenadas adyacentes
            adjacentCoordinatesList.Remove(newCoordinate);

            // Bloquear la nueva coordenada
            blockedCoordinatesList.Add(newCoordinate);

            // Generar las coordenadas adyacentes restantes, ignorando las coordenadas bloqueadas
            GenerateRemainingAdjacentCoordinates(newCoordinate);

            confirmedCoordinates++;
        }
        else
        {
            // Instanciar los objetos "build" en las coordenadas generadas
            foreach (Vector2Int coordinate in coordinatesList)
            {
                if (!instantiatedBuilds.ContainsKey(coordinate))
                {
                    GameObject instantiatedObject = Instantiate(build, new Vector3(coordinate.x, coordinate.y, 0), Quaternion.Euler(-90, 0, 0));
                    instantiatedBuilds.Add(coordinate, true);
                }
            }

            // Instanciar los objetos "adjacentBuild" en las coordenadas adyacentes generadas
            foreach (Vector2Int coordinate in adjacentCoordinatesList)
            {
                if (!instantiatedAdjacentBuilds.ContainsKey(coordinate))
                {
                    GameObject instantiatedObject = Instantiate(adjacentBuild, new Vector3(coordinate.x, coordinate.y, 0), Quaternion.Euler(-90, 0, 0));
                    instantiatedAdjacentBuilds.Add(coordinate, true);
                }
            }

            if (bridgeCount < maxCoordinates)
            {
                foreach (Vector2Int blockedCoordinate in blockedCoordinatesList)
                {
                    Vector2Int adjacentCoordinate1 = new Vector2Int(blockedCoordinate.x, blockedCoordinate.y + 1);
                    Vector2Int adjacentCoordinate2 = new Vector2Int(blockedCoordinate.x + 1, blockedCoordinate.y);

                    if (blockedCoordinatesList.Contains(adjacentCoordinate1) && !bridgeCoordinatesList.Contains(adjacentCoordinate1))
                    {
                        Vector2 bridgeCoordinate = new Vector2(blockedCoordinate.x, blockedCoordinate.y) + new Vector2(0, 0.5f);
                        bridgeCoordinatesList.Add(bridgeCoordinate);
                        bridgeCount++;
                    }

                    if (blockedCoordinatesList.Contains(adjacentCoordinate2) && !bridgeCoordinatesList.Contains(adjacentCoordinate2))
                    {
                        Vector2 bridgeCoordinate = new Vector2(blockedCoordinate.x, blockedCoordinate.y) + new Vector2(0.5f, 0);
                        bridgeCoordinatesList.Add(bridgeCoordinate);
                        bridgeCount++;
                    }
                }
            }

            // Instanciar todos los puentes una vez que la generación haya terminado
            if (bridgeCount == bridgeCoordinatesList.Count && bridgesplaced == false)
            {
                foreach (Vector2 bridgeCoordinate in bridgeCoordinatesList)
                {
                    Vector3 bridgePosition = new Vector3(bridgeCoordinate.x, bridgeCoordinate.y, 0);
                    GameObject instantiatedObject = Instantiate(bridge, bridgePosition, Quaternion.Euler(-90, 0, 0));
                }

                bridgesplaced = true;
            }
        }
    }

    void GenerateRemainingAdjacentCoordinates(Vector2Int coordinate)
    {
        Vector2Int up = new Vector2Int(coordinate.x, coordinate.y + 1);
        Vector2Int down = new Vector2Int(coordinate.x, coordinate.y - 1);
        Vector2Int left = new Vector2Int(coordinate.x - 1, coordinate.y);
        Vector2Int right = new Vector2Int(coordinate.x + 1, coordinate.y);

        if (!blockedCoordinatesList.Contains(up) && !coordinatesList.Contains(up) && !adjacentCoordinatesList.Contains(up))
            adjacentCoordinatesList.Add(up);
        if (!blockedCoordinatesList.Contains(down) && !coordinatesList.Contains(down) && !adjacentCoordinatesList.Contains(down))
            adjacentCoordinatesList.Add(down);
        if (!blockedCoordinatesList.Contains(left) && !coordinatesList.Contains(left) && !adjacentCoordinatesList.Contains(left))
            adjacentCoordinatesList.Add(left);
        if (!blockedCoordinatesList.Contains(right) && !coordinatesList.Contains(right) && !adjacentCoordinatesList.Contains(right))
            adjacentCoordinatesList.Add(right);
    }

    List<Vector2Int> GetAvailableAdjacentCoordinates()
    {
        List<Vector2Int> availableAdjacentCoordinates = new List<Vector2Int>();

        foreach (Vector2Int coordinate in adjacentCoordinatesList)
        {
            if (!blockedCoordinatesList.Contains(coordinate))
            {
                availableAdjacentCoordinates.Add(coordinate);
            }
        }

        return availableAdjacentCoordinates;
    }

    bool HasOverlappingBridge(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.1f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Bridge"))
            {
                return true;
            }
        }
        return false;
    }
}
