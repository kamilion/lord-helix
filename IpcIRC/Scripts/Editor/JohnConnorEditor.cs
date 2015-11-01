using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JohnConnor))]
public class JohnConnorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector(); // debugging
        JohnConnor myLog = (JohnConnor)target; // Grab a reference to the real john connor.
        myLog.debugLogPrivateMessages = EditorGUILayout.ToggleLeft("Log Private Messages?", myLog.debugLogPrivateMessages, GUILayout.ExpandWidth(true));
        myLog.debugEchoPrivateMessages = EditorGUILayout.ToggleLeft("Echo Private Messages?", myLog.debugEchoPrivateMessages, GUILayout.ExpandWidth(true));
        myLog.debugLogPrivateNotices = EditorGUILayout.ToggleLeft("Log Private Notices?", myLog.debugLogPrivateNotices, GUILayout.ExpandWidth(true));
        myLog.debugEchoPrivateNotices = EditorGUILayout.ToggleLeft("Echo Private Notices?", myLog.debugEchoPrivateNotices, GUILayout.ExpandWidth(true));
        myLog.debugLogChannelMessages = EditorGUILayout.ToggleLeft("Log Channel Messages?", myLog.debugLogChannelMessages, GUILayout.ExpandWidth(true));
        myLog.debugEchoChannelMessages = EditorGUILayout.ToggleLeft("Echo Channel Messages?", myLog.debugEchoChannelMessages, GUILayout.ExpandWidth(true));
        myLog.debugLogChannelNotices = EditorGUILayout.ToggleLeft("Log Channel Notices?", myLog.debugLogChannelNotices, GUILayout.ExpandWidth(true));
        myLog.debugEchoChannelNotices = EditorGUILayout.ToggleLeft("Echo Channel Notices?", myLog.debugEchoChannelNotices, GUILayout.ExpandWidth(true));
        myLog.debugLogDirectedMessages = EditorGUILayout.ToggleLeft("Log Directed Messages?", myLog.debugLogDirectedMessages, GUILayout.ExpandWidth(true));
        myLog.debugEchoDirectedMessages = EditorGUILayout.ToggleLeft("Echo Directed Messages?", myLog.debugEchoDirectedMessages, GUILayout.ExpandWidth(true));
        myLog.debugLogOutgoingMessages = EditorGUILayout.ToggleLeft("Log Outgoing Messages?", myLog.debugLogOutgoingMessages, GUILayout.ExpandWidth(true));
        myLog.debugEchoOutgoingMessages = EditorGUILayout.ToggleLeft("Echo Outgoing Messages?", myLog.debugEchoOutgoingMessages, GUILayout.ExpandWidth(true));
        myLog.debugLogOutgoingNotices = EditorGUILayout.ToggleLeft("Log Outgoing Notices?", myLog.debugLogOutgoingNotices, GUILayout.ExpandWidth(true));
        myLog.debugEchoOutgoingNotices = EditorGUILayout.ToggleLeft("Echo Outgoing Notices?", myLog.debugEchoOutgoingNotices, GUILayout.ExpandWidth(true));
        myLog.debugLogServerMessages = EditorGUILayout.ToggleLeft("Log Server Messages?", myLog.debugLogServerMessages, GUILayout.ExpandWidth(true));
        myLog.debugEchoServerMessages = EditorGUILayout.ToggleLeft("Echo Server Messages?", myLog.debugEchoServerMessages, GUILayout.ExpandWidth(true));
        myLog.debugLogServerRawMessages = EditorGUILayout.ToggleLeft("Log Server Raw Messages?", myLog.debugLogServerRawMessages, GUILayout.ExpandWidth(true));
        myLog.debugEchoServerRawMessages = EditorGUILayout.ToggleLeft("Echo Server Raw Messages?", myLog.debugEchoServerRawMessages, GUILayout.ExpandWidth(true));

    }
}
