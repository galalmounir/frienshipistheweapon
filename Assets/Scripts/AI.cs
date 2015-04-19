using UnityEngine;
using System.Collections.Generic;

public class AI : MonoBehaviour {

	public GameObject manager;

	public void main(){

		List<Nodes> friend = new List<Nodes>{};
		List<Nodes> friendU = new List<Nodes>{};
		List<Nodes> Enemy = new List<Nodes>{};
		List<Nodes> Neutral = new List<Nodes>{};

		List<int> friendScore = new List<int>{};
		List<int> friendScoreU = new List<int>{};
		List<int> EnemyScore = new List<int>{};
		List<int> NeutralScore = new List<int>{};

		List<int> friendMove = new List<int>{};
		List<int> enemyMove = new List<int>{};
		List<int> neutralMove = new List<int>{};

		for (int i = 0; i < 4; i++) {
			for (int x = 0; x < 4; x++) {
				GameObject variable = manager.GetComponent<GameManager> ().classroom [x, i];
				Nodes n = variable.GetComponent<Nodes> ();
				if (!n.enemy && !n.player){
					if (n.usableE){
						if (n.totalScore() > 10){
							friendU.Add(n);
							friendScoreU.Add (n.totalScore());
							friendMove.Add(n.youEffect + n.themEffect);
						}else{
							friend.Add(n);
							friendScore.Add(n.totalScore());
						}
					}else if(n.usableP){
					}
				}
			}
		}
	}
}
