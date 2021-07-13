using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunction : MonoBehaviour
{
	[SerializeField] ButtonController buttonController;
	public bool disableOnce;

	void PlaySound(AudioClip whichSound){
		if(!disableOnce){
			buttonController.audioSource.PlayOneShot (whichSound);
		}else{
			disableOnce = false;
		}
	}
}	
