using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour {

	public Transform target;
	public float speed;
	void Update() {
		float step = speed * Time.deltaTime;
		transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, step);
	}
}
