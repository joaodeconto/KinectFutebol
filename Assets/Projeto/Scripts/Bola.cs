using UnityEngine;
using System.Collections;

[AddComponentMenu("VisioramaKinect/Bola")]
public class Bola : MonoBehaviour
{
	internal bool 		bateu = true; 
	internal bool 		gol = false;
	internal bool		encostou = false;
	
	public AudioClip	somChute;
	public AudioClip	somTrave;
	public AudioSource	somGol;
	
	void Update(){
		if(encostou) {
			audio.clip = somChute;
			audio.Play();
			encostou = false;
		}
	}
	
	void OnCollisionEnter (Collision collisionInfo) {
//		if (collisionInfo.transform.tag == "Trave") {
//			audio.clip = somTrave;
//			audio.Play();
//			print("AQUI");
//		}
	}
	
	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Gol") {
			somGol.Play();
		}
	}
	
}
