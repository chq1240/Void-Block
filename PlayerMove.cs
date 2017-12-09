
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class PlayerMove : MonoBehaviour
{
    public GameObject box = null;
    public GameObject anibody;
public float speed = 6.0F;
public float jumpSpeed = 8.0F;
public float gravity = 20.0F;
private Vector3 moveDirection = Vector3.zero;
CharacterController controller;
Quaternion ed;
public Transform camPivot;
public Transform camDir;
public Transform forwardstep;
public Transform downstep;
    public PlayerAni playerani;
private Transform tr;

private float h, v;

private PhotonView pv = null;

private Vector3 currPos = Vector3.zero;
private Quaternion currRot = Quaternion.identity;

    void Awake()
{
    ed = new Quaternion(0, 0, 0, 0);
    tr = GetComponent<Transform>();
    pv = GetComponent<PhotonView>();

    pv.synchronization = ViewSynchronization.UnreliableOnChange;
    pv.ObservedComponents[0] = this;

    if (pv.isMine)
    {
        controller = GetComponent<CharacterController>();
        Camera.main.GetComponent<CameraFlow>().target = camPivot;
    }
        playerani = anibody.GetComponent<PlayerAni>();

    currPos = tr.position;
    currRot = tr.rotation;
}
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
    void Update()
{
        if (pv.isMine)
        {
            if (!Cursor.visible)
            {
                if (Input.anyKey)
                {
                    camDir = Camera.main.transform;
                    ed = camDir.rotation;
                    ed.x = 0;
                    ed.z = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, ed, Time.deltaTime * 5.0f);//ed;
                }

                if (controller.isGrounded)
                {
                    if (playerani.statejudge())
                    {
                        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                        moveDirection = transform.TransformDirection(moveDirection);
                        moveDirection *= speed;
                        if (Input.GetKeyDown(KeyCode.Space))
                            moveDirection.y = jumpSpeed;
                        moveDirection.y -= gravity * Time.deltaTime;
                        controller.Move(moveDirection * Time.deltaTime);
                    }
                }
                else
                {
                    moveDirection = new Vector3(Input.GetAxis("Horizontal") * speed, moveDirection.y, Input.GetAxis("Vertical") * speed);
                    moveDirection = transform.TransformDirection(moveDirection);
                    moveDirection.y -= gravity * Time.deltaTime;
                    controller.Move(moveDirection * Time.deltaTime);
                }
            }
        }
        else
        {
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
        }
}

}




