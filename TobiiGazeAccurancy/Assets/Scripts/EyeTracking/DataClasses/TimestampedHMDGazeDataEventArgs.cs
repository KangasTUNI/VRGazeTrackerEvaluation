using UnityEngine;
using System.Collections;
using Tobii.Research;

public class TimestampedHMDGazeDataEventArgs
{
	public long timestamp;
	public HMDGazeDataEventArgs data;

	public TimestampedHMDGazeDataEventArgs (long _timestamp, HMDGazeDataEventArgs _data) {
		timestamp = _timestamp;
		data = _data;
	}
}
