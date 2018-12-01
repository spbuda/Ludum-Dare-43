using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLight : MonoBehaviour
{
	UnityEngine.Light butLight;

    // Start is called before the first frame update
    void Start(){
		butLight = GetComponent<Light>();
	}

	public void onHit() {
		butLight.color = Color.green;
	}
}
