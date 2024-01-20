using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;
using TMPro;

public class TestRelay : MonoBehaviour
{

    public string lobbyCode;
    public TMP_InputField CodeImput;
    public TextMeshProUGUI code;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRelay() {
        try {

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(7);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log(joinCode);

            code.text = joinCode;
            code.color = Color.blue;

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

        } catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }

    public async void JoinRelay() {

        try {

            lobbyCode = CodeImput.text;
            
            Debug.Log("Joining Relay With " + lobbyCode);

            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(lobbyCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();

            code.text = lobbyCode;
            code.color = Color.blue;

        } catch (RelayServiceException e) {
            Debug.Log(e);
        }

    }

}
