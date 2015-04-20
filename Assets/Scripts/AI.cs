using UnityEngine;
using System.Collections.Generic;
using System;

public class AI : MonoBehaviour {

	public GameObject manager;

	//[7, 7, 8, 7, 5, 10, 9, 5, 6, 8];
	//{"hangOut", "introduce", "talk up", "Trash Talk", "Immunity", "Motivate", "Pressure", "accident", "courage", "study"};

	//3,6,10

	//1 , 10 are only 2

	public void assKicker(){

		GameObject variable2 = manager.GetComponent<GameManager> ().classroom [0, 0];
		Nodes n2 = variable2.GetComponent<Nodes> ();

		Nodes playerN = n2;
		bool playedAlready = false;

		List<Nodes> iterF = new List<Nodes> {};
		List<int> rowF = new List<int> ();
		List<int> columnF = new List<int> ();

		List<Nodes> iterE = new List<Nodes> {};
		List<int> rowE = new List<int> ();
		List<int> columnE = new List<int> ();


		List<Nodes> iterN = new List<Nodes> {};
		List<int> rowN = new List<int> ();
		List<int> columnN = new List<int> ();


		for (int i = 0; i < 4; i++) {
			for (int x = 0; x < 4; x++) {
				GameObject variable = manager.GetComponent<GameManager> ().classroom [x, i];
				if (!variable.GetComponent<Nodes> ().enemy && !variable.GetComponent<Nodes> ().player) {
					if (variable.GetComponent<Nodes> ().isEnemy) {
						iterF.Add(variable.GetComponent<Nodes> ());
						rowF.Add(x);
						columnF.Add(i);
					} else if (variable.GetComponent<Nodes> ().isFriend) {
						iterE.Add(variable.GetComponent<Nodes> ());
						rowE.Add(x);
						columnE.Add(i);
						
					} else if (variable.GetComponent<Nodes> ().usableN) {
						iterN.Add(variable.GetComponent<Nodes> ());
						rowN.Add(x);
						columnN.Add(i);
					}
				}
			}
		}

		int friends = iterF.Count;
		int enemmies = iterE.Count;
		int neutral = iterF.Count;

		List<int> usedAI = new List<int> ();
		List<int> usedPlayer = new List<int> ();
		List<int> usedNeutral = new List<int> ();
		bool varA = false;
		bool varB = false;
		
		for (int i = 0; i < friends; i++) {				
			if (iterF [i].moveType == 6) {
				for (int x = 0; x < friends; x++) {
					if (x != i && ((iterF [x].moveType == 3 || iterF [x].moveType == 10) && !usedAI.Contains (6))) {
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						variable.GetComponent<Nodes> ().callAction (0, 0, rowF [i], columnF [i]);
						usedAI.Add (6);
						Debug.Log (string.Format ("{0}", 6));
						if (iterF [x].moveType == 3){
							varA = true;
						}else if (iterF [x].moveType == 10){
							varB = true;
						}
					}
				}
				for (int x = 0; x < friends; x++) {
					if (x != i && !usedAI.Contains (6)) {
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						variable.GetComponent<Nodes> ().callAction (0, 0, rowF [i], columnF [i]);
						usedAI.Add (6);
						Debug.Log (string.Format ("{0}, got here", 6));
					}
				}
			}
			for (int z = 0; z < friends; z++) {
				if(iterF[z].moveType == 3 || iterF[z].moveType == 10){
					//Debug.Log(string.Format("{0}",iterF[i].moveType));
					if (iterF[z].moveType == 3 && !usedAI.Contains(3)){
						GameObject variable = manager.GetComponent<GameManager> ().classroom[rowF [z],columnF[z]];
						variable.GetComponent<Nodes> ().isMotivated(varA);
						variable.GetComponent<Nodes> ().callAction(0,0);
						usedAI.Add(3);
						//					//Debug.Log(string.Format("{0}",3));
					}
					if (iterF[z].moveType == 10 && !usedAI.Contains(10)){
						GameObject variable = manager.GetComponent<GameManager> ().classroom[rowF [z],columnF[z]];
						variable.GetComponent<Nodes> ().isMotivated(varB);
						variable.GetComponent<Nodes> ().callAction(0,0);
						usedAI.Add(10);
						//Debug.Log(string.Format("{0}",10));
					}
					
				}
			}
			if(iterF[i].moveType == 4 || iterF[i].moveType == 2|| iterF[i].moveType == 9){
				Debug.Log("gothere!!");
				for (int k = 0; k < enemmies; k++){
					if ((iterE[k].totalScore -5) < 10 && !usedAI.Contains(iterF[i].moveType)){
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						variable.GetComponent<Nodes> ().callAction (0, 0, rowE [k], rowE [k]);
						usedAI.Add(iterF[i].moveType);
					}else if ((iterN[k].totalScore -5) < 10 && !usedAI.Contains(iterN[i].moveType)){
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						variable.GetComponent<Nodes> ().callAction (0, 0, rowN [k], rowN [k]);
						usedAI.Add(iterF[i].moveType);
					}
				}
				for (int k = 0; k < enemmies; k++){
					if (!usedAI.Contains(iterF[i].moveType)){
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						variable.GetComponent<Nodes> ().callAction (0, 0, rowE [k], rowE [k]);
						usedAI.Add(iterF[i].moveType);
					}else if ((!usedAI.Contains(iterN[i].moveType))){
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						variable.GetComponent<Nodes> ().callAction (0, 0, rowN [k], rowN [k]);
						usedAI.Add(iterF[i].moveType);
					}
				}
			}
		}
		
		
		
		
	}
}
