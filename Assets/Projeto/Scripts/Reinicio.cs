using UnityEngine;
using System.Collections;

public class Reinicio : MonoBehaviour
{

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
			Application.Quit();
		if (Input.GetKeyUp(KeyCode.R))
			Application.LoadLevel(0);
	}
}

