using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(IpcIrc))]
public class IpcIrcEditor : Editor
{
    #region variables
    #region computed_variables
    IpcIrc t;
    SerializedObject GetTarget;
    AnimBool configGroupEnabled = new AnimBool(false);
    AnimBool serverGroupEnabled = new AnimBool(false);
    #endregion computed_variables
    #region serialized_variables
    SerializedProperty IpcIrcHandshake;
    SerializedProperty Servers;
    SerializedProperty ServerName;
    SerializedProperty ServerPort;
    SerializedProperty MessageDebug;
    SerializedProperty ConnectOnAwake;
    SerializedProperty SetInvisibleMode;
    SerializedProperty globalNickname;
    SerializedProperty globalAuthString;
    SerializedProperty CommandChannel;
    SerializedProperty Channels;
    SerializedProperty AutoJoinChannels;
    #endregion serialized_variables
    int ServersSize;
    int ChannelsSize;
    #endregion variables

    void OnEnable()
    {
        t = (IpcIrc)target;
        GetTarget = new SerializedObject(t);
        IpcIrcHandshake = GetTarget.FindProperty("IpcIrcHandshake");
        Servers = GetTarget.FindProperty("Servers");
        ServerName = GetTarget.FindProperty("ServerName");
        ServerPort = GetTarget.FindProperty("ServerPort");
        MessageDebug = GetTarget.FindProperty("MessageDebug");
        ConnectOnAwake = GetTarget.FindProperty("ConnectOnAwake");
        SetInvisibleMode = GetTarget.FindProperty("SetInvisibleMode");
        globalNickname = GetTarget.FindProperty("Nickname");
        globalAuthString = GetTarget.FindProperty("AuthString");
        CommandChannel = GetTarget.FindProperty("CommandChannel");
        Channels = GetTarget.FindProperty("Channels");
        AutoJoinChannels = GetTarget.FindProperty("AutoJoinChannels");
    }

    public override void OnInspectorGUI()
    {
        // Update our reference to the serialized object.
        GetTarget.Update();
        EditorGUILayout.PropertyField(IpcIrcHandshake);
        configGroupEnabled.target = EditorGUILayout.ToggleLeft("Configuration", configGroupEnabled.target);
        if (EditorGUILayout.BeginFadeGroup(configGroupEnabled.faded))
        {
            EditorGUILayout.LabelField("Automatically connect when the scene loads:");
            EditorGUILayout.PropertyField(ConnectOnAwake);
            EditorGUILayout.PropertyField(AutoJoinChannels);
            EditorGUILayout.LabelField("IRC Server Parameters:");
            // Display the IRC Server fields.
            UpdateServersView();
            EditorGUILayout.PropertyField(ServerName);
            EditorGUILayout.PropertyField(ServerPort);
            EditorGUILayout.PropertyField(globalNickname);
            globalAuthString.stringValue = EditorGUILayout.TextField("Authentication", globalAuthString.stringValue);
            EditorGUILayout.PropertyField(SetInvisibleMode);
            EditorGUILayout.PropertyField(MessageDebug);
            EditorGUILayout.LabelField("Channel Configuration:");
            // Display the channels fields.

            CommandChannel.stringValue = EditorGUILayout.TextField("Command Channel", CommandChannel.stringValue);
            // Display the Channels interface.
            UpdateChannelsView();
        }
        GetTarget.ApplyModifiedProperties(); // Apply the changes to our inspector
    }

