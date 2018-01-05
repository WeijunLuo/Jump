using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Jump : MonoBehaviour {

	public enum GameState
	{
		WaitStart,InGame,GameOver
	}
	public enum Direction
	{
		Front = 0,Up =1
	}
	[Header("随机的对象")]
	public GameObject[] LandObj;

	//s = gt*t/2
	[Header("角色")]
	public Transform Player;
	[Header("第一个方块")]
	public Transform First;

	public Vector3 frontCameraOffset = new Vector3(-4.1f,4.5f,-4.6f);
	public Vector3 frontCameraRoation = new Vector3(30.9f,51.5f,0);

	public Vector3 upCameraOffset = new Vector3(-1.3f,4.5f,-4.8f);
	public Vector3 upCameraRoation = new Vector3(30.9f,13.39f,0);

	public GameObject RestartBtn;
	public float startXSpeed = 10;
	public float startYSpeed = 10;
	public float addXSpeed = 0.1f;
	public float addYSpeed = 0.1f;
	public float af = 5;

	public float smothing = 1.5f;
	// Use this for initialization
	Vector3 startPos;
	Vector3 offset;
	List<GameObject> generateObj = new List<GameObject>();
	void Start () {

//		gameState = GameState.InGame;

//		generateObj.Add (First.gameObject);
//		genObj (First);
//		startPos = Player.localPosition;
//		Player.GetComponent<Rigidbody>().AddForce(new Vector3(200,200,0));
		offset =  transform.position - Player.position;

		print (offset);
	}
	float pressTime = 0;
	bool isPress = false;
	bool isMove = false;
	bool isGenerate = false;

	GameState gameState = GameState.WaitStart;
	Direction currentDirection = Direction.Front;
	// Update is called once per frame
	void Update () {

		if (gameState == GameState.InGame && !EventSystem.current.IsPointerOverGameObject()) {

			if (!isMove && Input.GetMouseButtonDown (0)) {
				pressTime = 0;
				isPress = true;
			}

			if (isPress) {
				if (pressTime < 5)
					pressTime += Time.deltaTime;
			}
			if (!isMove && Input.GetMouseButtonUp (0)) {
				isPress = false;
				isMove = true;
				isGenerate = false;

				if(currentDirection == Direction.Front)
				Player.GetComponent<Rigidbody> ().AddForce (new Vector3 (startXSpeed + pressTime * pressTime * addXSpeed + 100, startYSpeed + pressTime * pressTime * addYSpeed + 200
				, 0));
				else
					Player.GetComponent<Rigidbody> ().AddForce (new Vector3(0, startYSpeed + pressTime * pressTime * addYSpeed + 200
						, startXSpeed + pressTime * pressTime * addXSpeed + 100));
				return;
			}
			Vector3 vel = Player.GetComponent<Rigidbody> ().velocity;

			if (vel.x == 0 && vel.y == 0 && vel.z == 0 && isMove && !isGenerate) {
				isMove = false;
				if (Player.localPosition.y < 1) {
					print ("game over");
					gameState = GameState.GameOver;
					RestartBtn.SetActive (true);
					return;
				}
				isGenerate = true;
				print (1111111);
				genObj (generateObj [generateObj.Count - 1].transform);
			}
		}

//		if(isMove)
//			updatePos ();
	}
	float passTime = 0;
	void updatePos()
	{
//		passTime += Time.deltaTime;
//
//		float x = startXSpeed * passTime - af * passTime * passTime / 2;
//		float y = startYSpeed*passTime- g * passTime * passTime / 2;
//
//		Player.localPosition = startPos + new Vector3 (x,y,0);

//		Player.GetComponent<BoxCollider>().
	}
	void FixedUpdate()
	{
		if (gameState == GameState.InGame) {
			if (currentDirection == Direction.Front) {
				Vector3 targetCampos = Player.position + frontCameraOffset;
				transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, frontCameraRoation,smothing * Time.deltaTime);
				transform.position = Vector3.Lerp (transform.position, targetCampos, smothing * Time.deltaTime);

			} else {
				Vector3 targetCampos = Player.position + upCameraOffset;
				transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, upCameraRoation,smothing * Time.deltaTime);
				transform.position = Vector3.Lerp (transform.position, targetCampos, smothing * Time.deltaTime);
			}
		}
	}

	void genObj(Transform st)
	{
		GameObject obj = GameObject.Instantiate<GameObject> (LandObj[0]);
		obj.transform.position = st.position;
		obj.transform.localScale = st.localScale;
		obj.transform.localRotation = st.localRotation;

		float xoff = Random.Range (1.5f, 5.0f);
		if (Random.Range (0, 10) < 5) {
			currentDirection = Direction.Front;
			obj.transform.position += new Vector3 (xoff, 0, 0);
		} else {
			currentDirection = Direction.Up;
			obj.transform.position += new Vector3 (0, 0, xoff);
		}
		generateObj.Add (obj);

	}
	void startGame()
	{
		First.gameObject.SetActive (true);
		Player.gameObject.SetActive (true);
		Player.transform.localPosition = new Vector3 (2.18f,1.25f,0);
		First.transform.localPosition = new Vector3 (2,0.5f,0);

		GameObject obj = GameObject.Instantiate<GameObject> (LandObj[0]);
		obj.transform.position = First.position;
		obj.transform.localScale = First.localScale;
		obj.transform.localRotation = First.localRotation;

		float xoff = Random.Range (1.5f, 5.0f);

		obj.transform.position += new Vector3 (xoff,0,0);
		generateObj.Add (obj);

		gameState = GameState.InGame;
		currentDirection = Direction.Front;
	}

	#region UI Event

	public void onClickStartGame(GameObject btn)
	{
		btn.SetActive (false);
		startGame ();
	}
	public void onClickRestartGame(GameObject btn)
	{
		btn.SetActive (false);

		for (int i = generateObj.Count - 1; i >= 0; --i) {
			generateObj [i].SetActive (false);
			DestroyImmediate (generateObj[i]);
		}
		generateObj.Clear ();
		startGame ();


	}
	#endregion

}
