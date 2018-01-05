using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3 : MonoBehaviour {

	public float speed = 1;
	public float time = 0.1f;

	void Start () {

		StartCoroutine("Wait");

	}
	float totalTime = 0;
	void Rotate () {
		totalTime += Time.deltaTime * 100;
		print (Quaternion.AngleAxis(totalTime,new Vector3(1,0,0)).eulerAngles);
		transform.rotation = Quaternion.AngleAxis (totalTime,new Vector3(1,0,0));

		StartCoroutine("Wait");

	}

	IEnumerator Wait (){

		yield return new WaitForSeconds (time);
		Rotate ();

	}
}
