using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PhotonClientManager : MonoBehaviour, IChatClientListener
{
    public ChatClient chatClient;
    public InputField plrName;
    public Text connectionState;
    string worldChat;
    public InputField msgInput;
    public Text msgArea;
    public GameObject introGame;
    public GameObject msgPanel;
    public Dropdown UserS;
    private List<String> aea = new List<String>();
    //[SerializeField] string userID;
    void Start()
    {
        Application.runInBackground = true;

        if (string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat))
        {
            print("No chat ID provider");
            return;
        }

        connectionState.text = "Connectando...";
        worldChat = "world";
        //getConnected();
        /*chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion,
            new AuthenticationValues(userID));*/
    }

    // Update is called once per frame
    void Update()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Service();
        }
    }

    public void getConnected()
    {
        print("Trying to connect");
        this.chatClient = new ChatClient(this);
        this.chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,"anything",new AuthenticationValues(plrName.text));
        connectionState.text = "Connecting to chat";
        //msgArea.text = plrName.text + " se ha conectado";
    }

    public void senMsg()
    {
        
        this.chatClient.PublishMessage(UserS.options[UserS.value].text, msgInput.text);
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
        introGame.SetActive(false);
        msgPanel.SetActive(true);
        connectionState.text = "Connected";
        this.chatClient.Subscribe(new string[] {worldChat,plrName.text});
        this.chatClient.SetOnlineStatus(ChatUserStatus.Online);

    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            msgArea.text = msgArea.text + "\n"+ senders[i] + ": " + messages[i];
        }
    }
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        UserS.ClearOptions();
        foreach (string channel in channels)
        {
            aea.Add(channel);
            this.chatClient.PublishMessage(channel, "se ha unido");
        }
        
        UserS.AddOptions(aea);

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
