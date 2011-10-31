using UnityEngine;
using System.Collections;

public class Pontos : MonoBehaviour
{
	
	void OnTriggerEnter (Collider other)
	{
		if (other.rigidbody && other.tag == "Bola" && other.GetComponent<Bola> () && !other.GetComponent<Bola> ().bateu) {
			other.rigidbody.AddForce ((transform.forward * Random.Range (11.0f, 20.0f)), ForceMode.Force);
			StartCoroutine ("NovaBola", other.gameObject);
		}
	}
	
}

