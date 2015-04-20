using UnityEngine;
using System.Collections;

public class HideStart : MonoBehaviour {

	public void Hide(){
		GameObject.Find ("GameManager").GetComponent<GameManager>().PlayerName = transform.FindChild("InputField").GetComponent<UnityEngine.UI.InputField>().text;
		gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
