using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sender : MonoBehaviour {


	public GameObject message;

	public static string Parcel = "hello world!";	//string to be sent
	int ParcelPointer = 0;	//pointer to current packet
	char[] ParcelArr = Parcel.ToCharArray();	//list of packets
	int packetnumber = 0;	//packet identification number
	bool started =false;	//game initializer
	bool acknowledged = false;	//timeout trigger
	public Text TimeoutDisplay;
	int timeouts =0;	//number of timeouts
//	int stopper = 0;	

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)&&!started) {
			SendMessage ();
			started = true;
		}
	}

	void SendMessage()
	{
		GameObject shoot;
		shoot = Instantiate (message, transform.position+new Vector3(1,-0.5f,0),message.transform.rotation) as GameObject;
		shoot.name = "Packet";
		shoot.GetComponent<Renderer> ().material.color = Color.cyan;
		shoot.GetComponent<dataframe> ().data = this.ParcelArr [this.ParcelPointer];
		shoot.GetComponent<dataframe> ().packetNum = packetnumber;
		shoot.GetComponent<Rigidbody>().AddForce(new Vector3(5,0,0),ForceMode.VelocityChange);
		acknowledged = false;
		StartCoroutine( timeout ());
	}

	IEnumerator timeout(){
		Debug.Log ("stARTED");
		yield return new WaitForSeconds (5);

		if (!acknowledged) {
			Debug.Log ("triggered");
			SendMessage ();
			TimeoutDisplay.text = "Packets Lost: "+ ++timeouts;
		}
	}
	void OnCollisionEnter(Collision ack){
		if (ack.gameObject.name == "ack") {
			int num;
			num = ack.gameObject.GetComponent<dataframe> ().packetNum;

			if (num == packetnumber) {
				acknowledged = true;
				StopAllCoroutines ();	//to stop timeout
				packetnumber++;
				ParcelPointer++;
				if (ParcelPointer < ParcelArr.GetLength(0)) {
					SendMessage ();
			//		stopper++;
				}
				Debug.Log ("stopped");

			}
			Destroy (ack.gameObject);
		}
	}


}
