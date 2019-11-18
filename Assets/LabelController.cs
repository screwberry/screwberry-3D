using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelController : MonoBehaviour {

	public Transform ruuvi;
	public Camera camera;
	void Update () {
		Vector3 screenPos = camera.WorldToScreenPoint(ruuvi.position);
        this.transform.position = new Vector3(screenPos.x,screenPos.y+20,0);
	}
}
