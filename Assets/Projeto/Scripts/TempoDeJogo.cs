using UnityEngine;
using System.Collections;

[AddComponentMenu("VisioramaKinect/TempoDeJogo")]
public class TempoDeJogo : MonoBehaviour
{
	
	public GameObject[] objetosAtivar;
	public GameObject[] objetosDesativar;
	private float tempo = 0;
	public int m_tempoTotal = 15;
	public Prefs prefs;
	private int m_tempo = 0;
	private int initialTempo;
	private GUIStyle tempoStyle;
	
	void Start () {
		initialTempo = m_tempoTotal;
		tempoStyle = new GUIStyle();
		tempoStyle.fontSize = 20;
		tempoStyle.normal.textColor = new Color(1f, 1f, 1f, 1f);
	}
	
	// Use this for initialization
	/*
	void DA ()
	{
		foreach (GameObject objeto in objetosAtivar)
			objeto.SetActiveRecursively(true);
		
		prefs.CarregarScreen();
		
		tempo = 0;
		m_tempoTotal = initialTempo;
		TakePhoto.ResetValues();
		
		foreach (GameObject objeto in objetosDesativar)
			objeto.SetActiveRecursively(false);
		
	}
	 */

	// Update is called once per frame
	void Update ()
	{
		/*
		tempo += Time.deltaTime;
		m_tempo = (int)tempo;
		if (m_tempo	> 0)
		{
			m_tempoTotal--;
			tempo = 0;
		}
		
		if (m_tempoTotal < 0)
			DA ();
		*/
	}
	
	void OnGUI ()
	{
		//GUI.Label(new Rect(Screen.width - 150 - 20, 10, 150, 20), "TEMPO: " + m_tempoTotal + " s", tempoStyle);
	}
}

