using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LookScriptTrash : NetworkBehaviour {

    public float mouseSensitivity = 2f;

    public float minimumY = -90f;
    public float maximumY = 90f;

    private float yaw = 0;
    private float pitch = 0;
    private GameObject mainCamera;

    float rotationY;

	void Start () {
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

        Camera cam = GetComponentInChildren<Camera>();
        if (cam)
        {
            mainCamera = cam.gameObject;
        }
	}

    void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HandleInput()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        float mouseX = Input.GetAxis("Mouse X");
        Debug.Log("Y: " + mouseY + ", X: " + mouseX);


        yaw = rotationY + mouseX * mouseSensitivity;
        pitch += mouseY * mouseSensitivity;
        Mathf.Clamp(pitch, minimumY, maximumY);
        rotationY = yaw;

        
    }

    // Update is called once per frame
    void Update () {
		if(isLocalPlayer)
        {
            HandleInput();
        }
	}
}
