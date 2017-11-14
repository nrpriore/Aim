using UnityEngine;						// To inherit from Monobehaviour
using UnityEngine.UI;					// To access scrollrect
using System.Collections.Generic;		// For lists

 
public class EditorObjectScroll : MonoBehaviour {

	// Constant vars
	private ScrollRect _scrollRect;		// Reference to scrollrect component

	// Dynamic vars
	private List<EditorObject> _placedObjects;		// List of EditorObjects placed in edit view


	// On instantiation
	void Start() {
		InitVars();
		CreateScrollView();
	}

	// Runs every frame
	void Update() {
		
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_scrollRect = gameObject.GetComponent<ScrollRect>();
		_placedObjects = new List<EditorObject>();
	}

	// Populates scrollrect with objects
	private void CreateScrollView() {
		GameObject[] objects = Resources.LoadAll<GameObject>("Prefabs/Edit");
		float yVal = -160f;
		for(int i = 0; i < objects.Length; i++) {
			RectTransform obj = Instantiate<GameObject>(objects[i]).GetComponent<RectTransform>();
			obj.gameObject.GetComponent<EditorObject>().SetObjProperties();
			obj.SetParent(_scrollRect.content);

			float height = GetHeight(obj);

			yVal -= (i == 0)? 0 : height;
			obj.anchoredPosition = new Vector2(0, yVal);
			yVal -= (height + 100f);
		}
		//_scrollRect.content.sizeDelta = new Vector2(_scrollRect.content.sizeDelta.x, -yVal);
	}

	// Gets half of height of object, accounting for rotation
	private float GetHeight(RectTransform obj) {
		float rot = Mathf.PI * obj.eulerAngles.z / 180f;
		Vector2 size = new Vector2(obj.sizeDelta.x * obj.localScale.x, obj.sizeDelta.y * obj.localScale.y);
		float height = ((Mathf.Sin(rot) * size.x) + (Mathf.Cos(rot) * size.y)) / 2f;
		return height;
	}
}
