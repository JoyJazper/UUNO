using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClass : MonoBehaviour {

		public string color;
		public string number;
		public bool WildCard;
		public string power;

	void Awake()
	{
		color = GameManager.Control.newCard.color;
		number = GameManager.Control.newCard.number;
		WildCard = GameManager.Control.newCard.WildCard;
		power = GameManager.Control.newCard.power;
		Debug.Log ("The card is :" + color + " " + number);
	}
}
