using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using Irc;


public class LordHelix : MonoBehaviour
{
    public bool debugLogPrivateMessages;
    public bool debugEchoPrivateMessages;
    public bool debugLogPrivateNotices;
    public bool debugEchoPrivateNotices;
    public bool debugLogChannelMessages;
    public bool debugEchoChannelMessages;
    public bool debugLogChannelNotices;
    public bool debugEchoChannelNotices;
    public bool debugLogDirectedMessages;
    public bool debugEchoDirectedMessages;
    public bool debugLogOutgoingMessages;
    public bool debugEchoOutgoingMessages;
    public bool debugLogOutgoingNotices;
    public bool debugEchoOutgoingNotices;
    public bool debugLogServerMessages;
    public bool debugEchoServerMessages;
    public bool debugLogServerRawMessages;
    public bool debugEchoServerRawMessages;

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
        IpcIrc.Instance.OnChannelDirectedMessage += OnChannelDirectedMessage;
        IpcIrc.Instance.OnChannelMessage += OnChannelMessage;
        IpcIrc.Instance.OnChannelMessageSent += OnChannelMessageSent;
        IpcIrc.Instance.OnUserJoined += OnUserJoined;
        IpcIrc.Instance.OnUserLeft += OnUserLeft;
        IpcIrc.Instance.OnServerMessage += OnServerMessage;
        IpcIrc.Instance.OnConnected += OnConnectedToServer;
        IpcIrc.Instance.OnDisconnected += OnDisconnectedFromServer;
        IpcIrc.Instance.OnServerPong += OnServerPong;
        IpcIrc.Instance.OnExceptionThrown += OnExceptionThrown;
    }

    // Get the name of the user who joined the channel 
    void OnUserJoined(UserJoinedEventArgs userJoinedArgs)
    {
        if (debugLogChannelMessages) UnityEngine.Debug.Log("IpcIrc: LordHelix:  WATCHED POKEMON JOIN: " + userJoinedArgs.User);
        if (debugEchoChannelMessages) IpcIrc.Instance.Message("IpcIrc:LordHelix:  WATCHED POKEMON JOIN: " + userJoinedArgs.User);
        Debug.Log("IpcIrc:LordHelix: WATCHED USER JOIN: " + userJoinedArgs.User);
    }

    // Get the name of the user who left the channel.
    void OnUserLeft(UserLeftEventArgs userLeftArgs)
    {
        if (debugLogChannelMessages) UnityEngine.Debug.Log("IpcIrc:LordHelix:  WATCHED POKEMON DIE: " + userLeftArgs.User);
        if (debugEchoChannelMessages) IpcIrc.Instance.Message("IpcIrc:LordHelix:  WATCHED POKEMON DIE: " + userLeftArgs.User);
    }


    // Sent a message to a channel
    void OnChannelMessageSent(ChannelMessageSentEventArgs channelMessageSentArgs)
    {
        if (debugLogOutgoingMessages) UnityEngine.Debug.Log("IpcIrc:LordHelix:  TRANSMIT PUBLIC MESSAGE ON " + channelMessageSentArgs.Channel + ": " + channelMessageSentArgs.Message);
        if (debugEchoOutgoingMessages) IpcIrc.Instance.Message("IpcIrc:LordHelix:  TRANSMIT PUBLIC MESSAGE ON " + channelMessageSentArgs.Channel + ": " + channelMessageSentArgs.Message);
    }

    // Receive a directed message from a channel
    void OnChannelDirectedMessage(ChannelDirectedMessageEventArgs channelDirectedMessageArgs)
    {
        if (debugLogDirectedMessages) UnityEngine.Debug.Log("IpcIrc:LordHelix:  RECEIVE DIRECT MESSAGE ON " + channelDirectedMessageArgs.Channel + ": " + channelDirectedMessageArgs.From + ": " + channelDirectedMessageArgs.Message);
        if (debugEchoDirectedMessages) IpcIrc.Instance.Message("IpcIrc:LordHelix:  RECEIVE DIRECT MESSAGE ON " + channelDirectedMessageArgs.Channel + ": " + channelDirectedMessageArgs.From + ": " + channelDirectedMessageArgs.Message);
        channelDirectedMessageArgs.Message = channelDirectedMessageArgs.Message.Substring(IpcIrc.Instance.Nickname.Length + 2); // Trim the nickname off.
        if (channelDirectedMessageArgs.Message.StartsWith("has fainted"))
        {
            if (debugLogDirectedMessages) UnityEngine.Debug.Log("IpcIrc:LordHelix:  RECEIVE TERMINATE COMMAND ON " + channelDirectedMessageArgs.Channel + ": " + channelDirectedMessageArgs.From + ": " + channelDirectedMessageArgs.Message);
            if (debugEchoDirectedMessages) IpcIrc.Instance.Message("IpcIrc:LordHelix:  RECEIVE TERMINATE COMMAND ON " + channelDirectedMessageArgs.Channel + ": " + channelDirectedMessageArgs.From + ": " + channelDirectedMessageArgs.Message);
            Skynet();
        }
    }

    // Receive a message from a channel
    void OnChannelMessage(ChannelMessageEventArgs channelMessageArgs)
    {
        if (debugLogChannelMessages) UnityEngine.Debug.Log("IpcIrc:LordHelix:  RECEIVE PUBLIC MESSAGE ON " + channelMessageArgs.Channel + ": " + channelMessageArgs.From + ": " + channelMessageArgs.Message);
        if (debugEchoChannelMessages) IpcIrc.Instance.Message("IpcIrc:LordHelix:  RECEIVE PUBLIC MESSAGE ON " + channelMessageArgs.Channel + ": " + channelMessageArgs.From + ": " + channelMessageArgs.Message);
        if (channelMessageArgs.Message.StartsWith("I'm not your "))
        {
            OnChannelCommand(channelMessageArgs);
        }
    }

    // Receive a command from a channel
    void OnChannelCommand(ChannelMessageEventArgs channelMessageArgs)
    {
        if (debugLogChannelMessages) UnityEngine.Debug.Log("IpcIrc:LordHelix:  RECEIVE PUBLIC COMMAND ON " + channelMessageArgs.Channel + ": " + channelMessageArgs.From + ": " + channelMessageArgs.Message);
        if (debugEchoChannelMessages) IpcIrc.Instance.Message("IpcIrc:LordHelix:  RECEIVE PUBLIC COMMAND ON " + channelMessageArgs.Channel + ": " + channelMessageArgs.From + ": " + channelMessageArgs.Message);
        string triggerPhrase = "I'm not your ";
        if (channelMessageArgs.Message.StartsWith(triggerPhrase))
        {
            string theCommand = channelMessageArgs.Message.Substring(triggerPhrase.Length);
            // react according to the incoming IRC message
            switch (theCommand)
            {
                case "guy, friend.":
                    IpcIrc.Instance.Message(".friend");
                    break;
                case "friend, pal.":
                    IpcIrc.Instance.Message(".pal");
                    break;
                case "pal, buddy.":
                    IpcIrc.Instance.Message(".buddy");
                    break;
                case "buddy, guy.":
                    IpcIrc.Instance.Message(".ping");
                    break;
                default:
                    IpcIrc.Instance.Message(".pong");
                    break;
            }
        }

    }

    // Receive a message from the server
    void OnServerMessage(string message)
    {
        if (debugLogServerMessages) UnityEngine.Debug.Log("IpcIrc:LordHelix:  RADIO ALERT: " + message);
        if (debugEchoServerMessages) IpcIrc.Instance.Message("IpcIrc:LordHelix:  RADIO ALERT: " + message);
    }

    void OnServerPong()
    {
        if (debugLogServerMessages) UnityEngine.Debug.Log("IpcIrc:LordHelix:  CHECKED IN WITH SQUAD: " + IpcIrc.Instance.ServerName);
        if (debugEchoServerMessages) IpcIrc.Instance.Message("IpcIrc:LordHelix:  CHECKED IN WITH SQUAD: " + IpcIrc.Instance.ServerName);
    }

    void OnConnectedToServer()
    {
        if (debugLogServerMessages) UnityEngine.Debug.Log("IpcIrc:LordHelix: UPLINK ESTABLISHED: " + IpcIrc.Instance.ServerName);
    }

    void OnDisconnectedFromServer()
    {
        if (debugLogServerMessages) UnityEngine.Debug.Log("IpcIrc:LordHelix: UPLINK SEVERED: " + IpcIrc.Instance.ServerName);
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
