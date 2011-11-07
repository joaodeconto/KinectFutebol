using UnityEngine;
using System;
using System.Collections;
using System.Text;
using System.IO;
using Visiorama.Utils;

[AddComponentMenu("VisioramaKinect/Prefs")]
public class Prefs : MonoBehaviour
{
	public GameObject inicio;
	public GameObject m_foto;
	public GameObject mao;
	
	string diretorio = "questionario";
	string nomeDoArquivo = "questionario";
	Texture2D foto;
	
	float m_tempo = 0;

	int i = 0, s = 0, l = 0, d = 0;
	
	void Update () {
		if (m_tempo < 200.0f) {
			m_tempo += UnityEngine.Random.Range (0.25f, 1.0f);
		} else {
			TimerReset();
		}
	}
	
	public void TimerReset() {
		GameObject.Find("Jogo/goleira/Placar").GetComponent<ScoreTotal>().scoreTotal = 0;
		m_tempo = 0;
		inicio.SetActiveRecursively(true);
		mao.SetActiveRecursively(true);
		transform.parent.gameObject.SetActiveRecursively(false);
	}
	
//	void OnTriggerStay (Collider other)
//	{
//		if (other.tag == "Player") {
//			if (m_tempo < 50.0f) {
//				m_tempo += UnityEngine.Random.Range (0.25f, 1.0f);
//			} else {
//				TimerReset();
//			}
//		}
//	}
//
//	void OnTriggerExit (Collider other)
//	{
//		if (other.tag == "Player")
//			m_tempo = 0;
//	}
//	
//	public void CarregarScreen () {
//		foto = new Texture2D(1000, 1000);
//		foto.LoadImage(System.IO.File.ReadAllBytes(TakePhoto.ScreenShotName()));
//		m_foto.renderer.material.mainTexture = foto;
//	}
}