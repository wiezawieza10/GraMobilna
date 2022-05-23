using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Crate_Multiplayer : Fighter_Multiplayer
{
    PhotonView view;
    private void Start()
    {
        view = GetComponent<PhotonView>();
    }
    protected override void Death()
    {
        view.RPC("CrateDeathRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void CrateDeathRPC()
    {
        Destroy(gameObject);
    }
}
