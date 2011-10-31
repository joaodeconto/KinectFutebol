using UnityEngine;
using System.Collections;
using System.IO;

public class KinectEmulator : MonoBehaviour {
	
	public int numPoses = 3;
	private Vector4[,] _savedPoses;
	private int _curPose = 0;
	
	// Use this for initialization
	void Start () {
		//create space for each pose
		_savedPoses = new Vector4[numPoses,(int)BoneIndex.Num_Bones];
		//read in each pose file
		for(int ii = 0; ii < numPoses; ii++)
		{
			StreamReader poseFile = new StreamReader(@"Assets\KinectWrapper\Poses\Pose_" + ii + ".pose");
			for(int jj = 0; jj < (int)BoneIndex.Num_Bones; jj++)
			{
				//extract position data from file
				string boneData = poseFile.ReadLine();
				string[] data = boneData.Split(',');
				_savedPoses[ii,jj].x = float.Parse(data[0]);
				_savedPoses[ii,jj].y = float.Parse(data[1]);
				_savedPoses[ii,jj].z = float.Parse(data[2]);
				_savedPoses[ii,jj].w = 1;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		for(int ii = 0; ii < numPoses; ii++)
		{
			if(Input.GetKeyDown(""+ (ii + 1 % 10)))
			{
				_curPose = ii;
			}
		}
	}
	
	public Vector4 getBonePos(int bone)
	{
		return _savedPoses[_curPose,bone];
	}
}
