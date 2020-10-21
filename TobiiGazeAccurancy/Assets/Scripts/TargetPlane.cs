using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlane : MonoBehaviour {
    [SerializeField] private GameObject targetPlane, camera;
    [SerializeField] private Vector3 worldUp = Vector3.up;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        targetPlane.transform.LookAt(camera.transform, worldUp);
	}
}
