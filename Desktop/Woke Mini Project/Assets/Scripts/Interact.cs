using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {

    public bool size;
    public bool explode;
    public bool busy;
    public bool loaded;

    Vector3[] translations;
    Vector3[] setups;
    GameObject copyObject;
    public GameObject FitnessBand;
    public GameObject ModelCube;
    List<Transform> objectComponents;
    public List<GameObject> toBeDeleted;


	void Start() {

        translations = new Vector3[] { new Vector3(0.1f, 0, 0), new Vector3(-0.1f, 0, 0),
            new Vector3(0, 0.1f, 0), new Vector3(0, 0, 0.1f), new Vector3(0, 0, -0.1f),
            new Vector3(0.1f, 0.1f, 0.1f), new Vector3(-0.1f, 0.1f, 0.1f), new Vector3(0.1f, 0.1f, -0.1f), new Vector3(-0.1f, 0.1f, -0.1f) };
        setups = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0.1f), new Vector3(0, 0, -0.1f) };
        busy = false;
        loaded = false;
        toBeDeleted = new List<GameObject> { GameObject.Find("FitnessBand"), GameObject.Find("ModelCube"), GameObject.Find("ModelCube (1)")  };
    }

    void OnTriggerEnter(Collider other) {

		if (other.tag == "InteractiveEnvironment") {
            
            copyObject = Instantiate(other.gameObject, new Vector3(-6, 1, 0), Quaternion.Euler(0, 0, 180));
            copyObject.tag = "CopyInteractiveEnvironment";
            toBeDeleted.Add(copyObject);
            Rigidbody rbcopyObject = copyObject.GetComponent<Rigidbody>();
			rbcopyObject.isKinematic = true;
            rbcopyObject.useGravity = false;
			
			copyObject.transform.localScale += new Vector3(4.0f, 4.0f, 4.0f);

			objectComponents = new List<Transform>();

            foreach (Transform component in copyObject.GetComponentsInChildren<Transform>()) {
                
                objectComponents.Add(component);
             }

            loaded = true;
        }
    }

    void OnTriggerExit(Collider other) {

        //ExplodedView();
        //Destroy(copyObject);
    }

	public void IncreaseSize() {

        if (loaded) {
            
			copyObject.transform.localScale += new Vector3(2.0f, 2.0f, 2.0f);
		}
	}

    public void ResetEnvironment() {

      foreach (GameObject _gameObject in toBeDeleted) {
            
            Destroy(_gameObject);
        }

        Instantiate(FitnessBand, new Vector3(2.35f, 1.39f, 0.19f), new Quaternion());
		Instantiate(ModelCube);
		Instantiate(ModelCube);
	}

    public void ExplodedView() {

        if (!(busy) && (loaded)) {

            float count = 0.0f;
            float slicer = objectComponents.Count;
        
            foreach (Transform i in objectComponents) {
                
                ++count;
                int randomintSix = Random.Range(0, 8);
                int randomintThree = Random.Range(0, 3);
                float bias = 15 * (count / slicer) + 15;
                Vector3 translateSix = translations[randomintSix] * 3 * bias;
                Vector3 translateThree = setups[randomintThree] * bias;
     
                StartCoroutine(SlowMoveExplode(i.position, i.position + translateSix, new Vector3(-5, 2, 0) + translateThree,  i));
		    }
		}

	}


    IEnumerator SlowMoveExplode(Vector3 start, Vector3 end, Vector3 secondEnd, Transform child) {
        //Vector3 start, Vector3 end

        busy = true;
        yield return new WaitForSeconds(0.1f);
        float moveTime = 5.0f;
        float passedTime = 0.0f;

        while (passedTime / moveTime <= 1) {

            child.position = Vector3.Lerp(start, end, (passedTime / moveTime));
            passedTime += Time.deltaTime;
            print(passedTime / moveTime);
            yield return new WaitForSeconds(0.01f);
        }


        yield return new WaitForSeconds(0.5f);

        moveTime = 5.0f;
        passedTime = 0.0f;

		while (passedTime / moveTime <= 1) {

			child.position = Vector3.Lerp(child.position, secondEnd, (passedTime / moveTime));
			passedTime += Time.deltaTime;
			print(passedTime / moveTime);
            yield return new WaitForSeconds(0.001f);
		}

		yield return new WaitForSeconds(0.0001f);

		moveTime = 5.0f;
		passedTime = 0.0f;

		while (passedTime / moveTime <= 1)
		{

			child.position = Vector3.Lerp(child.position, start, (passedTime / moveTime));
			passedTime += Time.deltaTime;
			print(passedTime / moveTime);
			yield return new WaitForSeconds(0.01f);
		}

        busy = false;
		yield return null;
    }


}
