using UnityEngine;
using System.Collections;

public class MovePeca : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Move();
	}
	
	void Move()
	{
		rigidbody.MovePosition(new Vector3((Mathf.Sin(Time.time) * 5), transform.position.y, transform.position.z));
	}
}
