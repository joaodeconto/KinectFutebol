using UnityEngine;
using System.Collections;

[AddComponentMenu("VisioramaKinect/Score")]
public class Score : MonoBehaviour
{
	public int score;
	ScoreTotal scoreTotal;
	public Transform explosion;
	public Transform pontuation;
		
	void Start()
	{
		if (null == transform.parent.parent.GetComponent<ScoreTotal>())
			transform.parent.parent.gameObject.AddComponent<ScoreTotal>();
		
		scoreTotal = transform.parent.parent.GetComponent<ScoreTotal>();
		
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Bola" && !other.GetComponent<Bola>().gol)
		{
			other.GetComponent<Bola>().gol = true;
			Transform theClonedExplosion;
            theClonedExplosion = Instantiate(explosion, transform.position, transform.rotation) as Transform;
			Transform theClonedPontuation;
            theClonedPontuation = Instantiate(pontuation) as Transform;
			SendMessageUpwards("TimerSoma", new object[] {theClonedPontuation.animation.clip.length, score}, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	
}

