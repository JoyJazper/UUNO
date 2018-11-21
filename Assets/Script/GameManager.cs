using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;


public class GameManager : MonoBehaviour {

	public bool StartGameSer = false;

	public int junk;
	public GameObject instance;

	public bool token = true;

	public bool RuleFollowed;

	public bool IsServer = true;
	public static bool UnoPressed = false;

	public static int NoOfPlayer;

	public static ArrayList BaseCard;

	public InputField Username;

	public String[] CurrentCard;
	public String[] SelectedCard;

	//Player Variables
	public int NoOfCard = 7;
	protected ArrayList cards;
	public int TotalNoOPlayers = 0;
	//protected int NoOfWildCards = 4;
	//protected int NoOfGenCards;

	public ArrayList DifColor;
	public ArrayList CardNumber;
	public ArrayList ColorCardPower;
	public ArrayList WildCardPower;

	public int cardlimit = 112;
	public int WildcardLimit = 8;
	public int EachNumberLimit=8;
	public int PowerGenLimit = 24;
	public int Plus4Limit = 4;
	public int ColorChangeCardLimit = 4;
	public int LimitColorGen = 26;

	public int TotalDrawnCards = 0;
	public int ColorGenRed = 0;
	public int ColorGenGreen = 0;
	public int ColorGenBlue = 0;
	public int ColorGenYellow = 0;
	public int NoOfWildPlus4 = 0;
	public int NoOfWildColorChange = 0;
	public int NoOfWildCards = 0;

	public GameObject CardPrefab;
	public GameObject content;
	public GameObject CardButton;

	public GameObject ServerMadePanel;
	public GameObject ClientConPanel;
	public GameObject JoinGameButton;
	public GameObject CreateGameButton;
	public GameObject ExitButton;

	public GameObject serverPrefab;
	public GameObject clientPrefab;

	public GameObject ForegroundPanel;
	public GameObject BackgroundPanel;
	public GameObject AreUSurePanel;
	public GameObject ColorSelectPanel;
	public GameObject UserNamePanel;

	public CardObject newCard;
	private bool CurrentCardEmpty = true;

	public bool StartGameButton = false;

	#region CardObjectDefinition
	public class CardObject{

		public string color;
		public string number;
		public bool WildCard;
		public string power;

	}
	#endregion

	#region Singleton
	public static GameManager Control;

	void Awake(){
		if (Control == null) {
			DontDestroyOnLoad (gameObject);
			Control = this;
		} else if (Control != this) {
			Destroy (gameObject);
		}

	}
	#endregion

	public void StartGame()
	{
		ForegroundPanel.SetActive (false);
		BackgroundPanel.SetActive (true);
		CurrentCard = new string[2];
		SelectedCard = new string[2];
		CurrentCard[1] = "Null";
		CurrentCard[0] = "Null";

	}
		
	// Use this for initialization
	void Start () {
		
		DifColor = new ArrayList();
		DifColor.Add("Red");
		DifColor.Add("Green");
		DifColor.Add("Yellow");
		DifColor.Add("Blue");
		DifColor.Add("Wild");
		CardNumber = new ArrayList();
		CardNumber.Add("0");
		CardNumber.Add("1");
		CardNumber.Add("2");
		CardNumber.Add("3");
		CardNumber.Add("4");
		CardNumber.Add("5");
		CardNumber.Add("6");
		CardNumber.Add("7");
		CardNumber.Add("8");
		CardNumber.Add("9");
		CardNumber.Add("Rev");
		CardNumber.Add("Skip");
		CardNumber.Add("Plus2");
		ColorCardPower = new ArrayList();
		ColorCardPower.Add("Rev");
		ColorCardPower.Add("Skip");
		ColorCardPower.Add("Plus2");
		WildCardPower = new ArrayList();
		WildCardPower.Add("Plus4");
		WildCardPower.Add("ChangeColor");
		CurrentCard = new String[2];
		SelectedCard = new string[2];
		if (IsServer)
			junk = 8;
		for (junk = 0; junk < NoOfCard; junk++) {
			AddCard ();
			//StartCoroutine(WaitSec());
		}
	}

	/*IEnumerator WaitSec()
	{
		print(Time.time);
		AddCard ();
		yield return new WaitForSeconds(1000);
		print(Time.time);
	}*/

	public void onClickDeck()
	{
		if (token == true) {
			AddCard ();
			token = false;
		}
	}

	public void AddCard()
	{
		CardGenerator ();
	}

