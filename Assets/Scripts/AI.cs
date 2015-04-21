using UnityEngine;
using System.Collections.Generic;
using System;

public class AI : MonoBehaviour {

	public GameObject manager;

	//[7, 7, 8, 7, 5, 10, 9, 5, 6, 8];
	//{"hangOut", "introduce", "talk up", "Trash Talk", "Immunity", "Motivate", "Pressure", "accident", "courage", "study"};

	//2,3,4,6,9,10

	//1 ,3, 10 are only 2

	public void assKicker(){

		//GameObject variable2 = manager.GetComponent<GameManager> ().classroom [0, 0];
		//Nodes n2 = variable2.GetComponent<Nodes> ();

		//Nodes playerN = n2;
		//bool playedAlready = false;



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
				if (!variable.GetComponent<Nodes> ().enemy && (!variable.GetComponent<Nodes>().player)) {
					if (variable.GetComponent<Nodes> ().isEnemy) {
						iterF.Add (variable.GetComponent<Nodes> ());
						rowF.Add (x);
						columnF.Add (i);


					} else if (variable.GetComponent<Nodes> ().isFriend) {
						iterE.Add (variable.GetComponent<Nodes> ());
						rowE.Add (x);
						columnE.Add (i);

						//Debug.Log(string.Format("{0},{1}, this",x,i));
						
					} else if (variable.GetComponent<Nodes> ().usableN) {
						iterN.Add (variable.GetComponent<Nodes> ());
						rowN.Add (x);
						columnN.Add (i);
					}
				}
			}
		}

		int friends = iterF.Count;
		int enemmies = iterE.Count;
		int neutral = iterN.Count;

		int[] usedVal = new int[friends];
		int[] usedE = new int[enemmies];
		int[] usedN = new int[neutral];

		List<int> usedAI = new List<int> ();
		//List<int> usedPlayer = new List<int> ();
		//List<int> usedNeutral = new List<int> ();
