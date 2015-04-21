using UnityEngine;
using System.Collections;

public class ClassRoomManager : MonoBehaviour {


	public UnityEngine.UI.Image[] posts;
	public GameObject manager;
	public int currentStudent, currentTarget;
	public bool requireSecond = false, gotSecond = false;
	public Material selectedMat;
	Animator animator;
	public GameObject ActionUI;

	public void StartFB(int position){
		int x,y;
		x = position/4;
		y = (int)(((position/4.0f)-x)/ 0.25f);
		currentStudent = position;

		GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[x,y];
		GameObject fb = this.transform.FindChild("FB").gameObject;
		fb.SetActive(true);
		fb.transform.FindChild("Name").GetComponent<UnityEngine.UI.Text>().text = student.GetComponent<Personality>().fullName;
		fb.transform.FindChild("Profile Pic").FindChild("Pic").GetComponent<UnityEngine.UI.Image>().sprite = student.GetComponent<UnityEngine.UI.Image>().sprite;
		fb.transform.FindChild("About").FindChild("Birthday").GetComponent<UnityEngine.UI.Text>().text = student.GetComponent<Nodes>().moveDescription[student.GetComponent<Nodes>().moveType-1];
		fb.transform.FindChild("Power").FindChild("Effect").GetComponent<UnityEngine.UI.Text>().text =student.GetComponent<Nodes>().MovesType +"\nYou: "+ student.GetComponent<Nodes>().youEffect+ "\nThem: "+student.GetComponent<Nodes>().themEffect+ "\nCost: "+student.GetComponent<Nodes>().cost;

		for (int i=0; i<posts.Length;i++){
			posts[i].gameObject.transform.FindChild("Post Text").GetComponent<UnityEngine.UI.Text>().text = student.GetComponent<Personality>().posts[i];
			posts[i].gameObject.transform.FindChild("Post Name").GetComponent<UnityEngine.UI.Text>().text = student.GetComponent<Personality>().fullName;
			posts[i].gameObject.transform.FindChild("Comment Image").GetComponent<UnityEngine.UI.Image>().sprite = student.GetComponent<UnityEngine.UI.Image>().sprite;
			posts[i].gameObject.transform.FindChild("Post Image").GetComponent<UnityEngine.UI.Image>().sprite = student.GetComponent<UnityEngine.UI.Image>().sprite;
		}
		if(student.GetComponent<Nodes>().usableP || student.GetComponent<Nodes>().exceptionP || student.GetComponent<Nodes>().player){
			if(student.GetComponent<Nodes>().moveType == 3 || student.GetComponent<Nodes>().moveType == 10 || student.GetComponent<Nodes>().moved){
				requireSecond = false;
				fb.transform.FindChild("Power").FindChild("Pick Target").gameObject.SetActive(false);
				fb.transform.FindChild("Power").FindChild("Power").GetComponent<UnityEngine.UI.Button>().gameObject.SetActive(true);
			}
			else{
				fb.transform.FindChild("Power").FindChild("Pick Target").gameObject.SetActive(true);
				fb.transform.FindChild("Power").FindChild("Pick Target").GetComponent<UnityEngine.UI.Button>().enabled = true;
				fb.transform.FindChild("Power").FindChild("Power").GetComponent<UnityEngine.UI.Button>().gameObject.SetActive(true);
				fb.transform.FindChild("Power").FindChild("Power").GetComponent<UnityEngine.UI.Button>().enabled = false;
				requireSecond = true;
			}
		}
		else{
			fb.transform.FindChild("Power").FindChild("Pick Target").gameObject.SetActive(false);
			fb.transform.FindChild("Power").FindChild("Power").GetComponent<UnityEngine.UI.Button>().gameObject.SetActive(false);
		}

		if(student.GetComponent<Nodes>().moved){
			fb.transform.FindChild("Power").FindChild("Power").GetComponent<UnityEngine.UI.Button>().enabled = false;
		}
		else if (!requireSecond){
			fb.transform.FindChild("Power").FindChild("Power").GetComponent<UnityEngine.UI.Button>().enabled = true;
		}

	}

