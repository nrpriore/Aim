using UnityEngine;
using System;

public class EditorSelect : MonoBehaviour {

	private Transform _obj;
	private EditorMenu _menu;
	private EditorDeleteObject _delete;

	private bool _moving;
	private bool _rotating;
	private Vector2 _currPos;
	private Vector2 _deltaPos;
	private float _startAngle;
	private float _startRot;


	// Use this for initialization
	void Awake () {
		_obj = transform.parent.transform;
		_menu = GameObject.Find("EditorMenu").GetComponent<EditorMenu>();
		_delete = GameObject.Find("DeleteObject").GetComponent<EditorDeleteObject>();

		_moving = false;
		_rotating = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Application.isEditor) {
			_currPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

			if(_moving) {
				if(Input.GetMouseButtonUp(0)) {
					_moving = false;
					_menu.FadeIn();
					_delete.FadeOut();
				}
				_obj.position = _currPos + _deltaPos;
			}else if(_rotating) {
				if(Input.GetMouseButtonUp(0)) {
					_rotating = false;
					_menu.FadeIn();
					_delete.FadeOut();
				}
				_obj.eulerAngles = Vector3.forward * (_startRot + TargetAngle() - _startAngle);
			}else {
				if(Input.GetMouseButtonDown(0)) {
					Collider2D[] colls = Physics2D.OverlapPointAll(_currPos);
					if(colls.Length > 0) {
						Collider2D coll = null;
						if(Array.Exists(colls, x => x.gameObject.name == "Move")) {
							coll = Array.Find(colls, x => x.gameObject.name == "Move");
						}else if(Array.Exists(colls, x => x.gameObject.name == "Rotate")) {
							coll = Array.Find(colls, x => x.gameObject.name == "Rotate");
						}
						if(coll) {
							switch(coll.gameObject.name) {
								case "Move":
									_moving = true;
									_deltaPos = (Vector2)_obj.localPosition - _currPos;
									ShowDelete();
									break;
								case "Rotate":
									_rotating = true;
									_startRot = _obj.transform.eulerAngles.z;
									_startAngle = TargetAngle();
									ShowDelete();
									break;
							}
						}
					}
				}
			}
		}
	}

	public void MoveOnSelect() {
		_moving = true;
		_deltaPos = (Vector2)_obj.localPosition - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		_menu.FadeOut();
		_delete.FadeIn();
	}

	private void ShowDelete() {
		_menu.FadeOut();
		_delete.FadeIn();
	}

	private float TargetAngle() {
		Vector2 center = new Vector2(_obj.transform.position.x, _currPos.y);
		float adj = Vector2.Distance(_obj.transform.position, center);
		float hypo = Vector2.Distance(_obj.transform.position, _currPos);
		float angle = 180f * Mathf.Acos(adj/hypo) / Mathf.PI;
		if(_currPos.x < _obj.transform.position.x && _currPos.y >= _obj.transform.position.y) {
			//
		}else if(_currPos.x <= _obj.transform.position.x && _currPos.y < _obj.transform.position.y) {
			angle = 180 - angle;
		}else if(_currPos.x > _obj.transform.position.x && _currPos.y <= _obj.transform.position.y) {
			angle += 180;
		}else {
			angle = 360 - angle;
		}
		return angle;
	}

}
