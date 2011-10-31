using UnityEngine;
using System.Collections;

public class HUDmanipulation : MonoBehaviour {
	
	public GameObject targetBola1;
	public GameObject targetBola2;
	public GameObject targetBola3;
	public GameObject targetBola4;
	public GameObject targetBola5;
	
	public Texture texturaBolaNormal;
	public Texture texturaBolaOK;
	public Texture texturaBolaNotOK;
	
	public void Reset () {
		Debug.Log("RESETE AQUI RESETE AQUI RESETE AQUI RESETE AQUI RESETE AQUI RESETE AQUI ");
		targetBola1.renderer.material.mainTexture = texturaBolaNormal;
		targetBola2.renderer.material.mainTexture = texturaBolaNormal;
		targetBola3.renderer.material.mainTexture = texturaBolaNormal;
		targetBola4.renderer.material.mainTexture = texturaBolaNormal;
		targetBola5.renderer.material.mainTexture = texturaBolaNormal;
	}

	// Update is called once per frame
	public void SetarHud (int Hud, bool ok) {
		switch(Hud){
		case 1:
			if(ok)
				targetBola1.renderer.material.mainTexture = texturaBolaOK; // atribui a texturaBolaOK
			else
				targetBola1.renderer.material.mainTexture = texturaBolaNotOK; // atribui a texturaBolaOK
			break;
		case 2:
			if(ok)
				targetBola2.renderer.material.mainTexture = texturaBolaOK;
			else
				targetBola2.renderer.material.mainTexture = texturaBolaNotOK;
			break;
		case 3:
			if(ok)
				targetBola3.renderer.material.mainTexture = texturaBolaOK;
			else
				targetBola3.renderer.material.mainTexture = texturaBolaNotOK;
			break;
		case 4:
			if(ok)
				targetBola4.renderer.material.mainTexture = texturaBolaOK;
			else
				targetBola4.renderer.material.mainTexture = texturaBolaNotOK;
			break;
		case 5:
			if(ok)
				targetBola5.renderer.material.mainTexture = texturaBolaOK;
			else
				targetBola5.renderer.material.mainTexture = texturaBolaNotOK;
			break;
		}
	}
}
