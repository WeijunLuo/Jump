using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIInfo : MonoBehaviour {
	public static UIInfo Instance;
	public Text target_num,mark_num;
	public Button restart, notice;
	void Awake()
	{
		Instance = this;
	}
}
