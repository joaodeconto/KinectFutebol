using UnityEngine;
using System.Collections;

public enum Tipo {
	Roupa,
	Chuteira
}

public class EscolherPeca : MonoBehaviour
{
	public Texture2D texture;
	public GameObject objPrincipal;
	public GameObject objRepresentante;
	public Tipo tipo;
	
	private float m_tempo = 0;
	private bool m_escolhido = false;	

	// Update is called once per frame
	void Update ()
	{
		if (m_escolhido) {
			SkinnedMeshRenderer render = objRepresentante.transform.GetComponentInChildren<SkinnedMeshRenderer>();
			if (tipo == Tipo.Chuteira)
			{
				objPrincipal.transform.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials[0].mainTexture = texture;
				objRepresentante.transform.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials[0].mainTexture = texture;	
			}
			else
			{
				objPrincipal.transform.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials[1].mainTexture = texture;
				objRepresentante.transform.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials[1].mainTexture = texture;	
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