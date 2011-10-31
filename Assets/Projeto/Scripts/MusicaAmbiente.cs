using UnityEngine;
using System.Collections;

public class MusicaAmbiente : MonoBehaviour {
	
	public AudioClip[] sons;
	private AudioSource soundPlayer;
	
	void Start(){
		soundPlayer = GetComponent<AudioSource>();
		soundPlayer.loop = false;
		soundPlayer.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if(!soundPlayer.isPlaying){
			soundPlayer.clip = sons[ Random.Range(0,sons.Length - 1)];
		}
	}
}
