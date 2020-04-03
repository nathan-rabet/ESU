using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {

	public float minDistance = 1.0f;
	public float maxDistance = 2.5f;
	public float smooth = 1.0f;
	Vector3 dollyDir;
	public float distance;

	// Use this for initialization
	void Awake () {
		dollyDir = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 desiredCameraPos = transform.parent.TransformPoint (dollyDir * maxDistance);
		RaycastHit hit;

		if (Physics.Linecast (transform.parent.position, desiredCameraPos, out hit)) {
			distance = Mathf.Clamp ((hit.distance * 0.87f), minDistance, maxDistance);
				
				} else {
					distance = maxDistance;
				}

				transform.localPosition = Vector3.Lerp (transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
	}
	public void Scope(float distance, float timescope)
	{
		float d;
		if (maxDistance > distance) d = maxDistance - distance;
		else d = distance - maxDistance;

		while (maxDistance>distance)
		{
			maxDistance -= d * (Time.deltaTime / timescope);
		}
		while (maxDistance<distance)
		{
			maxDistance += d * (Time.deltaTime / timescope);
		}

		maxDistance = distance;
	}
}
