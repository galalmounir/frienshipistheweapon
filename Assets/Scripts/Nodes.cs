using UnityEngine;
using System.Collections.Generic;

public enum Actions {hang, intro, talkup, trash, immunity, motivate, pressure, accident, courage, study};
public class Nodes : MonoBehaviour {
	
	public int row, column;
	public int moveType = 1;
	public int you = 0;
	public int them = 0;
	public int cost = 10;
	public bool immune = false, isMot = false, actionable = true, enemy, player; 
	public bool usableE, usableP, usableN, excpetionE = false, exceptionP = false, moved= false;
	public static string[] moves = {"Hang out", "Introduce me", "Talk me up", "Trash talk her", "Skip school", 
		"Motivate", "Peer pressure", "Prank", "Liquid courage", "Study session"};
	public string[] moveDescription = {
		"Hang out with someone to win their approval", 
		"Help me win over your friend", 
		"Make me look good in front of the class", 
		"Trash talk THAT girl to your friend",
		"Get someone to miss school to avoid HER friends", 
		"Motivate someone to help me even more", 
		"Force someone to help me", 
		"Prank someone so they can't help her", 
		"Give someone some courage, see what happens", 
		"Let's all study together!"};
	public string MovesType = "hangOut";
	public GameObject manager;
	public int youEffect = 0, themEffect= 0; 
	public int hangPts = 10;
	public int introPts = 15;
	public int talkUpPts = 3;
	public int trashTalkPts = 10;
	public int couragePts = 10;
	public int studyPts = 7;
	public int immuneInt =0;
	public int accidentInt = 0;

	void Update () {
	}
	 
	public void set(){
		player = true;
	}

	public void set(bool input){
		enemy = true;
	}

	public void nodeAction(Actions type){
		moveType = (int)type;
		MovesType = moves[moveType-1];
		if (moveType == 1 || moveType == 9) {
			youEffect = hangPts;
			themEffect = 0;
		} else if (moveType == 2){
			youEffect = introPts;
			themEffect = 0;
		} else if (moveType == 4) {
			youEffect = 0;
			themEffect = -trashTalkPts;
		} else if (moveType == 3) {
			youEffect = talkUpPts;
			themEffect = 0;
		} else if (moveType == 10) {
			youEffect = studyPts;
			themEffect = 0;
		} else {
			youEffect = 0;
			themEffect = 0;
		}
			
	}

	public void location(int in1, int in2){
		row = in1;
		column = in2;
	}

	public int totalScore{
		get{
			return you - them;
		} 
	}

	public bool isEnemy{
		get{
			return totalScore < -5 ? true : false;
		}
	}
	public bool isFriend{
		get{
			return totalScore > 5 ? true : false;
		}
	}

	public void status(){
		int local = you - them;
		if (local > 5) {
			usableP = true;
			usableN = false;
			usableE = false;
		} else if (local < -5) {
			usableE = true;
			usableP = false;
			usableN = false;
		} else {
			usableN = true;
			usableP = false;
			usableE = false;
		}

		if (immuneInt > 0) {
			isImmune (true);
			immuneInt = immuneInt - 1;
		} else {
			isImmune (false);
		}
		if (accidentInt > 0) {
			isPranked (false);
			accidentInt = accidentInt - 1;
		} else {
			isPranked (true);
		}


		isMotivated ();
		peerPressure ();
		moved = false;
	}

	public void callAction(int a, int b){
		GameObject variable2 = manager.GetComponent<GameManager>().classroom[a,b];
		Nodes n1 = variable2.GetComponent<Nodes> ();
		int temp = 1;
		
		if (isMot) {
			Debug.Log(string.Format("MOTIVATED"));
			temp = 2;
		}
		if (actionable) {
			//Debug.Log(string.Format("{0} here", moveType));
			//Debug.Log(string.Format("{0} here", a));
			//Debug.Log(string.Format("{0} here", b));
			 if (moveType == 3) {
				//Debug.Log(string.Format("got here at least"));
				int input = talkUpPts * temp;
				talkUp (variable2.GetComponent<Nodes> (), input); //player node (so it knows what to subtract)
			} else if (moveType == 10) {
				int input = studyPts * temp;
				study (input);
			}
		}
	}

	public void callAction(int a, int b, int c, int d){
		GameObject variable = manager.GetComponent<GameManager>().classroom[a,b];
		Nodes n1 = variable.GetComponent<Nodes> ();
		GameObject variable2 = manager.GetComponent<GameManager>().classroom[c,d];
		Nodes n2 = variable2.GetComponent<Nodes> ();
		int temp = 1;

		if (isMot) {
			temp = 2;
		}

		if (actionable) {
			if (moveType == 1 && !variable2.GetComponent<Nodes> ().immune) {
				int input = hangPts * temp;
				hangOut (variable2.GetComponent<Nodes> (), input); //target node
			}else if (moveType == 2 && !variable2.GetComponent<Nodes> ().immune) {
				int input = introPts * temp;
				introTT (variable.GetComponent<Nodes> (), variable2.GetComponent<Nodes> (), input, true); //player node and target node
			} else if (moveType == 4 && !variable2.GetComponent<Nodes> ().immune) {
				int input = trashTalkPts * temp;
				introTT (variable.GetComponent<Nodes> (), variable2.GetComponent<Nodes> (), input, false); 
			} else if (moveType == 9 && !variable2.GetComponent<Nodes> ().immune) {
				int input = couragePts * temp;
				themEffect = -input;
				liquid (variable.GetComponent<Nodes> (), variable2.GetComponent<Nodes> (), input);
			} else if (moveType == 5 && !variable2.GetComponent<Nodes> ().immune) {
				immunity (variable.GetComponent<Nodes> (), variable2.GetComponent<Nodes> ());
			} else if (moveType == 6 && !variable2.GetComponent<Nodes> ().immune) {
				motivate (variable.GetComponent<Nodes> (), variable2.GetComponent<Nodes> ());
			} else if (moveType == 7 && !variable2.GetComponent<Nodes> ().immune) {
				peerPressure (variable.GetComponent<Nodes> (), variable2.GetComponent<Nodes> ());
			} else if (moveType == 8) {
				accident (n1, n2);
			}
		}
	}

