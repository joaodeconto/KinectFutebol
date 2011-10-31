using UnityEngine;
using System.Collections;

public class TrocarTextura : MonoBehaviour
{
	public Texture2D[] advProp;
	public int tempoDeTroca = 5;
	public bool delay = false;
	
	// Use this for initialization
	void Update ()
	{
		TrocaDeTextura();
	}

	void TrocaDeTextura () {
		if(advProp.Length == 0) // nao achar nenhuma textura
			return;

		// queremos está index de textura agora
		int index = (int)(Time.time / tempoDeTroca);
		// tomar um módulo com tamanho de modo que se repete de animação
		index = index % advProp.Length;
		// atribuir no material
		renderer.material.mainTexture = advProp[index];		
	}
	
}