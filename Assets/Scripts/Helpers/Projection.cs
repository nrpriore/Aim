using UnityEngine;						// To inherit from Monobehaviour

/// Handles the projection created during aiming the ball
public class Projection {

	// Constant vars
	private GameObject _parent;			// Reference to parent gameobject holding step objects
	private int _numSteps;				// The number of iterations over which to build the projection
	private int _interval;				// The interval in which to spawn step gameobjects
	private int _collInterval;			// The interval in which to check projection for collisions
	private float _redrawThreshold;		// Threshold at which movement redraws projection
	private GameObject _ballProjection; // Reference to ball projection prefab
	private int _objLayerMask;			// Reference to the layer mask holding objects for collision ("Objects")

	// Dynamic vars
	private GameObject[] _steps;		// The list of gameobjects building the projection
	private Vector2 _prevVel;			// Used to determine if the position has moved

	// Constructor
	public Projection() {
		_numSteps = 10000;
		_interval = 500;
		_collInterval = _interval / (Functions.POWER_MULT * 2);
		_redrawThreshold = 1f;

		_parent = new GameObject();
		_parent.name = "Projection";
		_ballProjection = Resources.Load<GameObject>("Prefabs/BallProjection");
		_objLayerMask = LayerMask.GetMask("Objects");

		_steps = new GameObject[_numSteps/_interval];
		for(int i = 0; i < _steps.Length; i++) {
			_steps[i] = GameObject.Instantiate(_ballProjection, _parent.transform) as GameObject;
		}
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	// Updates the projection
	public void Update(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity) {
		if(NeedRedraw(velocity)) {
			// Create new projection
			Plot(rigidbody, pos, velocity, _numSteps);
		}
	}

	// Removes gameobjects for update or destroy
	public void HideProjection(int startIndex = 0) {
		if(startIndex < 0 || startIndex >= _steps.Length) {
			return;
		}
		for(int i = startIndex; i < _steps.Length; i++) {
			_steps[i].GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	// Destroys Projection instance (different from hiding objects for update)
	public void Destroy() {
		GameObject.Destroy(_parent);
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Calculates the Vector2 array of positions
	private void Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps) {
		float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
		Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;
		float drag = 1f - timestep * rigidbody.drag;
		Vector2 moveStep = velocity * timestep;

		for (int i = 0; i < steps; i++) {
			moveStep += gravityAccel;
			moveStep *= drag;
			pos += moveStep;

			// Check _guide for collision each _collInterval
			if(i % _collInterval == 0) {
				Collider2D hit = Physics2D.OverlapCircle(pos, _ballProjection.GetComponent<CircleCollider2D>().radius, _objLayerMask);
				if(hit) {
					HideProjection(1 + (i/_interval));
					return;
				}
			}

			// Move projection on interval
			if(i % _interval == 0 && i != 0) {
				int index = i/_interval;
				_steps[index].transform.localPosition = pos;

				SpriteRenderer sr = _steps[index].GetComponent<SpriteRenderer>();
				if(!sr.enabled) {
					sr.enabled = true;
					sr.color = Functions.UpdateColor(sr.color, a: Mathf.Max(0.2f, 0.9f - Mathf.Log(index, 20)));
				}
			}
		}
	}

	// Determines if the aim has moved far enough to be redrawn
	private bool NeedRedraw(Vector2 newVel) {
		if(Mathf.Abs((newVel.x * 10) - (_prevVel.x * 10)) > _redrawThreshold || Mathf.Abs((newVel.y * 10) - (_prevVel.y * 10)) > _redrawThreshold) {
			_prevVel = newVel;
			return true;
		}
		return false;
	}
	
}
