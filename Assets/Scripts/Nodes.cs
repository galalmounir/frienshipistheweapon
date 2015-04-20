using UnityEngine;
using System.Collections.Generic;

public enum Actions {hang, intro, talkup, trash, immunity, motivate, pressure, accident, courage, study};
public class Nodes : MonoBehaviour {
	
	public static int row, column;
	public int moveType = 1;
	public int you = 0;
	public int them = 0;
	public int cost = 10;
	public bool immune = false, isMot = false, actionable = true, enemy, player; 
	public bool usableE, usableP, usableN, excpetionE = false, exceptionP = false, moved= false;
	public static string[] moves = {"Hang out", "Introduce me", "Talk me up", "Trash talk her", "Skip school", "Motivate", "Peer pressure", "Prank", "Liquid courage", "Study session"};
	public string MovesType = "hangOut";
	public GameObject manager;
	public int youEffect = 0, themEffect= 0; 
	public int hangPts = 10;
	public int introPts = 10;
	public int talkUpPts = 3;
	public int trashTalkPts = 10;
	public int couragePts = 10;
	public int studyPts = 7;

	void Update () {
	}

	public void set(){
		player = true;
	}

	public void set(bool input){
		enemy = true;
	}

	public void nodeAction(Actions type){
		cost = 10;
//		int hangPts = 10;
//		int introPts = 10;
//		int talkUpPts = 3;
//		int trashTalkPts = 10;
//		int couragePts = 10;
//		int studyPts = 7;
		
		moveType = (int)type;
		MovesType = moves[moveType-1];
		if (moveType == 1 || moveType == 9 || moveType == 2) {
			youEffect = 10;
			themEffect = 0;
		} else if (moveType == 4) {
			youEffect = 0;
			themEffect = -10;
		} else if (moveType == 3) {
			youEffect = 5;
			themEffect = 0;
		} else if (moveType == 10) {
			youEffect = 7;
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

		isImmune ();
		isMotivated ();
		isPranked ();
		peerPressure ();
		moved = false;
	}

	public void callAction(int a, int b){
		GameObject variable2 = manager.GetComponent<GameManager>().classroom[a,b];
		Nodes n1 = variable2.GetComponent<Nodes> ();
		int temp = 1;
		
		if (isMot) {
			temp = 2;
		}
		Debug.Log(string.Format("{0} here", temp));
		if (actionable) {
			//Debug.Log(string.Format("{0} here", moveType));
			//Debug.Log(string.Format("{0} here", a));
			//Debug.Log(string.Format("{0} here", b));
			if (moveType == 1 && !variable2.GetComponent<Nodes> ().immune) {
				int input = hangPts * temp;
				hangOut (variable2.GetComponent<Nodes> (), input); //target node
			} else if (moveType == 3) {
				//Debug.Log(string.Format("got here at least"));
				int input = talkUpPts * temp;
				talkUp (variable2.GetComponent<Nodes> (), input); //player node (so it knows what to subtract)
			} else if (moveType == 10) {
				int input = studyPts * temp;
				if (row < 3 && row > 0 && column > 0 && column < 3) { //first pair is player node, rest are targets
					int t1 = row + 1;
					int t2 = row - 1;
					int t3 = column + 1;
					int t4 = column - 1;
					study (a, b, row, t3, row, t4, t2, column, t1, column, input);
				} else if (row < 3 && row > 0 && (column == 0 || column == 3)) {
					int t1 = row + 1;
					int t2 = row - 1;
					int t3 = column;
					if (column == 0) {
						t3 = column + 1;
					} else {
						t3 = column - 1;
					}
					study (a, b, row, t3, t2, column, t1, column, input);
				} else if ((row == 3 || row == 0) && column > 0 && column < 3) {
					int t1 = column + 1;
					int t2 = column - 1;
					int t3 = row;
					if (column == 0) {
						t3 = row + 1;
					} else {
						t3 = row - 1;
					}
					study (a, b, row, t1, row, t2, t3, column, input);
				} else if ((row == 3 || row == 0) && (column == 0 || column == 3)) {
					int t1 = row;
					int t2 = column;
					if (row == 3) {
						t1 = row - 1;
					} else {
						t1 = row + 1;
					}
					if (column == 3) {
						t2 = column - 1;
					} else {
						t2 = column + 1;
					}
					study (a, b, t1, column, row, t2, input);
				}
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
			if (moveType == 2 && !variable2.GetComponent<Nodes> ().immune) {
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
		
	public void isMotivated(bool input){
		isMot = input;
	}

	public void isMotivated(){
		isMot = false;
	}

	public void isPranked(bool input){
		actionable = input;
	}

	public void isPranked(){
		actionable = true;
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
		int sum = n2.getRow () + n2.getColumn ();
		//if ((row + column) - (sum) == 1 || (row + column) - (sum) == -1) {
			if (n1.enemy) {
				enemyCh (cost);
				if (value){
					n2.enemyCh (-input);
				}else{
					n2.playerCh(input);
				}
			} else if (n1.player) {
				playerCh (cost);
				if (value){
					n2.playerCh (-input);
				}else{
					n2.enemyCh(input);
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
		n2.isImmune (true);
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
		n2.isPranked(false);
		moved = true;
	}

	public void liquid(Nodes n1, Nodes n2, int value){
		int modfier = (int) Random.Range (-2, 2);
		if (n1.enemy) {
			enemyCh (cost);
			n2.enemyCh (value*modfier);
		} else if (n1.player) {
			playerCh (cost);
			n2.playerCh(value*modfier);
		}
		moved = true;
	}

	public void study(int n1, int n2,int n3,int n4, int n5, int n6, int n7, int n8, int n9, int n10, int value){
		GameObject variable1 = manager.GetComponent<GameManager>().classroom[n1,n2];
		GameObject variable2 = manager.GetComponent<GameManager>().classroom[n3,n4];
		GameObject variable3 = manager.GetComponent<GameManager>().classroom[n5,n6];
		GameObject variable4 = manager.GetComponent<GameManager>().classroom[n7,n8];
		GameObject variable5 = manager.GetComponent<GameManager>().classroom[n9,n10];
		Nodes nod1 = variable1.GetComponent<Nodes> ();
		Nodes nod2 = variable2.GetComponent<Nodes> ();
		Nodes nod3 = variable3.GetComponent<Nodes> ();
		Nodes nod4 = variable4.GetComponent<Nodes> ();
		Nodes nod5 = variable5.GetComponent<Nodes> ();

		if (nod1.enemy) {
			enemyCh (cost);
			if (!nod2.player && !nod2.enemy && !nod2.immune){
				nod2.enemyCh(-value);
			}
			if (!nod3.player && !nod3.enemy && !nod3.immune){
				nod3.enemyCh(-value);
			}
			if (!nod4.player && !nod4.enemy && !nod4.immune){
				nod4.enemyCh(-value);
			}
			if (!nod5.player && !nod5.enemy && !nod5.immune){
				nod5.enemyCh(-value);
			}
		} else if (nod1.player) {
			playerCh (cost);
			if (!nod2.player && !nod2.enemy && !nod2.immune){
				nod2.playerCh(-value);
			}
			if (!nod3.player && !nod3.enemy && !nod3.immune){
				nod3.playerCh(-value);
			}
			if (!nod4.player && !nod4.enemy && !nod4.immune){
				nod4.playerCh(-value);
			}
			if (!nod5.player && !nod5.enemy && !nod5.immune){
				nod5.playerCh(-value);
			}
		}
		moved = true;
	}

	public void study(int n1, int n2,int n3,int n4, int n5, int n6, int n7, int n8, int value){
		GameObject variable1 = manager.GetComponent<GameManager>().classroom[n1,n2];
		GameObject variable2 = manager.GetComponent<GameManager>().classroom[n3,n4];
		GameObject variable3 = manager.GetComponent<GameManager>().classroom[n5,n6];
		GameObject variable4 = manager.GetComponent<GameManager>().classroom[n7,n8];
		Nodes nod1 = variable1.GetComponent<Nodes> ();
		Nodes nod2 = variable2.GetComponent<Nodes> ();
		Nodes nod3 = variable3.GetComponent<Nodes> ();
		Nodes nod4 = variable4.GetComponent<Nodes> ();
		
		if (nod1.enemy) {
			enemyCh (cost);
			if (!nod2.player && !nod2.enemy && !nod2.immune){
				nod2.enemyCh(-value);
			}
			if (!nod3.player && !nod3.enemy && !nod3.immune){
				nod3.enemyCh(-value);
			}
			if (!nod4.player && !nod4.enemy && !nod4.immune){
				nod4.enemyCh(-value);
			}
		} else if (nod1.player) {
			playerCh (cost);
			if (!nod2.player && !nod2.enemy && !nod2.immune){
				nod2.playerCh(-value);
			}
			if (!nod3.player && !nod3.enemy && !nod3.immune){
				nod3.playerCh(-value);
			}
			if (!nod4.player && !nod4.enemy && !nod4.immune){
				nod4.playerCh(-value);
			}
		}
		moved = true;
	}
	public void study(int n1, int n2,int n3,int n4, int n5, int n6, int value){
		GameObject variable1 = manager.GetComponent<GameManager>().classroom[n1,n2];
		GameObject variable2 = manager.GetComponent<GameManager>().classroom[n3,n4];
		GameObject variable3 = manager.GetComponent<GameManager>().classroom[n5,n6];
		Nodes nod1 = variable1.GetComponent<Nodes> ();
		Nodes nod2 = variable2.GetComponent<Nodes> ();
		Nodes nod3 = variable3.GetComponent<Nodes> ();
		
		if (nod1.enemy) {
			enemyCh (cost);
			if (!nod2.player && !nod2.enemy && !nod2.immune){
				nod2.enemyCh(-value);
			}
			if (!nod3.player && !nod3.enemy && !nod3.immune){
				nod3.enemyCh(-value);
			}
		} else if (nod1.player) {
			playerCh (cost);
			if (!nod2.player && !nod2.enemy && !nod2.immune){
				nod2.playerCh(-value);
			}
			if (!nod3.player && !nod3.enemy && !nod3.immune){
				nod3.playerCh(-value);
			}
		}
		moved = true;
	}

}
