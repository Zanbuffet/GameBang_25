using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public int cur_index;
    public int max_index;
    public bool key_pressed;
	void Start () {

	}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis ("Vertical") != 0){
			if(!key_pressed){
				if (Input.GetAxis ("Vertical") < 0) {
					if(cur_index < max_index){
						cur_index++;
					}else{
						cur_index = 0;
					}
				} else if(Input.GetAxis ("Vertical") > 0){
					if(cur_index > 0){
						cur_index --; 
					}else{
						cur_index = max_index;
					}
				}
				key_pressed = true;
			}
		}else{
			key_pressed = false;
		}
    }
}
