using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;

public class Chat : MonoBehaviour, IChatClientListener
{
    public ChatClient chatClient;
    public Text connectionState;
    public string worldChat;
    public InputField msgInput;
    public GameObject msgArea,msg;
    public int maxMessages = 25;
    public Color playerMessage, info;


    [SerializeField]
    public List<Message> messagelist = new List<Message>();

    void Start()
    {
        Application.runInBackground = true;

        if (string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat))
        {
            print("No chat ID provider");
            return;
        }

        connectionState.text = "Connectando...";
    }

    void Update()
    {
        if(msgInput.text != ""){
            
            if(Input.GetKeyDown(KeyCode.Return)){
                senMsg();
            }
        }
        else{
            if(!msgInput.isFocused && Input.GetKeyDown(KeyCode.Return)){
                msgInput.ActivateInputField();
            }
        }
        if (this.chatClient != null)
        {
            this.chatClient.Service();
        }
    }
    public void getConnected()
    {
        print("Trying to connect: " + PhotonNetwork.NickName+"  "+worldChat);
        this.chatClient = new ChatClient(this);
        this.chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,"anything",new AuthenticationValues(PhotonNetwork.NickName));
        connectionState.text = "Connecting to chat";
    }

    public void senMsg()
    {
        this.chatClient.PublishMessage(worldChat, msgInput.text);
        msgInput.text = "";
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnDisconnected()
    {
    }

    public void OnConnected()
    {
        print("**********************  Conectado");
        connectionState.text = "Connected";
        this.chatClient.Subscribe(new string[] {worldChat});
        this.chatClient.SetOnlineStatus(ChatUserStatus.Online);

    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {

        if(channelName == PhotonNetwork.CurrentRoom.Name){
            if(messagelist.Count >= maxMessages){
                Destroy(messagelist[0].textObject.gameObject);
                messagelist.Remove(messagelist[0]);
            }

            for (int i = 0; i < senders.Length; i++)
            {
                Debug.Log(senders[i] + ": " + messages[i]);
                Message newmsj = new Message();
                newmsj.text = senders[i] + ": " + messages[i];
                GameObject newText = Instantiate(msg,msgArea.transform);
                newmsj.textObject = newText.GetComponent<Text>();
                newmsj.textObject.text = newmsj.text;

                Debug.Log(messages[i]);

                if(messages[i].Equals("SE HA UNIDO")){
                    newmsj.textObject.color = info;
                    newmsj.textObject.fontStyle = FontStyle.Italic;
                }
                else{
                    newmsj.textObject.color = playerMessage;
                }
                messagelist.Add(newmsj);
            }
        }
    }
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        foreach (string channel in channels)
        {
            this.chatClient.PublishMessage(channel, "SE HA UNIDO");
        }

    }

    public void OnUnsubscribed(string[] channels)
    {

    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {

    }

    public void OnUserSubscribed(string channel, string user)
    {
        
    }

    public void OnUserUnsubscribed(string channel, string user)
    {

    }
}

[System.Serializable]
public class Message{
    public string text;
    public Text textObject;
}