	#region CardGenerator 
	public void CardGenerator(){
		//var MyIndex = UnityEngine.Random.Range(0, MyArray.length);
		newCard = new CardObject ();
		var MyIndex = UnityEngine.Random.Range (0, 5);
		switch (MyIndex) {
		case 0: 
			if (ColorGenRed < LimitColorGen && TotalDrawnCards < cardlimit) {
				newCard.color = "Red";
				ColorGenRed++;
				Debug.Log ("The New Card Color is red!");
				var MyIndex1 = UnityEngine.Random.Range (0, CardNumber.Count);
				newCard.number = (string)CardNumber [MyIndex1];
				Debug.Log (newCard.number);
				TotalDrawnCards++;
				instance = Instantiate (CardButton) as GameObject;
				instance.transform.SetParent (content.transform, true);
				instance.GetComponent<Image> ().color = Color.red;
				break;
			} else {
				Debug.Log ("Red Skipped!");
				break;
			}

		case 1:
			if (ColorGenGreen < LimitColorGen && TotalDrawnCards < cardlimit) {
				newCard.color = "Green";
				ColorGenGreen++;
				Debug.Log ("The New Card Color is Green!");
				var MyIndex1 = UnityEngine.Random.Range (0, CardNumber.Count);
				newCard.number = (string)CardNumber [MyIndex1];
				Debug.Log (newCard.number);
				TotalDrawnCards++;
				instance = Instantiate (CardButton) as GameObject;
				instance.transform.SetParent (content.transform, true);
				instance.GetComponent<Image> ().color = Color.green;
				break;
			} else {
				Debug.Log ("Green Skipped!");
				break;
			}

		case 2:
			if (ColorGenYellow < LimitColorGen && TotalDrawnCards < cardlimit) {
				newCard.color = "Yellow";
				ColorGenYellow++;
				Debug.Log ("The New Card Color is Yellow!");
				var MyIndex1 = UnityEngine.Random.Range (0, CardNumber.Count);
				newCard.number = (string)CardNumber [MyIndex1];
				Debug.Log (newCard.number);
				TotalDrawnCards++;
				instance = Instantiate (CardButton) as GameObject;
				instance.transform.SetParent (content.transform, true);
				instance.GetComponent<Image> ().color = Color.yellow;
				break;
			} else {
				Debug.Log ("Yellow Skipped!");
				break;
			}

		case 3:
			if (ColorGenBlue < LimitColorGen && TotalDrawnCards < cardlimit) {
				newCard.color = "Blue";
				ColorGenBlue++;
				Debug.Log ("The New Card Color is Blue!");
				var MyIndex1 = UnityEngine.Random.Range (0, CardNumber.Count);
				newCard.number = (string)CardNumber [MyIndex1];
				Debug.Log (newCard.number);
				TotalDrawnCards++;
				instance = Instantiate (CardButton) as GameObject;
				instance.transform.SetParent (content.transform, true);
				instance.GetComponent<Image> ().color = Color.blue;
				break;
			} else {
				Debug.Log ("Blue Skipped!");
				break;
			}

		case 4:
			if (NoOfWildCards < WildcardLimit && TotalDrawnCards < cardlimit) {
				newCard.color = "Wild";
				NoOfWildCards++;
				newCard.WildCard = true;
				Debug.Log ("The New Card Color is Wild/Black!");
				var MyIndex1 = UnityEngine.Random.Range (0, WildCardPower.Count);
				switch (MyIndex1) {
				case 0: 
					if (NoOfWildPlus4 < Plus4Limit) {
						newCard.number = "Plus4";
						NoOfWildPlus4++;
						Debug.Log (newCard.number);
						break;
					} else {
						if (NoOfWildColorChange < ColorChangeCardLimit) {
							newCard.number = "ChangeColor";
							NoOfWildColorChange++;
							Debug.Log (newCard.number);
							break;
						} else {
							Debug.Log ("Could not insert after Wild +4 Check!");
							break;
						}
					}
				case 1: 
					if (NoOfWildColorChange < ColorChangeCardLimit) {
						newCard.number = "ChangeColor";
						NoOfWildColorChange++;
						Debug.Log (newCard.number);
						break;
					} else {
						if (NoOfWildPlus4 < Plus4Limit) {
							newCard.number = "Plus4";
							NoOfWildPlus4++;
							Debug.Log (newCard.number);
							break;
						} else {
							Debug.Log ("Could not insert after Wild +4 Check!");
							break;
						}
					}
				}
				TotalDrawnCards++;
				instance = Instantiate (CardButton) as GameObject;
				instance.transform.SetParent (content.transform, true);
				instance.GetComponent<Image> ().color = Color.white;
				break;
			} else {
				Debug.Log ("WildCard Basic Error!");
				break;
			}

		default :
			Debug.Log ("Creation Failed!");
			break;

		}
	}

		//Generator End!
	#endregion

	public void JoinButtonOnClick(){
		ClientConPanel.SetActive (true);
		ServerMadePanel.SetActive (false);
		JoinGameButton.SetActive (false);
		CreateGameButton.SetActive (false);
		ExitButton.SetActive (false);
	}

	public void CreateGameOnClick()
	{
		token = true;
		try{
			Server s = Instantiate(serverPrefab).GetComponent<Server>();
			s.init();

			Client c = Instantiate(clientPrefab).GetComponent<Client>();
			c.ClientName = Username.text;
			c.IsHost = true;
			if(c.ClientName == "")
				c.ClientName = "AnonymousHost";
			c.ConnectToServer("127.0.0.1",6548);
		}

		catch (Exception e){
			Debug.Log ("Error : "+ e.Message);
		}

		ClientConPanel.SetActive (false);
		ServerMadePanel.SetActive (true);
		JoinGameButton.SetActive (false);
		CreateGameButton.SetActive (false);
		ExitButton.SetActive (false);
	}

