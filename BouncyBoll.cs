using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBoll : MonoBehaviour {
    public float speed = 6000.0f;
    private Vector3 m_preVelocity = Vector3.zero;//上一帧速度
    private Rigidbody rig;
    private SphereCollider coll;
    private int colltime = 0;
    private GameObject expEffect = null;
    void Start () {
        rig = GetComponent<Rigidbody>();
        coll = GetComponent<SphereCollider>();
        expEffect = Resources.Load<GameObject>("blockeffect1");
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        StartCoroutine(this.ExplosionCannon(6.0f));
        Debug.Log("Fire");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter(Collision collision)
    {
        GameObject obj = (GameObject)Instantiate(expEffect, transform.position, Quaternion.identity);

        Destroy(obj, 1.0f);
        if (collision.gameObject.tag == "Block")
        {
            Object effect = GameObject.Instantiate(expEffect, collision.gameObject.transform.position, Quaternion.identity);
            
            Destroy(collision.gameObject, 0.8f);
            Destroy(effect, 1.2f);
            colltime++;
            
        }
        if(collision.gameObject.tag == "Player"|| colltime == 4)
        {
            StartCoroutine(this.ExplosionCannon(0.0f));
        }
    }
    IEnumerator ExplosionCannon(float tm)
    {
        yield return new WaitForSeconds(tm);
        coll.enabled = false;
        rig.isKinematic = true;
        Destroy(this.gameObject, 0.5f);
    }
}
