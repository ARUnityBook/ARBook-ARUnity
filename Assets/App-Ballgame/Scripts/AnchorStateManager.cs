using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.iOS;

// games starts in anchor mode
// anchor mode: shows instructions for user to place the game court, shows spatial map and lets user touch screen to place court, when ok user presses Done
// Done button switches to game mode 
// game mode: disables anchor mode and starts game
// Reset button: switches to anchor mode

public class AnchorStateManager : MonoBehaviour {

	[Tooltip("Root game object to position as anchor")]
	public Transform anchor;
	public GameController gameController;

	[Header("Anchor mode objects")]
	public GameObject instructionHolder;
	public GameObject anchorParticles;
	public GameObject anchorObject;

	[Header("Game mode objects")]
	public GameObject ballgameObject;
	public GameObject resetButton;    



	private bool isAnchorMode = true;

	void Start() {
		SetAnchorMode(true);
	}

	void Update() {
		if (isAnchorMode) {
			HitHandler();
		}
	}

	// SetAnchorMode true=anchor mode, false=game mode
	public void SetAnchorMode(bool anchor) {
		if (anchor)
			Debug.Log("** SetAnchorMode **");
		else
			Debug.Log("** SetGameMode **");
		
		instructionHolder.SetActive(anchor);
		anchorParticles.SetActive(anchor);
		anchorObject.SetActive(anchor);

		ballgameObject.SetActive(!anchor);
		resetButton.SetActive(!anchor);

		isAnchorMode = anchor;

		// Begin the game!
		if (!anchor) {
			gameController.StartGame();
		}
	}
		

	private void HitHandler() {
		if (Input.touchCount > 0 && anchor != null)
		{
			var touch = Input.GetTouch(0);
			if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) &&
				 (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved))
			{
				var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
				ARPoint point = new ARPoint {
					x = screenPosition.x,
					y = screenPosition.y
				};

				// prioritize reults types
				ARHitTestResultType[] resultTypes = {
					ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
					// if you want to use infinite planes use this:
					//ARHitTestResultType.ARHitTestResultTypeExistingPlane,
					ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
					ARHitTestResultType.ARHitTestResultTypeFeaturePoint
				}; 

				foreach (ARHitTestResultType resultType in resultTypes)
				{
					if (HitTestWithResultType (point, resultType))
					{
						return;
					}
				}
			}
		}
	}

	bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
	{
		Camera camera = Camera.main;
		List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
		if (hitResults.Count > 0) {
			anchor.position = UnityARMatrixOps.GetPosition (hitResults[0].worldTransform);
			//anchor.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);

			anchor.LookAt(anchor.position + camera.transform.rotation * Vector3.forward,
				camera.transform.rotation * Vector3.up);
			
			Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", anchor.position.x, anchor.position.y, anchor.position.z));
			return true;
		}
		return false;
	}
}
