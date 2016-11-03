using UnityEngine;
using System.Collections;

public class Sender : MonoBehaviour {


	public GameObject message;

	public static string Parcel = "hello world!";
	int ParcelPointer = 0;
	char[] ParcelArr = Parcel.ToCharArray();
	int packetnumber = 0;
	bool started =false;
	bool acknowledged = false;
	int stopper = 0;

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
		shoot = Instantiate (message, transform.position+new Vector3(1,0,0),message.transform.rotation) as GameObject;
		shoot.name = "Packet";
		shoot.GetComponent<dataframe> ().data = this.ParcelArr [this.ParcelPointer];
		shoot.GetComponent<dataframe> ().packetNum = packetnumber;
		shoot.GetComponent<Rigidbody>().AddForce(new Vector3(5,0,0),ForceMode.VelocityChange);
		acknowledged = false;
		StartCoroutine( timeout ());
	}

	IEnumerator timeout(){
		yield return new WaitForSeconds (5);
		if (!acknowledged)
			SendMessage ();
	}
	void OnCollisionEnter(Collision ack){
		if (ack.gameObject.name == "ack") {
			int num;
			num = ack.gameObject.GetComponent<dataframe> ().packetNum;

			if (num == packetnumber) {
				packetnumber++;
				ParcelPointer++;
				if (stopper < ParcelArr.GetLength(0)-1) {
					SendMessage ();
					stopper++;
				}
				StopCoroutine (timeout ());
				acknowledged = true;
			}
			Destroy (ack.gameObject);
		}
	}


}
