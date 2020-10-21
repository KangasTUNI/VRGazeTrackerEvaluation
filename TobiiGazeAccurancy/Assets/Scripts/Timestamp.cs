using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timestamp : MonoBehaviour {

    [Tooltip("Define key for the action")]
    [SerializeField] private KeyCode keyPressed = KeyCode.Space;
  
    
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(keyPressed))
        {
            GameObject.FindObjectOfType<FileLogger>().printProgress("Timestamp_by_user\t" + keyPressed);
        }
    }
}