    public void UpdateServersView()
    {
        // Display the Servers interface.
        ServersSize = Servers.arraySize; // Display the List size field
        ServersSize = EditorGUILayout.IntField("Servers Registered", ServersSize);
        if (ServersSize != Servers.arraySize)
        { // Repaint the list if it changes
            while (ServersSize > Servers.arraySize)
                Servers.InsertArrayElementAtIndex(Servers.arraySize);
            while (ServersSize < Servers.arraySize)
                Servers.DeleteArrayElementAtIndex(Servers.arraySize - 1);
        }
        for (int i = 0; i < Servers.arraySize; i++)
        { // Display our list to the inspector window
            SerializedProperty MyListRef = Servers.GetArrayElementAtIndex(i);
            // These are known beforehand and can be edited.
            SerializedProperty serverName = MyListRef.FindPropertyRelative("serverName");  // What is the server's name?
            SerializedProperty serverPort = MyListRef.FindPropertyRelative("serverPort");  // What is the server's port?
            SerializedProperty nickname = MyListRef.FindPropertyRelative("nickname");  // What is the user's nickname?
            SerializedProperty authString = MyListRef.FindPropertyRelative("authString");  // What is the user's authentifier?
            SerializedProperty serverSetUserInvisible = MyListRef.FindPropertyRelative("serverSetUserInvisible"); // Set invisible mode after connecting?
            SerializedProperty serverConnectAutomatically = MyListRef.FindPropertyRelative("serverConnectAutomatically"); // Connect automatically?
            // These are learned after we connect.
            SerializedProperty serverActive = MyListRef.FindPropertyRelative("serverActive");  // Is the server currently active?
            SerializedProperty serverIdent = MyListRef.FindPropertyRelative("serverIdent");  // What is the server's identification? (msgtype 002)
            SerializedProperty serverAge = MyListRef.FindPropertyRelative("serverAge");  // What is the server's age? (msgtype 003)
            SerializedProperty serverVersion = MyListRef.FindPropertyRelative("serverVersion");  // What is the server's Version? (msgtype 004)
            SerializedProperty serverCapabilities = MyListRef.FindPropertyRelative("serverCapabilities");  // What are the server's capabilities? (msgtype 005)
            SerializedProperty serverConnectionCount = MyListRef.FindPropertyRelative("serverConnectionCount");  // What is the server's Connection Count? (msgtype 250)
            SerializedProperty serverUserCount = MyListRef.FindPropertyRelative("serverUserCount");  // What is the server's User Count? (msgtype 251)
            SerializedProperty serverOperCount = MyListRef.FindPropertyRelative("serverOperCount");  // What is the server's Oper Count? (msgtype 252)
            SerializedProperty serverChannelCount = MyListRef.FindPropertyRelative("serverChannelCount");  // What is the server's Channel Count? (msgtype 254)
            SerializedProperty serverClientCount = MyListRef.FindPropertyRelative("serverClientCount");  // What is the server's Client Count? (msgtype 255)
            SerializedProperty serverLocalUserMax = MyListRef.FindPropertyRelative("serverLocalUserMax");  // What is the server's User Count? (msgtype 265)
            SerializedProperty serverGlobalUserMax = MyListRef.FindPropertyRelative("serverGlobalUserMax");  // What is the server's User Count? (msgtype 266)

            EditorGUILayout.Space();
            GUI.color = Color.white; // Return the GUI color to default white.
            EditorGUILayout.PropertyField(serverName);
            EditorGUILayout.PropertyField(serverPort);
            EditorGUILayout.PropertyField(nickname);
            EditorGUILayout.PropertyField(authString);
            EditorGUILayout.PropertyField(serverSetUserInvisible);
            EditorGUILayout.PropertyField(serverConnectAutomatically);
            serverGroupEnabled.target = EditorGUILayout.ToggleLeft("Extended Info", serverGroupEnabled.target);
            if (EditorGUILayout.BeginFadeGroup(serverGroupEnabled.faded))
            {
                EditorGUILayout.BeginToggleGroup("Extended Info", false);
                EditorGUILayout.PropertyField(serverActive);
                EditorGUILayout.PropertyField(serverIdent);
                EditorGUILayout.PropertyField(serverAge);
                EditorGUILayout.PropertyField(serverVersion);
                EditorGUILayout.PropertyField(serverCapabilities);
                EditorGUILayout.PropertyField(serverConnectionCount);
                EditorGUILayout.PropertyField(serverUserCount);
                EditorGUILayout.PropertyField(serverOperCount);
                EditorGUILayout.PropertyField(serverChannelCount);
                EditorGUILayout.PropertyField(serverClientCount);
                EditorGUILayout.PropertyField(serverLocalUserMax);
                EditorGUILayout.PropertyField(serverGlobalUserMax);
                EditorGUILayout.EndToggleGroup();
            }
            EditorGUILayout.EndFadeGroup();


            GUI.color = Color.red; // Change the GUI color for the next element.
            if (GUILayout.Button("Remove This Server (" + i.ToString() + ")"))
                Servers.DeleteArrayElementAtIndex(i); // Remove this index from the List
        }
        EditorGUILayout.Space();
        GUI.color = Color.green; // Change the GUI color for the next element.
        if (GUILayout.Button("Add new Server"))
            t.AddServer();
        GUI.color = Color.white; // Change the GUI color for the next element.
    }

    public void UpdateChannelsView()
    {
        // Display the Channels interface.
        ChannelsSize = Channels.arraySize; // Display the List size field
        ChannelsSize = EditorGUILayout.IntField("Channels Registered", ChannelsSize);
        if (ChannelsSize != Channels.arraySize)
        { // Repaint the list if it changes
            while (ChannelsSize > Channels.arraySize)
                Channels.InsertArrayElementAtIndex(Channels.arraySize);
            while (ChannelsSize < Channels.arraySize)
                Channels.DeleteArrayElementAtIndex(Channels.arraySize - 1);
        }
        for (int i = 0; i < Channels.arraySize; i++)
        { // Display our list to the inspector window
            SerializedProperty MyListRef = Channels.GetArrayElementAtIndex(i);
            SerializedProperty channelName = MyListRef.FindPropertyRelative("channelName");
            SerializedProperty channelActive = MyListRef.FindPropertyRelative("channelActive");
            SerializedProperty channelTopic = MyListRef.FindPropertyRelative("channelTopic");
            SerializedProperty channelModes = MyListRef.FindPropertyRelative("channelModes");
            SerializedProperty channelNicklist = MyListRef.FindPropertyRelative("channelNicklist");
            EditorGUILayout.Space();
            GUI.color = Color.white; // Return the GUI color to default white.
            EditorGUILayout.PropertyField(channelName);
            EditorGUILayout.PropertyField(channelActive);
            EditorGUILayout.PropertyField(channelTopic);
            EditorGUILayout.PropertyField(channelModes);
            EditorGUILayout.PropertyField(channelNicklist);
            GUI.color = Color.red; // Change the GUI color for the next element.
            if (GUILayout.Button("Remove This Channel (" + i.ToString() + ")"))
                Channels.DeleteArrayElementAtIndex(i); // Remove this index from the List
        }
        EditorGUILayout.Space();
        GUI.color = Color.green; // Change the GUI color for the next element.
        if (GUILayout.Button("Add new Channel"))
            t.AddChannel();
        GUI.color = Color.white; // Change the GUI color for the next element.
    }
}
