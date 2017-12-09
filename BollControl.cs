using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BollControl : MonoBehaviour {

    private Transform tr;
    private PhotonView pv = null;
    private Quaternion currRot = Quaternion.identity;

    // Use this for initialization
    void Awake()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        pv.ObservedComponents[0] = this;

        pv.synchronization = ViewSynchronization.UnreliableOnChange;

        currRot = tr.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.isMine)
        {
            tr.rotation = Camera.main.transform.rotation;
        }
        else
        {
            tr.localRotation = Quaternion.Slerp(tr.localRotation, currRot, Time.deltaTime * 10.0f);
        }
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(tr.localRotation);
        }
        else
        {
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
