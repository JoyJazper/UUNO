using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IPointerExitHandler
{
	public static GameObject itemBeingDragged;

	public static bool isCustomerDragged;

	public Transform customerScrollRect;
	public Transform dragParent;

	public float holdTime;
	public float maxScrollVelocityInDrag;

	private Transform startParent;

	private ScrollRect scrollRect;

	private float timer;

	private bool isHolding;
	private bool canDrag;
	private bool isPointerOverGameObject;

	private CanvasGroup canvasGroup;

	private Vector3 startPos;

	public Transform StartParent
	{
		get { return startParent; }
	}

	public Vector3 StartPos
	{
		get { return startPos; }
	}

	void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}

	// Use this for initialization
	void Start()
	{
		timer = holdTime;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (EventSystem.current.currentSelectedGameObject == gameObject)
			{
				//Debug.Log("Mouse Button Down");
				scrollRect = customerScrollRect.GetComponent<ScrollRect>();
				isPointerOverGameObject = true;
				isHolding = true;
				StartCoroutine(Holding());
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			if (EventSystem.current.currentSelectedGameObject == gameObject)
			{
				//Debug.Log("Mouse Button Up");
				isHolding = false;

				if (canDrag)
				{
					itemBeingDragged = null;
					isCustomerDragged = false;
					if (transform.parent == dragParent)
					{
						canvasGroup.blocksRaycasts = true;
						transform.SetParent(startParent);
						transform.localPosition = startPos;
					}
					canDrag = false;
					timer = holdTime;
				}
			}
		}

		if (Input.GetMouseButton(0))
		{
			if (EventSystem.current.currentSelectedGameObject == gameObject)
			{
				if (canDrag)
				{
					//Debug.Log("Mouse Button");
					transform.position = Input.mousePosition;
				}
				else
				{
					if (!isPointerOverGameObject)
					{
						isHolding = false;
					}
				}
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isPointerOverGameObject = false;
	}

	IEnumerator Holding()
	{
		while (timer > 0)
		{
			if (scrollRect.velocity.x >= maxScrollVelocityInDrag)
			{
				isHolding = false;
			}

			if (!isHolding)
			{
				timer = holdTime;
				yield break;
			}

			timer -= Time.deltaTime;
			//Debug.Log("Time : " + timer);
			yield return null;
		}

		isCustomerDragged = true;
		itemBeingDragged = gameObject;
		startPos = transform.localPosition;
		startParent = transform.parent;
		canDrag = true;
		canvasGroup.blocksRaycasts = false;
		transform.SetParent(dragParent);
	}

	public void Reset()
	{
		isHolding = false;
		canDrag = false;
		isPointerOverGameObject = false;
	}
}