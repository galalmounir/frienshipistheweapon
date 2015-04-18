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

	public GameObject manager;
	public int youEffect = 0, themEffect= 0; 
	// NOTE: WHEN YOU KNOW THIS WORKS, MODIFY SO THAT EXCEPTIONS ALLOW USE WITHOUT PENELTY

	public void nodeAction(Actions type){
		moveType = (int)type;
	}

	public void location(int in1, int in2){
		row = in1;
		column = in2;
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

		if (moveType == 1) {
			int input = 10;
			hangOut (n1, input);
			youEffect = input;
			themEffect = 0;
		}else if (moveType == 3) {
			int input = 5;
			//talkUp (n1, input);
			youEffect = input;
			themEffect = 0;
		}
	}

	public void callAction(int a, int b, int c, int d){
		GameObject variable = manager.GetComponent<GameManager>().classroom[a,b];
		Nodes n1 = variable.GetComponent<Nodes> ();
		GameObject variable2 = manager.GetComponent<GameManager>().classroom[c,d];
		Nodes n2 = variable2.GetComponent<Nodes> ();

		if (moveType == 2) {
			int input = 10;
			youEffect = input;
			themEffect = 0;
			introTT (n1, n2, input);
		} else if (moveType == 4) {
			int input = 10;
			youEffect = input;
			themEffect = 0;
			introTT (n1, n2, -input);
		} else if (moveType == 9) {
			int input = 10;
			youEffect = 0;
			themEffect = -input;
			liquid (n1, n2, input);
		}else if (moveType == 5) {
			immunity (n1, n2);
		} else if (moveType == 6) {
			motivate (n1, n2);
		} else if (moveType == 7) {
			peerPressure (n1, n2);
		} else if (moveType == 8) {
			accident (n1, n2);
		}
	}

	public void callAction(int a, int b, int c, int d, int e, int f){
		GameObject variable = manager.GetComponent<GameManager>().classroom[a,b];
		Nodes n1 = variable.GetComponent<Nodes> ();
		GameObject variable2 = manager.GetComponent<GameManager>().classroom[c,d];
		Nodes n2 = variable2.GetComponent<Nodes> ();
		GameObject variable3 = manager.GetComponent<GameManager>().classroom[e,f];
		Nodes n3 = variable3.GetComponent<Nodes> ();
		if (moveType == 10) {
			int input = 7;
			youEffect = input;
			themEffect = 0;
			study (n1, n2, n3, input);
		}
	}

	public int totalScore(){
		int local = you - them;
		return local;
	}

	public int playerS(){
		return you;
	}

	public int enemyS(){
		return them;
	}

	public void playerCh(int change){
		you = you + change;
	}

	public void enemyCh(int change){
		them = them + change;
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
			n1.enemyCh (input);
		} else if (player) {
			n1.playerCh (input);
		}
		moved = true;
	}

	public int getRow(){
		return row;
	}
	public int getColumn(){
		return column;
	}

	public void introTT(Nodes n1, Nodes n2, int input){
		int sum = n2.getRow () + n2.getColumn ();
		if ((row + column) - (sum) == 1 || (row + column) - (sum) == -1) {
			if (n1.enemy) {
				enemyCh (cost);
				n2.enemyCh (input);
			} else if (n1.player) {
				playerCh (cost);
				n2.playerCh (input);
			}
			moved = true;
		}
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
		n2.isPranked(true);
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

	public void study(Nodes n1, Nodes n2,Nodes n3, int value){
		if (n1.enemy) {
			enemyCh (cost);
			n2.enemyCh(value);
			n3.enemyCh (value);
		} else if (n1.player) {
			playerCh (cost);
			n2.playerCh(value);
			n3.playerCh(value);
		}
		moved = true;
	}


}
