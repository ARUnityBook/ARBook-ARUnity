using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class MoveTool : MonoBehaviour {
	private bool isEditing;
	private Vector3 originaButtonScale;
	private BoxCollider collider;
	private Vector3 originColliderSize;
	//	private SpatialMappingManager spatialMapping;

	private PictureController picture;
	private Vector3 relativeOffset;
	private float upNormalThreshold = 0.9f;

	void Start() {
		isEditing = false;
		originaButtonScale = transform.localScale;
		collider = GetComponent<BoxCollider>();
		originColliderSize = collider.size;
		//spatialMapping = SpatialMappingManager.Instance;
		picture = GetComponentInParent<PictureController>();
		relativeOffset = transform.position - picture.transform.position;
		relativeOffset.z = -relativeOffset.z;
	}

	void Update() {
		List<ARHitTestResult> hitResults;
		ARPoint point;

		if (isEditing) {
			//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//			RaycastHit hit;
			//			if (Physics.Raycast(ray, out hit, Mathf.Infinity, WallLayerMask)) {
			//				Debug.DrawLine(ray.origin, hit.point);
			//				picture.transform.position = hit.point - relativeOffset;
			//			}

			Vector3 screenPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
			point.x = screenPosition.x;
			point.y = screenPosition.y;

			hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest( point, 
				ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);

			if (hitResults.Count == 0) {
				hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest( point, 
					ARHitTestResultType.ARHitTestResultTypeVerticalPlane);        
			}
			if (hitResults.Count == 0) {
				hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest( point, 
					ARHitTestResultType.ARHitTestResultTypeFeaturePoint);       
			}

			if (hitResults.Count > 0) {
				picture.transform.position = UnityARMatrixOps.GetPosition( hitResults[0].worldTransform);
				picture.transform.rotation = UnityARMatrixOps.GetRotation( hitResults[0].worldTransform);
			}
		}
		if (!Input.GetMouseButton(0)) {
			DoneEdit();
		}
	}

	private void OnMouseDown() {
		if (!isEditing) {
			BeginEdit();
		}
	}

	private void OnMouseUp() {
		if (isEditing) {
			DoneEdit();
		}
	}

	private void BeginEdit() {
		if (!isEditing) {
			isEditing = true;
			transform.localScale = originaButtonScale * 2.5f;
			collider.size = Vector3.one;
			//			spatialMapping.DrawVisualMeshes = true;
		}
	}

	private void DoneEdit() {
		if (isEditing) {
			isEditing = false;
			transform.localScale = originaButtonScale;
			collider.size = originColliderSize;
			//			spatialMapping.DrawVisualMeshes = false;
		}
	}
}