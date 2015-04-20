using UnityEngine;
using System.Collections;

public class Animate : MonoBehaviour {

	public Vector2 originalPosition, speed, targetPos;

	Animator animator;

	public void Shake(){
		animator.SetBool("Shake",true);
	}

	public void StopShake(){
		animator.SetBool("Shake",false);
	}


	// Use this for initialization
	void Awake () {
		animator = GetComponent<Animator>();
	}

	public void Reset (){
		animator.SetTrigger("Reset");

	}

	void DisableGrid(){
		gameObject.GetComponentInParent<UnityEngine.UI.GridLayoutGroup>().enabled = false;
	}

	void EnableGrid(){
		gameObject.GetComponentInParent<UnityEngine.UI.GridLayoutGroup>().enabled = true;
	}
	// Update is called once per frame
	void Update () {

	}
}
