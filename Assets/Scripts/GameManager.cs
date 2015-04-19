using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

enum WinState
{
	Win,
	Lose,
	Tie,
	Continue
};

public class GameManager : MonoBehaviour {

	private static GameManager instance;
	public static GameManager Instance{
		get{
			return instance;
		}
	}

	public GameObject[,] classroom;
	List<string> lastNames = new List<string> { 
		"Stefan","Soria","Quesenberry","Silvernail","Turrentine",
		"Jasik","Tutor","Ipock","Ayotte","Stainbrook",
		"Velazco","Branch","Ericson","Boozer","Graham",
		"Vanleer","Doherty","Mccargo","Fane","Loch",
		"Mcnerney","Capps","Keim","Metoyer","Lisle",
		"Towsend","Lager","Cosenza","Visser","Corrado",
		"Almendarez","Amerson","Byrd","Napoli","Garduno",
		"Labrie","Grass","Nicodemus","Kilmer","Yee",
		"Meserve","Aye","Lomanto","Luck","Verge", 
		"Roy","Duffy","Jacobus","Johansson","Villeda"};
	List<string> firstNames = new List<string> { 
		"Arianna","Shawna","Damian","Stanford","Benton",  
		"Elly","Venice","Maricruz","Retha","Jacqulyn",
		"Germaine","Terra","Chasity","Denna","Sabina", 
		"Diedre","Dallas","Allene","Moises","Christie",  
		"Kum","Lenard","Leah","Felipe","Leif",
		"Edelmira","Oscar","Devora","Dayle","Jenise", 
		"Mavis","Sasha","Kaycee","Linh","Wendell",
		"Malcom","Jude","Cletus","Ernesto","Lela",
		"Peggy","Tom","Agatha","Brandie","Lyman",
		"Leida","Celesta","Mariella","Azalee","Alden"};
	List<string> moves = new List<string> { 
		"HangOut","TalkUp","TrashTalk","Introduce","Immunity",
		"Motivate","PeerPressure","Prank","LiquidCourage","StudySession"};
	List<string> posts = new List<string> {
		"Do you prefer ___ or ___?","This or that? This, this or that?",
		"The best photograph I ever took was ____.","The worst photograph I ever took was ____.",
		"If I could snap my fingers and be any person in history, I’d be ___.",
		"By a show of likes, how many of you ______?",
		"What did you think of the Superbowl/Grammy’s/other event?",
		"If I could go back in time to my previous self, I’d visit age ___.",
		"Two things I cannot live without are ____ and ____.",
		"You’re stranded on a desert island with only one item. Look to your left right now. The first object you see is that item. What is it?",
		"I love my boyfriend so much! Hearts! Stars! Unicorns!",
		"Fool me once, shame on you. Fool me twice, shame on me. Revenge will never taste sweeter",
		"Here goes nothin'!","This sucks...","UGH... SO STRESSED!!! :(",
		"OMG... I'm so EXCITED!!!",	"Well, I'll never do that again",
		"I can't believe this is happening","...SIGH...",":-(",
		" I have the best friends ever!" ," going out!"," school dance tomorrow!",
		" ______ is the best person ever!"," I'm bored, anyone want to hang?",
		" I'm sick :("," can't deal with this anymore..."," I need a real job.",
		" goig to the movies!"," back from the movies!",
		" it's so hot out!"," it's so cold out!","____ just broke up with me!" ,"slept in...",
		" it's snowing!","sun!","rain rain go away..."," RAINBOW!"};
	int week = 1;
	bool pauseForAnimation = true;
	WinState currentState = WinState.Continue;
	public GameObject enemy;
	public GameObject player;
	public GameObject classManager;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else if (instance != this)
			Destroy (gameObject);

		classroom = new GameObject[4,4];
		classroom[0,0] = GameObject.FindGameObjectWithTag("Enemy");
		classroom [classroom.GetUpperBound(0),classroom.GetUpperBound(1)] = GameObject.FindGameObjectWithTag("Player");

		int index = 0;
		GameObject[] others = GameObject.FindGameObjectsWithTag("Student");

