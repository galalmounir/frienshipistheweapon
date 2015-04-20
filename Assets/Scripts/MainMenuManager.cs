using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

	public void StartLevel(){
		Application.LoadLevel("Classroom");
	}

	public void Exit(){
		Application.Quit();
	}

	public void MainMenu(){
		Application.LoadLevel("Main Menu");
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
