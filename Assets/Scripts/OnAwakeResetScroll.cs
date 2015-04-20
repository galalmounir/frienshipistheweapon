using UnityEngine;
using System.Collections;

public class OnAwakeResetScroll : MonoBehaviour {

	public bool first = true;
	// Use this for initialization
	void Start () {

	}

	void OnGUI(){
		if (first){
			transform.parent.parent.parent.FindChild("Scrollbar").GetComponent<ScrollReset>().Reset();
			first = false;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
