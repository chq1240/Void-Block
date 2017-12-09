using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBlock : MonoBehaviour {
    public GameObject block0;
    public GameObject block1;
    public GameObject block2;
    public GameObject block3;
    public GameObject blockcontrol;
    private GameObject newblock;
    public GameObject SpaceBlock;
    public GameObject body;
    CharacterController controller;
    private GameObject expEffect = null;
    private BlockCheck blockcheck0;
    private BlockCheck blockcheck1;
    private BlockCheck blockcheck2;
    private BlockCheck blockcheck3;
    private MeshRenderer blockmesh0;
    private MeshRenderer blockmesh1;
    private MeshRenderer blockmesh2;
    private MeshRenderer blockmesh3;
    private PhotonView pv = null;
    private bool setting = true;
    private BlockControl blockctrl;
    // Use this for initialization
    void Awake () {
        blockcheck0 = block0.GetComponent<BlockCheck>();
        blockcheck1 = block1.GetComponent<BlockCheck>();
        blockcheck2 = block2.GetComponent<BlockCheck>();
        blockcheck3 = block3.GetComponent<BlockCheck>();
        blockmesh0 = block0.GetComponent<MeshRenderer>();
        blockmesh1 = block1.GetComponent<MeshRenderer>();
        blockmesh2 = block2.GetComponent<MeshRenderer>();
        blockmesh3 = block3.GetComponent<MeshRenderer>();
        blockctrl = blockcontrol.GetComponent<BlockControl>();
        controller = body.GetComponent<CharacterController>();
        expEffect = Resources.Load<GameObject>("blockeffect1");
        SpaceBlock = (GameObject)Resources.Load("SpaceBlock");
        newblock = null;
        pv = GetComponent<PhotonView>();

        if (pv.isMine)
        {
            blockcheck3.iscollider = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (pv.isMine)
        {
            if(Input.anyKey && setting)
            {
                blockcheck0.iscollider = true;
                blockcheck1.iscollider = true;
                blockcheck2.iscollider = false;
                blockcheck3.iscollider = false;
                setting = false;

            }
            if (!Input.GetKey(KeyCode.G))
            {
                SetBlockActive(false);
            }
            if (!blockcheck0.iscollider)
            {
                if (Input.GetKey(KeyCode.G))
                {
                    blockmesh0.enabled = true;
                    blockmesh1.enabled = false;
                    blockmesh2.enabled = false;
                    blockmesh3.enabled = false;
                }
                    if (Input.GetKeyUp(KeyCode.G) && blockctrl.blocknum > 0)
                {
                    blockctrl.blocknum--;
                    CreatNewBlock0();
                    pv.RPC("CreatNewBlock0", PhotonTargets.Others, null);

                }
            }
            else if (!blockcheck1.iscollider)
            {
                if (Input.GetKey(KeyCode.G))
                {
                    blockmesh1.enabled = true;
                    blockmesh0.enabled = false;
                    blockmesh2.enabled = false;
                    blockmesh3.enabled = false;
                }
                if (Input.GetKeyUp(KeyCode.G) && blockctrl.blocknum > 0)
                {
                    blockctrl.blocknum--;
                    CreatNewBlock1();
                    pv.RPC("CreatNewBlock1", PhotonTargets.Others, null);

                }
            }
                else if (!blockcheck2.iscollider)
            {
                if (Input.GetKey(KeyCode.G))
                {
                    blockmesh2.enabled = true;
                    blockmesh1.enabled = false;
                    blockmesh0.enabled = false;
                    blockmesh3.enabled = false;
                }
                if (Input.GetKeyUp(KeyCode.G) && blockctrl.blocknum > 0)
                {
                        blockctrl.blocknum--;
                        CreatNewBlock2();
                        pv.RPC("CreatNewBlock2", PhotonTargets.Others, null);
                    
                }
                }
                else if (!blockcheck3.iscollider)
            {
                if (Input.GetKey(KeyCode.G))
                {
                    blockmesh3.enabled = true;
                    blockmesh1.enabled = false;
                    blockmesh2.enabled = false;
                    blockmesh0.enabled = false;
                }
                if (Input.GetKeyUp(KeyCode.G) && blockctrl.blocknum > 0)
                {
                    blockctrl.blocknum--;
                    CreatNewBlock3();
                    pv.RPC("CreatNewBlock3", PhotonTargets.Others, null);

                }
            }
                else
                {
                    SetBlockActive(false);
                }
            
            
        }
        else
        {
            SetBlockActive(false);
        }
	}
    void SetBlockActive(bool active)
    {
        blockmesh0.enabled = active;
        blockmesh1.enabled = active;
        blockmesh2.enabled = active;
        blockmesh3.enabled = active;
    }
    [PunRPC]
    void CreatNewBlock0()
    {
            GameObject _block = (GameObject)Instantiate(SpaceBlock, block0.transform.position, Quaternion.identity);
            Object effect = GameObject.Instantiate(expEffect, block0.transform.position, Quaternion.identity);
            Destroy(effect, 1.2f);

    }
    [PunRPC]
    void CreatNewBlock1()
    {
        GameObject _block = (GameObject)Instantiate(SpaceBlock, block1.transform.position, Quaternion.identity);
        Object effect = GameObject.Instantiate(expEffect, block1.transform.position, Quaternion.identity);
        Destroy(effect, 1.2f);

    }
    [PunRPC]
    void CreatNewBlock2()
    {
        GameObject _block = (GameObject)Instantiate(SpaceBlock, block2.transform.position, Quaternion.identity);
        Object effect = GameObject.Instantiate(expEffect, block2.transform.position, Quaternion.identity);
        Destroy(effect, 1.2f);

    }
    [PunRPC]
    void CreatNewBlock3()
    {
        GameObject _block = (GameObject)Instantiate(SpaceBlock, block3.transform.position, Quaternion.identity);
        Object effect = GameObject.Instantiate(expEffect, block3.transform.position, Quaternion.identity);
        Destroy(effect, 1.2f);

    }
}
