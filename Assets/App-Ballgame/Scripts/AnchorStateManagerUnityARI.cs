using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityARInterface;

public class AnchorStateManagerUnityARI : ARBase {


	[Tooltip("Root game object to position as anchor")]
	public Transform anchor;
	public GameController gameController;

	[Header("Anchor mode objects")]
	public GameObject instructionHolder;
	public GameObject arRoot;
	public GameObject anchorObject;

	[Header("Game mode objects")]
	public GameObject ballgameObject;
	public GameObject resetButton;    



	private bool isAnchorMode = true;
	private ARPointCloudVisualizer arPoints;
	private ARPlaneVisualizer arPlanes;

	void Start() {
		arPoints = arRoot.GetComponent<ARPointCloudVisualizer>();
		arPlanes = arRoot.GetComponent<ARPlaneVisualizer>();
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
		arPoints.enabled = anchor;
		arPlanes.enabled = anchor;
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
		if (Input.touchCount > 0 && anchor != null) {

			var touch = Input.GetTouch (0);
			if (touch.phase == TouchPhase.Began) {
				var camera = GetCamera();

				Ray ray = camera.ScreenPointToRay(Input.mousePosition);

				int layerMask = 1 << LayerMask.NameToLayer("ARGameObject"); // Planes are in layer ARGameObject

				RaycastHit rayHit;
				if (Physics.Raycast(ray, out rayHit, float.MaxValue, layerMask)) {
					anchor.position = rayHit.point;

					anchor.LookAt(anchor.position + camera.transform.rotation * Vector3.forward,
						camera.transform.rotation * Vector3.up);
				}

			}
		}

	}
}
