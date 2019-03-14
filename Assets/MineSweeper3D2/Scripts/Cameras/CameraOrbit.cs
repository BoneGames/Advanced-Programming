using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour {

    public float zoomSpeed = 5f;
    public float xSpeed = 120f, ySpeed = 120f;
    public float yMin = -80, yMax = 80;
    public float distanceMin = 10f, distanceMax = 15f;
    private float x = 0f, y = 0f;
    private float distance;

	void Start () {
        // Furthest distance at start
        distance = distanceMax;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		// Check if right mouse is pressed
        if(Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            x += mouseX * xSpeed * Time.deltaTime;
            y -= mouseY * ySpeed * Time.deltaTime;
            y = Mathf.Clamp(y, yMin, yMax);
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            distance -= scrollWheel * zoomSpeed;
            distance = Mathf.Clamp(distance, distanceMin, distanceMax);
        }

        

        // Update transform
        transform.rotation = Quaternion.Euler(y, x, 0);
        transform.position = -transform.forward * distance;
	}
}
