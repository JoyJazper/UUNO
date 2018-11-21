using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardOnclickPrefab : MonoBehaviour {

	//public static CardOnclickPrefab Instance { set; get;}

	public bool RuleFollowed = true;
	public GameObject Slot;
	public bool tken = false;
	private string[] ThisCard;
	public bool UnoBool = false;

	private string color;
	private string number;
	//private bool WildCard;
	//private string power;
	//private Client client;

	/*void start(){
		
		//client = FindObjectOfType<Client> ();
	}*/

	// Use this for initialization
	void Awake(){
		//Instance = this;
		ThisCard = new string[3];
		color = GameManager.Control.newCard.color;
		number = GameManager.Control.newCard.number;
		//WildCard = GameManager.Control.newCard.WildCard;
		//power = GameManager.Control.newCard.power;
		Debug.Log ("The card is :" + color + " " + number);

	}
	// Update is called once per frame
	void Update () {
		RuleFollowed = GameManager.Control.RuleFollowed;
	}

	public void OnClickPrefab(){
		if (GameManager.Control.token == true) {
			ThisCard [0] = color;
			ThisCard [1] = number;
			for (int i = 0; i < 2; i++) {
				GameManager.Control.SelectedCard [i] = ThisCard [i];
			}
			GameManager.Control.RuleFollowedCheck ();
			RuleFollowed = GameManager.Control.RuleFollowed;
			if (GameManager.Control.token == true)
				tken = true;
			else
				tken = false;

			
			Debug.Log ("The card is :" + ThisCard [0] + " " + ThisCard [1]);

			if (RuleFollowed) {
				Debug.Log ("RFTK true");
				Slot = GameObject.FindWithTag ("GamePlayPanel");
				this.transform.SetParent (Slot.transform, true);
				GameManager.Control.token = false;
				Debug.Log (GameManager.Control.token);

				string msg = "CMDT|";
				msg += ThisCard [0] + "|";
				msg += ThisCard [1] + "|";
				msg += UnoBool.ToString () + "|";
				msg += (Client.Instance.ClientID).ToString ();
				Debug.Log (msg + "(1)");
				Send (msg);

				for (int i = 0; i < 2; i++) {
					GameManager.Control.CurrentCard [i] = ThisCard [i];
				}
			}
		}
	}

	public void Send(string data)
	{
		if (!(Client.Instance.socketReady)) {
			Debug.Log ("The check is done in send!");
			return;
		}
		Client.Instance.Writer.WriteLine (data);
		Client.Instance.Writer.Flush ();
		Debug.Log (data + "(2)");
	}
}
