using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{

    public static Launcher Instance;
    [SerializeField] InputField nameRoomInput;
    [SerializeField] Text errorText;
    [SerializeField] Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    Chat chat;

    void Awake(){
        Instance = this;
        chat = GetComponent<Chat>();
    }

    void Start()
    {
        PhotonNetwork.GameVersion = "0.1";

        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Se va a conectar al servidor Master");
        
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Se ha conectado al servidor");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        
    }

    public override void OnJoinedLobby()
    {   
        Debug.Log("Te conectaste al lobby");
        MenuManager.Instance.OpenMenu("MenuP");
        //PhotonNetwork.NickName = "Player" + Random.Range(0,1000).ToString("0000"); 
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Se ha desoonnectado del servidor");
    }

    public void CrearRoom(){
        if(!string.IsNullOrEmpty(nameRoomInput.text)){
            PhotonNetwork.CreateRoom(nameRoomInput.text,new RoomOptions(){MaxPlayers = 4},TypedLobby.Default);
            Debug.Log("Ser creó la sala: "+nameRoomInput.text);
            MenuManager.Instance.OpenMenu("Loading");
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Error al crear la sala: "+message;
        MenuManager.Instance.OpenMenu("Error");
    }

    public void StartGame(){
        PhotonNetwork.LoadLevel(1);
        MenuManager.Instance.OpenMenu("LoadingGame");
    }
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("Room");
        roomNameText.text = "Sala "+PhotonNetwork.CurrentRoom.Name;

        chat.worldChat = PhotonNetwork.CurrentRoom.Name;
        chat.getConnected();

        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform player in playerListContent){
            Destroy(player.gameObject);
        }
        
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab,playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnJoinRandomFailed(short returnCode,string message){
        Debug.Log("No se puedo unir a nunguna, se creará una nueva sala");
    }

    public void JoinRoom(RoomInfo info){
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("Loading");
    }

    public void leaveRoom(){
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("Loading");
    }

    public override void OnLeftRoom(){

        foreach(Message message in chat.messagelist){
            if(message != null){
                Destroy(message.textObject.gameObject);
            }
        }
        chat.messagelist.Clear();
        /**/
        MenuManager.Instance.OpenMenu("MenuP");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList){
        foreach(Transform trans in roomListContent){
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if(roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab,roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        Instantiate(playerListItemPrefab,playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
    
}
