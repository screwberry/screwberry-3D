using UnityEngine;

public class CamControl : MonoBehaviour {

	public float camSpeed = 30.00f;
	public float smoothing = 10.0f;
	private float angle;

	void Update() {
		//angle += camSpeed * Input.GetAxis("Mouse X");
		angle += camSpeed * 0.02f;
		
		Vector3 targetPos = new Vector3(0f, angle, 0f);
		Vector3 smoothPos = Vector3.Lerp(transform.eulerAngles, targetPos, smoothing);
		transform.eulerAngles = smoothPos;
	}


}
