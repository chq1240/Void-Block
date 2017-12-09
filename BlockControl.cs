using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour {

    public int blocknum = 0;
    public GameObject SpaceBlock;
    CharacterController controller;
    public Transform Camtarget;
    private GameObject expEffect = null;
    private RaycastHit hit;
    private GameObject gameObj, temp = null;
    public GameObject body;

    public Text blockcount;
    private PhotonView pv = null;
    private Vector3 dir, currdir;
    Vector2 v = new Vector2(Screen.width / 2, Screen.height / 2);
    // Use this for initialization
    void Awake () {
        SpaceBlock = (GameObject)Resources.Load("SpaceBlock");
        controller = body.GetComponent<CharacterController>();
        expEffect = Resources.Load<GameObject>("blockeffect1");
        pv = GetComponent<PhotonView>();

        pv.ObservedComponents[0] = this;

        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        currdir = Vector3.zero;
        blockcount.text = "0";
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (pv.isMine)
        {
        //Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(v);//new Ray(MainCam.position, MainCam.forward);
            blockcount.text = blocknum.ToString();
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.blue);
                dir = ray.direction;
                gameObj = hit.collider.gameObject;
                if(gameObj.tag == "Block" )//当射线碰撞目标为Block类型的物品 ，执行拾取操作
                {
                    if (Vector3.Distance(gameObj.transform.position, this.transform.position) <= 15
                        && Input.GetKeyDown(KeyCode.F))
                    {
                        temp = gameObj;
                        Debug.Log("click object name is " + gameObj.name);
                        blocknum++;
                        DestroyBlock();
                        pv.RPC("DestroyBlock", PhotonTargets.Others, null);
                    }
                 }
            }
        }
        else
        {
            if(Physics.Raycast(Camtarget.position, currdir, out hit, Mathf.Infinity))
            {
            Debug.DrawRay(Camtarget.position, currdir * 100.0f, Color.red);
                if (hit.collider.gameObject.tag == "Block")
                    gameObj = hit.collider.gameObject;
            }
        }


        /*createblock
        if(Input.GetKeyDown(KeyCode.G)&&blocknum> 0 && pv.isMine)
        {
            blocknum--;
            if (controller.isGrounded == true)
            {
                CreatBlock2();
                pv.RPC("CreatBlock2", PhotonTargets.Others, null);
            }
            else
            {
                CreatBlock1();
                pv.RPC("CreatBlock1", PhotonTargets.Others, null);
            }
        }*/


    }
    

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(dir);
        }
        else
        {
            currdir = (Vector3)stream.ReceiveNext();
        }
    }
    /*[PunRPC]
    void CreatBlock2()
    {
        GameObject _block = (GameObject)Instantiate(SpaceBlock, ForwardBlock.position, Quaternion.identity);
        Object effect = GameObject.Instantiate(expEffect, ForwardBlock.position, Quaternion.identity);
        Destroy(effect, 1.2f);
    }
    
    [PunRPC]
    void CreatBlock1()
    {
        GameObject _block = (GameObject)Instantiate(SpaceBlock, DownBlock.position, Quaternion.identity);
        Object effect = GameObject.Instantiate(expEffect, DownBlock.position, Quaternion.identity);
        Destroy(effect, 1.2f);
    }
    */
    [PunRPC]
    void DestroyBlock()
    {
        if (gameObj)
        {
            gameObj.tag = "Untagged";
            StartCoroutine(this.destroyNcreate());
            
        }
    }
    IEnumerator destroyNcreate()
    {
        Object effect1 = GameObject.Instantiate(expEffect, gameObj.transform.position, Quaternion.identity);
        Vector3 pos = gameObj.transform.position;
        Destroy(gameObj, 0.6f);
        Destroy(effect1, 1f);
        yield return new WaitForSeconds(80);
        GameObject _block = (GameObject)Instantiate(SpaceBlock, pos, Quaternion.identity);
        Object effect2 = GameObject.Instantiate(expEffect, pos, Quaternion.identity);
        Destroy(effect2, 1.2f);
    }
}
