using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityARInterface;

public class MoveToolUnityARI : ARBase {
	
	private bool isEditing;
	private Vector3 originaButtonScale;
	private BoxCollider collider;
	private Vector3 originColliderSize;

	private PictureController picture;
	private Vector3 relativeOffset;
	private float upNormalThreshold = 0.9f;

	void Start() {
		isEditing = false;
		originaButtonScale = transform.localScale;
		collider = GetComponent<BoxCollider>();
		originColliderSize = collider.size;

		picture = GetComponentInParent<PictureController>();
		relativeOffset = transform.position - picture.transform.position;
		relativeOffset.z = -relativeOffset.z;
	}

	void Update() {

		if (!isEditing) 
			return;

		if (!Input.GetMouseButton(0)) {
			DoneEdit();
		}

		if (Input.touchCount <= 0)
			return;

		var touch = Input.GetTouch (0);
		Camera camera = GetCamera();

		if (touch.phase == TouchPhase.Began) {

			Ray ray = camera.ScreenPointToRay(Input.mousePosition);

			int layerMask = 1 << LayerMask.NameToLayer("ARGameObject"); // Planes are in layer ARGameObject

			RaycastHit rayHit;
			if (Physics.Raycast(ray, out rayHit, float.MaxValue, layerMask)) {
				picture.transform.position = new Vector3(rayHit.point.x, camera.transform.position.y, rayHit.point.z);

				// since ARInterface only checks for horizontal planes, make sure anchor is vertical and facing camera
				picture.transform.LookAt(picture.transform.position + camera.transform.rotation * Vector3.forward,
					camera.transform.rotation * Vector3.up);

			}
		} else {
			// slide object vertically from starting hit point
			picture.transform.position = new Vector3(picture.transform.position.x, camera.transform.position.y, picture.transform.position.z);
			picture.transform.LookAt(picture.transform.position + camera.transform.rotation * Vector3.forward,
				camera.transform.rotation * Vector3.up);
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