	public void CancelButtonOnClick(){
		ClientConPanel.SetActive (false);
		ServerMadePanel.SetActive (false);
		JoinGameButton.SetActive (true);
		CreateGameButton.SetActive (true);
		ExitButton.SetActive (true);

		Server s = FindObjectOfType<Server> ();
		if (s != null)
			Destroy (s.gameObject);

		Client c = FindObjectOfType<Client> ();
		if (c != null)
			Destroy (c.gameObject);
	}

	public void ExitButtonOnClick(){
		Application.Quit ();
	}

	public void ConnectButtonClientOnClick(){
		token = false;
		Debug.Log ("TriedConnection!");
		string hostAddress = GameObject.Find ("hostInput").GetComponent<InputField> ().text;
		if(hostAddress == "")
			hostAddress = "127.0.0.1";

		try{
			Client c = Instantiate(clientPrefab).GetComponent<Client>();
			c.ClientName = Username.text;
			if(c.ClientName == "")
				c.ClientName = "AnonymousClient";
			c.ConnectToServer(hostAddress,6548);
			ClientConPanel.SetActive(false);
		}
		catch (Exception e){
			Debug.Log ("Error : "+ e.Message);
		}

	}

	public void OnClickPassButton()
	{
		if (token == false) {
			token = false;
		}
	}

	public void OnClickUnoButton()
	{
		token = true;
	}

	#region Game Rule Checking

	public void RuleFollowedCheck()
	{
		Debug.Log ("InruleCheck!");
		if ( SelectedCard[0] == CurrentCard [0]) {
			Debug.Log ("Case 1 true");
			if (SelectedCard [1] == "ChangeColor" && CurrentCard [1] == "Plus4") {
				Debug.Log ("Case 1-2 true");
				RuleFollowed = false;
			} else if (SelectedCard [1] == "Plus2" && CurrentCard [1] == "Rev" ) {
				Debug.Log ("Case 1-3 true");
				RuleFollowed = false;
			} else if (SelectedCard [1] == "Plus2" && CurrentCard [1] == "Rev" ) {
				Debug.Log ("Case 1-3 true");
				RuleFollowed = false;
			} else if (SelectedCard [1] == "Plus4") {
				Debug.Log ("Case 1-3-1 true");
				RuleFollowed = false;
			}else {
				Debug.Log ("Case 1-4 true");
				RuleFollowed = true;
			}
		} else if (SelectedCard [0] != CurrentCard [0]) {
			Debug.Log ("Case 2 true");
			if (SelectedCard [1] == CurrentCard [1]) {
				Debug.Log ("Case 2-1 true");
				RuleFollowed = true;
			} else if (SelectedCard [1] == "Plus4") {
				Debug.Log ("Case 2-2 true");
				RuleFollowed = true;
				ChooseAnyColor ();
			} else if (SelectedCard [1] == "ChangeColor" && CurrentCard [1] == "Plus2") {
				Debug.Log ("Case 2-3 true");
				RuleFollowed = false;
			}  else if (CurrentCardEmpty == true) {
				Debug.Log ("Case 2-4 true");
				RuleFollowed = true;
				CurrentCardEmpty = false;
			}else if (SelectedCard [1] == "ChangeColor" && CurrentCard [1] != "Plus2") {
				Debug.Log ("Case 2-3 true");
				RuleFollowed = true;
			} else {
				Debug.Log ("Case 2-5 true");
				RuleFollowed = false;
			}
		}  else {
			Debug.Log ("Case 3 true");
			RuleFollowed = false;
		}
		if (SelectedCard [1] == "Plus4" || SelectedCard [1] == "ChangeColor") {
			Debug.Log ("Case 3 true");
			ChooseAnyColor ();
		}
	}

	#endregion
	/*
	public void AreUSurePanelYes(){
		AreUSurePanel.SetActive (false);
	}
	public void AreUSurePanelNo(){
		AreUSurePanel.SetActive (false);
		RuleFollowed = false;
	}
	*/
	public void ChooseAnyColor(){
		ColorSelectPanel.SetActive (true);
	}

	public void RedColorChange(){
		CurrentCard[0] = "Red";
		CurrentCard[1] = "";
		ColorSelectPanel.SetActive (false);
	}

	public void GreenColorChange(){
		CurrentCard[0] = "Green";
		ColorSelectPanel.SetActive (false);
	}

	public void BlueColorChange(){
		CurrentCard[0] = "Blue";
		ColorSelectPanel.SetActive (false);
	}

	public void YellowColorChange(){
		CurrentCard[0] = "Yellow";
		ColorSelectPanel.SetActive (false);
	}

	public void StartButtonClick(){
		StartGameButton = true;
		ForegroundPanel.SetActive (false);
		BackgroundPanel.SetActive (true);
		UserNamePanel.SetActive (false);
	}
}
