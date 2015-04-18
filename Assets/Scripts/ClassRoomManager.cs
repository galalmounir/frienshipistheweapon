using UnityEngine;
using System.Collections;

public class ClassRoomManager : MonoBehaviour {


	public UnityEngine.UI.Image[] posts;
	public GameObject manager;
	public int currentStudent, currentTarget;
	public bool requireSecond = false;

	public void StartFB(int position){
		int x,y;
		x = position/4;
		y = (int)(((position/4.0f)-x)/ 0.25f);
		currentStudent = position;

		GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[x,y];
		GameObject fb = this.transform.FindChild("FB").gameObject;
		fb.SetActive(true);
		fb.transform.FindChild("Name").GetComponent<UnityEngine.UI.Text>().text = student.GetComponent<Personality>().name;
		fb.transform.FindChild("Profile Pic").FindChild("Pic").GetComponent<UnityEngine.UI.Image>().sprite = student.GetComponent<UnityEngine.UI.Image>().sprite;
		fb.transform.FindChild("About").FindChild("Birthday").GetComponent<UnityEngine.UI.Text>().text = student.GetComponent<Personality>().birthday;

		Debug.Log(x.ToString()+y.ToString());

		for (int i=0; i<posts.Length;i++){
			posts[i].gameObject.transform.FindChild("Post Text").GetComponent<UnityEngine.UI.Text>().text = student.GetComponent<Personality>().posts[i];
			posts[i].gameObject.transform.FindChild("Post Name").GetComponent<UnityEngine.UI.Text>().text = student.GetComponent<Personality>().name;
			posts[i].gameObject.transform.FindChild("Comment Image").GetComponent<UnityEngine.UI.Image>().sprite = student.GetComponent<UnityEngine.UI.Image>().sprite;
			posts[i].gameObject.transform.FindChild("Post Image").GetComponent<UnityEngine.UI.Image>().sprite = student.GetComponent<UnityEngine.UI.Image>().sprite;
		}

	}


	public void Action(){
		int x,y,w,z;
		x = currentStudent/4;
		y = (int)(((currentStudent/4.0f)-x)/ 0.25f);


		GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[x,y];

		if(student.GetComponent<Nodes>().moveType == 1 || student.GetComponent<Nodes>().moveType == 3 || student.GetComponent<Nodes>().moveType == 10 )
			student.GetComponent<Nodes>().callAction(x,y);
		else if(requireSecond){
			RequireSecondChoice();
		}
		else{
			w = currentTarget/4;
			z = (int)(((currentTarget/4.0f)-w)/ 0.25f);
			student.GetComponent<Nodes>().callAction(x,y,w,z);
		}
		requireSecond = false;

	}

	void RequireSecondChoice(){
		requireSecond = true;
	}

	// Use this for initialization
	void Start () {
		Rearrange();
	}

	public void HandleClick(int position){
		if(requireSecond){
			currentTarget = position;
			requireSecond = false;
		}
		else{
			StartFB(position);
		}

	}

	public void Rearrange(){
		for (int i=0; i<=manager.gameObject.GetComponent<GameManager>().classroom.GetUpperBound(0);i++){
			for(int j=0; j<=manager.gameObject.GetComponent<GameManager>().classroom.GetUpperBound(1);j++){
				GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[i,j];
				if (student.gameObject.tag == "Student"){
					int index = i*4 + j;
					student.gameObject.transform.SetSiblingIndex(index);
					student.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {StartFB(index);});
				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
