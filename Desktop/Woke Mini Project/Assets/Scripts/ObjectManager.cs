using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    public enum MotionState {
        still,
        moving
    }


    public enum GrabState {
        grabable,
        notGrabable
    }

    public MotionState currentMotionState;
    public GrabState currentGrabState;

    public bool colliding;

    void Awake() {

        currentMotionState = MotionState.still;
        currentGrabState = GrabState.grabable;
    }

    void FixedUpdate() {

        if (colliding && (currentMotionState == MotionState.moving)) {
            
           currentGrabState = GrabState.notGrabable;
        }
    }

    void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag != "InteractiveEnvironment") {
            
            if ((currentMotionState == MotionState.still)) {
                
                colliding = false;
            }
        }

        if ((currentMotionState == MotionState.moving)) {
            
            colliding = true;
        }
    }

    void OnTriggerStay(Collider other) {

        if ((currentMotionState == MotionState.moving)) {

            colliding = true;
            currentGrabState = GrabState.notGrabable;
        }
    }
}



