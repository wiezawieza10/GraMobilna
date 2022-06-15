using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject spawnPosition;
    private void Start()
    {
        GameObject Player = PhotonNetwork.Instantiate(playerPrefab.name, 
            spawnPosition.transform.position + new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f), 0), Quaternion.identity);
        GameManager_Multiplayer.instance.currentPlayer = Player;
    }
}
