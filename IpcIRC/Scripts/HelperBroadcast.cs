using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using Irc;


public class HelperBroadcast : MonoBehaviour
{
    public void Skynet()
    {
        IpcIrc.Instance.LeaveServer("This IpcIRC Instance was terminated normally.");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void Start()
    { // Subscribe for events
        IpcIrc.Instance.OnServerPong += OnServerPong;
        IpcIrc.Instance.OnChannelDirectedMessage += OnChannelDirectedMessage;
    }

    // Receive a directed message from a channel
    void OnChannelDirectedMessage(ChannelDirectedMessageEventArgs channelDirectedMessageArgs)
    {
        UnityEngine.Debug.Log("IpcIrc:HelperBroadcast:  RECEIVE DIRECT MESSAGE ON " + channelDirectedMessageArgs.Channel + ": " + channelDirectedMessageArgs.From + ": " + channelDirectedMessageArgs.Message);
        channelDirectedMessageArgs.Message = channelDirectedMessageArgs.Message.Substring(IpcIrc.Instance.Nickname.Length + 2); // Trim the nickname off.
        if (channelDirectedMessageArgs.Message.StartsWith("you are terminated"))
        {
            UnityEngine.Debug.Log("IpcIrc:HelperBroadcast:  RECEIVE TERMINATE COMMAND ON " + channelDirectedMessageArgs.Channel + ": " + channelDirectedMessageArgs.From + ": " + channelDirectedMessageArgs.Message);
            Skynet();
        }
    }

    void OnServerPong()
    {
        UnityEngine.Debug.Log("IpcIrc:HelperBroadcast:  Server pong from: " + IpcIrc.Instance.ServerName);
        IpcIrc.Instance.Message("This channel has MOVED! Please connect to irc.coldfront.net / #twitchinstalls -- Flash: http://goo.gl/8lfPgq or HTML5: http://goo.gl/2BuHD2");
    }

    void OnConnectedToServer()
    {
        UnityEngine.Debug.Log("IpcIrc:HelperBroadcast: UPLINK ESTABLISHED: " + IpcIrc.Instance.ServerName);
    }

    void OnDisconnectedFromServer()
    {
        UnityEngine.Debug.Log("IpcIrc:HelperBroadcast: UPLINK SEVERED: " + IpcIrc.Instance.ServerName);
        Reconnect();
    }

    IEnumerator Reconnect()
    { // Reconnect to the server after some time.
        yield return new WaitForSeconds(2);
        IpcIrc.Instance.Connect();
    }

    // Receive exception if something goes wrong
    private void OnExceptionThrown(Exception oops)
    {
        Debug.Log(oops);
    }
}
