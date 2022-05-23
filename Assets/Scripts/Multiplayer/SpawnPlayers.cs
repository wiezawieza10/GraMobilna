using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject spawnPosition;
    //CameraMotor cameraMotor;
    Joystick joystick;
    private void Awake()
    {
       // cameraMotor = FindObjectOfType<CameraMotor>();
        //joystick = FindObjectOfType<Joystick>();
    }
    private void Start()
    {
        GameObject Player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.transform.position, Quaternion.identity);
        GameManager_Multiplayer.instance.currentPlayer = Player;
        //cameraMotor.SetCameraTarget(Player.transform);
        //Player.GetComponent<Player>().SetJoystick(joystick);
    }
}