		for (int i =0; i < classroom.GetLength(0); i++) {
			for (int j =0; j < classroom.GetLength(1); j++) {
				if((i==0&&j==0)||(i==classroom.GetUpperBound(0)&&j==classroom.GetUpperBound(1)))
					continue;
				classroom[i,j] = others[index];
				index++;
			}
		}
	}
	// Use this for initialization
	void Start () {
		//instantiate player nodes
		enemy = classroom [0,0];
		enemy.GetComponent<Nodes> ().location (0, 0);
		//details

		player = classroom [classroom.GetUpperBound(0),classroom.GetUpperBound(1)];
		player.GetComponent<Nodes> ().location (classroom.GetUpperBound(0),classroom.GetUpperBound(1));
		//details

		//instantiate all neutral here
		for (int i =0; i < classroom.GetLength(0); i++) {
			for (int j =0; j < classroom.GetLength(1); j++) {
				if((i==0&&j==0)||(i==classroom.GetUpperBound(0)&&j==classroom.GetUpperBound(1)))
					continue;

				classroom[i,j].tag = "Student";
				Nodes n = classroom[i,j].GetComponent<Nodes>();
				Personality p = classroom[i,j].GetComponent<Personality>();

				string randFirst = firstNames[(int)Random.Range(0, Mathf.Round(firstNames.Count - 1))];
				firstNames.Remove(randFirst);
				string randLast = lastNames[(int)Random.Range(0, Mathf.Round(lastNames.Count - 1))];
				lastNames.Remove(randLast);
				p.name = randFirst+randLast;

				p.posts.Add(posts[(int)Random.Range(0, Mathf.Round(posts.Count - 1))]);
				p.posts.Add(posts[(int)Random.Range(0, Mathf.Round(posts.Count - 1))]);
				p.posts.Add(posts[(int)Random.Range(0, Mathf.Round(posts.Count - 1))]);
				p.posts.Add(posts[(int)Random.Range(0, Mathf.Round(posts.Count - 1))]);

				Actions randAction = (Actions)Random.Range(1, 10);
				n.GetComponent<Nodes> ().location (i, j);
				n.GetComponent<Nodes> ().nodeAction(randAction);
				//same thing for pictures
			}
		}
		SetDefaultScores ();
	}

	void SetDefaultScores(){
		classroom [3, 2].GetComponent<Nodes> ().you = 30;
		classroom [2, 3].GetComponent<Nodes> ().you = 30;
		classroom [0, 1].GetComponent<Nodes> ().them = 30;
		classroom [1, 1].GetComponent<Nodes> ().them = 30;
		classroom [1, 0].GetComponent<Nodes> ().them = 30;
	}

	void Organize (){

		GameObject[] newOrder = GameObject.FindGameObjectsWithTag("Student");
		newOrder = newOrder.OrderBy (go => (go.GetComponent<Nodes>().totalScore())).ToArray ();

		for(int row = 0, col = 0, pos = 0; pos < 4*4; pos++, ++row, --col){
			if(row > 3) {
				row = col+2;
				col = 3;
			} else if(col < 0) {
				col = row;
				row = 0;
			}
			if((row==0&&col==0)||(row==3&&col==3))
				continue;
			classroom[row,col] = newOrder[pos-1];
			classroom[row,col].GetComponent<Nodes>().location(row,col);
		}
	}

	void WinConditions(){
		bool win = false;
		bool lose = false;
		for (int i =0; i < classroom.GetLength(0); i++) {
			for (int j =0; j < classroom.GetLength(1); j++) {
				if((i==0&&j==0)||(i==classroom.GetUpperBound(0)&&j==classroom.GetUpperBound(1)))
					continue;
				win = win && !((classroom[i,j].GetComponent<Nodes>().usableE));
				lose = lose || (classroom[i,j].GetComponent<Nodes>().usableP);
			}
		}
		if (!win && !lose)
			currentState = WinState.Continue;
		if (win && !lose)
			currentState = WinState.Win;
		if ((win && lose) || week > 12)
			currentState = WinState.Tie;
		if (!win && lose)
			currentState = WinState.Lose;
	}

	void EnemyTurn(){
		//enemy.GetComponent<EnemyAI> ().executeNextTurn ();
	}

	void EndTurn(){
		week++;
		WinConditions ();
		if (currentState != WinState.Continue) {
			EndGame();
		}
	}
	
	void SetUpNewTurn(){
		for (int i =0; i < classroom.GetLength(0); i++) {
			for (int j =0; j < classroom.GetLength(1); j++) {
				classroom[i,j].GetComponent<Nodes>().status();
			}
		}
		Organize ();

		//get rid of this for animations
		classManager.GetComponent<ClassRoomManager>().Rearrange ();
	}

	public void NextTurn(){
		EnemyTurn ();

		//pauseForAnimation = true;
		EndTurn ();
		SetUpNewTurn ();
	}

	// Update is called once per frame
	void Update () {
		//if(pauseForAnimation){
		//	pauseForAnimation = MoveToPositions();
		//}
	}

	void EndGame(){
		var go = new GameObject();
		GUIText gui = go.AddComponent<GUIText>();
		go.transform.position.Set (0.5f, 0.5f, 0.0f);
		gui.text = "Game Over!";
	}
}