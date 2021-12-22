using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;



public class PlayerController : MonoBehaviour
{
    
    [SerializeField] float speed = 2;
    [SerializeField] float maxSpeed = 100;
    [SerializeField] float minSpeed = 5;
    [SerializeField] float posSpedd = 20;
    [SerializeField] float rootSpeed1 = 50;
    [SerializeField] float rootSpeed2 = 50;
    PhotonView PV;
    static UdpClient client;
    public List<string> list1 = new List<string>();
    public List<int> mano1 = new List<int>();
    public List<int> mano2 = new List<int>();
    public List<int> rostro = new List<int>();

    Shot shot;

    void Awake(){
        shot = GetComponent<Shot>();
        PV = GetComponent<PhotonView>();
    }
    void Start(){
        
        if(!PV.IsMine){
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }
        if(PV.IsMine){
            client = new UdpClient(5002);
        }
        
    }
    
    void Update()
    {
        if(!PV.IsMine){
            return;
        }
        Move();
    }

    void Move(){
         
        try{
            
            IPEndPoint remoteEndPoint = null;
            byte[] bytesRecibidos = client.Receive(ref remoteEndPoint);
            String mensaje = Encoding.ASCII.GetString(bytesRecibidos);

            list1 = mensaje.Split('/').ToList();    

            mano1 = convertInt(list1[0].Split(',').ToList());
            mano2 = convertInt(list1[1].Split(',').ToList());
            rostro = convertInt(list1[2].Split(',').ToList());

            if(mano1.Any()){
                if(mano1[1]==1){

                    shot.disparar();
                }
            }

            if(mano2.Any()){

               if(mano2[1]==1){
                    if(Time.timeScale == 1){    //si la velocidad es 1
			            Time.timeScale = 0; 	//que la velocidad del juego sea 0
		            } else if(Time.timeScale == 0) {   // si la velocidad es 0
			            Time.timeScale = 1;  	// que la velocidad del juego regrese a 1
		}
                }

            }
            if(rostro.Any()){

                transform.position += transform.forward * speed * Time.deltaTime;
                
                if(rostro[2]==1){
                    transform.Rotate(Vector3.forward * rootSpeed1 * Time.deltaTime);
                    //direccionx.text = "Dirección: DERECHA";
                }
                if(rostro[0]==1){
                    transform.Rotate(Vector3.back * rootSpeed1 * Time.deltaTime);
                    //direccionx.text = "Dirección: IZQUIERDA";
                }
                if(rostro[3]==1){
                    transform.Rotate(Vector3.left * rootSpeed1 * Time.deltaTime);
                    //direcciony.text = "Dirección: ABAJO";
                }
                if(rostro[5]==1){
                    transform.Rotate(Vector3.right * rootSpeed1 * Time.deltaTime);
                    //direcciony.text = "Dirección: ARRIBA";
                }
                if(rostro[1]==1){
                    transform.Rotate(Vector3.zero * rootSpeed1 * Time.deltaTime);
                    //direcciony.text = "Dirección: Centro";
                }
                
            }

        }
        catch(Exception e){
            
            Debug.Log("No se esta recibiendo nada " + e);
        }

    }

    public List<int> convertInt(List<String> lista){

        var listaInt = new List<int>();

        listaInt = lista.Select(s => Int32.TryParse(s, out int n) ? n : (int?)null)
                .Where(n => n.HasValue)
                .Select(n => n.Value)
                .ToList();    

        return listaInt;
    }


}
