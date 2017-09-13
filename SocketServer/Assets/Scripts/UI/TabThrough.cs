using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabThrough : MonoBehaviour
{
	EventSystem system;

	void Start()
	{
		system = EventSystem.current;// EventSystemManager.currentSystem;

	}
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (system.currentSelectedGameObject == null) {
				return;
			}
			Selectable current = system.currentSelectedGameObject.GetComponent<Selectable> ();

			InputField currentInputField = current.GetComponent<InputField> ();
			if (currentInputField != null) {
				currentInputField.text = currentInputField.text.Trim();
			}

			Selectable next = current.FindSelectableOnDown();

			if (next != null) {

				InputField inputfield = next.GetComponent<InputField> ();
				if (inputfield != null) {
					inputfield.OnPointerClick (new PointerEventData (system));  //if it's an input field, also set the text caret
				}
				system.SetSelectedGameObject (next.gameObject, new BaseEventData (system));
			} else {
				AddRemoveOption.singleton.AddNewOption ();
				//OptionScript.optionList [OptionScript.optionList.Count - 1].mainInputField.OnPointerClick (new PointerEventData (system));
				//system.SetSelectedGameObject (OptionScript.optionList [OptionScript.optionList.Count - 1].mainInputField.gameObject, new BaseEventData (system));
			}

		}
	}
}
