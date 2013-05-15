using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	
	public PlayerType initialPlayerType = PlayerType.Basic;
	
	private Hashtable playerHT = new Hashtable();
	private ArrayList playerTypes = new ArrayList();
	private int activePlayerIndex = 0;
	private GameObject activePlayer;
	
	// Use this for initialization
	void Start () {
		foreach(Transform child in transform)
			Destroy(child.gameObject);
		
		PlayerType[] playerTypes_array = (PlayerType[]) System.Enum.GetValues(typeof(PlayerType));
		playerTypes.AddRange(playerTypes_array);
		foreach(PlayerType type in playerTypes){
			AddNewPlayerAsChildAndDisable(type);
		}
		
		activePlayer = (GameObject) playerHT[initialPlayerType];
		activePlayer.SetActive(true);
		
		activePlayer.transform.parent = null;
		transform.parent = activePlayer.transform;
	}
	
	private void AddNewPlayerAsChildAndDisable(PlayerType type){
		GameObject player = (GameObject) Instantiate(Resources.Load("Players" + "/" + type.ToString()));
		player.transform.parent = transform;
		player.transform.localPosition = Vector3.zero;
		player.SetActive(false);
		
		playerHT[type] = player;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.X)){
			IncrementActivePlayer();
		}	
	}
	
	private void IncrementActivePlayer(){
		const float HEIGHT_DEPLACEMENT = 2;
		
		transform.parent = null;
		activePlayer.transform.parent = transform;
		activePlayer.SetActive(false);
		
		activePlayerIndex++;
		if(activePlayerIndex >= playerTypes.Count){			
			activePlayerIndex = 0;
		}
		
		PlayerType newPlayerType = (PlayerType) playerTypes[activePlayerIndex];
		activePlayer = (GameObject) playerHT[newPlayerType];
		activePlayer.transform.rotation = Quaternion.identity;
		activePlayer.SetActive(true);
		
		activePlayer.transform.parent = null;
		transform.parent = activePlayer.transform;
		
		Vector3 tempPos = activePlayer.transform.position;
		tempPos.y += HEIGHT_DEPLACEMENT;
		activePlayer.transform.position = tempPos;
	}
}
