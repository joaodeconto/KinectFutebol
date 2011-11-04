using UnityEngine;
using System.Collections;

public class AnimacaoMao : MonoBehaviour {
	
	private Animation animacao;
	private bool play = false;
	
	void Start () {
		animacao = GetComponentInChildren<Animation>();
		animacao["fecha_mao"].speed = -1;
		animacao["fecha_mao"].time = animacao["fecha_mao"].length;
	}
	
	// Update is called once per frame
	void OnTriggerStay () {
		if (!play) {
			animacao["fecha_mao"].speed = -1;
			animacao["fecha_mao"].time = animacao["fecha_mao"].length;
			animacao.CrossFade("fecha_mao");
			play = true;
		}
 	}
	
	void OnTriggerExit () {
		animacao["fecha_mao"].speed = 1;
		animacao["fecha_mao"].time = 0;
		animacao.CrossFade("fecha_mao");
		play = false;
	}
}
