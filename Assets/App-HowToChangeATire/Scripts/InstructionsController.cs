using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InstructionEvent : UnityEvent<InstructionStep>
{

}

public class InstructionsController : MonoBehaviour
{
	public InstructionEvent OnInstructionUpdate = new InstructionEvent ();

	private int currentStep;
	private InstructionModel currentInstructionModel = new InstructionModel ();

	public GameObject standardContent;
	private bool arMode;

	public GameObject anchorButton;
	public GameObject arPrompt;
	private bool anchorMode;

	void Awake ()
	{
		currentInstructionModel.LoadData ();
	}

	void Start ()
	{
		currentStep = 0;
		CurrentInstructionUpdate ();
	}

	public void NextStep ()
	{
		if (currentStep < currentInstructionModel.GetCount () - 1) {
			currentStep++;
			CurrentInstructionUpdate ();
		}
	}

	public void PreviousStep ()
	{
		if (currentStep > 0) {
			currentStep--;
			CurrentInstructionUpdate ();
		}
	}

	private void CurrentInstructionUpdate ()
	{
		InstructionStep step = currentInstructionModel.GetInstructionStep (currentStep);
		OnInstructionUpdate.Invoke (step);
	}

	public void ToggleAr ()
	{
		arMode = !arMode;
		if (arMode) {
			TurnOnArMode ();
		} else {
			TurnOffArMode ();
		}
	}

	void TurnOnArMode ()
	{
		standardContent.SetActive (false);

		TurnOffAnchorMode ();
	}

	void TurnOffArMode ()
	{
		standardContent.SetActive (true);

		anchorButton.SetActive (false);
		arPrompt.SetActive (false);
	}

	public void ToggleAnchor ()
	{
		anchorMode = !anchorMode;
		if (anchorMode) {
			TurnOnAnchorMode ();
		} else {
			TurnOffAnchorMode ();
		}
	}

	void TurnOnAnchorMode ()
	{
		anchorButton.SetActive (false);
		arPrompt.SetActive (true);
	}

	void TurnOffAnchorMode ()
	{
		anchorButton.SetActive (true);
		arPrompt.SetActive (false);
	}

}
