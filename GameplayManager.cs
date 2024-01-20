using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayManager : MonoBehaviour
{

    public List<GameObject> playerObjects = new List<GameObject>();

    public TMP_InputField CodeImput;
    public TextMeshProUGUI playerno;
    public TextMeshProUGUI players;
    public Button CrearRelay;
    public Button ConectarRelay;
    public bool isCodeValid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {

        isCodeValid = CodeImput.text.Length == 6;

        ConectarRelay.interactable = isCodeValid;

        // Obtener todos los Game Objects con el tag "Player"
        playerObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        
        foreach (GameObject playerObject in playerObjects)
        {
            PlayerNetwork playerNetwork = playerObject.GetComponent<PlayerNetwork>();
            if (playerNetwork != null)
            {
                if (playerNetwork.IsOwner)
                {
                    // Actualizar UI con información del jugador propietario
                    playerno.text = "P" + playerNetwork.Playerno.Value.ToString();
                    players.text = playerObjects.Count.ToString();
                    // Puedes realizar más acciones con el jugador propietario aquí si es necesario
                }
            }
        }

    }
}
