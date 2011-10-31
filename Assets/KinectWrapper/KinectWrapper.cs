/*
 * KinectWrapper.cs - Handles the connection to the mircosoft kinect sdk through
 * 			the plugin.
 * 
 * 		Developed by Peter Kinney -- 6/30/2011
 * 
 */

using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

//Assignments for a bitmask to control which bones to look at and which to ignore
public enum BoneMask
{
	None = 0x0,
	Hip_Center = 0x1,
	Spine = 0x2,
	Shoulder_Center = 0x4,
	Head = 0x8,
	Shoulder_Left = 0x10,
	Elbow_Left = 0x20,
	Wrist_Left = 0x40,
	Hand_Left = 0x80,
	Shoulder_Right = 0x100,
	Elbow_Right = 0x200,
	Wrist_Right = 0x400,
	Hand_Right = 0x800,
	Hip_Left = 0x1000,
	Knee_Left = 0x2000,
	Ankle_Left = 0x4000,
	Foot_Left = 0x8000,
	Hip_Right = 0x10000,
	Knee_Right = 0x20000,
	Ankle_Right = 0x40000,
	Foot_Right = 0x80000,
	All = 0xFFFFF,
	Torso = 0x10000F, //the leading bit is used to force the ordering in the editor
	Left_Arm = 0x1000F0,
	Right_Arm = 0x100F00,
	Left_Leg = 0x10F000,
	Right_Leg = 0x1F0000,
	R_Arm_Chest = Right_Arm | Spine,
	No_Feet = All & ~(Foot_Left | Foot_Right)
}

//Ties the appropriate bone name to each index
public enum BoneIndex
{
	Hip_Center,	Spine, Shoulder_Center, Head,
	Shoulder_Left,Elbow_Left, Wrist_Left, Hand_Left,
	Shoulder_Right, Elbow_Right, Wrist_Right,	Hand_Right,
	Hip_Left, Knee_Left,	Ankle_Left,	Foot_Left,
	Hip_Right, Knee_Right, Ankle_Right, Foot_Right,
	Num_Bones
}

[RequireComponent(typeof(KinectEmulator))]
public class KinectWrapper : MonoBehaviour
{	
	//variables to control how the wrapper works
	public bool facingCamera;
	public bool autoCalibrate;
	public float manualCalibrate;
	public Vector3 kinectCenter; //Where should the kinect center it's coordinates
	public Vector4 lookAt; //what point the kinect angles itself to look at
	
	//kinect smoothing parameters
	public float smoothing;
	public float correction;
	public float prediction;
	public float jitterRadius;
	public float maxDeviationRadius;
	
	//variables for other scripts to use
	public Vector4[,] BonePos;
	public Vector4[,] BoneVel;
	[HideInInspector]
	public Vector4 nullVector;
	
	//For each bone, what global vector should it use as the starting point of it's up vector
	public static Vector3[] BoneBaseUp = {
		Vector3.right,Vector3.right, Vector3.zero, Vector3.zero,
		-Vector3.forward,-Vector3.up,-Vector3.forward,Vector3.zero,
		Vector3.forward,Vector3.up,Vector3.forward,Vector3.zero,
		-Vector3.forward,-Vector3.right,Vector3.right,Vector3.zero,
		Vector3.forward,-Vector3.right,Vector3.right,Vector3.zero
	};

	//For each bone, which other bone should it base it's secondary rotation (correcting the up vector) on
	public static int[] PrevBoneList = {
		-1,-1,1,-1,
		1,4,-1,-1,
		1,8,-1,-1,
		0,-1,-1,-1,
		0,-1,-1,-1
	};
	
	[HideInInspector]
	public bool newData = false;
	
	//Lets make our calls from the Plugin
	[DllImport ("UnityKinectPlugin")]
	private static extern bool startKinect();
	
	[DllImport ("UnityKinectPlugin")]
	private static extern void setKinectAngle(long angle);
	
	[DllImport ("UnityKinectPlugin")]
	private static extern void stopKinect();
	
	[DllImport ("UnityKinectPlugin")]
	private static extern void setSmoothingParams(float fSmoothing,float fCorrection,float fPrediction,
		float fJitterRadius,float fMaxDeviationRadius, bool smooth);
	
	[DllImport ("UnityKinectPlugin")]
	private static extern bool updateFrame();
	
	[DllImport ("UnityKinectPlugin")]
	private static extern Vector4 getSkeleton(int bone);
	
	[DllImport ("UnityKinectPlugin")]
	private static extern Vector4 getVelocity(int bone);
	
	[DllImport ("UnityKinectPlugin")]
	private static extern Vector4 getSkeletonByPlayer(int player, int bone);
	
	[DllImport ("UnityKinectPlugin")]
	private static extern Vector4 getVelocityByPlayer(int player, int bone);
	
	[DllImport ("UnityKinectPlugin")]
	private static extern Vector4 getNormal();
	
