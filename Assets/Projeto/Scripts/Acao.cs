using UnityEngine;
using System.Collections;

public enum TipoDeAcao
{
	MudarCena,
	MudarCor,
	MudarMaterial
}

public enum TipoChamadaDeCena
{
	String,
	Int
}

[AddComponentMenu("VisioramaKinect/Acao")]
public class Acao : MonoBehaviour
{
	public Color cor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	public Material material;
	public GameObject objetoOri, objetoRep;
	public TipoDeAcao tipoDeAcao;
	public TipoChamadaDeCena tipoChamadaDeCena;
	public string cenaStr;
	public int cenaInt = 0;
	private float m_tempo;
	private bool m_escolhido = false;

	// Use this for initialization
	void Start ()
	{
		if (GetComponent<TimerInteracao> () == null)
			gameObject.AddComponent<TimerInteracao> ();
	}

	void Update ()
	{
		if (m_escolhido) {
			switch (tipoDeAcao) {
				case TipoDeAcao.MudarCena:
					if (tipoChamadaDeCena == TipoChamadaDeCena.String)
						Application.LoadLevel(cenaStr);
					else
						Application.LoadLevel(cenaInt);
					break;
				case TipoDeAcao.MudarCor:
					renderer.material.color = cor;
					break;
				default:
					objetoOri.transform.GetComponentInChildren<Renderer>().material = material;
					objetoRep.transform.GetComponentInChildren<Renderer>().material = material;
					break;
			}
		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.tag == "Player") {
			if (m_tempo < 50.0f) {
				m_tempo += Random.Range (0.25f, 1.0f);
			} else {
				m_escolhido = true;
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player") {
			m_escolhido = false;
			m_tempo = 0;
		}
	}
}
