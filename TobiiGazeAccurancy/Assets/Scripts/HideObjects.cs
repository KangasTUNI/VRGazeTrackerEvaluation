using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjects : MonoBehaviour {
    [Tooltip("Define key for the hide action")]
    [SerializeField] private KeyCode keyPressed = KeyCode.Space;
    [SerializeField] private bool hidden = false;
    [SerializeField] private MeshRenderer[] hiddenObjects;
    
    // Use this for initialization
    void Start ()
    {
        foreach (MeshRenderer mr in hiddenObjects)
        {
            mr.enabled = !hidden;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(keyPressed))
        {
            hidden = !hidden;
            GameObject.FindObjectOfType<FileLogger>().printProgress("Hide_key_pressed\t" + keyPressed.ToString() + "\t" + (hidden ? "hide":"show"));
            foreach (MeshRenderer mr in hiddenObjects)
            {
                mr.enabled = !hidden;
            }
        }
    }
}
