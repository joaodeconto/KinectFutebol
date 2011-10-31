using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour
{
	void Destroy() {
		Destroy(gameObject);
	}
}