	public int playerS(){
		return you;
	}

	public int enemyS(){
		return them;
	}

	public void playerCh(int change){
		you = you - change;
	}

	public void enemyCh(int change){
		them = them - change;
	}

	public int youScore(){
		return you;
	}

	public int themScore(){
		return them;
	}

	public void isImmune(bool input){
		immune = input;
	}

	public void isImmune(){
		immune = false;
	}

	public void becomeImmune(){
		immuneInt = 2;
		immune = true;
	}
		
	public void haveAccident(){
		accidentInt = 2;
		actionable = false;
	}

	public void isMotivated(bool input){
		isMot = input;
	}

	public void isMotivated(){
		isMot = false;
	}

	public int random(){
		if (isMot) {
			return 1;
		}
		return 2;
	}

	public void isPranked(bool input = true){
		actionable = input;
	}


	public void cheaper(int c){
		cost = c;
	}

	public void hangOut(Nodes n1, int input){
		if (enemy) {
			n1.enemyCh (-input);
		} else if (player) {
			n1.playerCh (-input);
		}
		moved = true;
	}

	public int getRow(){
		return row;
	}
	public int getColumn(){
		return column;
	}


	public void introTT(Nodes n1, Nodes n2, int input, bool value){
		//int sum = n2.getRow () + n2.getColumn ();
		//if ((row + column) - (sum) == 1 || (row + column) - (sum) == -1) {
			if (n1.enemy) {
				enemyCh (cost);
				if (value){
					n2.enemyCh(-input);
				}else{
					n2.playerCh (input);
				}
			} else if (n1.player) {
				playerCh (cost);
				if (value){				
					n2.playerCh(-input);
				}else{
					n2.enemyCh (input);
				}
			}
			moved = true;
		}
	//}

	public void talkUp(Nodes n1, int input){
		//Debug.Log(string.Format("got here"));
		//Debug.Log(n1.enemy);
		if (n1.enemy) {
			enemyCh (cost + talkUpPts);
		} else if (n1.player) {
			playerCh (cost + talkUpPts);
		}
		for(int i = 0; i < 4; i++){
			for(int x = 0; x < 4; x++){
				GameObject variable = manager.GetComponent<GameManager>().classroom[x, i];
				if (variable.GetComponent<Nodes>().player == false && variable.GetComponent<Nodes>().enemy == false && !variable.GetComponent<Nodes>().immune){
					if (n1.enemy){
						variable.GetComponent<Nodes>().enemyCh (-input);
					}else if (n1.player){
						variable.GetComponent<Nodes>().playerCh(-input);
					}						    
				}
			}
		}
		moved = true;
	}

	public void immunity(Nodes n1, Nodes n2){
		if (n1.enemy) {
			enemyCh (cost);
		} else if (n1.player) {
			playerCh (cost);
		}
		n2.becomeImmune ();
		moved = true;
	}

	public void motivate(Nodes n1, Nodes n2){
		if (n1.enemy) {
			enemyCh (cost);
		} else if (n1.player) {
			playerCh (cost);
		}
		n2.isMotivated (true);
		moved = true;
	}

	public void peerPressure(Nodes n1, Nodes n2){
		if (n2.usableN) {
			if (n1.enemy) {
				enemyCh (cost);
				n2.excpetionE = true;
			} else if (n1.player) {
				playerCh (cost);
				n2.exceptionP = true;
			}
		}
		moved = true;
	}

	public void peerPressure(){
		exceptionP = false;
		excpetionE = false;
	}

	public void accident(Nodes n1, Nodes n2){
		if (n1.enemy) {
			enemyCh (cost);
		} else if (n1.player) {
			playerCh (cost);
		}
		n2.haveAccident();
		moved = true;
	}

	public void liquid(Nodes n1, Nodes n2, int value){
		int modfier = (int) Random.Range (1, 4);
		if (modfier < 4) {
			value = -value;
		}
		if (n1.enemy) {
			enemyCh (cost);
			n2.enemyCh (value);
		} else if (n1.player) {
			playerCh (cost);
			n2.playerCh(value);
		}

		moved = true;
	}

	public void study(int value){
		List<Nodes> adjList = manager.GetComponent<GameManager> ().AdjacentNodes (row, column);
		
		if (isEnemy || excpetionE) {
			enemyCh (cost);
			for(int i = 0;i<adjList.Count;i++){
				if(!adjList[i].player && !adjList[i].enemy && !adjList[i].immune){
					adjList[i].enemyCh(-value);
				}
			}
		} else if (isFriend || exceptionP) {
			playerCh (cost);
			for(int i = 0;i<adjList.Count;i++){
				if(!adjList[i].player && !adjList[i].enemy && !adjList[i].immune){
					adjList[i].playerCh(-value);
				}
			}
		}
		moved = true;
	}

}
