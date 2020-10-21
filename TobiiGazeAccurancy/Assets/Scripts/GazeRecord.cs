using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeRecord : MonoBehaviour {
    
    [Tooltip("Tobii gaze tracker availability")]
    [SerializeField] private bool useTobii = false;
    //[SerializeField] private GameObject EyeTracker;
    [SerializeField] private EyeTrackerManager trackerManager;
    [SerializeField] private EyeVisalizer eyeVisualizer = null;

    private FileLogger fileLogger;

    // Use this for initialization
    void Start () {
        fileLogger = GameObject.FindObjectOfType<FileLogger>();

    }

    // Update is called once per frame
    void Update () {
        if (useTobii)
        {
            try
            {
                TimestampedHMDGazeDataEventArgs p = null;
                lock (trackerManager.GazeDataQueue)
                {
                    while (trackerManager.GazeDataQueue.Count > 0)
                    {
                        p = trackerManager.GazeDataQueue.Pop();
                    }
                }
                if (p != null)
                {
                    decodeTobii(p);
                    //Debug.Log("read datapoint with tobii, left eye validity"+p.data.LeftEye.Pupil.Validity);
                    //mousePainter.DrawRayHit(new Ray(HMD.transform.position, HMD.transform.forward), 1 / 40f);
                    if (eyeVisualizer != null)
                        eyeVisualizer.setEyes(p);
                }
                else
                    fileLogger.printProgress("NoGazeData\tTobii");
            }
            catch (Exception e)
            {
                Debug.LogError("useTobii failed: " + e.Message);
                fileLogger.printProgress("UseTobiiFailed\t" + e.Message);
            }
        }
        
    }

    private void decodeTobii(TimestampedHMDGazeDataEventArgs gaze = null)
    {

        if (gaze != null)
        {
            fileLogger.printProgress("Left_Gaze_UnitVector\t" + -gaze.data.LeftEye.GazeDirection.UnitVector.X + "\t" 
                + gaze.data.LeftEye.GazeDirection.UnitVector.Y + "\t" 
                + gaze.data.LeftEye.GazeDirection.UnitVector.Z + "\t"
                + gaze.data.LeftEye.GazeDirection.Validity);
            fileLogger.printProgress("Left_Origin\t" + -gaze.data.LeftEye.GazeOrigin.PositionInHMDCoordinates.X * 0.001f + "\t" 
                + gaze.data.LeftEye.GazeOrigin.PositionInHMDCoordinates.Y * 0.001f + "\t" 
                + gaze.data.LeftEye.GazeOrigin.PositionInHMDCoordinates.Z * 0.001f + "\t" 
                + gaze.data.LeftEye.GazeOrigin.Validity);
            fileLogger.printProgress("Left_Pupil\t" + gaze.data.LeftEye.Pupil.PupilDiameter + "\t" 
                + gaze.data.LeftEye.Pupil.Validity + "\t"
                + gaze.data.LeftEye.PupilPosition.PositionInTrackingArea.X + "\t"
                + gaze.data.LeftEye.PupilPosition.PositionInTrackingArea.Y + "\t"
                + gaze.data.LeftEye.PupilPosition.Validity);

            fileLogger.printProgress("Right_Gaze_UnitVector\t" + -gaze.data.RightEye.GazeDirection.UnitVector.X + "\t"
                + gaze.data.RightEye.GazeDirection.UnitVector.Y + "\t" 
                + gaze.data.RightEye.GazeDirection.UnitVector.Z + "\t"
                + gaze.data.RightEye.GazeDirection.Validity);
            fileLogger.printProgress("Right_Origin\t" + -gaze.data.RightEye.GazeOrigin.PositionInHMDCoordinates.X * 0.001f + "\t"
                + gaze.data.RightEye.GazeOrigin.PositionInHMDCoordinates.Y * 0.001f + "\t" 
                + gaze.data.RightEye.GazeOrigin.PositionInHMDCoordinates.Z * 0.001f + "\t" 
                + gaze.data.RightEye.GazeOrigin.Validity);
            fileLogger.printProgress("Right_Pupil\t" + gaze.data.RightEye.Pupil.PupilDiameter + "\t"
               + gaze.data.RightEye.Pupil.Validity + "\t"
               + gaze.data.RightEye.PupilPosition.PositionInTrackingArea.X + "\t"
               + gaze.data.RightEye.PupilPosition.PositionInTrackingArea.Y + "\t"
               + gaze.data.RightEye.PupilPosition.Validity);
           
        }
        else
        {
            fileLogger.printProgress("No_gaze_data_received");
        }
    }

}
