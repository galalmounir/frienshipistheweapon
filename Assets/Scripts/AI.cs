using UnityEngine;
using System.Collections.Generic;
using System;

public class AI : MonoBehaviour {

	public GameObject manager;

	//[7, 7, 8, 7, 5, 10, 9, 5, 6, 8];
	//{"hangOut", "introduce", "talk up", "Trash Talk", "Immunity", "Motivate", "Pressure", "accident", "courage", "study"};

	//2,3,4,6,9,10

	//1 , 10 are only 2

	public void assKicker(){

		//GameObject variable2 = manager.GetComponent<GameManager> ().classroom [0, 0];
		//Nodes n2 = variable2.GetComponent<Nodes> ();

		//Nodes playerN = n2;
		//bool playedAlready = false;

		bool one = false;
		bool two = false;
		bool three = false;
		bool four = false;
		bool five = false;
		bool six = false;
		bool seven = false;
		//bool eight = false;
		bool nine = false;
		bool ten = false;

		List<Nodes> iterF = new List<Nodes> {};
		List<int> rowF = new List<int> ();
		List<int> columnF = new List<int> ();

		List<Nodes> iterE = new List<Nodes> {};
		List<int> rowE = new List<int> ();
		List<int> columnE = new List<int> ();


		List<Nodes> iterN = new List<Nodes> {};
		List<int> rowN = new List<int> ();
		List<int> columnN = new List<int> ();
		//Debug.Log(string.Format("here!!!"));

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

						//Debug.Log(string.Format("{0},{1}, this",x,i));
						
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
		//int neutral = iterF.Count;

		List<int> usedAI = new List<int> ();
		//List<int> usedPlayer = new List<int> ();
		//List<int> usedNeutral = new List<int> ();
//		bool varA = false;
//		bool varB = false;
		
		for (int i = 0; i < friends; i++) {				
			//Debug.Log(string.Format("{0}",iterF [i].moveType));
			if (iterF [i].moveType == 6) {
				for (int x = 0; x < friends; x++) {
					if (x != i && ((iterF [x].moveType == 3 || iterF [x].moveType == 10) && !six)) {
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						variable.GetComponent<Nodes> ().callAction (0, 0, rowF [x], columnF [x]);
						usedAI.Add (6);
						Debug.Log(string.Format("first"));
						GameObject variable3 = manager.GetComponent<GameManager> ().classroom [rowF [x], columnF [x]];
						variable3.GetComponent<Nodes> ().callAction(0,0);
						if (iterF [x].moveType == 3){
							three = true;
							if (!three){
								Debug.Log ("contains it");
							}
						}else if (iterF [x].moveType == 10){
							ten = true;
						}
						break;
					}
				}
				for (int x = 0; x < friends; x++) {
					if (x != i && (iterF[i].moveType == 4 || iterF[i].moveType == 2|| iterF[i].moveType == 9) && !six) {
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						variable.GetComponent<Nodes> ().callAction (0, 0, rowF [x], columnF [x]);
						six = true;
						Debug.Log(string.Format("second"));
					}break;
				}
			}
			if (!three){
				Debug.Log ("second check ");
			}
			for (int z = 0; z < friends; z++) {
				if((iterF[z].moveType == 3 && !three)|| (iterF[z].moveType == 10 && !ten)){
					//Debug.Log(string.Format("{0}",iterF[i].moveType));
					if (iterF[z].moveType == 3 && !three){
						GameObject variable = manager.GetComponent<GameManager> ().classroom[rowF [z],columnF[z]];
						//variable.GetComponent<Nodes> ().isMotivated(varA);
						variable.GetComponent<Nodes> ().callAction(0,0);
						three = true;
						//					//Debug.Log(string.Format("{0}",3));
					}
					if (iterF[z].moveType == 10 && !ten){
						GameObject variable = manager.GetComponent<GameManager> ().classroom[rowF [z],columnF[z]];
						//variable.GetComponent<Nodes> ().isMotivated(varB);
						variable.GetComponent<Nodes> ().callAction(0,0);
						ten = true;
					}
					
				}
			}
			if (!three){
			Debug.Log(string.Format("Failed again"));
			}
			if(iterF[i].moveType == 4 || iterF[i].moveType == 2|| iterF[i].moveType == 9){
				bool check = false;
				for (int k = 0; k < enemmies; k++){
					if ((iterE[k].totalScore -5) < 10 && !usedAI.Contains(iterF[i].moveType)){
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						variable.GetComponent<Nodes> ().callAction (0, 0, rowE [k], rowE [k]);
						if (iterF[i].moveType == 4){
							four = true;
						}else if (iterF[i].moveType == 2){
							two = true;
						}else if (iterF[i].moveType == 9){
							nine = true;
						}
						//Debug.Log(string.Format("sdfadsfasflkewjf"));
						check = true;
						break;
					}else if ((iterN[k].totalScore -5) < 10 && !usedAI.Contains(iterN[i].moveType)){
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						//Debug.Log(string.Format("{0},{1}",rowN [k],rowN [k]));
						variable.GetComponent<Nodes> ().callAction (0, 0, 2,2);
						usedAI.Add(iterF[i].moveType);
						if (iterF[i].moveType == 4){
							four = true;
						}else if (iterF[i].moveType == 2){
							two = true;
						}else if (iterF[i].moveType == 9){
							nine = true;
						}
						check = true;
						break;
					}
				}
				if (!check){
				for (int k = 0; k < enemmies; k++){
						if ((iterF[i].moveType == 4 && ! four) || (iterF[i].moveType == 2 && !two)|| (iterF[i].moveType == 9 && !nine)){
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						variable.GetComponent<Nodes> ().callAction (0, 0, rowE [k], rowE [k]);
							if (iterF[i].moveType == 4){
								four = true;
							}else if (iterF[i].moveType == 2){
								two = true;
							}else if (iterF[i].moveType == 9){
								nine = true;
							}
						//Debug.Log(string.Format("sdfadsfasflkewjf"));
					}else if ((!usedAI.Contains(iterN[i].moveType))){
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						variable.GetComponent<Nodes> ().callAction (0, 0, rowN [k], rowN [k]);
							if (iterF[i].moveType == 4){
								four = true;
							}else if (iterF[i].moveType == 2){
								two = true;
							}else if (iterF[i].moveType == 9){
								nine = true;
							}
						//Debug.Log(string.Format("sdfadsfasflkewjf"));
					}
				}
				}
			}

		}
//		
		
		
		
	}
}