	public void HideFB(){
		GameObject fb = this.transform.FindChild("FB").gameObject;
		requireSecond = false;
		gotSecond = false;
		int x = currentTarget/4;
		int y = (int)(((currentTarget/4.0f)-x)/ 0.25f);
		GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[x,y];
		student.GetComponent<UnityEngine.UI.Button>().image.material = null;
		student.GetComponent<Animate>().StopShake();

		fb.SetActive (false);
	}

	public void UpdateLables(){
		GameObject[] students = GameObject.FindGameObjectsWithTag("Student");
		for (int i=0; i<students.Length;i++){
			PlayParticles(i+1);
			students[i].transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = "You: "+students[i].GetComponent<Nodes>().you + "\nThem: " +students[i].GetComponent<Nodes>().them;

			if (students[i].GetComponent<Nodes>().isMot){
				students[i].transform.FindChild("X2").gameObject.SetActive(true);
			}

			if (!students[i].GetComponent<Nodes>().actionable){
				students[i].transform.FindChild("SkipSchool").gameObject.SetActive(true);
			}

			if (students[i].GetComponent<Nodes>().immune){
				students[i].transform.FindChild("Immune").gameObject.SetActive(true);
			}
		}
	}

	void PlayParticles(int i){
		int x,y;
		x = i/4;
		y = (int)(((i/4.0f)-x)/ 0.25f);
		GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[x,y];
		int newYou = student.GetComponent<Nodes>().you - student.GetComponent<Personality>().prevYou;
		int newThem = student.GetComponent<Nodes>().them - student.GetComponent<Personality>().prevThem;

		if (newYou >0){
			student.transform.FindChild("Gain").GetComponent<ParticleSystem>().Play();
		}
		else if (newYou <0){
			student.transform.FindChild("Lose").GetComponent<ParticleSystem>().Play();
		}
		else if (newThem >0){
			student.transform.FindChild("Lose 1").GetComponent<ParticleSystem>().Play();
		}
		else if (newThem <0){
			student.transform.FindChild("Gain 1").GetComponent<ParticleSystem>().Play();
		}
	}

	void PlayParticles (int x, int y, int w, int z){
		GameObject target = manager.gameObject.GetComponent<GameManager>().classroom[w,z];
		GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[x,y];


		if (student.GetComponent<Nodes>().youEffect > 0){
			target.transform.FindChild("Gain").GetComponent<ParticleSystem>().Play();
		}

		else if (student.GetComponent<Nodes>().youEffect < 0){
			target.transform.FindChild("Lose").GetComponent<ParticleSystem>().Play();
		}

		else if (student.GetComponent<Nodes>().themEffect > 0){
			target.transform.FindChild("Lose 1").GetComponent<ParticleSystem>().Play();
		}

		else if (student.GetComponent<Nodes>().themEffect < 0){
			target.transform.FindChild("Gain 1").GetComponent<ParticleSystem>().Play();
		}

	}

	public void SaveScores(){
		GameObject[] students = GameObject.FindGameObjectsWithTag("Student");
		for (int i=0; i<students.Length;i++){
			students[i].GetComponent<Personality>().prevYou = students[i].GetComponent<Nodes>().you;
			students[i].GetComponent<Personality>().prevThem = students[i].GetComponent<Nodes>().them;
		}
	}

