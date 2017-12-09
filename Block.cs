using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    private bool stable = true;
    RaycastHit hit;
    // Use this for initialization
    void Start () {
		if(stable == true)
        {
            if (Physics.SphereCast(this.transform.position, 3f,transform.forward, out hit ))
            {
                Debug.Log(hit.collider.tag);
                    this.gameObject.AddComponent<Rigidbody>();
                    Debug.Log(1);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(1);
        if(GetComponent<Rigidbody>()&& 
            Vector3.Distance(collision.gameObject.transform.position, this.transform.position) >= 5)
        {
            Debug.Log(3);
            Destroy(this.gameObject.GetComponent<Rigidbody>(),3f);
            stable = false;
        }
    }
    
}
