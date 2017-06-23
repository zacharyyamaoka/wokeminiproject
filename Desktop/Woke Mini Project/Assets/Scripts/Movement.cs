using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public int speed;
	public int thrust;

	Camera lineOfSight;
    Rigidbody body;
    Vector3 forwardDirectionAngle;
    Vector3 forwardDirectionFlat;
    Vector3 leftDirectionFlat;
    Vector3 rightDirectionFlat;
    Vector3 backDirectionFlat;
    Vector3 upDirectionFlat;

    void Start() {
        
		speed = 1;

        lineOfSight = GetComponentInChildren<Camera>();
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        
        MoveWithArrows();
    }

    void MoveWithArrows() {
        
		forwardDirectionAngle = lineOfSight.transform.forward;
		forwardDirectionFlat = Vector3.ProjectOnPlane(forwardDirectionAngle, Vector3.up).normalized;
		leftDirectionFlat = Quaternion.Euler(0, -90, 0) * forwardDirectionFlat;
		rightDirectionFlat = Quaternion.Euler(0, 90, 0) * forwardDirectionFlat;
		backDirectionFlat = Vector3.RotateTowards(forwardDirectionFlat, -forwardDirectionFlat, 3.14f, 0).normalized;
		upDirectionFlat = new Vector3(0, 1, 0);

        if (Input.GetKey(KeyCode.A)) {
            
            body.MovePosition(this.transform.position + leftDirectionFlat * speed * Time.fixedDeltaTime);
        }

        if(Input.GetKey(KeyCode.D)) {
            
            body.MovePosition(this.transform.position + rightDirectionFlat * speed * Time.fixedDeltaTime);
        }

        if(Input.GetKey(KeyCode.S)) { 
            
            body.MovePosition(this.transform.position + backDirectionFlat * speed * Time.fixedDeltaTime);
        }

        if(Input.GetKey(KeyCode.W)) { 
            
            body.MovePosition(this.transform.position + forwardDirectionFlat * speed * Time.fixedDeltaTime);
        }
        /* if(Input.GetKeyDown(KeyCode.Space)) {
            body.AddForce(new Vector3(0, 1, 0) * thrust, ForceMode.VelocityChange);
            //body.MovePosition(this.transform.position + upDirectionFlat * speed * Time.fixedDeltaTime);
        } */
    }

    /*void MoveDebug() {
		Debug.DrawRay(transform.position, forwardDirectionFlat);
		Debug.DrawRay(transform.position, leftDirectionFlat, Color.red);
		Debug.DrawRay(transform.position, rightDirectionFlat, Color.red);
		Debug.DrawRay(transform.position, backDirectionFlat);
		Debug.DrawRay(transform.position, upDirectionFlat);
    }*/
}