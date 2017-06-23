using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour {
    
    int sensitivity;
    float cursorPosX;
    float cursorPosY;

	GameObject head;
	GameObject body;

	void Start() {

		sensitivity = 1;

		body = this.gameObject;
		head = GameObject.Find("Main Camera");
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {

		cursorPosX = Input.GetAxis("Mouse X") * sensitivity;
		cursorPosY = Input.GetAxis("Mouse Y") * sensitivity;
    }

    void FixedUpdate() {

        Look();
    }

	void Look() {
        
		body.transform.localRotation = Quaternion.Euler(0, cursorPosX, 0) * body.transform.localRotation;
		head.transform.localRotation = Quaternion.Euler(-cursorPosY, 0, 0) * head.transform.localRotation;

        // Debug.DrawRay(body.transform.position, head.transform.forward, Color.red, 0);
	}
}