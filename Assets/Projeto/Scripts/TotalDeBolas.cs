using UnityEngine;
using System.Collections;

public class TotalDeBolas : MonoBehaviour {
	//VAriaveis
	public int numeroDeBolas = 0;
	public bool trava = true;
	
	void Update(){
		if(trava){
			numeroDeBolas = 1;
		}	
	}
		
}