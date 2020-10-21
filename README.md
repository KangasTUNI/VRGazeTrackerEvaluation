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
(During the development we were using the Unity version 2018.2.8f1.)

The software consists of two phases.
First there is an initialization phase where the VR device is set up and the gaze tracker is calibrated.
In practice we make a call to the calibration routine provided by the gaze tracker manufacturer. 
(We rely on manufacturer components as often as possible.}
Next there is a measurement phase during which the viewer is shown a ball moving from location to location in an otherwise empty VR environment.
The participant is asked to follow the target ball by his/her gaze.
The locations are fixed in the display coordinates and not in the VR environment, which means that they follow possible head turns. 
I.e., the participant has to turn his/her eyes and not head to see different targets.

The gaze data is collected into log-files, one file for each collection session.
The log-files are self-contained, i.e. they have in them all the necessary information to compute the accuracy and precision.
For each target shown to the viewer in the collection session we first log the position/direction of the target in the VR environment.
Then the gaze data, the gaze directions, are logged using a 60~Hz sampling rate.

### Gaze data analysis

The gaze data analysis software is a set of Python functions in a Jupyter Notebook environment.
The functions read the data from a set of files, does some data cleaning and then create a number of visualizations and compute quantitative results.
The functions can be easily modified and new functions can be implemented by extending the Notebook.

### Logfile folder

The logfiles are usually stored in a separate folder.
The folder name and a list of logfiles are written in the Jupyter Notebook and the reading routines go through the data.
We have included three example logfiles in the repository.


