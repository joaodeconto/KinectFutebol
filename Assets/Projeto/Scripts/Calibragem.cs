using UnityEngine;
using System.Collections;

public class Calibragem : MonoBehaviour
{
	public GUIText 	m_texto;
	private float 	m_tempo = 0;
	// Use this for initialization
	void Start ()
	{
		if (m_texto != null)
			m_texto.text = "Fique na marca 'BRANCA'";
		else
		{
			Debug.LogError("Coloque um Editor de Texto");
			return;
		}
	}
	
	void OnTriggerStay (Collider other)
	{
		if (other.tag == "Player")
		{
			if (m_tempo < 100.0f)
			{
				m_tempo += Random.Range(0.25f, 1.0f);
				m_texto.text = "Calibrando: " + m_tempo;
			}
			else
				m_texto.text = "Está calibrado";
		}
	}
	
	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player")
		{
			m_tempo = 0;
			m_texto.text = "Fique na marca 'BRANCA'";
		}
	}
}