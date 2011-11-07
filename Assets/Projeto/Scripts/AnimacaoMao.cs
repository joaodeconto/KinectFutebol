using UnityEngine;
using System.Collections;

public class AnimacaoMao : MonoBehaviour {
	
	private Animation animacao;
	private bool play = false;
	
	void Awake () {
		animacao = GetComponentInChildren<Animation>();
		animacao["fecha_mao"].speed = -3;
		animacao["fecha_mao"].time = animacao["fecha_mao"].length;
		animacao.CrossFade("fecha_mao");
	}
	
	// Update is called once per frame
	void OnTriggerStay (Collider other) {
		if (animacao["fecha_mao"].time >= (animacao["fecha_mao"].length - 0.4f) && play)
		{
			if (other.tag == "SelecaoRoupa")
				other.GetComponent<EscolherPeca>().m_escolhido = true;
			else if (other.tag == "SelecaoInicio")
				other.GetComponent<ObjetosDA>().DA();
			else if (other.tag == "SelecaoFim")
				other.GetComponent<Prefs>().TimerReset();
		}
		else if (!play) {
			animacao["fecha_mao"].speed = 1;
			animacao["fecha_mao"].time = 0;
			animacao.CrossFade("fecha_mao");
			play = true;
			audio.Play();
		}
 	}
	
	void OnTriggerExit (Collider other) {
		animacao["fecha_mao"].speed = -3;
		animacao["fecha_mao"].time = animacao["fecha_mao"].length;
		animacao.CrossFade("fecha_mao");
		play = false;
		if (other.tag == "SelecaoRoupa")
			other.GetComponent<EscolherPeca>().m_escolhido = false;
	}
}
