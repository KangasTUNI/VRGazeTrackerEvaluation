using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeVisalizer : MonoBehaviour
{
    [SerializeField] private GameObject leftEye, rightEye;
    [SerializeField] private GameObject leftEyeTarget, rightEyeTarget;

    private FileLogger fileLogger;

    // Use this for initialization
    void Start()
    {
        fileLogger = GameObject.FindObjectOfType<FileLogger>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void calculateHit(Transform eyeTr, string eyeName, GameObject eyeTarget = null)
    {
        Ray raycast = new Ray(eyeTr.position, eyeTr.forward);
        int layerMask = 1 << 8;
        RaycastHit hit;
        bool bHit = Physics.Raycast(raycast, out hit, Mathf.Infinity, layerMask);
        if (!bHit)
        {
            Debug.Log("NoHit");   
        }
        else
        {
            //Debug.Log("Osui ja upposi " + hit.collider.name + ", pos=" + hit.point + ", " + hit.textureCoord);
            eyeTarget.transform.position = hit.point;
            fileLogger.printProgress("GazeTarget" + eyeName + "\t" + eyeTarget.transform.localPosition.x * 1000f + "\t" + eyeTarget.transform.localPosition.y * 1000f + "\t" + eyeTarget.transform.localPosition.z * 1000f + "\t");
            //Debug.Log("paikka " + eyeTarget.transform.localPosition);
        }
    }


    private void setOneEye(Tobii.Research.HMDEyeData eyeData, GameObject eye, string eyeName, GameObject target = null)
    {
        //Debug.Log("SetOneEye "+eye);
        if (eyeData.GazeOrigin.Validity == Tobii.Research.Validity.Valid &&
            eyeData.GazeDirection.Validity == Tobii.Research.Validity.Valid &&
            eye != null)
        {
            Vector3 origin = new Vector3(-eyeData.GazeOrigin.PositionInHMDCoordinates.X * 0.001f,
                    eyeData.GazeOrigin.PositionInHMDCoordinates.Y * 0.001f,
                    eyeData.GazeOrigin.PositionInHMDCoordinates.Z * 0.001f);
            Vector3 gaze = new Vector3(-eyeData.GazeDirection.UnitVector.X,
                    eyeData.GazeDirection.UnitVector.Y,
                    eyeData.GazeDirection.UnitVector.Z);
            eye.transform.localPosition = origin;
            eye.transform.localRotation = Quaternion.LookRotation(gaze, Vector3.up);
            calculateHit(eye.transform, eyeName, target);
        }
    }

    public void setEyes(TimestampedHMDGazeDataEventArgs gaze = null)
    {
        setOneEye(gaze.data.LeftEye, leftEye, "Left", leftEyeTarget);
        setOneEye(gaze.data.RightEye, rightEye, "Right", rightEyeTarget);
    }
}