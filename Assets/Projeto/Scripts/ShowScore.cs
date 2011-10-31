using UnityEngine;
using System.Collections;

public class ShowScore : MonoBehaviour
{
	public ScoreTotal score;
	
	// Update is called once per frame
	void Update ()
	{
		GetComponent<TextMesh>().text = ""+score.scoreTotal;
	}
}

