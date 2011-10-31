using UnityEngine;
using System.Collections;
using System.IO;

[RequireComponent(typeof(KinectWrapper))]
public class KinectDataLogger : MonoBehaviour {
	
	public string output = "Log.csv";
	private KinectWrapper kinectWrapper;
	private bool _logging = false;
	private StreamWriter _outputFile;
	private string _currentData = "";
	
	private int _numPoses = 0;
	
	// Use this for initialization
	void Start () {
		kinectWrapper = GetComponent<KinectWrapper>();
		_outputFile = new StreamWriter(@output);
	}
	
	// Update is called once per frame
	void Update () {
		kinectWrapper.pollKinect();
		if(!_logging)
		{
			//if the Log button was pushed, start logging
			if(Input.GetButtonDown("Log"))
			{
				_logging = true;
				Debug.Log("start logging");
			}
		}
		else
		{
			//if the Log button was pushed, output the current data and stop logging
			if(Input.GetButtonDown("Log"))
			{
				_logging = false;
				_outputFile.WriteLine(_currentData);
				_outputFile.Flush();
				Debug.Log("Wrote line " + _currentData);
				_currentData = "";
				return;
			}
			//if there's new data, add it to current data
			if(kinectWrapper.newData)
			{
				//Put the values you want to log here.
				_currentData += kinectWrapper.BoneVel[0,(int)BoneIndex.Hip_Center].x + ",";
			}
		}
		//if the SavePose button was pushed, save all 20 bone positions to a file
		if(Input.GetButtonDown("SavePose"))
		{
			//open a new file for the pose
			StreamWriter poseFile = new StreamWriter(@"Assets\KinectWrapper\Poses\Pose_" + _numPoses + ".pose");
			_numPoses++;
			//write the position of each bone (of player 0)
			for(int ii = 0; ii < (int)BoneIndex.Num_Bones; ii++)
			{
				poseFile.WriteLine(""+kinectWrapper.BonePos[0,ii].x+","+
				                   kinectWrapper.BonePos[0,ii].y+","+
				                   kinectWrapper.BonePos[0,ii].z);
			}
			poseFile.Flush();
			poseFile.Close();
			Debug.Log("saved pose " + _numPoses);
		}
	}
	
	void OnDestroy () {
		if(_outputFile != null)
		{
			_outputFile.Close();
		}
	}
}
