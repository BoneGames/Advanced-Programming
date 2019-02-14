using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class SyncTransform : NetworkBehaviour {

    public float lerpRate = 15;

    [SyncVar] private Vector3 syncPosition;
    [SyncVar] private Quaternion syncRotation;

    private Rigidbody rigid;

    public float positionThreshold = 0.5f;
    public float rotationThreshold = 5f;

    private Vector3 lastPosition;
    private Quaternion lastRotation;

    void Start () {
        rigid = GetComponent<Rigidbody>();
	}

    void LerpPosition()
    {
        rigid.position = Vector3.Lerp(rigid.position, syncPosition, Time.deltaTime * lerpRate);
    }
    void LerpRotation()
    {
        rigid.rotation = Quaternion.Lerp(rigid.rotation, syncRotation, Time.deltaTime * lerpRate);
    }

    [Command]
    void CmdSendPositionToServer(Vector3 _position)
    {
        syncPosition = _position;
        Debug.Log("Position Command");
    }

    [Command]
    void CmdSendRotationToServer(Quaternion _rotation)
    {
        syncRotation = _rotation;
        Debug.Log("Rotation Command");
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if(Vector3.Distance(rigid.position, lastPosition) > positionThreshold)
        {
            CmdSendPositionToServer(rigid.position);
            lastPosition = rigid.position;
        }
    }

    [ClientCallback]
    void TransmitRotation()
    {
        if(Quaternion.Angle(rigid.rotation, lastRotation) > rotationThreshold)
        {
            CmdSendRotationToServer(rigid.rotation);
            lastRotation = rigid.rotation;
        }
    }


    void FixedUpdate () {
        if(isLocalPlayer)
        {
            TransmitPosition();
            TransmitRotation();
        }
        else
        {
            LerpPosition();
            LerpRotation();
        }
	}
}


/*  case KeyCode.A:
               position -= transform.right * movementSpeed * Time.deltaTime;
               break;
           case KeyCode.D:
               position -= transform.right * movementSpeed * Time.deltaTime; ;
             break;*/
