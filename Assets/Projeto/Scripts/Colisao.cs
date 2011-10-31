using UnityEngine;
using System.Collections;

[AddComponentMenu("VisioramaKinect/Colisao")]
public class Colisao : MonoBehaviour
{
		
	private GUIStyle tempoStyle;
	
	//=============================================
	public GameObject[] objetosAtivar;
	public GameObject[] objetosDesativar;
	public string 	text;
	public GameObject target, targetHud;
	public HUDmanipulation hud;
	//=============================================
	
	// Use this for initialization
	void Start ()
	{
		text 		  = ""+target.GetComponent<TotalDeBolas>().numeroDeBolas+" / 5";
		Application.runInBackground = true;
		//==========================================================
		Screen.showCursor = true;//PADRãO /FALSE/
	
		tempoStyle = new GUIStyle();
		tempoStyle.fontSize = 20;
		tempoStyle.normal.textColor = new Color(1f, 1f, 1f, 1f);
		//==========================================================
	}
	

	
	void OnTriggerEnter (Collider other)
	{
		if (other.rigidbody && other.tag == "Bola" && other.GetComponent<Bola>() && !other.GetComponent<Bola>().bateu)
		{
			other.rigidbody.AddForce((transform.forward * Random.Range(15.0f, 20.0f)	), ForceMode.Force);
			StartCoroutine("NovaBola", other.gameObject);
		}
	}
	
	void DA ()
	{
		foreach (GameObject objeto in objetosAtivar)
			objeto.SetActiveRecursively(true);
		
//		prefs.CarregarScreen();
		target.GetComponent<TotalDeBolas>().numeroDeBolas = 1;
		targetHud.GetComponent<HUDmanipulation>().Reset();
		//m_tempoTotal = initialTempo;
		TakePhoto.ResetValues();
		hud.Reset();
		
		foreach (GameObject objeto in objetosDesativar)
			objeto.SetActiveRecursively(false);
		
		Debug.Log("FIM DO JOGO |o| ");
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	//Debug.Log(rigidbody.velocity);
	}
	
	IEnumerator NovaBola(GameObject bola)
	{
		bola.GetComponent<Bola>().bateu = true;
		Debug.Log("Renew");
		yield return new WaitForSeconds(3.5f);
		hud.SetarHud(target.GetComponent<TotalDeBolas>().numeroDeBolas, bola.GetComponent<Bola>().gol);
		Debug.Log("Renew Fim");
//			GameObject newBola = new GameObject("Bola", bola.GetComponents);
		GameObject newBola = Instantiate(bola, new Vector3(0f, 2f, 1f), Quaternion.identity) as GameObject;
		//======================================================
		target.GetComponent<TotalDeBolas>().numeroDeBolas +=1;//aumenta o numero de bolas ja instanciiadas
		text 			= ""+target.GetComponent<TotalDeBolas>().numeroDeBolas+" / 5";
		Debug.Log(text);
		//======================================================
		Destroy(bola);
		newBola.GetComponent<Bola>().bateu = false;
		newBola.GetComponent<Bola>().gol = false;
		if (target.GetComponent<TotalDeBolas>().numeroDeBolas > 5){
			target.GetComponent<TotalDeBolas>().trava = true;
			DA ();
		}
		
	}	
	
	void OnGUI (){
		 
	}
}