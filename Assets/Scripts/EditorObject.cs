using UnityEngine;						// To inherit from Monobehaviour
using UnityEngine.EventSystems;

 
public class EditorObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	// Constant vars
	private float _selectionTime;
	private EditorMenu _menu;

	// Dynamic vars
	private float _timer;
	private bool _counting;
	private int _pointerID;


	// On instantiation
	void Awake() {
		InitVars();
		
	}

	// Runs every frame
	void Update() {
		if(_counting) {
			_timer += Time.deltaTime;
			if(_timer >= _selectionTime) {
				AddObject();
			}
		}
	}

	public void OnPointerDown(PointerEventData e) {
		Select(e.pointerId);
	}

	public void OnPointerUp(PointerEventData e) {
		Unselect();
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	// Called after instantiation into scrollview
	public void SetProperties() {
		gameObject.name = gameObject.name.Substring(0, gameObject.name.Length - 7);
	}


/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() { 
		_menu = GameObject.Find("EditorMenu").GetComponent<EditorMenu>();
		_selectionTime = 1f;
		_timer = 0f;
		_counting = false;
		_pointerID = -1;
	}

	private void AddObject() {
		Unselect();
		GameObject obj = Instantiate<GameObject>(Static.Get(name), GameObject.Find("Objects").transform);
		obj.name = obj.name.Substring(0, obj.name.Length - 7);
		EditController.UpdateObject(obj);
		obj.transform.position = gameObject.GetComponent<RectTransform>().position;
		GameObject.Find("EditController").GetComponent<EditController>().SelectAddedObject(obj, _pointerID);
	}

	private void Select(int id) {
		if(!EditorMenu.ObjSelected && _menu.Open()) {
			EditorMenu.ObjSelected = true;
			_pointerID = id;
			_counting = true;
		}
	}
	private void Unselect() {
		EditorMenu.ObjSelected = false;
		_pointerID = -1;
		_timer = 0f;
		_counting = false;
	}
	
}
