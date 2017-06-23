using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Grab : MonoBehaviour {
    
	bool pointer;
	bool mouseDown;
    public bool busy;
	int reticalDistance;
	public int holdDistance;

	GameObject head;
    GameObject body;
	Collider dummy1;
	Collider dummy2;
    Interact interaction;
    Collider activeObject;

    RaycastHit hit;
    public ObjectManager objectManager;
    public List<Collider> isActive;

    void Start() {
        
        holdDistance = 2;
		reticalDistance = 2;

        head = GameObject.Find("Main Camera");
        body = GameObject.Find("User");
        dummy1 = new Collider();
        dummy2 = new Collider();
        isActive = new List<Collider> { dummy1, dummy2 };
        interaction = GameObject.Find("Master").GetComponent<Interact>();
    }

    void FixedUpdate() {

        pointer = Physics.Raycast(body.transform.position, head.transform.forward, out hit);
        activeObject = hit.collider;

        if (mouseDown) {
            
            if (activeObject != isActive[1]) {
                isActive.RemoveAt(0);
                isActive.Add(activeObject);
            }

            ButtonCheck(isActive[1]);
            CarryOver(isActive[0]);
            PickUp(isActive[1]);
        }

        else {
            Drop();

			isActive.RemoveAt(0);
			isActive.Add(activeObject);
        }

    }

    void Update() {

        this.transform.position = body.transform.position + (head.transform.forward / reticalDistance);
        mouseDown = Input.GetMouseButton(0);
    }

    void ButtonCheck(Collider _gameObject) {
	    if (_gameObject != null) {
         
            if (_gameObject.tag == "ExplodeButton") {
            
                Image buttonColour = _gameObject.gameObject.GetComponentInChildren<Image>();
                if (!(busy)) {
					StartCoroutine(ExplodeButton(buttonColour));
				}
            }

			if (_gameObject.tag == "SizeButton") {

				Image buttonColour = _gameObject.gameObject.GetComponentInChildren<Image>();
				if (!(busy))
				{
					StartCoroutine(SizeButton(buttonColour));
				}
	        }

            if (_gameObject.tag == "ResetButton") {

				Image buttonColour = _gameObject.gameObject.GetComponentInChildren<Image>();
				if (!(busy))
				{
					StartCoroutine(ResetButton(buttonColour));
				}
            }
        }
	}

    void CarryOver(Collider _carryObject) {

        if (_carryObject != null && mouseDown) {
            
            if (_carryObject.tag == "InteractiveEnvironment") {
                
                if (_carryObject.GetComponent<ObjectManager>() != null) {
			
				    objectManager = _carryObject.GetComponent<ObjectManager>();
				    if (objectManager.currentGrabState == ObjectManager.GrabState.grabable
                        && objectManager.currentMotionState == ObjectManager.MotionState.moving) {

					isActive[1] = _carryObject;
				    }
			    }
		    }
        }
    }

    void PickUp(Collider _pickObject) {

		if (_pickObject != null) {
            
			if (_pickObject.tag == "InteractiveEnvironment") {
                
				if (_pickObject.GetComponent<ObjectManager>() != null) {
                    
					objectManager = _pickObject.GetComponent<ObjectManager>();
					if (objectManager.currentGrabState == ObjectManager.GrabState.grabable) {
                        
						if (_pickObject.attachedRigidbody != null) {
                            
							_pickObject.attachedRigidbody.isKinematic = true;
							_pickObject.attachedRigidbody.useGravity = false;

							if ((_pickObject.gameObject.transform.position - (body.transform.position + head.transform.forward)).magnitude < 0.9) {
								
                                _pickObject.attachedRigidbody.MovePosition(body.transform.position + head.transform.forward);
				    		}

                            else {
                        
					            _pickObject.attachedRigidbody.MovePosition(body.transform.position + head.transform.forward * holdDistance);
							}

							objectManager.currentMotionState = ObjectManager.MotionState.moving;
						}
					}

					else {
						_pickObject.attachedRigidbody.isKinematic = false;
						_pickObject.attachedRigidbody.useGravity = true;
						objectManager.currentMotionState = ObjectManager.MotionState.still;

						isActive[0] = dummy1;
						isActive[1] = dummy2;
					}
				}
		    }
		}
    }

    void Drop() {
        
        foreach (Collider col in isActive) {

            if (col != null) {
                    
			    if (col.tag == "InteractiveEnvironment") {
                        
				        if (col.GetComponent<ObjectManager>() != null) {
                            
				        	objectManager = col.GetComponent<ObjectManager>();
					        objectManager.currentMotionState = ObjectManager.MotionState.still;
					        objectManager.currentGrabState = ObjectManager.GrabState.grabable;
					        objectManager.colliding = false;
				        }

				        if (col.attachedRigidbody != null) {
                            
					        col.attachedRigidbody.isKinematic = false;
					        col.attachedRigidbody.useGravity = true;
                        }
                }
            }
        }
    }

    IEnumerator ExplodeButton(Image clickedButton) {
		busy = true;

        clickedButton.color = new Color32(0, 255, 0, 100);
        interaction.ExplodedView();
        yield return new WaitForSeconds(10.0f);
		clickedButton.color = new Color32(255, 255, 255, 100);


		busy = false;
		yield return null;
    }

	IEnumerator SizeButton(Image clickedButton)
	{
		busy = true;

		clickedButton.color = new Color32(0, 255, 0, 100);
		interaction.IncreaseSize();
		yield return new WaitForSeconds(0.5f);
		clickedButton.color = new Color32(255, 255, 255, 100);


		busy = false;
		yield return null;
	}

	IEnumerator ResetButton(Image clickedButton)
	{
		busy = true;

		clickedButton.color = new Color32(0, 255, 0, 100);
		interaction.ResetEnvironment();
		yield return new WaitForSeconds(0.5f);
		clickedButton.color = new Color32(255, 255, 255, 100);


		busy = false;
		yield return null;
	}

}
