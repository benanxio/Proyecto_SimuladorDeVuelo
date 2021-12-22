using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoPuntos : MonoBehaviour
{
	public GameObject ObjPuntos;
	public float puntosQueDa;

	public AudioSource tickSource;
	public AudioClip elsonido;
	public float volumen = 1f;

	 void Start()
    {
    }
	private void OnTriggerEnter(Collider other){
		AudioSource.PlayClipAtPoint(elsonido,gameObject.transform.position);
		ObjPuntos.GetComponent<Puntos>().puntos += puntosQueDa;
		Destroy(gameObject);
		Debug.Log("OBJETO DESTRUIDO");
	}

}
