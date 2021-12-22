using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawnPoint;

    public float shotForce = 1800;
    public float shotRate = 0.3f;

    private float shotRateTime = 0;

    // Update is called once per frame
    void Update()
    {
        
    }
    public void disparar(){
        if(Time.time>shotRateTime){
            GameObject newBullet;
            newBullet = Instantiate(bullet,spawnPoint.position,spawnPoint.rotation);
            newBullet.GetComponent<Rigidbody>().AddForce(spawnPoint.forward*shotForce);
            shotRateTime = Time.time + shotRate;
            Destroy(newBullet,5);
        }
    }



}
