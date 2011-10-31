/*
 * KinectModelController.cs - Handles rotating the bones of a model to match 
 * 			rotations derived from the bone positions given by the kinect
 * 
 * 		Developed by Peter Kinney -- 6/30/2011
 * 
 */

using UnityEngine;
using System;
using System.Collections;

public class NewModelController : MonoBehaviour {

	public KinectWrapper KinectBridge;
	
	/* 
	 * The naming of the variables is for ease of drag and drop, so they reflect
	 * where they are in the model rather than how they are referred to in the script
	 * (the script mostly uses the Kinect BoneIndex) and is a little bit scrambled
	 * because there are more bones in the model than there are points returned by the
	 * kinect. The Comment next to each name describes which point from the kinect is
	 * closest to the origin of the bone, and which BoneIndex will be used to call it.
	 */
	public GameObject RootJoint; //At Hip_Center. BoneIndex.Hip_Center
	public GameObject Hip_Center; //At Hip_Center. BoneIndex.Spine
	public GameObject Spine; //At Spine. BoneIndex.Shoulder_Center
	public GameObject Shoulder_Center; //At Shoulder_Center. BoneIndex.Head
	public GameObject Head; //At Head. N/A
	public GameObject Collar_Left; //Closest To Shoulder_Center. BoneIndex.Shoulder_Left
	public GameObject Shoulder_Left; //At Shoulder_Left. BoneIndex.Elbow_Left
	public GameObject Elbow_Left; //At Elbow_Left. BoneIndex.Wrist_Left
	public GameObject Wrist_Left; //At Wrist_Left. BoneIndex.Hand_Left
	public GameObject Hand_Left; //At Hand_Left. N/A
	public GameObject Fingers_Left; //Extra bone, not used in mapping. N/A
	public GameObject Collar_Right; //Closest To Shoulder_Center. BoneIndex.Shoulder_Right
	public GameObject Shoulder_Right; //At Shoulder_Right. BoneIndex.Elbow_Right
	public GameObject Elbow_Right; //At Elbow_Right. BoneIndex.Wrist_Right
	public GameObject Wrist_Right; //At Wrist_Right. BoneIndex.Hand_Right
	public GameObject Hand_Right; //At HandS_Right. N/A
	public GameObject Fingers_Right; //Extra bone, not used in mapping. N/A
	public GameObject Hip_Left; //At Hip_Center. BoneIndex.Hip_Left
	public GameObject Thigh_Left; //At Hip_Left. BoneIndex.Knee_Left
	public GameObject Knee_Left; //At Knee_Left. BoneIndex.Ankle_Left
	public GameObject Ankle_Left; //At Ankle_Left. BoneIndex.Foot_Left
	public GameObject Foot_Left; //At Foot_Left. N/A
	public GameObject Hip_Right; //At Hip_Center. BoneIndex.Hip_Right
	public GameObject Thigh_Right; //At Hip_Right. BoneIndex.Knee_Right
	public GameObject Knee_Right; //At Knee_Right. BoneIndex.Ankle_Right
	public GameObject Ankle_Right; //At Ankle_Right. BoneIndex.Foot_Right
	public GameObject Foot_Right; //At Foot_Right. N/A
	
	public int player;
	public BoneMask Mask = BoneMask.All;
	public bool animated;
	public float blendWeight = 1;
	
	private GameObject[] _bones; //internal handle for the bones of the model
	
