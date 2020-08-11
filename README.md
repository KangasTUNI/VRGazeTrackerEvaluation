# VRGazeTrackerEvaluation
Gaze tracker evaluation in VR devices

## The software
The software implements the VR based gaze tracker evaluation as described in our ICMI 2020 conference paper.
The software consists of two components, the first one collecting gaze data and the second one analyzing the collected data. 
The first components was coded in C# and can be run with any gaze-tracker enabled VR device that implements the Unity environment. 
The second component is more generic and was coded in Python (in Jupyter notebook).

The software is available 'as is' and can be freely used and modified.

### Gaze data collection

The gaze data collecting software has been developed in Unity environment and uses C# language.
The code is otherwise ready to be used, but the gaze tracker API varies between implementations.
Therefore, one is adviced to modify that part of the code, the code should be easy to understand and correct.

The software consisted of two phases.
First there was an initialization phase where the VR device was set up and the gaze tracker was calibrated.
In practice we made a call to the calibration routine provided by the gaze tracker manufacturer. 
(We relied on manufacturer components as often as possible.}
Next there was a measurement phase during which the participant was shown a ball moving from location to location in an otherwise empty VR environment.
The participant was asked to follow the target ball by his/her gaze.
The locations were fixed in the display coordinates and not in the VR environment, which meant that they followed possible head turns. 
I.e., the participant had to turn his/her eyes and not head to see different targets.

The gaze data was collected into log-files, one file for each collection session.
The log-files were self-contained, i.e. they had in them all the necessary information to compute the accuracy and precision.
For each target shown to the viewer in the collection session we first logged the position/direction of the target in the VR environment.
Then the gaze data, the gaze directions, were logged using a 60~Hz sampling rate.

### Gaze data analysis

The gaze data analysis is a set of functions 
