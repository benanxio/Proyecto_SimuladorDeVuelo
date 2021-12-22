using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class PushToTalk : MonoBehaviour
{
    
    public KeyCode PushButton = KeyCode.T;  
    public Recorder VoiceRecorder;
    private PhotonView V;
    
    void Awake(){
        V = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
        VoiceRecorder.TransmitEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(V.IsMine){
            if(Input.GetKeyDown(PushButton)){
                VoiceRecorder.TransmitEnabled = true;
            }
            else if (Input.GetKeyUp(PushButton)){  
                VoiceRecorder.TransmitEnabled = false;
            }
        }
        
    }
}
