using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCheck : MonoBehaviour {
    public bool iscollider = false;
    RaycastHit hit;
    // Use this for initialization
    void Start () {
		
	}
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != 11)
            iscollider = true;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 11)
            iscollider = true;
    }
    void OnTriggerExit(Collider other)
    {
        iscollider = false;
    }
    // Update is called once per frame
    void Update () {
        /*
        if (Physics.SphereCast(this.transform.position, 1f, transform.forward, out hit))
        {
            Debug.Log(hit.collider.name);
            iscollider = true;
            Debug.Log(1);
        }
        else
            iscollider = false;*/
    }
}
