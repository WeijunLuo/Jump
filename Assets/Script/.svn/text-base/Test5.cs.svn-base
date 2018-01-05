using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test5 : MonoBehaviour {

	public GameObject sphere;  
	public GameObject target;  
	// Use this for initialization  

	void Start () {  
		Debug.Log (222);
		if (sphere == null)  
		{  
			Debug.Log("球为空");  
			return;  
		}         
	}     
	// Update is called once per frame  
	void Update () {  
		if (Input.GetMouseButton(0))  
		{  
			Debug.Log (333);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  
			RaycastHit hit;  
			if (Physics.Raycast(ray, out hit, 1000))//8层，球的层级  
			{        
				Debug.Log (111111);
				Debug.DrawRay (ray.origin, ray.direction);
				//胶囊体在一个父级空对象下  
				transform.parent.position = hit.point;  

				//计算球上一点的法线向量  
				Vector3 normal = transform.position-sphere.transform.position;  

				//球心到目标点的向量  
				Vector3 SphereToTarget = target.transform.position - sphere.transform.position;  

				//计算次法线的向量（即与切线和法线所在平面垂直的向量）  
				Vector3 binormal = Vector3.Cross(normal,SphereToTarget).normalized;  

				//计算出指向目标物的切线向量  
				Vector3 tangent = Vector3.Cross(binormal,normal);  

				//计算父级的前方向和目标切线的角度  
				float angle = Vector3.Angle(transform.parent.forward,tangent);  

				//胶囊体旋转相反的角度对准目标物体  
				transform.localEulerAngles = new Vector3(0,-angle,0);  

				//将UP向量朝向法线  
				transform.parent.up = normal;  

			}  
		}
		Camera.main.transform.LookAt (sphere.transform, Vector3.up);
	}  

}
