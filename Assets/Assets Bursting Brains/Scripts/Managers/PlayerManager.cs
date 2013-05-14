using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	
	public PlayerType initialPlayerType = PlayerType.Basic;
	
	Hashtable playerHT = new Hashtable();
	
	// Use this for initialization
	void Start () {
		
		PlayerType[] playerTypes = (PlayerType[]) System.Enum.GetValues(typeof(PlayerType));
		foreach(PlayerType type in playerTypes){
			AddNewPlayerAsChildAndDisable(type);
		}
		
		((GameObject) playerHT[initialPlayerType]).SetActive(true);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void AddNewPlayerAsChildAndDisable(PlayerType type){
		GameObject player = (GameObject) Instantiate(Resources.Load("Players" + "/" + type.ToString()));
		player.transform.parent = transform;
		player.transform.localPosition = Vector3.zero;
		player.SetActive(false);
		
		playerHT[type] = player;
	}
}
