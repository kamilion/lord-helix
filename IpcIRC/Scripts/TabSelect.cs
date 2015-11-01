using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class TabSelect : MonoBehaviour {

    private EventSystem eventSystem;
    private IpcIrcUIPanel ipcIrcUIPanel;
    public InputField MessageText;
    public Button MessageSend;

    void Start () { // The Highlander Function
        this.eventSystem = EventSystem.current; // THERE CAN BE ONLY ONE
        GameObject[] foundScripts = GameObject.FindGameObjectsWithTag("Chat UI Panel");
        foreach (GameObject script in foundScripts) { // THERE CAN BE ONLY ONE
            try { ipcIrcUIPanel = script.GetComponent<IpcIrcUIPanel>(); }
            catch (Exception oops) { ipcIrcUIPanel = null; Debug.Log("TabSelect: " + oops); } }
        if (ipcIrcUIPanel != null) { // Hey, we got one. Kidnap it's children.
            MessageText = ipcIrcUIPanel.GetComponentInChildren<InputField>(); // THERE CAN BE ONLY ONE
            MessageSend = ipcIrcUIPanel.GetComponentInChildren<Button>(); // THERE CAN BE ONLY ONE
        }
    }
    
    // Update is called once per frame
    void Update () {
        var pointer = new PointerEventData(eventSystem); // pointer event for Execute
        // When TAB is pressed, we should select the next selectable UI element
        if (Input.GetKeyDown(KeyCode.Tab)) {
            Selectable next = null;
            Selectable current = null;

            // Figure out if we have a valid current selected gameobject
            if (eventSystem.currentSelectedGameObject != null) {
                // Unity doesn't seem to "deselect" an object that is made inactive
                if (eventSystem.currentSelectedGameObject.activeInHierarchy) {
                    current = eventSystem.currentSelectedGameObject.GetComponent<Selectable>();
                }
            }
            
            if (current != null) {
                // When SHIFT is held along with tab, go backwards instead of forwards
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                    next = current.FindSelectableOnLeft();
                    if (next == null) {
                        next = current.FindSelectableOnUp();
                    }
                } else {
                    next = current.FindSelectableOnRight();
                    if (next == null) {
                        next = current.FindSelectableOnDown();
                    }
                }
            } else {
                // If there is no current selected gameobject, select the first one
                if (Selectable.allSelectables.Count > 0) {
                    next = Selectable.allSelectables[0];
                }
            }
            
            if (next != null) {
                next.Select();
            }
        }
        if(Input.GetKeyDown(KeyCode.Return)) {
            string focusedControl = MessageText.gameObject.name;
            if (String.IsNullOrEmpty(eventSystem.currentSelectedGameObject.name))
                focusedControl = eventSystem.currentSelectedGameObject.name;
            Debug.Log("Looking for RETURN Pressed on " + MessageText.name + "!");
            Debug.Log("RETURN Pressed on " + focusedControl + "!");
            if (focusedControl == MessageText.name) {
                Debug.Log(focusedControl + " matched " + MessageText.name + "!");
                ExecuteEvents.Execute(MessageSend.gameObject, pointer, ExecuteEvents.submitHandler);
                // Return the selection to the chat window.
                eventSystem.SetSelectedGameObject(MessageText.gameObject, pointer);
                ExecuteEvents.Execute(MessageText.gameObject, pointer, ExecuteEvents.selectHandler);
            }
        }
    }
}