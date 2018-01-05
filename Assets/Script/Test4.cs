﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Test4 : MonoBehaviour {
	public float radiu = 10;
	public Transform target,mycamera,smallBall;
	// Use this for initialization
	void Start () {
		mainCrma = Camera.main;
		target.GetComponent<Rigidbody> ().AddRelativeTorque (new Vector3 (10, 0, 0), ForceMode.Acceleration);
//		StartCoroutine (MoveCameraOnBall());
	}
	Vector3 startDown,startRotate;
	Quaternion startQua;
	private Camera mainCrma;
	private RaycastHit objhit;
	private Ray _ray;
	Vector3 startPos;
	void Update () 
	{
		if (Input.GetMouseButtonDown(0)) 
		{
			startDown = Input.mousePosition;
			target.GetComponent<Rigidbody> ().velocity = Vector3.zero;
//			_ray=mainCrma.ScreenPointToRay(Input.mousePosition);//从摄像机发出一条射线,到点击的坐标
//			Debug.DrawLine(_ray.origin,objhit.point,Color.red,2);//显示一条射线，只有在scene视图中才能看到
//			if (Physics.Raycast (_ray, out objhit, 1000)) 
//			{
//				smallBall.position = objhit.point;
//				startPos = objhit.point;
//				GameObject gameObj = objhit.collider.gameObject;
//
//				Debug.Log("Hit objname:"+gameObj.name+"Hit objlayer:"+gameObj.layer);
//			}

		}
	
	

		if (Input.GetMouseButton (0)) {

			float xoff =startDown.x-Input.mousePosition.x ;
			float yoff =  startDown.y-Input.mousePosition.y ;

			target.GetComponent<Rigidbody> ().AddTorque(new Vector3(-yoff,xoff,0));
//
//
////			target.eulerAngles = startRotate - new Vector3 (yoff,0,0);
//			target.Rotate(Vector3.right,yoff);
//			startDown = Input.mousePosition;
//			_ray=mainCrma.ScreenPointToRay(Input.mousePosition);//从摄像机发出一条射线,到点击的坐标
//			Debug.DrawLine(_ray.origin,objhit.point,Color.red,2);//显示一条射线，只有在scene视图中才能看到
//			if (Physics.Raycast (_ray, out objhit, 1000)) {
////				smallBall.position = objhit.point;
////				startPos = objhit.point;
////				GameObject gameObj = objhit.collider.gameObject;
////
////				Debug.Log("Hit objname:"+gameObj.name+"Hit objlayer:"+gameObj.layer);
//				//计算球上一点的法线向量  
//				Vector3 normal = startPos-target.position;  
//
//				//球心到目标点的向量  
//				Vector3 SphereToTarget = objhit.point - target.position;  
//
//				//计算次法线的向量（即与切线和法线所在平面垂直的向量）  
//				Vector3 binormal = Vector3.Cross(normal,SphereToTarget).normalized;  
//
//				//计算出指向目标物的切线向量  
//				Vector3 tangent = Vector3.Cross(binormal,normal); 
//				Debug.DrawRay (objhit.point, tangent, Color.red);
////				target.GetComponent<Rigidbody> ().AddTorque (tangent, ForceMode.Acceleration);
//				startPos = objhit.point;
//			}


		}
		if (Input.GetMouseButtonUp (0)) {
			target.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray,out hit)) {
				print (hit.collider.gameObject.name);	
				GameObject.DestroyImmediate (hit.collider.gameObject);
			}
//			for (int i = 0; i < hit.Length; ++i) {
//				print (hit[i].collider.gameObject.name);
//			}
//			if (Physics.RaycastAll(ray))
//			{
//				Debug.Log(hit.transform.name);
//				//Debug.Log(hit.transform.tag);
//			}
		}
		mycamera.LookAt (target, Vector3.up);


	}
	IEnumerator MoveCameraOnBall()
	{
		Vector3 beginPoint = Vector3.zero;
		Vector3 firstPoint = Vector3.zero;
		float theta = 0;

		for (int i = 0; i < 10; i += 1) {

			for (int fai = 0; fai < 360; fai += 10) {
				beginPoint = new Vector3 (radiu* Mathf.Sin ((theta + fai /20.0f) * Mathf.Deg2Rad) * Mathf.Cos (fai * Mathf.Deg2Rad),radiu* Mathf.Cos ((theta + fai /20.0f) * Mathf.Deg2Rad), radiu* Mathf.Sin ((theta + fai /20.0f) * Mathf.Deg2Rad) * Mathf.Sin (fai * Mathf.Deg2Rad));
				Vector3 endPoint = new Vector3 (radiu* Mathf.Sin ((theta + fai /20.0f) * Mathf.Deg2Rad) * Mathf.Cos ((fai + 10) * Mathf.Deg2Rad), radiu*Mathf.Cos ((theta+fai /20.0f) * Mathf.Deg2Rad), radiu*Mathf.Sin ((theta + fai /20.0f) * Mathf.Deg2Rad) * Mathf.Sin ((fai + 10) * Mathf.Deg2Rad));
//				Gizmos.DrawLine (beginPoint, endPoint);
				mycamera.DOMove(endPoint,0.5f);
				yield return new WaitForSeconds (0.5f);

			}
			theta += 18;
		}
	}
}