	public void Action(){
		int x,y,w,z;
		x = currentStudent/4;
		y = (int)(((currentStudent/4.0f)-x)/ 0.25f);

		GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[x,y];

		if(!requireSecond){
			SaveScores();
			student.GetComponent<Nodes>().callAction(3,3);
			ActionTaken(x,y,3,3);
			student.transform.FindChild("Check").gameObject.SetActive(true);
			UpdateLables();
			HideFB();
		}
		else if(gotSecond){
			w = currentTarget/4;
			z = (int)(((currentTarget/4.0f)-w)/ 0.25f);
			SaveScores();
			student.GetComponent<Nodes>().callAction(3,3,w,z);
			ActionTaken(x,y,w,z);
			student.transform.FindChild("Check").gameObject.SetActive(true);
			UpdateLables();
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

	public void VisualizeAIAction(){
		UpdateLables();
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
	void Awake () {
		GameObject students = this.transform.FindChild("Students").gameObject;
		students.GetComponent<UnityEngine.UI.GridLayoutGroup>().enabled = true;
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
				if (true){
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
		student.GetComponent<Animate>().StopShake();

		x = currentTarget/4;
		y = (int)(((currentTarget/4.0f)-x)/ 0.25f);
		student = manager.gameObject.GetComponent<GameManager>().classroom[x,y];
		student.GetComponent<UnityEngine.UI.Button>().image.material = selectedMat;
		student.GetComponent<Animate>().Shake();
		gotSecond = true;
		ResetStudentListeners();
		GameObject fb = this.transform.FindChild("FB").gameObject;

		fb.transform.FindChild("Power").FindChild("Power").GetComponent<UnityEngine.UI.Button>().enabled = true;

	}
	
	public void HandleClick(int position){
		int x = currentTarget/4;
		int y = (int)(((currentTarget/4.0f)-x)/ 0.25f);
		GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[x,y];
		student.GetComponent<UnityEngine.UI.Button>().image.material = null;
		student.GetComponent<Animate>().StopShake();

		gotSecond = false;
		StartFB(position);
	}

	public void Rearrange(){
		Invoke("Arrange", 1.5f);
	}

	void Arrange(){
		for (int i=0; i<=manager.gameObject.GetComponent<GameManager>().classroom.GetUpperBound(0);i++){
			for(int j=0; j<=manager.gameObject.GetComponent<GameManager>().classroom.GetUpperBound(1);j++){
				GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[i,j];
				if (true){
					int index = i*4 + j;
					
					student.gameObject.transform.SetSiblingIndex(index);
					
					student.transform.FindChild("Check").gameObject.SetActive(false);
					student.transform.FindChild("X2").gameObject.SetActive(false);
					student.transform.FindChild("SkipSchool").gameObject.SetActive(false);
					student.transform.FindChild("Immune").gameObject.SetActive(false);

					student.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
					student.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {HandleClick(index);});
					
					student.transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = "You: "+student.GetComponent<Nodes>().you + "\nThem: " +student.GetComponent<Nodes>().them;
					
					if(student.GetComponent<Nodes>().isEnemy){
						student.transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().color = Color.red;
					}
					else if (student.GetComponent<Nodes>().isFriend){
						student.transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().color = Color.green;
					}
					else{
						student.transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().color = Color.blue;
					}
				}
			}
		}
		AnimationRearrange();
	}
	public void AnimationRearrange(){
		GameObject[] students = GameObject.FindGameObjectsWithTag("Student");
		for (int i=0; i<students.Length;i++){
			students[i].GetComponent<Animate>().Reset();
		}
	}

	public void ActionTaken(int x, int y, int w, int z){
		GameObject target = manager.gameObject.GetComponent<GameManager>().classroom[w,z];
		GameObject student = manager.gameObject.GetComponent<GameManager>().classroom[x,y];

		GameObject temp = (GameObject) Instantiate(ActionUI,transform.position,Quaternion.identity);
		temp.transform.SetParent(transform.FindChild("History").FindChild("Actions").transform);
		temp.transform.localScale= new Vector3(1,1,1);

		temp.transform.FindChild("Attacker").GetComponent<UnityEngine.UI.Image>().sprite = student.GetComponent<UnityEngine.UI.Image>().sprite;
		temp.transform.FindChild("Target").GetComponent<UnityEngine.UI.Image>().sprite = target.GetComponent<UnityEngine.UI.Image>().sprite;
		temp.transform.FindChild("Power").FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = student.GetComponent<Nodes>().MovesType;

		//transform.FindChild("Scrollbar").GetComponent<ScrollReset>().Reset();
	}

	public void Win(){
		transform.FindChild("YouWin").gameObject.SetActive(true);
	}

	public void Lose(){
		transform.FindChild("GameOver").gameObject.SetActive(true);
	}

	public void RestartLevel(){
		Application.LoadLevel("Classroom");
	}

	public void StartMainMenu(){
		Application.LoadLevel("Main Menu");
	}


	// Update is called once per frame
	void Update () {
	
	}
}
