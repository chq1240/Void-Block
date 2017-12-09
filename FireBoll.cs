using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class FireBoll : MonoBehaviour {
    public GameObject cannon1 = null;
    public GameObject cannon2 = null;
    public GameObject body;
    public Transform Direction;
    public Transform firePos;
    public float firerate = 0.8f;
    private float nextfire = 0.0f;
    private AudioClip fireSfx = null;
    private AudioSource sfx = null;
    private BlockControl blockctrl;
    private PhotonView pv = null;
    private RaycastHit hit;
    Vector2 v = new Vector2(Screen.width / 2, Screen.height / 2);
    // Use this for initialization
    void Awake () {
        cannon2 = (GameObject)Resources.Load("BouncyBoll");

        cannon1 = (GameObject)Resources.Load("Cannon");
        pv = GetComponent<PhotonView>();
        blockctrl = body.GetComponent<BlockControl>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!Cursor.visible)
        {
            if (blockctrl.blocknum >= 3 && Input.GetMouseButtonUp(1) && pv.isMine && Time.time > nextfire)
            {
                nextfire = Time.time + firerate;
                Ray ray = Camera.main.ScreenPointToRay(v);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
                {
                    FireBoll2();
                    pv.RPC("FireBoll2", PhotonTargets.Others, null);
                    blockctrl.blocknum -= 3;
                }
            }
            if (blockctrl.blocknum >= 1 && Input.GetMouseButtonUp(0) && pv.isMine && Time.time > nextfire)
            {
                nextfire = Time.time + firerate;
                Ray ray = Camera.main.ScreenPointToRay(v);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
                {
                    FireBoll1();
                    pv.RPC("FireBoll1", PhotonTargets.Others, null);
                    blockctrl.blocknum -= 1;
                }
            }
        }
    }
    [PunRPC]
    void FireBoll1()
    {
        //sfx.PlayOneShot(fireSfx, 1.0f);
        GameObject _cannon = (GameObject)Instantiate(cannon1, firePos.position, firePos.rotation);
        //_cannon.GetComponent<Rigidbody>().AddForce( Dir*1000);
    }
    [PunRPC]
    void FireBoll2()
    {
        //sfx.PlayOneShot(fireSfx, 1.0f);
        GameObject _cannon = (GameObject)Instantiate(cannon2, firePos.position, firePos.rotation);
        //_cannon.GetComponent<Rigidbody>().AddForce( Dir*1000);
    }
}
