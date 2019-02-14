using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShootScript : NetworkBehaviour {

    public float fireRate = 1f;

    public float range = 100f;
    public LayerMask mask;

    private float fireFactor = 0f;
    private GameObject mainCamera;
    // Use this for initialization
    void Start() {
        mainCamera = GetComponentInChildren<Camera>().gameObject;
    }

    [Command]
    void Cmd_PlayerShot(string _id)
    {
        Debug.Log("Player " + _id + " has been shot!");
    }

    [Client]
    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(mainCamera.transform.position, transform.forward, out hit, range, mask))
        {
            if(hit.transform.tag == "Player")
            {
                Cmd_PlayerShot(hit.transform.name);
            }
        }
    }

    void HandleInput()
    {
        float fireInterval = 0;
        fireFactor += Time.deltaTime;
        fireInterval = 1 / fireRate;

        if(fireFactor >= fireInterval)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(isLocalPlayer)
        {
            HandleInput();
        }
	}
}
