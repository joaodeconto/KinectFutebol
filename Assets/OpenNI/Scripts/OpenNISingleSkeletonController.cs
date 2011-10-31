using UnityEngine;
using System.Collections;
using OpenNI;

public class OpenNISingleSkeletonController : MonoBehaviour 
{
	public OpenNIUserTracker UserTracker;
	public OpenNISkeleton[] Skeletons;
	
	public Texture[] propagandas;
	public float tempoDeTroca = 5f;
	public Texture posicaoCalibrar;
	public Texture voltarPosicao;
	
	public GameObject hand;
	
	private int userId;
	private bool outOfFrame;
	
	public bool IsTracking {
		get { return userId != 0; }
	}
	
	// Use this for initialization
	void OnEnable () 
	{
		if (null == UserTracker) return;
		if (!UserTracker.enabled) UserTracker.enabled = true;
		UserTracker.MaxCalibratedUsers = 1;
	}
	
	void Start()
	{
        if (!UserTracker) {
            UserTracker = GetComponent<OpenNIUserTracker>();
        }
        if (!UserTracker) {
            Debug.LogWarning("Missing a User Tracker. Adding...");
            UserTracker = gameObject.AddComponent<OpenNIUserTracker>();
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
		// do we have a valid calibrated user?
		if (0 != userId)
		{
			// is the user still valid?
			if (!UserTracker.CalibratedUsers.Contains(userId))
			{
				userId = 0;
				foreach (OpenNISkeleton skel in Skeletons)
				{
					skel.RotateToCalibrationPose();
				}
			}
		}
		
		// look for a new userId if we dont have one
		if (0 == userId)
		{
			// just take the first calibrated user
			if (UserTracker.CalibratedUsers.Count > 0)
			{				
				userId = UserTracker.CalibratedUsers[0];
				outOfFrame = false;
			}
		}
		
		// we have a valid userId, lets use it for something!
		if (0 != userId)
		{
			// see if user is out o'frame
			Vector3 com = UserTracker.GetUserCenterOfMass(userId);
			if (outOfFrame != (com == Vector3.zero))
			{
				outOfFrame = (com == Vector3.zero);
				SendMessage("UserOutOfFrame", outOfFrame, SendMessageOptions.DontRequireReceiver);
				userId = 0;
			}
			
			// update our skeleton based on active user id	
			foreach (OpenNISkeleton skel in Skeletons)
			{
				UserTracker.UpdateSkeleton(userId, skel);
			}
		}
	}
	
	void OnGUI()
	{
		if (userId == 0)
		{
			hand.SetActiveRecursively(false);
			if (UserTracker.CalibratingUsers.Count > 0)
			{
				// Calibrating
				GUILayout.Box(string.Format("Calibrando: {0}", UserTracker.CalibratingUsers[0]));
			}
			else
			{
				if (UserTracker.AllUsers.Count > 0)
				{
					if (outOfFrame) {
						hand.SetActiveRecursively(false);
						GUILayout.BeginArea (new Rect (Screen.width/2 - 128, Screen.height/2 - 128, 256, 256), voltarPosicao);
						GUILayout.EndArea();
					}
					else {
						// Looking
						GUILayout.BeginArea (new Rect (Screen.width/2 - 128, Screen.height/2 - 128, 256, 256), posicaoCalibrar);
						GUILayout.EndArea();
					}
				}
				else {
					Propagandas();
					outOfFrame = false;
				}
			}
		}
		else if (outOfFrame) {
			hand.SetActiveRecursively(false);
			GUILayout.BeginArea (new Rect (Screen.width/2 - 128, Screen.height/2 - 128, 256, 256), voltarPosicao);
			GUILayout.EndArea();
		}
		else {
			hand.SetActiveRecursively(true);
		}
		/*else
		{
			// Calibrated
			GUILayout.BeginVertical("box");
			GUILayout.Label(string.Format("Calibrando: {0}", userId));
			GUILayout.Label(string.Format("Fora da cena: {0}", (outOfFrame) ? "SIM" : "NAO"));
			GUILayout.EndVertical();
		}*/
	}
	
	void Propagandas () {
		if(propagandas.Length == 0) // nao achar nenhuma textura
			return;

		// queremos está index de textura agora
		int index = (int)(Time.time / tempoDeTroca);
		// tomar um módulo com tamanho de modo que se repete de animação
		index = index % propagandas.Length;
		// atribuir no material
		Texture textura = propagandas[index];
		print(Screen.width + " : " + Screen.height);
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), textura);
	}
}