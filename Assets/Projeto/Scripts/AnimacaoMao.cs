using UnityEngine;
using System.Collections;

public class AnimacaoMao : MonoBehaviour {
	
	private Animation animacao;
	
	void Start () {
		animacao = GetComponentInChildren<Animation>();
		animacao["fecha_mao"].speed = -1;
		animacao["fecha_mao"].time = animacao["fecha_mao"].length;
	}
	
	// Update is called once per frame
	void OnTriggerEnter () {
		animacao["fecha_mao"].speed = -1;
		animacao["fecha_mao"].time = animacao["fecha_mao"].length;
		animacao.Play();
 	}
//	void OnTriggerExit () {
//		animacao.Stop();
//		animacao["fecha_mao"].speed = -1;
//		animacao["fecha_mao"].time = animacao["fecha_mao"].length;
//	}
}
