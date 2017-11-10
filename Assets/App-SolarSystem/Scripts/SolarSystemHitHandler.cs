using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.iOS;

public class SolarSystemHitHandler : MonoBehaviour {
	public Transform anchor;

	void Update () {
		List<ARHitTestResult> hitResults;
		ARPoint point;

		if (Input.touchCount > 0 && anchor != null) {

			var touch = Input.GetTouch(0);
			if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && touch.phase == TouchPhase.Began) {
				Vector3 screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
				point.x = screenPosition.x;
				point.y = screenPosition.y;

				hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest( point, 
					ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
				if (hitResults.Count == 0) {
					hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest( point, 
						ARHitTestResultType.ARHitTestResultTypeHorizontalPlane);        
				}
				if (hitResults.Count == 0) {
					hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest( point, 
						ARHitTestResultType.ARHitTestResultTypeFeaturePoint);       
				}

				if (hitResults.Count > 0) {
					anchor.position = UnityARMatrixOps.GetPosition( hitResults[0].worldTransform);
					anchor.rotation = UnityARMatrixOps.GetRotation( hitResults[0].worldTransform);
				}
			}
		}
	}
}
