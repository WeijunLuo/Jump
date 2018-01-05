using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Play1 : MonoBehaviour {

	//先用扫雷的方法让玩家找到雷，当玩家标记为地雷


	public float radiu = 10;
	public Transform target,mycamera,smallBall;

	public GameObject block;
	public Transform root;

	public Sprite[] number;
	public Material mineMat;

	const int w=3,l=3,h=3,mine = 5;
	int[,,] blocksInfo	 = new int[w,l,h];
	List<Vector3> mineArrayIndex = new List<Vector3> ();
	List<Vector3> markMineArrayIndex = new List<Vector3>();
	// Use this for initialization
	GameObject[,,] objInfo ;
	List<GameObject> objList = new List<GameObject>();
	void Start () {
		
		initEvent ();

		StartCoroutine (startGame());

	}
	void initEvent()
	{
		UIInfo.Instance.notice.onClick.AddListener (()=>{
			//如果提示次数不足，这里弹出窗口询问玩家是否愿意观看广告获得一次免费提示次数
			Debug.Log("notice-----------");
		});
		UIInfo.Instance.restart.onClick.AddListener (()=>{
			Debug.Log("restart game");
			StartCoroutine(startGame());
		});
	}

	IEnumerator startGame()
	{
		//初始化UI显示
		UIInfo.Instance.target_num.text = ""+mine;
		UIInfo.Instance.mark_num.text = "0";
		//删除已创建的对象
		if (objList.Count > 0)
			for (int i = 0; i < objList.Count; i += 1)
				GameObject.Destroy (objList[i]);
		//清理保存数据
		objList.Clear ();
		mineArrayIndex.Clear ();
		markMineArrayIndex.Clear ();

		objInfo = new GameObject[w*l*h*2,w*l*h*2,w*l*h*2];
		for(int i=0;i<w;i+=1)
		{
			for (int m = 0; m < l; m += 1) {

				for (int n = 0; n < h; n += 1) {
					GameObject temp = GameObject.Instantiate<GameObject> (block);
					temp.name = "" + i + "" + m + "" + n;
					temp.transform.SetParent (root);
					temp.transform.localScale = Vector3.one;
					temp.transform.localEulerAngles = Vector3.zero;
//					Material mat = new Material (Shader.Find("Standard"));
					temp.GetComponent<MeshRenderer> ().material.SetColor("mycolor",new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), 1));
//					temp.GetComponent<MeshRenderer> ().material = mat;

					//first block position is zero
					objList.Add(temp);
					if (i == 0 && m == 0 && n == 0) {
						temp.transform.localPosition = new Vector3 (0,10,0);

						temp.transform.localPosition = Vector3.zero;
						objInfo [w * l * h,w * l * h,w * l * h] = temp;

					} else {
						Vector3 arrayIndex = randomCanusePoint ();
						temp.transform.localPosition = getPositionWithArrayIndex (arrayIndex);
						objInfo [(int)arrayIndex.x,(int)arrayIndex.y,(int)arrayIndex.z] = temp;

					}



				}
			}
		}

		//随机布雷
		for (int i = 0; i < mine; i += 1) {
			mineArrayIndex.Add (randomMineArrayIndex());
			//			getObj (mineArrayIndex [i]).GetComponent<MeshRenderer> ().material = mineMat;
		}



		yield return new WaitForEndOfFrame ();
	}

	GameObject getObj(Vector3 arrayIndex)
	{
		return objInfo[(int)arrayIndex.x,(int)arrayIndex.y,(int)arrayIndex.z];
	}
	Vector3 randomMineArrayIndex()
	{
		Vector3 rArrayIndex = getArrayIndexWithPosion (objList[Random.Range(0,objList.Count)].transform.localPosition);
		if (IsMine (rArrayIndex))
			return randomMineArrayIndex ();
		else
			return rArrayIndex;
	}
	bool IsMine(Vector3 arrayIndex)
	{
		for (int i = 0; i < mineArrayIndex.Count; i += 1) {
			if (mineArrayIndex [i] == arrayIndex)
				return true;
		}
		return false;
	}
	/// <summary>
	/// Randoms the canuse point.随机生成一个与之有相邻的位置
	/// </summary>
	/// <returns>The canuse point.</returns>
	Vector3 randomCanusePoint()
	{
		GameObject temp = objList [Random.Range (0, objList.Count)];
		List<Vector3> pos = validPoint (getArrayIndexWithPosion(temp.transform.localPosition));
		if (pos.Count == 0)
			return randomCanusePoint ();
		else
			return pos [Random.Range (0, pos.Count)];
	}
	/// <summary>
	/// Gets the index of the position with array.位置转化为数组下标
	/// </summary>
	/// <returns>The position with array index.</returns>
	/// <param name="arrayIndex">Array index.</param>
	Vector3 getPositionWithArrayIndex(Vector3 arrayIndex)
	{
		// w*l*h 为0点---vector（0，0，0）
		int orgin = w*l*h;
		return new Vector3 (arrayIndex.x - orgin,arrayIndex.y - orgin,arrayIndex.z - orgin);
	}
	/// <summary>
	/// Gets the array index with posion.数组下标转化为位置
	/// </summary>
	/// <returns>The array index with posion.</returns>
	/// <param name="position">Position.</param>
	Vector3 getArrayIndexWithPosion(Vector3 position)
	{
		return position + new Vector3(w*l*h,w*l*h,w*l*h);
	}
	/// <summary>
	/// Valids the point.当前随机到的点是否是有效的点
	/// </summary>
	/// <returns>The point.</returns>
	/// <param name="arrayIndex">Array index.</param>
	List<Vector3> validPoint(Vector3 arrayIndex)
	{
		List<Vector3> pos = new List<Vector3> ();
		//down
		if (objInfo [(int)(arrayIndex.x),(int)(arrayIndex.y) - 1,(int)arrayIndex.z] == null)
			pos.Add (arrayIndex + new Vector3(0,-1,0));
		//top
		if (objInfo [(int)arrayIndex.x,(int)arrayIndex.y +1,(int)arrayIndex.z] == null)
			pos.Add (arrayIndex + new Vector3(0,1,0));
		//right
		if (objInfo [(int)arrayIndex.x+1,(int)arrayIndex.y,(int)arrayIndex.z] == null)
			pos.Add (arrayIndex + new Vector3(1,0,0));
		//left
		if (objInfo [(int)arrayIndex.x-1,(int)arrayIndex.y ,(int)arrayIndex.z] == null)
			pos.Add (arrayIndex + new Vector3(-1,0,0));
		//front
		if (objInfo [(int)arrayIndex.x,(int)arrayIndex.y ,(int)arrayIndex.z+1] == null)
			pos.Add (arrayIndex + new Vector3(0,0,1));
		//back
		if (objInfo [(int)arrayIndex.x,(int)arrayIndex.y ,(int)arrayIndex.z-1] == null)
			pos.Add (arrayIndex + new Vector3(0,0,-1));

		return pos;
		
	}
	void OnGUI()
	{
//		if (GUI.Button (new Rect (0, 0, 100, 50), "test")) {
//			listObj [0].obj.GetComponent<Rigidbody> ().AddExplosionForce(50,Vector3.zero,10);
//		}
	}
	int getPointMineNum(Vector3 arrayIndex)
	{
		int mineNum = 0;

		if (arrayIndex.x - 1 > 0 && IsMine (arrayIndex + new Vector3 (-1, 0, 0)))
			mineNum += 1;
		if (arrayIndex.x + 1 <w*l*h*2 && IsMine (arrayIndex + new Vector3 (1, 0, 0)))
			mineNum += 1;
		if (arrayIndex.y - 1 > 0 && IsMine (arrayIndex + new Vector3 (0, -1, 0)))
			mineNum += 1;
		if (arrayIndex.y + 1 <w*l*h*2 && IsMine (arrayIndex + new Vector3 (0, 1, 0)))
			mineNum += 1;
		if (arrayIndex.z - 1 > 0 && IsMine (arrayIndex + new Vector3 (0, 0, -1)))
			mineNum += 1;
		if (arrayIndex.z +1 <w*l*h*2 && IsMine (arrayIndex + new Vector3 (0, 0, 1)))
			mineNum += 1;

		return mineNum;
	}

	Vector3 startDown,startRotate;
	Quaternion startQua;
	private Camera mainCrma;
	private RaycastHit objhit;
	private Ray _ray;
	Vector3 startPos;
	Vector3 CheckMovePos;
	bool IsMarkedMine(Vector3 arrayIndex)
	{
		for (int i = 0; i < markMineArrayIndex.Count; i += 1) {
			if (markMineArrayIndex [i] == arrayIndex)
				return true;
		}
		return false;
	}
	bool isDrag = false;
	void Update () 
	{
		if (Input.GetMouseButtonDown (1)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray,out hit)) {
				print (hit.collider.gameObject.name);

				if (IsMarkedMine (getArrayIndexWithPosion (hit.collider.gameObject.transform.localPosition))) {
					SetObjMineNum (hit.collider.gameObject, -1);
					markMineArrayIndex.Remove (getArrayIndexWithPosion (hit.collider.gameObject.transform.localPosition));
				}
				else {
					markMineArrayIndex.Add (getArrayIndexWithPosion (hit.collider.gameObject.transform.localPosition));
					SetObjMineNum (hit.collider.gameObject,0);
				}
				UIInfo.Instance.mark_num.text = "" + markMineArrayIndex.Count;


			}
		}
		
		if (Input.GetMouseButtonDown(0)) 
		{
			CheckMovePos = Input.mousePosition;
			startDown = Input.mousePosition;
			target.GetComponent<Rigidbody> ().velocity = Vector3.zero;

		}



		if (Input.GetMouseButton (0)) {
			float xoff =startDown.x-Input.mousePosition.x ;
			float yoff =  startDown.y-Input.mousePosition.y ;
			target.GetComponent<Rigidbody> ().AddTorque(new Vector3(-yoff,xoff,0));
			startDown = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp (0)) {
//			target.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			if (Input.mousePosition == CheckMovePos) {
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit)) {
					print (hit.collider.gameObject.name);
					if (IsMine (getArrayIndexWithPosion (hit.collider.gameObject.transform.localPosition))) {
						print ("game over");
					} else {
						int num = getPointMineNum (getArrayIndexWithPosion (hit.collider.gameObject.transform.localPosition));
						if (num > 0) {
							SetObjMineNum (hit.collider.gameObject, num);

						} else {
					
							GameObject.DestroyImmediate (hit.collider.gameObject);
						}
					}
				}
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
	void SetObjMineNum(GameObject obj,int num)
	{
		print ("mine number--------"+num);
		Color color= obj.GetComponent<MeshRenderer> ().material.GetColor("mycolor");
		color.a = 0.4f;
		obj.GetComponent<MeshRenderer> ().material.SetColor("mycolor",color);
		for (int i = 0; i < obj.transform.childCount; i += 1) {
			if (num < 0)
				obj.transform.GetChild (i).GetComponent<SpriteRenderer> ().sprite = null;
			else {
				obj.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = number[num];
			}
			
		}
	}

}
