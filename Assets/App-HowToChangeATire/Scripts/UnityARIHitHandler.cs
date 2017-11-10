using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityARInterface;

public class UnityARIHitHandler : ARBase {
	
	public Transform anchor;
	public InstructionsController controller;


	void Update () {

		if (Input.touchCount > 0 && anchor != null) {

			var touch = Input.GetTouch (0);
			if (touch.phase == TouchPhase.Began) {
				var camera = GetCamera();

				Ray ray = camera.ScreenPointToRay(Input.mousePosition);

				int layerMask = 1 << LayerMask.NameToLayer("ARGameObject"); // Planes are in layer ARGameObject

				RaycastHit rayHit;
				if (Physics.Raycast(ray, out rayHit, float.MaxValue, layerMask)) {
					anchor.position = rayHit.point;

					// since ARInterface only checks for horizontal planes, make sure anchor is vertical and facing camera
					anchor.LookAt(anchor.position + camera.transform.rotation * Vector3.forward,
						camera.transform.rotation * Vector3.up);

					controller.ToggleAnchor ();
				}
					
			}
		}
	}
}