	[DllImport ("UnityKinectPlugin")]
	private static extern float getHeight();
	
	//private Vector3 _normal;
	//private float _mirrorPlane;
	private float _kinectHeight;
	private long _kinectAngle;
	
	private Matrix4x4 _kinectToWorld;
	
	private bool _frameUpdated = false;
	
	private bool _useEmulator;
	private KinectEmulator _emulator;
	
	void Start () {
		//start the kinect and get it's height off the ground
		if(!startKinect()){
			Debug.LogError("Kinect Initialization Failed, starting emulator");
			_emulator = GetComponent<KinectEmulator>();
			_emulator.enabled = true;
			_useEmulator = true;
			nullVector = new Vector4(0,0,0,1);
		}
		else
		{
			//send smoothing parameters
			setSmoothingParams(smoothing,correction,prediction,jitterRadius,maxDeviationRadius,true);
			//calibrate kinect height
			if(autoCalibrate){
				_kinectHeight = getHeight();
			}else{
				_kinectHeight = manualCalibrate;
			}
			Debug.Log("Kinect at " + _kinectHeight + "m");
			
			//determine what angle the kinect should be at, and set it
			double theta = Math.Atan((lookAt.y-_kinectHeight) / lookAt.z);
			_kinectAngle = (long)(theta * (180 / Math.PI));
			setKinectAngle(_kinectAngle);
			
			//create the transform matrix that converts from kinect-space to world-space
			Matrix4x4 trans = new Matrix4x4();
			trans.SetTRS( new Vector3(-kinectCenter.x,_kinectHeight-kinectCenter.y, -kinectCenter.z), Quaternion.identity, Vector3.one );
			Matrix4x4 rot = new Matrix4x4();
			Quaternion quat = new Quaternion();
			quat.eulerAngles = new Vector3(-_kinectAngle, 0, 0);
			rot.SetTRS( Vector3.zero, quat, Vector3.one );
			Matrix4x4 flip = Matrix4x4.identity;
			if(facingCamera){
				flip[0,0] = -1;
			}
			flip[2,2] = -1;
			//final transform matrix offsets the rotation of the kinect, translates to a new center, and flips the z axis
			_kinectToWorld = flip*trans*rot;
			
			//set the public nullVector equal to the zero Vector4 processed by the translation matrix
			//this value is used by other functions to tell when they are getting bad data from the kinect
			nullVector = new Vector4(0,0,0,1);
			nullVector = _kinectToWorld.MultiplyPoint3x4(nullVector);
		}
		//initialize the array of bone positions and velocities
		BonePos = new Vector4[2,(int)BoneIndex.Num_Bones];
		BoneVel = new Vector4[2,(int)BoneIndex.Num_Bones];
	}
	
	void Update() {
		
	}
	
	void LateUpdate() {
		_frameUpdated = false;
		newData = false;
	}
	
	public void pollKinect() {
		//if the kinect hasn't been polled this frame and either either the emulator
		//is being used or the kinect has updated, update the BonePos and BoneVel arrays
		if(!_frameUpdated && (_useEmulator || updateFrame()))
		{
			if(_useEmulator)
			{
				for(int ii = 0; ii < 2; ii++)
				{
					for(int jj = 0; jj < (int)BoneIndex.Num_Bones; jj++)
					{
						BonePos[ii,jj] = _emulator.getBonePos(jj);
						BoneVel[ii,jj] = new Vector4(0,0,0,1);
					}
				}
			}
			else
			{
				for(int ii = 0; ii < 2; ii++)
				{
					for(int jj = 0; jj < (int)BoneIndex.Num_Bones; jj++)
					{
						BonePos[ii,jj] = getBonePos(ii,jj);
						BoneVel[ii,jj] = getBoneVel(ii,jj);
					}
				}
				newData = true;
			}
		}
		_frameUpdated = true;
	}
	
	private Vector4 getBonePos(int player, int index) {
		if(facingCamera && index >= (int)BoneIndex.Shoulder_Left){
			//if the data needs to be mirrored and the index is either a left or right side bone,
			//add 4 to left side bones and subtract 4 from right side bones
			//(odd groupings of 4 is left, even groupings of 4 are right side)
			int plusMinus = ((index / 4) % 2) * 2 - 1;
			Debug.Log(index);
			index += 4 * plusMinus;
			Debug.Log(index);
		}
		Vector4 pos;
		pos = getSkeletonByPlayer(player, index);
		pos = _kinectToWorld.MultiplyPoint3x4(pos);
		return pos;
	}
	
	private Vector3 getBoneVel(int player, int index) {
		Vector3 vel;
		vel = getVelocityByPlayer(player, index);
		vel = _kinectToWorld.MultiplyVector(vel);
		return vel;
	}
	
	void OnApplicationQuit() {
		stopKinect();
	}
}
