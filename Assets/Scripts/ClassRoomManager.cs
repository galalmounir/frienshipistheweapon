using UnityEngine;
using System.Collections;

public class ClassRoomManager : MonoBehaviour {


	public UnityEngine.UI.Image[] posts;
	public GameObject manager;
	public int currentStudent, currentTarget;
	public bool requireSecond = false, gotSecond = false;
	public Material selectedMat;

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
		fb.transform.FindChild("Power").FindChild("Effect").GetComponent<UnityEngine.UI.Text>().text =student.GetComponent<Nodes>().MovesType +"\nYou: "+ student.GetComponent<Nodes>().youEffect+ "\nThem: "+student.GetComponent<Nodes>().themEffect+ "\n";

		for (int i=0; i<posts.Length;i++){
			posts[i].gameObject.transform.FindChild("Post Text").GetComponent<UnityEngine.UI.Text>().text = student.GetComponent<Personality>().posts[i];
			posts[i].gameObject.transform.FindChild("Post Name").GetComponent<UnityEngine.UI.Text>().text = student.GetComponent<Personality>().name;
			posts[i].gameObject.transform.FindChild("Comment Image").GetComponent<UnityEngine.UI.Image>().sprite = student.GetComponent<UnityEngine.UI.Image>().sprite;
			posts[i].gameObject.transform.FindChild("Post Image").GetComponent<UnityEngine.UI.Image>().sprite = student.GetComponent<UnityEngine.UI.Image>().sprite;
		}
		if(student.GetComponent<Nodes>().moveType == 1 || student.GetComponent<Nodes>().moveType == 3 || student.GetComponent<Nodes>().moveType == 10 || student.GetComponent<Nodes>().moved)
			requireSecond = false;
		else{
			fb.transform.FindChild("Power").FindChild("Pick Target").gameObject.SetActive(true);
			fb.transform.FindChild("Power").FindChild("Pick Target").GetComponent<UnityEngine.UI.Button>().enabled = true;

			fb.transform.FindChild("Power").FindChild("Power").GetComponent<UnityEngine.UI.Button>().enabled = false;
			requireSecond = true;
		}

		if(student.GetComponent<Nodes>().moved){
			fb.transform.FindChild("Power").FindChild("Power").GetComponent<UnityEngine.UI.Button>().enabled = false;
		}
		else if (!requireSecond){
			fb.transform.FindChild("Power").FindChild("Power").GetComponent<UnityEngine.UI.Button>().enabled = true;
		}

	}

	void HideFB(){
		GameObject fb = this.transform.FindChild("FB").gameObject;
		requireSecond = false;
		gotSecond = false;
		int x = currentTarget/4;
		int y = (int)(((currentTarget/4.0f)-x)/ 0.25f);
		GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[x,y];
		student.GetComponent<UnityEngine.UI.Button>().image.material = null;

		x = currentStudent/4;
		y = (int)(((currentStudent/4.0f)-x)/ 0.25f);
		student = manager.gameObject.GetComponent<GameManager>().classroom[x,y];
		student.transform.FindChild("Check").gameObject.SetActive(true);

		fb.SetActive (false);
	}


	public void Action(){
		int x,y,w,z;
		x = currentStudent/4;
		y = (int)(((currentStudent/4.0f)-x)/ 0.25f);

		GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[x,y];

		if(!requireSecond){
			student.GetComponent<Nodes>().callAction(x,y);
			HideFB();
		}
		else if(gotSecond){
			w = currentTarget/4;
			z = (int)(((currentTarget/4.0f)-w)/ 0.25f);
			student.GetComponent<Nodes>().callAction(x,y,w,z);
			HideFB();
		}
		else{
			GameObject fb = this.transform.FindChild("FB").gameObject;
			GameObject students = this.transform.FindChild("Students").gameObject;
			GameObject error = this.transform.FindChild("Error").gameObject;

			UnityEngine.UI.Button[] buttons = fb.GetComponentsInChildren<UnityEngine.UI.Button>();
			foreach (UnityEngine.UI.Button button in buttons){
				button.enabled = false;
			}

			buttons = students.GetComponentsInChildren<UnityEngine.UI.Button>();
			foreach (UnityEngine.UI.Button button in buttons){
				button.enabled = false;
			}

			error.SetActive(true);
		}
	}

	public void ExitError(){
		GameObject fb = this.transform.FindChild("FB").gameObject;
		GameObject students = this.transform.FindChild("Students").gameObject;
		GameObject error = this.transform.FindChild("Error").gameObject;
		
		UnityEngine.UI.Button[] buttons = fb.GetComponentsInChildren<UnityEngine.UI.Button>();
		foreach (UnityEngine.UI.Button button in buttons){
			button.enabled = true;
		}
		
		buttons = students.GetComponentsInChildren<UnityEngine.UI.Button>();
		foreach (UnityEngine.UI.Button button in buttons){
			button.enabled = true;
		}
		
		error.SetActive(false);
	}
	// Use this for initialization
	void Start () {
	}

	public void PickClicked(){
		for (int i=0; i<=manager.gameObject.GetComponent<GameManager>().classroom.GetUpperBound(0);i++){
			for(int j=0; j<=manager.gameObject.GetComponent<GameManager>().classroom.GetUpperBound(1);j++){
				GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[i,j];
				if (student.gameObject.tag == "Student"){
					int index = i*4 + j;

					student.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
					student.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {PickTarget(index);});
					
				}
			}
		}
	}

	void ResetStudentListeners(){
		for (int i=0; i<=manager.gameObject.GetComponent<GameManager>().classroom.GetUpperBound(0);i++){
			for(int j=0; j<=manager.gameObject.GetComponent<GameManager>().classroom.GetUpperBound(1);j++){
				GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[i,j];
				if (student.gameObject.tag == "Student"){
					int index = i*4 + j;
					
					student.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
					student.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {HandleClick(index);});
					
				}
			}
		}
	}
	public void PickTarget(int position){
		int prevTarget = currentTarget;
		currentTarget = position;
		int x = prevTarget/4;
		int y = (int)(((prevTarget/4.0f)-x)/ 0.25f);
		GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[x,y];
		student.GetComponent<UnityEngine.UI.Button>().image.material = null;
		
		x = currentTarget/4;
		y = (int)(((currentTarget/4.0f)-x)/ 0.25f);
		student = manager.gameObject.GetComponent<GameManager>().classroom[x,y];
		student.GetComponent<UnityEngine.UI.Button>().image.material = selectedMat;
		gotSecond = true;
		ResetStudentListeners();

	}
	
	public void HandleClick(int position){
		int x = currentTarget/4;
		int y = (int)(((currentTarget/4.0f)-x)/ 0.25f);
		GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[x,y];
		student.GetComponent<UnityEngine.UI.Button>().image.material = null;
		gotSecond = false;
		StartFB(position);


	}

	public void Rearrange(){
		for (int i=0; i<=manager.gameObject.GetComponent<GameManager>().classroom.GetUpperBound(0);i++){
			for(int j=0; j<=manager.gameObject.GetComponent<GameManager>().classroom.GetUpperBound(1);j++){
				GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[i,j];
				if (student.gameObject.tag == "Student"){
					int index = i*4 + j;
					student.gameObject.transform.SetSiblingIndex(index);
					student.transform.FindChild("Check").gameObject.SetActive(false);
					student.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
					student.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {HandleClick(index);});
					Debug.Log(student.gameObject.name + index);

				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
