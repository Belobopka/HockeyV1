using UnityEngine;
using System.Collections;

	public abstract class MovingObject : MonoBehaviour
	{
		public float maxMoveTime = 2f;			//Time it will take object to move, in seconds.
		private Rigidbody _rb3D;				//The Rigidbody2D component attached to this object.
		private float _inverseMoveTime;			//Used to make movement more efficient.
		public bool isMoving;					//Is the object currently moving.
		public AnimationCurve _moveCurve;
		private float _t;
		public float remDist;
		public float evaluation;
		//Protected, virtual functions can be overridden by inheriting classes.
		protected virtual void Start ()
		{
			
			//Get a component reference to this object's Rigidbody2D
			_rb3D = GetComponent <Rigidbody> ();
			
			//By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
		}
		
		
		//Move returns true if it is able to move and false if not. 
		//Move takes parameters for x direction, y direction and a RaycastHit2D to check collision.
		protected bool Move (Vector3 moveTo )
		{
			//Store start position to move from, based on objects current transform position.
			var start = transform.position;
			StartCoroutine (SmoothMovement (moveTo));

			return true;
		}
		
		
		//Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
		protected IEnumerator SmoothMovement (Vector3 end)
		{
			_moveCurve = AnimationCurve.EaseInOut(0F, 0, maxMoveTime, 1F);
			_t = 0.0f;
			//The object is now moving.
			isMoving = true;
			var startPosition = _rb3D.position;
			//Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
			//Square magnitude is used instead of magnitude because it's computationally cheaper.
			float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			//While that distance is greater than a very small amount (Epsilon, almost zero):
			while(sqrRemainingDistance > float.Epsilon)
			{
				_t += Time.deltaTime;

				remDist = sqrRemainingDistance;
				evaluation =  _moveCurve.Evaluate(_t);
		
				var newPosition = Vector3.Lerp(startPosition, end, evaluation );

				_rb3D.MovePosition(newPosition);
				
				//Recalculate the remaining distance after moving.
				sqrRemainingDistance = (transform.position - end).sqrMagnitude;
				
				//Return and loop until sqrRemainingDistance is close enough to zero to end the function
				yield return null;
			}
			
			//Make sure the object is exactly at the end of its movement.
			_rb3D.MovePosition (end);
			
			//The object is no longer moving.
			isMoving = false;
		}
		
	}

