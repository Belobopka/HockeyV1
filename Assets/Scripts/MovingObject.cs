using UnityEngine;
using System.Collections;

	public abstract class MovingObject : MonoBehaviour
	{
		public float maxMoveTime = 1;			//Time it will take object to move, in seconds.
		public float minMoveTime = 5;
		private Rigidbody _rb3D;				//The Rigidbody2D component attached to this object.
		private float _inverseMoveTime;			//Used to make movement more efficient.
		public bool isMoving;					//Is the object currently moving.
		// public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0F, 0F, 5F, 1F);
		
		//Protected, virtual functions can be overridden by inheriting classes.
		protected virtual void Start ()
		{
			
			//Get a component reference to this object's Rigidbody2D
			_rb3D = GetComponent <Rigidbody> ();
			
			//By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
			_inverseMoveTime = 1f / minMoveTime;
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
			_inverseMoveTime = 1f / minMoveTime;
			//The object is now moving.
			isMoving = true;
			
			//Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
			//Square magnitude is used instead of magnitude because it's computationally cheaper.
			float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			//While that distance is greater than a very small amount (Epsilon, almost zero):
			while(sqrRemainingDistance > float.Epsilon)
			{
				// ease-in out
				if (sqrRemainingDistance < 0.2f && _inverseMoveTime > 0.2f)
				{
					
					_inverseMoveTime *= 0.99f;
				} else if (_inverseMoveTime > maxMoveTime)
				{
					_inverseMoveTime *= 1.01f;
				}
				//Find a new position proportionally closer to the end, based on the moveTime
				Vector3 newPostion = Vector3.MoveTowards(_rb3D.position, end, _inverseMoveTime * Time.deltaTime);
				//Call MovePosition on attached Rigidbody2D and move it to the calculated position.
				_rb3D.MovePosition (newPostion);
				
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