//		bool varA = false;
//		bool varB = false;

		bool yourT = false;

		for (int k = 0; k < enemmies; k++) {
			if ((iterE [k].totalScore - 5) < 10  && usedE[k] == 0 && yourT == false) {
				GameObject variable = manager.GetComponent<GameManager> ().classroom [0,0];
				Debug.Log (string.Format ("Action first, {0},{1}", rowE [k], columnE [k]));
				if (variable.GetComponent<Nodes> ().actionable){
				variable.GetComponent<Nodes> ().callAction (0,0,rowE [k], columnE [k]);
				yourT = true;
				usedE[k] = 1;
				transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(0,0,rowE [k], columnE [k]);
				break;
				}
				
			}
		}
		for (int k = 0; k < neutral; k++) {
			if ((iterN [k].totalScore - 5) < 10 && usedN[k] == 0 && yourT == false) {
				GameObject variableN = manager.GetComponent<GameManager> ().classroom [0,0];
				//Debug.Log(string.Format("{0},{1}",rowN [k],rowN [k]));
				Debug.Log (string.Format ("Action second, {0},", k));
				if (variableN.GetComponent<Nodes> ().actionable){
				variableN.GetComponent<Nodes> ().callAction (0,0,rowN [k], columnN [k]);
				yourT = true;
				usedN[k] = 1;
				
				Debug.Log (string.Format ("second"));
				transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(0,0,rowN [k], columnN[k]);
				break;
				}
				
			}
		}

		for (int k = 0; k < enemmies; k++) {
			if (yourT == false) {
				GameObject variable = manager.GetComponent<GameManager> ().classroom [0,0];
				Debug.Log (string.Format ("Action first, {0},{1}", rowE [k], columnE [k]));
				if (variable.GetComponent<Nodes> ().actionable){
				variable.GetComponent<Nodes> ().callAction (0,0,rowE [k], columnE [k]);
				yourT = true;
				usedE[k] = 1;
				transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(0,0,rowE [k], columnE [k]);
				break;
				}
				
			}
		}
		
		for (int i = 0; i < friends; i++) {				
			if (iterF [i].moveType == 6 && usedVal[i] == 0) {
				for (int x = 0; x < friends; x++) {
					if (x != i && ((iterF [x].moveType == 3 || iterF [x].moveType == 10) && usedVal[i] == 0)) {
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						if (variable.GetComponent<Nodes> ().actionable){
						variable.GetComponent<Nodes> ().callAction (0, 0, rowF [x], columnF [x]);
						usedAI.Add (6);
						Debug.Log (string.Format ("first"));
						transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowF[i], columnF[i],rowF [x], columnF [x]);

						usedVal[i] = 1;
						break;
						}
					}
				}
				for (int x = 0; x < friends; x++) {
					if (x != i && (iterF [x].moveType == 4 || iterF [x].moveType == 2 || iterF [x].moveType == 9) && usedVal[i] == 0) {
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						if (variable.GetComponent<Nodes> ().actionable){
						variable.GetComponent<Nodes> ().callAction (0, 0, rowF [x], columnF [x]);
						transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowF[i], columnF[i],rowF [x], columnF [x]);
						usedVal[i] = 1;
						Debug.Log (string.Format ("second"));
						break;
						}
					}
				}
			}
		}//use 6
		for (int z = 0; z < friends; z++) {
			if ((iterF [z].moveType == 3 && usedVal[z] == 0) || (iterF [z].moveType == 10 && usedVal[z] == 0)) {
				Debug.Log(string.Format("{0}, GETSH ASDFASKD",iterF[z].moveType));
				if (iterF [z].moveType == 3 && usedVal[z] == 0) {
					GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [z], columnF [z]];
					if (variable.GetComponent<Nodes> ().actionable){
					transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowF[z], columnF[z],0,0);
					variable.GetComponent<Nodes> ().callAction (0, 0);
					usedVal[z] = 1;
					}

					//					//Debug.Log(string.Format("{0}",3));
				}
				if (iterF [z].moveType == 10 && usedVal[z] == 0) {
					GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [z], columnF [z]];
					if (variable.GetComponent<Nodes> ().actionable){
					transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowF[z], columnF[z],0,0);
					variable.GetComponent<Nodes> ().callAction (0, 0);
					usedVal[z] = 1;
					}
		
				}
				
			}
		}//use, 3, 10


		for (int i = 0; i < friends; i++) {
			if (((iterF [i].moveType == 4 && usedVal[i] == 0) || (iterF [i].moveType == 2 && usedVal[i] == 0) || (usedVal[i] == 0 && iterF [i].moveType == 9))) {
				for (int k = 0; k < enemmies; k++) {
					if ((iterE [k].totalScore - 5) < 10  && usedE[k] == 0 && (iterF [i].moveType == 4 && usedVal[i] == 0) || 
					    (iterF [i].moveType == 2 && usedVal[i] == 0) || 
					    (iterF [i].moveType == 9 && usedVal[i] == 0)) {
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						Debug.Log (string.Format ("Action first, {0},{1}", rowE [k], columnE [k]));
						if (variable.GetComponent<Nodes> ().actionable){
						variable.GetComponent<Nodes> ().callAction (0, 0, rowE [k], columnE [k]);
						usedE[k] = 1;
						usedVal[i] = 1;
						transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowF[i], columnF[i],rowE [k], columnE [k]);
						break;
						}

					}
				}
				for (int k = 0; k < neutral; k++) {
					if ((iterN [k].totalScore - 5) < 10 && usedN[k] == 0 &&(iterF [i].moveType == 4 && usedVal[i] == 0) || (iterF [i].moveType == 2 && usedVal[i] == 0) || (iterF [i].moveType == 9 && usedVal[i] == 0)) {
						GameObject variableN = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						//Debug.Log(string.Format("{0},{1}",rowN [k],rowN [k]));
							Debug.Log (string.Format ("Action second, {0},", k));
						if (variableN.GetComponent<Nodes> ().actionable){
						variableN.GetComponent<Nodes> ().callAction (0, 0, rowN [k], columnN [k]);
						usedVal[i] = 1;
						usedN[k] = 1;

						Debug.Log (string.Format ("second"));
						transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowF[i], columnF[i],rowN [k], columnN [k]);
						break;
						}
	
					}
				}
				for (int k = 0; k < enemmies; k++) {
					if ((iterF [i].moveType == 4 && usedVal[i] == 0) || (iterF [i].moveType == 2 && usedVal[i] == 0) || (iterF [i].moveType == 9 && usedVal[i] == 0)) {
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						if (variable.GetComponent<Nodes> ().actionable){
						Debug.Log (string.Format ("Action third, {0},{1}", rowE [k], columnE [k]));
						variable.GetComponent<Nodes> ().callAction (0, 0, rowE [k], columnE [k]);
						usedVal[i] = 1;
						Debug.Log (string.Format ("third"));
						transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowF[i], columnF[i],rowE [k], columnE [k]);
						break;
						}
					}
				}
				for (int k = 0; k < neutral; k++) {
					if ((iterF [i].moveType == 4 && usedVal[i] == 0) || (iterF [i].moveType == 2 && usedVal[i] == 0) || (iterF [i].moveType == 9 && usedVal[i] == 0)) {
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						Debug.Log (string.Format ("Action fourth, {0},{1}", rowN [k], columnN [k]));
						if (variable.GetComponent<Nodes> ().actionable){
						variable.GetComponent<Nodes> ().callAction (0, 0, rowN [k], columnN [k]);
						usedVal[i] = 1;
						Debug.Log (string.Format ("fourth"));
						transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowF[i], columnF[i],rowN [k], columnN [k]);
						break;
						}
					}
				}
			}

		}//use 2, 4, 9

		for (int i = 0; i < friends; i++) {
			if (((iterF [i].moveType == 5 && usedVal [i] == 0) || (iterF [i].moveType == 8 && usedVal [i] == 0) || (usedVal [i] == 7 && iterF [i].moveType == 0))) {
				for (int x = 0; x < friends; x++) {
					if ((x != i && iterF[x].moveType == 3 || iterF [x].moveType == 10) && iterF[i].moveType == 5 && usedVal[i] == 0) {
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						if (variable.GetComponent<Nodes> ().actionable){
						variable.GetComponent<Nodes> ().callAction (0, 0, rowF [x], columnF [x]);
						Debug.Log (string.Format ("immune"));
						transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowF[i], columnF[i],rowF [x], columnF [x]);
						usedVal [i] = 1;
						break;
						}
					}
					if ((iterF [x].totalScore > -10) && iterF [i].moveType == 5 && usedVal[i] == 0) {
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						if (variable.GetComponent<Nodes> ().actionable){
						variable.GetComponent<Nodes> ().callAction (0, 0, rowF [x], columnF [x]);
						Debug.Log (string.Format ("immune"));
						transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowF[i], columnF[i],rowF [x], columnF [x]);
						usedVal [i] = 1;
						break;
						}
					}
				}

				for (int x = 0; x < enemmies; x++){
					if ((iterE[x].moveType == 3 || iterE[x].moveType == 10) && iterF[i].moveType == 8 && usedVal[i] == 0) {
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						if (variable.GetComponent<Nodes> ().actionable){
						variable.GetComponent<Nodes> ().callAction (0, 0, rowE [x], columnE [x]);
						Debug.Log (string.Format ("accident"));
						transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowF[i], columnF[i],rowE [x], columnE [x]);
						usedVal [i] = 1;
						break;
						}
					}
				}
				for (int x = 0; x < enemmies; x++){
					if ((iterE[x].moveType == 2 || iterE[x].moveType == 4 || iterE[x].moveType == 9) && iterF[i].moveType == 8 && usedVal[i] == 0) {
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						if (variable.GetComponent<Nodes> ().actionable){
						variable.GetComponent<Nodes> ().callAction (0, 0, rowE [x], columnE [x]);
						Debug.Log (string.Format ("accident"));
						transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowF[i], columnF[i],rowE [x], columnE [x]);
						usedVal [i] = 1;
						break;
						}
					}
				}
				for (int x = 0; x < enemmies; x++){
					if (iterF[i].moveType == 8 && usedVal[i] == 0) {
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						if (variable.GetComponent<Nodes> ().actionable){
						variable.GetComponent<Nodes> ().callAction (0, 0, rowE [x], columnE [x]);
						Debug.Log (string.Format ("accident"));
						transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowF[i], columnF[i],rowE [x], columnE [x]);
						usedVal [i] = 1;
						break;
						}
					}
				}
			}
			if (iterF [i].moveType == 7 && usedVal[i] == 0){
				for (int k = 0; k < neutral; k++) {
					if ((iterN[k].moveType == 3 || iterN[k].moveType == 10)&& usedVal[i] == 0) {
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						if (variable.GetComponent<Nodes> ().actionable){
						variable.GetComponent<Nodes> ().callAction (0, 0, rowN [k], columnN [k]);
						usedVal[i] = 1;
						transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowF[i], columnF[i],rowN [k], columnN [k]);
						}
						GameObject variableX = manager.GetComponent<GameManager> ().classroom [rowN [k], columnN [k]];
						if (variableX.GetComponent<Nodes> ().actionable){
						
						variableX.GetComponent<Nodes> ().callAction (0, 0);

						transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowN[k], columnN[k],0,0);
						break;
						}

					}
				}
				for (int k = 0; k < neutral; k++) {
					if ((iterN[k].moveType == 2 || iterN[k].moveType == 9 || iterN[k].moveType == 10)&& usedVal[i] == 0) {
						GameObject variable = manager.GetComponent<GameManager> ().classroom [rowF [i], columnF [i]];
						if (variable.GetComponent<Nodes> ().actionable){
						variable.GetComponent<Nodes> ().callAction (0, 0, rowN [k], columnN [k]);
						usedVal[i] = 1;
						transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowF[i], columnF[i],rowN [k], columnN [k]);
						}

						for (int b = 0; b < neutral; b++) {
							if (usedE[b] == 0){
								GameObject variableX = manager.GetComponent<GameManager> ().classroom [rowN [k], columnN [k]];
								if (variableX.GetComponent<Nodes> ().actionable){
								variableX.GetComponent<Nodes> ().callAction (0, 0,rowE[b], columnE[b]);
								usedE[b] = 1;
								transform.parent.parent.GetComponent<ClassRoomManager>().ActionTaken(rowN[k], columnN[k],rowE[k], columnE[k]);
								break;
								}
							}
						}

						break;
						
					}
				}
			}
		}

				

		
	}
}
