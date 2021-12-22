using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if(PV.IsMine){
            CreateController();
        }
    }
    void CreateController(){
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerAvion"),new Vector3(-74f,3.6f,-685.75f),Quaternion.identity);
    }

}
