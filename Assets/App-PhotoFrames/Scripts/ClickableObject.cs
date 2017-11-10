using UnityEngine;
using UnityEngine.Events;

public class ClickableObjectEvent : UnityEvent<GameObject> { }

public class ClickableObject : MonoBehaviour {

	public ClickableObjectEvent OnClickableObjectClicked = new ClickableObjectEvent();

	void OnMouseDown() {
		PhotoFrameController.instance.PlayClickFeedback();
		OnClickableObjectClicked.Invoke(gameObject);
	}
}
