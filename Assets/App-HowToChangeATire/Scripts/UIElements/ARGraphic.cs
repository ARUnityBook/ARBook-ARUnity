using UnityEngine;

public class ARGraphic : InstructionElement {
    private GameObject currentGraphic;

    protected override void InstructionUpdate(InstructionStep step) {
        // clear current graphic
        if (currentGraphic != null) {
            Destroy(currentGraphic);
            currentGraphic = null;
        }

        // load step's graphic
        if (!string.IsNullOrEmpty(step.ARPrefabName)) {
			Debug.Log("ARGraphic: " + step.ARPrefabName);
            currentGraphic = Instantiate(Resources.Load(step.ARPrefabName, typeof(GameObject)), transform) as GameObject;
        }
    }
}
