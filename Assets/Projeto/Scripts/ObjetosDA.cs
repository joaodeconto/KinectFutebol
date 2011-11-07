using UnityEngine;
using System.Collections;

[AddComponentMenu("VisioramaKinect/ObjetosDA")]
public class ObjetosDA : MonoBehaviour
{
	public GameObject[] objetosAtivar;
	public GameObject[] objetosDesativar;
	private float m_tempo;
	public GameObject target,targetHud;
	// Use this for initialization
	public void DA ()
	{
		foreach (GameObject objeto in objetosAtivar)
			objeto.SetActiveRecursively(true);
		
		GameObject bola = GameObject.FindWithTag("Bola");
		bola.GetComponent<Bola>().bateu = false;
		
		m_tempo = 0;
		
		foreach (GameObject objeto in objetosDesativar)
			objeto.SetActiveRecursively(false);
		
	}
//	
//	void OnTriggerStay (Collider other)
//	{
//		if (other.tag == "Player") {
//			if (m_tempo < 50.0f) {
//				m_tempo += Random.Range (0.25f, 1.0f);
//			} else {
//				target.GetComponent<TotalDeBolas>().numeroDeBolas = 1;//seta a variavel para 1 iniciando o jogo
//				target.GetComponent<TotalDeBolas>().trava = false;
//				targetHud.GetComponent<HUDmanipulation>().Reset();
//				DA();
//			}
//		}
//	}
//
//	void OnTriggerExit (Collider other)
//	{
//		if (other.tag == "Player")
//			m_tempo = 0;
//	}
}

