using UnityEngine;
using Unity.Netcode;


public class CustomPlayerSpawner : MonoBehaviour
{    [SerializeField] private GameObject playerCube;  
    [SerializeField] private GameObject playerPrefabA;
    [SerializeField] private GameObject playerPrefabB;

    private void Awake()
    {
        
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        
        GameObject chosenPrefab;

        if (request.ClientNetworkId % 2 == 0) 
        {
            chosenPrefab = playerPrefabA;
        }
        else // impares usan B
        {
            chosenPrefab = playerPrefabB;
        }

        response.Approved = true;
        response.CreatePlayerObject = true;
       response.PlayerPrefabHash = chosenPrefab.GetComponent<NetworkObject>().PrefabIdHash;
    }
}
