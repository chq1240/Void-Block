
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CameraFlow : MonoBehaviour
{
    public Transform target;
    //防止镜头卡死,在使用前把镜头放在合适位置
    Vector3 CameraDis;
    //Vector3 Oripos;
    // 初始化
    void Start()
    {
        CameraDis = transform.position - target.position;
    }
    
    void LateUpdate()
    {
        if (!target)
            return;
        Scale();
        transform.position = target.position + CameraDis;
        transform.LookAt(target);
        Rotate();
    }
    //缩放
    private void Scale()
    {
        float Scaledis = CameraDis.magnitude;
        Scaledis -= Input.GetAxis("Mouse ScrollWheel") * 5;
        //限制缩放
        if ((Scaledis <= 3))
        {
            Scaledis = 3;
        }
        else if(Scaledis >= 7)
            Scaledis = 7;
        CameraDis = CameraDis.normalized * Scaledis;
    }
    //摄像头环绕
    private void Rotate()
    {

        //对旋转的角度加以限制
        float t = Input.GetAxis("Mouse Y") * -1 / 5;
        transform.RotateAround(target.position, Vector3.up, Input.GetAxis("Mouse X") * 15);
       
        CameraDis = transform.position - target.position;
        CameraDis.y += t;
        if(CameraDis.y>=5)
        {
            CameraDis.y = 5;
        }
        else if(CameraDis.y < -2)
        {
            CameraDis.y = -2;
        }
    }
}