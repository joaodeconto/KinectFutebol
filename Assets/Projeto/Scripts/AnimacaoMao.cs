using UnityEngine;
using System.Collections;

public class AnimacaoMao : MonoBehaviour {
	
	// Update is called once per frame
	void OnTriggerEnter () {
		GetComponentInChildren<Animation>().Play();
 	}
}
