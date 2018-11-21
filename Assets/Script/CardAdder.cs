using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAdder : MonoBehaviour {

	int count=7;
	public GameObject ItemGameObject;
	public GameObject Scrollbar;

	// Use this for initialization
	void Start () {

		for (int i = 0; i < count; i++)
		{
			//ItemGameObject is my prefab pointer that i previous made a public property  
			//and  assigned a prefab to it
			GameObject card = Instantiate(ItemGameObject) as GameObject;

			//scroll = GameObject.Find("CardScroll");
			if (Scrollbar != null)
			{
				//ScrollViewGameObject container object
				card.transform.SetParent(Scrollbar.transform,false);
			}
		}
		
	}


	
	// Update is called once per frame
	void Update () {
		
	}
}
