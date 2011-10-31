using UnityEngine;
using System.Collections;

[AddComponentMenu("VisioramaKinect/ScoreTotal")]
public class ScoreTotal : MonoBehaviour
{
	public int scoreTotal = 0;
	private TextMesh textMeshPlacar;
	
	void Start () {
		if (null == textMeshPlacar)
			textMeshPlacar = transform.GetComponentInChildren<TextMesh>();
	}
	
	void Update () {
		textMeshPlacar.text = ""+scoreTotal;
	}
	
	IEnumerator TimerSoma (object[] s) {
		yield return new WaitForSeconds((float)s[0]);
		scoreTotal += (int)s[1];
	}
}

