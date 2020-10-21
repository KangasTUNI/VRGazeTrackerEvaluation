using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Tobii.Research;

public class EyeTrackerManager : MonoBehaviour {
	public EyeTrackerManager instance;
	public IEyeTracker eyeTracker;
	//private EyeTrackerCalibrationManager calibrationManager;
	public bool logGazeData;
	private GameObject HMD;
	public Stack<TimestampedHMDGazeDataEventArgs> GazeDataQueue;
	private bool skip = false;
    public bool startImmediately = true;
    private bool loggingStarted = false;

	// Use this for initialization
	void Awake () {
		if (instance != null && instance != this)
		{
				DestroyImmediate(this.gameObject);
				return;
		}
		else {
				instance = this;
		}

		eyeTracker = EyeTrackingOperations.FindAllEyeTrackers()[0];
		Debug.Log("Connected eye tracker: " + eyeTracker.Model);
		//calibrationManager = GetComponent<EyeTrackerCalibrationManager>();
		//calibrationManager.SetEyeTracker(eyeTracker);

		GazeDataQueue = new Stack<TimestampedHMDGazeDataEventArgs>();
	}

	void Start() {
		HMD = transform.parent.gameObject;
		Debug.Log("EyeTrackerManager.HMD: " + HMD);
        if (startImmediately)
        {
            StartLogging();
        }
	}

	// Update is called once per frame
	void Update () {

		if(eyeTracker == null)
		{
            Debug.Log("ERROR: " + "Eyetracker lost, trying to find it again!");
			eyeTracker = EyeTrackingOperations.FindAllEyeTrackers()[0];
		}

        /*while(GazeDataQueue.Count >  0) {
			LogData(GazeDataQueue.Dequeue());
		}*/

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!loggingStarted)
            {
                StartLogging();
                loggingStarted = true;
            }
            else
            {
                StopLogging();
                loggingStarted = false;
            }

        }

    }

	void OnApplicationQuit()
	{
		StopLogging();
		EyeTrackingOperations.Terminate();
	}

	public void StartLogging () {
		eyeTracker.HMDGazeDataReceived += EyeTracker_LOGHMDGazeData;
	}

	public void StopLogging () {
		eyeTracker.HMDGazeDataReceived -= EyeTracker_LOGHMDGazeData;
	}

	public void QueueGazeData(HMDGazeDataEventArgs data) {
        if (skip)
        {
            lock (GazeDataQueue)
            {
                GazeDataQueue.Push(new TimestampedHMDGazeDataEventArgs(Utils.GetCurrentUnixTimestampMillis(), data));
                //Debug.Log(data.LeftEye.GazeOrigin.PositionInHMDCoordinates.X + "x");
                //Debug.Log(data.LeftEye.GazeDirection.UnitVector.X + "x dir");
                skip = false;
            }
        }
        else
        {
            skip = true;
        }
        //Debug.Log("Queued gazedata");
	}

	private void EyeTracker_LOGHMDGazeData(object sender, HMDGazeDataEventArgs e)
	{
		QueueGazeData(e);
	}

	private void LogData(TimestampedHMDGazeDataEventArgs timestampedData) {
		HMDGazeDataEventArgs e = timestampedData.data;
		string [] data = {
			e.SystemTimeStamp.ToString(), // TIMESTAMPS
			e.DeviceTimeStamp.ToString(),
			HMD.transform.rotation.eulerAngles.x.ToString(), // HMD
			HMD.transform.rotation.eulerAngles.y.ToString(),
			HMD.transform.rotation.eulerAngles.z.ToString(),
			HMD.transform.localPosition.x.ToString(), // HMD
			HMD.transform.localPosition.y.ToString(),
			HMD.transform.localPosition.z.ToString(),
			e.LeftEye.GazeDirection.UnitVector.X.ToString(), // LEFT EYE
			e.LeftEye.GazeDirection.UnitVector.Y.ToString(),
			e.LeftEye.GazeDirection.UnitVector.Z.ToString(),
			e.LeftEye.GazeDirection.Validity.ToString(),
			e.LeftEye.GazeOrigin.PositionInHMDCoordinates.X.ToString(),
			e.LeftEye.GazeOrigin.PositionInHMDCoordinates.Y.ToString(),
			e.LeftEye.GazeOrigin.PositionInHMDCoordinates.Z.ToString(),
			e.LeftEye.GazeOrigin.Validity.ToString(),
			e.LeftEye.Pupil.PupilDiameter.ToString(),
			e.LeftEye.Pupil.Validity.ToString(),
			e.RightEye.GazeDirection.UnitVector.X.ToString(), // RIGHT EYE
			e.RightEye.GazeDirection.UnitVector.Y.ToString(),
			e.RightEye.GazeDirection.UnitVector.Z.ToString(),
			e.RightEye.GazeDirection.Validity.ToString(),
			e.RightEye.GazeOrigin.PositionInHMDCoordinates.X.ToString(),
			e.RightEye.GazeOrigin.PositionInHMDCoordinates.Y.ToString(),
			e.RightEye.GazeOrigin.PositionInHMDCoordinates.Z.ToString(),
			e.RightEye.GazeOrigin.Validity.ToString(),
			e.RightEye.Pupil.PupilDiameter.ToString(),
			e.RightEye.Pupil.Validity.ToString()
		};
            
			//Debug.Log("GAZE", data, timestampedData.timestamp);
		 }
	}
