using UnityEngine;
using System.Collections;

public class MobileInputHandler : MonoBehaviour {

	
	#region EVENTS
	// Gesture Events
	public delegate void TapAction(Touch t);
	public static event TapAction OnTap;
	
	public delegate void PanBeganAction(Touch t);
	public static event PanBeganAction OnPanBegan;
	
	public delegate void PanHeldAction(Touch t);
	public static event PanHeldAction OnPanHeld;
	
	public delegate void PanEndedAction(Touch t);
	public static event PanEndedAction OnPanEnded;
	#endregion
	
	#region PUBLIC VARIABLES
	// Maximum pixels a tap can move.
	public float tapMaxMovement = 50f;
	
	// Minimum time a touch must last to count as a pan gesture.
	public float panMinTime = 0.01f;
	#endregion
	
	#region PRIVATE VARIABLES
	private float startTime;
	private Vector2 movement;
	
	private bool tapGestureFailed = false; 
	private bool panGestureRecognized = false;
	#endregion
	
	#region MONOBEHAVIOUR METHODS
	void Update () {
		if (Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];
			
			if (touch.phase == TouchPhase.Began)
			{
				startTime = Time.time;
				movement = Vector2.zero;
			}
			else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
			{
				movement += touch.deltaPosition;
				
				if (!panGestureRecognized && Time.time - startTime > panMinTime)
				{
					panGestureRecognized = true;
					tapGestureFailed = true;
					
					if (OnPanBegan != null)
						OnPanBegan(touch);
				}
				else if (panGestureRecognized)
				{
					if (OnPanHeld != null)
						OnPanHeld(touch);
				}
				else if (movement.magnitude > tapMaxMovement)
					tapGestureFailed = true;
			}
			else
			{
				if (panGestureRecognized)
				{
					if (OnPanEnded != null)
						OnPanEnded(touch);
				}
				else if (!tapGestureFailed)
				{
					if (OnTap != null)
						OnTap(touch);
				}
				
				panGestureRecognized = false;
				tapGestureFailed = false;
			}
		}
	}
	#endregion
}
