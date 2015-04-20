using UnityEngine;
using System.Collections;

public class ScrollReset : MonoBehaviour {


	public void Reset(){
		GetComponent<UnityEngine.UI.Scrollbar>().value = 0;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
