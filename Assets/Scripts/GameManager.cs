using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum WinState
{
	Win,
	Lose,
	Tie,
	Continue
};

public class GameManager : MonoBehaviour {
	public string PlayerName;

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
	public int week = 1;
	public WinState currentState = WinState.Continue;
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

	public void SetPlayerName(){
		classroom[3,3].GetComponent<Personality>().fullName = PlayerName;
	}
	// Use this for initialization
	void Start () {
		//instantiate all neutral here
		for (int i =0; i < classroom.GetLength(0); i++) {
			for (int j =0; j < classroom.GetLength(1); j++) {
				classroom[i,j].tag = "Student";
				Nodes n = classroom[i,j].GetComponent<Nodes>();
				Personality p = classroom[i,j].GetComponent<Personality>();

				string randFirst = firstNames[(int)Random.Range(0, Mathf.Round(firstNames.Count))];
				firstNames.Remove(randFirst);
				string randLast = lastNames[(int)Random.Range(0, Mathf.Round(lastNames.Count))];
				lastNames.Remove(randLast);
				p.fullName = randFirst+" " +randLast;

				p.posts.Add(posts[(int)Random.Range(0, Mathf.Round(posts.Count))]);
				p.posts.Add(posts[(int)Random.Range(0, Mathf.Round(posts.Count))]);
				p.posts.Add(posts[(int)Random.Range(0, Mathf.Round(posts.Count))]);
				p.posts.Add(posts[(int)Random.Range(0, Mathf.Round(posts.Count))]);

				Actions randAction = (Actions)Random.Range(2, 11);

				n.GetComponent<Nodes> ().location (i, j);
				n.GetComponent<Nodes> ().nodeAction(randAction);
				n.GetComponent<Nodes> ().cost = 5;
				//same thing for pictures
			}
		}
		//instantiate player nodes
		classroom [0, 0].tag = "Enemy";
		enemy = classroom [0,0];
		enemy.GetComponent<Nodes> ().nodeAction((Actions) ((int)Actions.hang + 1) );
		enemy.GetComponent<Nodes> ().enemy = true;
		//details

		classroom [classroom.GetUpperBound(0),classroom.GetUpperBound(1)].tag = "Player";
		player = classroom [classroom.GetUpperBound(0),classroom.GetUpperBound(1)];
		player.GetComponent<Nodes> ().nodeAction((Actions) ((int)Actions.hang + 1) );
		player.GetComponent<Nodes> ().set();

		SetDefaultScores ();
		SetUpNewTurn ();
	}

	public List<Nodes> AdjacentNodes(int x, int y){
		List<Nodes> adjList = new List<Nodes>();
		if (x > 0 && !(x - 1 == 0 && y==0)) 
			adjList.Add(classroom [x - 1, y].GetComponent<Nodes>());
		if (x < classroom.GetUpperBound(0) && !(x +1 == classroom.GetUpperBound(0) && y==classroom.GetUpperBound(1))) 
			adjList.Add (classroom [x + 1, y].GetComponent<Nodes>());
		if (y > 0 && !(x == 0 && y-1==0)) 
			adjList.Add (classroom [x, y-1].GetComponent<Nodes>());
		if (y < classroom.GetUpperBound(1) && !(x == classroom.GetUpperBound(0) && y+1==classroom.GetUpperBound(1))) 
			adjList.Add (classroom [x, y+1].GetComponent<Nodes>());
		return adjList;
	}

	void SetDefaultScores(){
		classroom [3, 2].GetComponent<Nodes> ().you = 30;
		classroom [3, 2].GetComponent<Nodes> ().nodeAction((Actions) ((int)Actions.pressure + 1) );
		classroom [2, 3].GetComponent<Nodes> ().you = 30;
		classroom [0, 1].GetComponent<Nodes> ().them = 30;
		classroom [1, 0].GetComponent<Nodes> ().them = 30;
	}

	public int currentAllies{
		get{
			int totalAllies = 0;
			for (int i =0; i < classroom.GetLength(0); i++) {
				for (int j =0; j < classroom.GetLength(1); j++) {
					if((i==0&&j==0)||(i==classroom.GetUpperBound(0)&&j==classroom.GetUpperBound(1)))
						continue;
					totalAllies += (classroom[i,j].GetComponent<Nodes>().isFriend) ? 1 : 0;
				}
			}
			return totalAllies;
		}
	}

	public int currentEnemies{
		get{
			int totalEnemies = 0;
			for (int i =0; i < classroom.GetLength(0); i++) {
				for (int j =0; j < classroom.GetLength(1); j++) {
					if((i==0&&j==0)||(i==classroom.GetUpperBound(0)&&j==classroom.GetUpperBound(1)))
						continue;
					totalEnemies += (classroom[i,j].GetComponent<Nodes>().isEnemy) ? 1 : 0;
				}
			}
			return totalEnemies;
		}
	}

	void Organize (){

		GameObject[] newOrder = GameObject.FindGameObjectsWithTag("Student");
		newOrder = newOrder.OrderBy (go => (go.GetComponent<Nodes>().totalScore)).ToArray ();

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
		bool win = true;
		bool lose = true;
		for (int i =0; i < classroom.GetLength(0); i++) {
			for (int j =0; j < classroom.GetLength(1); j++) {
				if((i==0&&j==0)||(i==classroom.GetUpperBound(0)&&j==classroom.GetUpperBound(1)))
					continue;
				Nodes n = classroom[i,j].GetComponent<Nodes>();
				win = win && !(n.isEnemy);
				lose = lose && !(n.isFriend);
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
		for (int i =0; i < classroom.GetLength(0); i++) {
			for (int j =0; j < classroom.GetLength(1); j++) {
				classroom[i,j].GetComponent<Nodes>().status();
			}
		}
		enemy.GetComponent<AI> ().assKicker ();
		classManager.GetComponent<ClassRoomManager> ().VisualizeAIAction ();
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
		classManager.GetComponent<ClassRoomManager>().Rearrange ();
	}

	public void NextTurn(){
		EnemyTurn ();

		//pauseForAnimation = true;
		EndTurn ();
		SetUpNewTurn ();
	}

	void EndGame(){
		if (currentState == WinState.Win)
			classManager.GetComponent<ClassRoomManager> ().Win ();
		if (currentState == WinState.Lose || currentState == WinState.Tie)
			classManager.GetComponent<ClassRoomManager> ().Lose ();
	}

}
