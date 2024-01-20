using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    // Update is called once per frame

    public NetworkVariable<int> Playerno = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> Status = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> ResponseType = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> Response = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> IsOut = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> IsLiar = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public bool changed = false;

    public List<GameObject> playerObjects = new List<GameObject>();

    public int lastnum = 0;

    public override void OnNetworkSpawn() {
        ResponseType.OnValueChanged += (int previousValue, int newValue) => {
            Debug.Log(ResponseType.Value);
        };
        Response.OnValueChanged += (int previousValue, int newValue) => {
            Debug.Log(Response.Value);
        };
        IsLiar.OnValueChanged += (bool previousValue, bool newValue) => {
            Debug.Log(IsLiar.Value);
        };
    }

    private void Update()
    {
        if (!IsOwner) return;

        playerObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));

        foreach (GameObject playerObject in playerObjects)
        {
            PlayerNetwork playerNetwork = playerObject.GetComponent<PlayerNetwork>();
            if (playerNetwork != null)
            {
                int playerNumber = playerNetwork.Playerno.Value;

                // Verificar si el jugador es el dueño del script
                bool isOwner = playerNetwork.IsOwner;

                switch (playerNumber)
                {
                    case 0:
                        if (isOwner)
                        {
                            Playerno.Value = playerObjects.Count + (int)OwnerClientId;
                        }
                        lastnum = Playerno.Value;
                        break;
                    case 1:
                        lastnum = 1;
                        break;
                    case 2:
                        if (isOwner && lastnum < 1)
                        {
                            Playerno.Value = 1;
                            lastnum = 1;
                            break;
                        }
                        lastnum = 2;
                        break;
                    case 3:
                        if (isOwner && lastnum < 2)
                        {
                            Playerno.Value = 2;
                            lastnum = 2;
                            break;
                        }
                        lastnum = 3;
                        break;
                    case 4:
                        if (isOwner && lastnum < 3)
                        {
                            Playerno.Value = 3;
                            lastnum = 3;
                            break;
                        }
                        lastnum = 4;
                        break;
                    case 5:
                        if (isOwner && lastnum < 4)
                        {
                            Playerno.Value = 4;
                            lastnum = 4;
                            break;
                        }
                        lastnum = 5;
                        break;
                    case 6:
                        if (isOwner && lastnum < 5)
                        {
                            Playerno.Value = 5;
                            lastnum = 5;
                            break;
                        }
                        lastnum = 6;
                        break;
                    case 7:
                        if (isOwner && lastnum < 6)
                        {
                            Playerno.Value = 6;
                            lastnum = 6;
                            break;
                        }
                        lastnum = 7;
                        break;
                    case 8:
                        if (isOwner && lastnum < 7)
                        {
                            Playerno.Value = 7;
                            lastnum = 7;
                            break;
                        }
                        lastnum = 8;
                        break;
                    default:
                        if (isOwner && lastnum > 8)
                        {
                            Playerno.Value = 8;
                            lastnum = 8;
                        }
                        lastnum = playerObjects.Count;
                        break;
                }
            }
        }
    }
}