using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IpcIrcEventHandler))]
public class IpcIrcEventHandlerEditor : Editor {
    IpcIrcEventHandler t;
    SerializedObject GetTarget;
    SerializedProperty ThisString;
    SerializedProperty ThisList;
    int ListSize;

    void OnEnable() {
        t = (IpcIrcEventHandler)target;
        GetTarget = new SerializedObject(t);
        ThisString = GetTarget.FindProperty("triggerPhrase"); // Find the triggerphrase string
        ThisList = GetTarget.FindProperty("myEventList"); // Find the List in our script and create a refrence of it
    }

    public override void OnInspectorGUI() {
        // Update our list
        GetTarget.Update();
        EditorGUILayout.LabelField("Define a trigger phrase, then the text following.");
        // Display the trigger phrase field.
        ThisString.stringValue = EditorGUILayout.TextField("Trigger Phrase", ThisString.stringValue);
        ListSize = ThisList.arraySize; // Display the List size field
        ListSize = EditorGUILayout.IntField("Triggers Registered", ListSize);
        if (ListSize != ThisList.arraySize) { // Repaint the list if it changes
            while (ListSize > ThisList.arraySize)
                ThisList.InsertArrayElementAtIndex(ThisList.arraySize);
            while (ListSize < ThisList.arraySize)
                ThisList.DeleteArrayElementAtIndex(ThisList.arraySize - 1);
        }
        for (int i = 0; i < ThisList.arraySize; i++) { // Display our list to the inspector window
            SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);
            SerializedProperty MyString = MyListRef.FindPropertyRelative("startsWith");
            SerializedProperty MyEvent = MyListRef.FindPropertyRelative("triggerEvent");
            EditorGUILayout.Space();
            GUI.color = Color.white; // Return the GUI color to default white.
            EditorGUILayout.PropertyField(MyString);
            EditorGUILayout.PropertyField(MyEvent);
            GUI.color = Color.red; // Change the GUI color for the next element.
            if (GUILayout.Button("Remove This Trigger (" + i.ToString() + ")"))
                ThisList.DeleteArrayElementAtIndex(i); // Remove this index from the List
        }
        EditorGUILayout.Space();
        GUI.color = Color.green; // Change the GUI color for the next element.
        if (GUILayout.Button("Add New Trigger"))
            t.myEventList.Add(new IpcIrcEventHandler.IpcIrcEventList());
        GetTarget.ApplyModifiedProperties(); // Apply the changes to our inspector
    }
}
