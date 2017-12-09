using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAni : MonoBehaviour
{
    public Animation playerani;
    public string playerstate;
    private string receivestate;
    public GameObject Body, blockctrlbody;
    public BlockControl blockctrl;
    private GameObject DesObject = null;
    CharacterController controller;

    private PhotonView pv = null;
    // Use this for initialization
    void Awake()
    {
        playerani = GetComponent<Animation>();
        controller = Body.GetComponent<CharacterController>();
        blockctrl = blockctrlbody.GetComponent<BlockControl>();
        pv = GetComponent<PhotonView>();

        pv.ObservedComponents[0] = this;

        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        playerstate = "idle";
        receivestate = playerstate;
    }
    // Update is called once per frame
    void Update()
    {
        if (pv.isMine)
        {
            if (statejudge())
            {
                motionani();
            }
            /*
            foreach (AnimationState anim in playerani)
            {
                if (playerani.IsPlaying(anim.name))
                {
                    playerstate = anim.name;
                }
            }
            */
        }
        else
        {
            playerani.CrossFade(receivestate);
        }
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(playerstate);
            stream.SendNext(DesObject);
        }
        else
        {
            receivestate = (string)stream.ReceiveNext();
            DesObject = (GameObject)stream.ReceiveNext();
        }
    }
    void motionani()
    {

        if (!Input.anyKey)
        {
            playerani.CrossFade("idle");
            playerstate = "idle";
        }
        if(Input.GetMouseButtonDown(0))
        {
            if (blockctrl.blocknum >= 1 &&!EventSystem.current.IsPointerOverGameObject())
            {
                playerani.CrossFade("Shooting");
                playerstate = "Shooting";
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            if (blockctrl.blocknum >= 3)
            {
                playerani.CrossFade("Shooting");
                playerstate = "Shooting";
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            playerani.CrossFade("left_strafe_inPlace");
            playerstate = "left_strafe_inPlace";
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerani.CrossFade("right_strafe_inPlace");
            playerstate = "right_strafe_inPlace";
        }
        else if(Input.GetKey(KeyCode.W))
        {
            playerani.CrossFade("running_inPlace");
            playerstate = "running_inPlace";
        }
        else if (Input.GetKey(KeyCode.S))
        {
            playerani.CrossFade("jog_backward_inPlace");
            playerstate = "jog_backward_inPlace";
        }
        /*if (!controller.isGrounded)
        {
            playerani.CrossFade("Idle");
            playerstate = "Idle";
        }*/
    }
    public bool statejudge()
    {
        if (!playerani.IsPlaying("stand_up")
            && !playerani.IsPlaying("standing")
            && !playerani.IsPlaying("standing_up")
            && !playerani.IsPlaying("Stand To Freehang")
            && !playerani.IsPlaying("knocked_down")
            && !playerani.IsPlaying("startjump")
            && !playerani.IsPlaying("jumping")
            && !playerani.IsPlaying("Shooting")
            && !playerani.IsPlaying("Shooting")
            && !playerani.IsPlaying("Standing React Large Gut")
            && !playerani.IsPlaying("Dying Backwards")
             && !Cursor.visible)

            return true;
        else
            return false;
    }
}