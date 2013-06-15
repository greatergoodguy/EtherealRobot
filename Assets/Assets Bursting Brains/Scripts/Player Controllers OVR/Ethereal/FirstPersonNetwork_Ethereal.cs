using UnityEngine;
using System.Collections;

public class FirstPersonNetwork_Ethereal : Photon.MonoBehaviour {

    private EtherealPC etherealPC_script;

    void Awake() {
        etherealPC_script = GetComponent<EtherealPC>();
    }

	void Start() {
		if (PhotonNetwork.connected){
        	if (photonView.isMine) {
            	etherealPC_script.enabled = true;
        	}
        	else {          
            	etherealPC_script.enabled = false;
        	}
		}
		else{
			etherealPC_script.enabled = true;
		}

        gameObject.name = gameObject.name + photonView.viewID;
		print ("gameObject.name: " + gameObject.name);
	}
	
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            //We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation); 
        }
        else {
            //Network player, receive data
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }

    private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this

    void Update()
    {
        if (!photonView.isMine) {
            //Update remote player (smooth this, this looks good, at the cost of some accuracy)
            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);
        }
    }

}