	private Quaternion[] _baseRotation; //starting orientation of the joints
	private Vector3[] _boneDir; //in the bone's local space, the direction of the bones
	private Vector3[] _boneUp; //in the bone's local space, the up vector of the bone
	private Vector3 _hipRight; //right vector of the hips
	private Vector3 _chestRight; //right vectory of the chest
	
	
	// Use this for initialization
	void Start () {
		//store bones in a list for easier access
		_bones = new GameObject[(int)BoneIndex.Num_Bones + 5] {
			RootJoint, Hip_Center, Spine, Shoulder_Center,
			Collar_Left, Shoulder_Left, Elbow_Left, Wrist_Left,
			Collar_Right, Shoulder_Right, Elbow_Right, Wrist_Right,
			Hip_Left, Thigh_Left, Knee_Left, Ankle_Left,
			Hip_Right, Thigh_Right, Knee_Right, Ankle_Right,
			Head, Hand_Left, Hand_Right, Foot_Left, Foot_Right
			};
		
		//store the base rotations and bone directions (in bone-local space)
		_baseRotation = new Quaternion[(int)BoneIndex.Num_Bones];
		_boneDir = new Vector3[(int)BoneIndex.Num_Bones];
		//_boneUp = new Vector3[(int)BoneIndex.Num_Bones];
		
		//first save the special rotations for the hip and spine
		_baseRotation[0] = _bones[0].transform.localRotation;
		_hipRight = _bones[(int)BoneIndex.Knee_Right].transform.position - _bones[(int)BoneIndex.Knee_Left].transform.position;
		_hipRight = _bones[(int)BoneIndex.Hip_Center].transform.InverseTransformDirection(_hipRight);
		_chestRight = _bones[(int)BoneIndex.Shoulder_Right].transform.position - _bones[(int)BoneIndex.Shoulder_Left].transform.position;
		_chestRight = _bones[(int)BoneIndex.Spine].transform.InverseTransformDirection(_chestRight);
		
		//get direction of all other bones
		for( int ii = 1; ii < (int)BoneIndex.Num_Bones; ii++)
		{
			//save initial rotation
			_baseRotation[ii] = _bones[ii].transform.localRotation;
			
			if(ii % 4 == 3)
			{
				_boneDir[ii] = _bones[(ii/4) + (int)BoneIndex.Num_Bones].transform.position - _bones[ii].transform.position;
			}
			else
			{
				_boneDir[ii] = _bones[ii+1].transform.position - _bones[ii].transform.position;
			}
			//if it's the spine bone, rotate the target back 45 degrees to compensate
			//for the placement of the kinect data.
			if(ii == (int)BoneIndex.Spine)
			{
				_boneDir[ii] = Quaternion.AngleAxis(-40,transform.right) * _boneDir[ii];
			}
			_boneDir[ii] = _bones[ii].transform.InverseTransformDirection(_boneDir[ii]);
		}
		//make _chestRight orthogonal to the direction of the spine.
		_chestRight -= Vector3.Project(_chestRight, _boneDir[(int)BoneIndex.Spine]);
	}
	
	void Update () {
		//update the data from the kinect if necessary
		KinectBridge.pollKinect();	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//update the flagged bones rotations based on bone positions
		for( int ii = 0; ii < (int)BoneIndex.Num_Bones; ii++)
		{
			if( ((uint)Mask & (uint)(1 << ii) ) > 0 )
			{
				RotateJoint(ii);
			}
		}
	}
	
	void RotateJoint(int bone) {
		//if blendWeight is 0 there is no need to compute the rotations
		if( blendWeight <= 0 ){ return; }
		//if the required bone data from the kinect isn't available, reset to base rotations
		if( KinectBridge.BonePos[player,bone] == KinectBridge.nullVector)
		{
			_bones[bone].transform.localRotation = _baseRotation[bone];
			return;
		}
		if(!animated){_bones[bone].transform.localRotation = _baseRotation[bone];}
		//get the target direction of the bone in world space
		//for hip-center this is the vector from hip-left to hip-right, everything else it's
		//bone - 1 to bone.
		
		Vector3 dir,target;
		if(bone == (int)BoneIndex.Hip_Center)
		{
			dir = _hipRight;
			//if bone is the center hip, use the vector from left->right hip
			target = KinectBridge.BonePos[player,(int)BoneIndex.Hip_Right] - KinectBridge.BonePos[player,(int)BoneIndex.Hip_Left];
		}
		else if (bone % 4 == 0)
		{
			dir = _boneDir[bone];
			//if bone is an outside shoulder or hip, use the vector from center-outside
			//2 - (bone/3) gives 2(shoulder-center) if bone is less than 12 (the shoulders)
			//and 0(hip-center) if bone is greater than 12 (the hips)
			target = KinectBridge.BonePos[player,bone] - KinectBridge.BonePos[player,2-((bone/12)*2)];
		}
		else
		{
			dir = _boneDir[bone];
			//for every other bone, use the vector from the previous bone to bone.
			target = KinectBridge.BonePos[player,bone] - KinectBridge.BonePos[player,bone-1];
		}
		//transform it into bone-local space (independant of the transform of the controller)
		target = transform.TransformDirection(target);
		target = _bones[bone].transform.InverseTransformDirection(target);
		//create a rotation that rotates dir into target
		Quaternion quat = Quaternion.FromToRotation(dir,target);
		//if necessary, add in the rotation along the spine
		if(bone == (int)BoneIndex.Spine)
		{
			//rotate the chest so that it faces forward (determined by the shoulders)
			dir = _chestRight;
			target = KinectBridge.BonePos[player,(int)BoneIndex.Shoulder_Right] - KinectBridge.BonePos[player,(int)BoneIndex.Shoulder_Left];
			
			target = transform.TransformDirection(target);
			target = _bones[bone].transform.InverseTransformDirection(target);
			target -= Vector3.Project(target,_boneDir[bone]);
			
			quat *= Quaternion.FromToRotation(dir,target);
		}
		
		//reduce the effect of the rotation using the blend parameter
		quat = Quaternion.Lerp(Quaternion.identity, quat, blendWeight);
		//apply the rotation to the local rotation of the bone
		_bones[bone].transform.localRotation = _bones[bone].transform.localRotation  * quat;
		
		return;
	}
}
