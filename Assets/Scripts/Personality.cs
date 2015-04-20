using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Personality : MonoBehaviour {
	public string fullName;
	public string birthday;
	public List<string> posts;

	public int prevYou, prevThem;
	// Use this for initialization
	void Start () {
		prevYou = GetComponent<Nodes>().you;
		prevThem = GetComponent<Nodes>().them;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
