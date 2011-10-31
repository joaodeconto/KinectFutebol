using UnityEngine;
using System.Collections;

public class Reset : MonoBehaviour {
	
	public int segundos = 60;
	private int count = 0;
	
	void Start () {
		DontDestroyOnLoad(this);
		count++;
		print(count);
		StartCoroutine("TimerReset");
	}
	
//	IEnumerator TimerReset() {
//		float s = (float)segundos;
//		yield return new WaitForSeconds(s);
//		GetComponent<OpenNIContext>().TurnOffSensor();
//		yield return new WaitForSeconds(3);
//		Application.LoadLevel(Application.loadedLevel);
//	}
	
}
