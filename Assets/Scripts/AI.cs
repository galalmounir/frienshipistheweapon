using UnityEngine;
using System.Collections.Generic;

public class AI : MonoBehaviour {

	public GameObject manager;

	public void main(){

		GameObject variable2 = manager.GetComponent<GameManager> ().classroom [0, 0];
		Nodes n2 = variable2.GetComponent<Nodes> ();

		Nodes playerN = n2;

		List<Nodes> friend = new List<Nodes>{};
		List<Nodes> friendU = new List<Nodes>{};
		List<Nodes> Enemy = new List<Nodes>{};
		List<Nodes> Neutral = new List<Nodes>{};

		List<int> friendScore = new List<int>{};
		List<int> friendScoreU = new List<int>{};
		List<int> FUrow = new List<int>{};
		List<int> FUcolumn = new List<int>{};
		List<int> Frow = new List<int>{};
		List<int> Fcolumn = new List<int>{};
		List<int> EnemyScore = new List<int>{};
		List<int> Erow = new List<int>{};
		List<int> Ecolumn = new List<int>{};
		List<int> NeutralScore = new List<int>{};
		List<int> Nrow = new List<int>{};
		List<int> Ncolumn = new List<int>{};

		List<int> friendMove = new List<int>{};
		List<int> enemyMove = new List<int>{};
		List<int> neutralMove = new List<int>{};

		for (int i = 0; i < 4; i++) {
			for (int x = 0; x < 4; x++) {
				GameObject variable = manager.GetComponent<GameManager> ().classroom [x, i];
				Nodes n = variable.GetComponent<Nodes> ();
				if (!n.enemy && !n.player) {
					if (n.usableE) {
						if (n.totalScore () > 10) {
							friendU.Add (n);
							friendScoreU.Add (n.totalScore ());
							friendMove.Add (n.youEffect - n.themEffect);
							FUrow.Add (n.getRow ());
							FUcolumn.Add (n.getColumn ());
						} else {
							friend.Add (n);
							friendScore.Add (n.totalScore ());
							Frow.Add (n.getRow ());
							Fcolumn.Add (n.getColumn ());
						}
					} else if (n.usableP) {
						Enemy.Add (n);
						EnemyScore.Add (n.totalScore ());
						enemyMove.Add (n.youEffect - n.themEffect);
						Erow.Add (n.getRow ());
						Ecolumn.Add (n.getColumn ());

					} else if (n.usableN) {
						Neutral.Add (n);
						NeutralScore.Add (n.totalScore ());
						neutralMove.Add (n.youEffect - n.themEffect);
						Nrow.Add (n.getRow ());
						Ncolumn.Add (n.getColumn ());
					}
				}
			}
		}

		int inter1 = 0;

		while (inter1 < friendScoreU.Count) {

			int inter2 = 0;
			while (inter2 < EnemyScore.Count){

				if ((EnemyScore [inter2] - 5) < friendMove [inter1]) {
					int moveT = friendU [inter1].moveType;
					if (moveT == 3 || moveT == 10) {
						friendU [inter1].callAction (playerN.getRow (), playerN.getColumn ());
					} else {
						friendU [inter1].callAction (playerN.getRow (), playerN.getColumn (), Enemy [inter2].getRow (), Enemy [inter2].getColumn ());
					}
					break;
				}

				inter2 = inter2 +1;
			}
			inter1 = inter1+1;
		}

		inter1 = 0;
		
		while (inter1 < friendMove.Count) {
			
			int inter2 = 0;
			while (inter2 < EnemyScore.Count){
				
				if ((NeutralScore [inter2] + friendMove [inter1] ) > 5) {
					int moveT = friendU [inter1].moveType;
					if (moveT == 3 || moveT == 10) {
						friendU [inter1].callAction (playerN.getRow (), playerN.getColumn ());
					} else {
						friendU [inter1].callAction (playerN.getRow (), playerN.getColumn (), Neutral [inter2].getRow (), Neutral [inter2].getColumn ());
					}
					break;
				}
				
				inter2 = inter2 +1;
			}
			inter1 = inter1+1;
		}


	}
}
