using UnityEngine;
using System.Collections;

[AddComponentMenu("VisioramaKinect/Gol")]
public class Gol : MonoBehaviour
{
	public GUIText textoGol;
	private int gols;
	
	void Start()
	{
		textoGol.text = "GOLS: " + gols;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Bola")
			gols++;
	}
}

