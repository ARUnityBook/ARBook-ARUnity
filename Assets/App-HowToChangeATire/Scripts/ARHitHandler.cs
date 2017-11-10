using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.iOS;

public class ARHitHandler : MonoBehaviour
{
	public Transform anchor;
	public InstructionsController controller;

	void Update ()
	{
		List<ARHitTestResult> hitResults;
		ARPoint point;
		//float scale;


		if (Input.touchCount > 0 && anchor != null) {

			var touch = Input.GetTouch (0);
			if (touch.phase == TouchPhase.Began) {
				Vector2 center = new Vector2 (Screen.width / 2, Screen.height / 2);
				Vector3 screenPosition = Camera.main.ScreenToViewportPoint (center);
				point.x = screenPosition.x;
				point.y = screenPosition.y;
				//Vector2 edge = new Vector2 (Screen.width, Screen.height / 2);
				//Vector3 screenEdge = Camera.main.ScreenToViewportPoint (edge);
				//scale = screenPosition.x - screenEdge.x;

				hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point,
					ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
				if (hitResults.Count == 0) {
					hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point,
						ARHitTestResultType.ARHitTestResultTypeHorizontalPlane);
				}
				if (hitResults.Count == 0) {
					hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point,
						ARHitTestResultType.ARHitTestResultTypeFeaturePoint);
				}

				if (hitResults.Count > 0) {
					anchor.position = UnityARMatrixOps.GetPosition (hitResults [0].worldTransform);
					anchor.rotation = UnityARMatrixOps.GetRotation (hitResults [0].worldTransform);
					//Debug.Log("ARHitHandler scale: " + scale);
					//anchor.localScale = new Vector3(scale, scale, scale);
					controller.ToggleAnchor ();
				}
			}
		}
	}
}
