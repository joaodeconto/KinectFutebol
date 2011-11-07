using UnityEngine;
using System.Collections;

[AddComponentMenu("VisioramaKinect/Bola")]
public class Bola : MonoBehaviour
{
	internal bool 		bateu = false; 
	internal bool 		gol = false;
	
	public AudioClip	somChute;
	
	void Update(){
		if(bateu) {
			audio.PlayOneShot(somChute);
		}
	}
	
}
