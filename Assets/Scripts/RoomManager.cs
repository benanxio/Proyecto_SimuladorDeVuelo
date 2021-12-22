using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    void Awake(){
        if(Instance){
            Destroy(gameObject);
            return;
        }
        else{
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode){
        if(scene.buildIndex == 1){
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerManager"),new Vector3(-74f,3.6f,-685.75f), Quaternion.identity);
        }
    }
}
