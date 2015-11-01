using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Irc;

public class IpcIrcDebugLogger : MonoBehaviour {
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

    // Use this for initialization
    void Start()
    {
        // Subscribe for events
        IpcIrc.Instance.OnChannelAction += OnChannelAction;
        IpcIrc.Instance.OnChannelActionSent += OnChannelActionSent;
        IpcIrc.Instance.OnChannelDirectedMessage += OnChannelDirectedMessage;
        IpcIrc.Instance.OnChannelMessage += OnChannelMessage;
        IpcIrc.Instance.OnChannelMessageSent += OnChannelMessageSent;
        IpcIrc.Instance.OnChannelNotice += OnChannelNotice;
        IpcIrc.Instance.OnChannelNoticeSent += OnChannelNoticeSent;
        IpcIrc.Instance.OnUserAction += OnUserAction;
        IpcIrc.Instance.OnUserActionSent += OnUserActionSent;
        IpcIrc.Instance.OnUserJoined += OnUserJoined;
        IpcIrc.Instance.OnUserLeft += OnUserLeft;
        IpcIrc.Instance.OnUserMessage += OnUserMessage;
        IpcIrc.Instance.OnUserMessageSent += OnUserMessageSent;
        IpcIrc.Instance.OnUserNotice += OnUserNotice;
        IpcIrc.Instance.OnUserNoticeSent += OnUserNoticeSent;
        IpcIrc.Instance.OnServerMessage += OnServerMessage;
        IpcIrc.Instance.OnServerRawMessage += OnServerRawMessage;
        IpcIrc.Instance.OnConnected += OnConnectedToServer;
        IpcIrc.Instance.OnDisconnected += OnDisconnectedFromServer;
        IpcIrc.Instance.OnServerPong += OnServerPong;
        IpcIrc.Instance.OnExceptionThrown += OnExceptionThrown;
    }

    // Receive a message from a user
    void OnUserMessage(UserMessageEventArgs userMessageArgs)
    {
        if (debugLogPrivateMessages) UnityEngine.Debug.Log("IpcIrcDebug: PRIVATE MESSAGE: " + userMessageArgs.From + ": " + userMessageArgs.Message);
        if (debugEchoPrivateMessages) IpcIrc.Instance.Message("IpcIrcDebug: PRIVATE MESSAGE: " + userMessageArgs.From + ": " + userMessageArgs.Message);
    }

    // Sent a message to a user
    void OnUserMessageSent(UserMessageSentEventArgs userMessageSentArgs)
    {
        if (debugLogOutgoingMessages) UnityEngine.Debug.Log("IpcIrcDebug: SENT PRIVATE MESSAGE: " + userMessageSentArgs.To + ": " + userMessageSentArgs.Message);
        if (debugEchoOutgoingMessages) IpcIrc.Instance.Message("IpcIrcDebug: SENT PRIVATE MESSAGE: " + userMessageSentArgs.To + ": " + userMessageSentArgs.Message);
    }

    // Receive an action from a user
    void OnUserAction(UserActionEventArgs userActionArgs)
    {
        if (debugLogPrivateMessages) UnityEngine.Debug.Log("IpcIrcDebug: PRIVATE ACTION: " + userActionArgs.From + ": " + userActionArgs.Message);
        if (debugEchoPrivateMessages) IpcIrc.Instance.Message("IpcIrcDebug: PRIVATE ACTION: " + userActionArgs.From + ": " + userActionArgs.Message);
    }

    // Sent an action to a user
    void OnUserActionSent(UserActionSentEventArgs userActionSentArgs)
    {
        if (debugLogOutgoingMessages) UnityEngine.Debug.Log("IpcIrcDebug: SENT PRIVATE ACTION: " + userActionSentArgs.To + ": " + userActionSentArgs.Message);
        if (debugEchoOutgoingMessages) IpcIrc.Instance.Message("IpcIrcDebug: SENT PRIVATE ACTION: " + userActionSentArgs.To + ": " + userActionSentArgs.Message);
    }

    // Receive a notice from a user
    void OnUserNotice(UserNoticeEventArgs userNoticeArgs)
    {
        if (debugLogPrivateNotices) UnityEngine.Debug.Log("IpcIrcDebug: PRIVATE NOTICE: " + userNoticeArgs.From + ": " + userNoticeArgs.Message);
        if (debugEchoPrivateNotices) IpcIrc.Instance.Message("IpcIrcDebug: PRIVATE NOTICE: " + userNoticeArgs.From + ": " + userNoticeArgs.Message);
    }

    // Sent a message to a user
    void OnUserNoticeSent(UserNoticeSentEventArgs userNoticeSentArgs)
    {
        if (debugLogOutgoingNotices) UnityEngine.Debug.Log("IpcIrcDebug: SENT PRIVATE NOTICE: " + userNoticeSentArgs.To + ": " + userNoticeSentArgs.Message);
        if (debugEchoOutgoingNotices) IpcIrc.Instance.Message("IpcIrcDebug: SENT PRIVATE NOTICE: " + userNoticeSentArgs.To + ": " + userNoticeSentArgs.Message);
    }

    // Receive a directed message from a channel
    void OnChannelDirectedMessage(ChannelDirectedMessageEventArgs channelDirectedMessageArgs)
    {
        if (debugLogDirectedMessages) UnityEngine.Debug.Log("IpcIrcDebug: DIRECTED MESSAGE ON " + channelDirectedMessageArgs.Channel + ": " + channelDirectedMessageArgs.From + ": " + channelDirectedMessageArgs.Message);
        if (debugEchoDirectedMessages) IpcIrc.Instance.Message("IpcIrcDebug: DIRECTED MESSAGE ON " + channelDirectedMessageArgs.Channel + ": " + channelDirectedMessageArgs.From + ": " + channelDirectedMessageArgs.Message);
    }

    // Receive a message from a channel
    void OnChannelMessage(ChannelMessageEventArgs channelMessageArgs)
    {
        if (debugLogChannelMessages) UnityEngine.Debug.Log("IpcIrcDebug: MESSAGE ON " + channelMessageArgs.Channel + ": " + channelMessageArgs.From + ": " + channelMessageArgs.Message);
        if (debugEchoChannelMessages) IpcIrc.Instance.Message("IpcIrcDebug: MESSAGE ON " + channelMessageArgs.Channel + ": " + channelMessageArgs.From + ": " + channelMessageArgs.Message);
    }

    // Sent a message to a channel
    void OnChannelMessageSent(ChannelMessageSentEventArgs channelMessageSentArgs)
    {
        if (debugLogOutgoingMessages) UnityEngine.Debug.Log("IpcIrcDebug: SENT MESSAGE ON " + channelMessageSentArgs.Channel + ": " + channelMessageSentArgs.Message);
        if (debugEchoOutgoingMessages) IpcIrc.Instance.Message("IpcIrcDebug: SENT MESSAGE ON " + channelMessageSentArgs.Channel + ": " + channelMessageSentArgs.Message);
    }

    // Receive an action from a channel
    void OnChannelAction(ChannelActionEventArgs channelActionMessageArgs)
    {
        if (debugLogChannelMessages) UnityEngine.Debug.Log("IpcIrcDebug: ACTION ON " + channelActionMessageArgs.Channel + ": " + channelActionMessageArgs.From + ": " + channelActionMessageArgs.Message);
        if (debugEchoChannelMessages) IpcIrc.Instance.Message("IpcIrcDebug: ACTION ON " + channelActionMessageArgs.Channel + ": " + channelActionMessageArgs.From + ": " + channelActionMessageArgs.Message);
    }

    // Sent an action to a channel
    void OnChannelActionSent(ChannelActionSentEventArgs channelActionMessageSentArgs)
    {
        if (debugLogOutgoingMessages) UnityEngine.Debug.Log("IpcIrcDebug: SENT ACTION ON " + channelActionMessageSentArgs.Channel + ": " + channelActionMessageSentArgs.Message);
        if (debugEchoOutgoingMessages) IpcIrc.Instance.Message("IpcIrcDebug: SENT ACTION ON " + channelActionMessageSentArgs.Channel + ": " + channelActionMessageSentArgs.Message);
    }

    // Receive a notice from a channel
    void OnChannelNotice(ChannelNoticeEventArgs channelNoticeArgs)
    {
        if (debugLogChannelNotices) UnityEngine.Debug.Log("IpcIrcDebug: CHANNEL NOTICE: " + channelNoticeArgs.From + ": " + channelNoticeArgs.Message);
        if (debugEchoChannelNotices) IpcIrc.Instance.Message("IpcIrcDebug: CHANNEL NOTICE: " + channelNoticeArgs.From + ": " + channelNoticeArgs.Message);
    }

    // Sent a notice to a channel
    void OnChannelNoticeSent(ChannelNoticeSentEventArgs channelNoticeSentArgs)
    {
        if (debugLogOutgoingNotices) UnityEngine.Debug.Log("IpcIrcDebug: SENT CHANNEL NOTICE: " + channelNoticeSentArgs.Channel + ": " + channelNoticeSentArgs.Message);
        if (debugEchoOutgoingNotices) IpcIrc.Instance.Message("IpcIrcDebug: SENT CHANNEL NOTICE: " + channelNoticeSentArgs.Channel + ": " + channelNoticeSentArgs.Message);
    }

    // Get the name of the user who joined the channel 
    void OnUserJoined(UserJoinedEventArgs userJoinedArgs)
    {
        if (debugLogChannelMessages) UnityEngine.Debug.Log("IpcIrcDebug: USER JOINED: " + userJoinedArgs.User);
        if (debugEchoChannelMessages) IpcIrc.Instance.Message("IpcIrcDebug: USER JOINED: " + userJoinedArgs.User);
    }

    // Get the name of the user who left the channel.
    void OnUserLeft(UserLeftEventArgs userLeftArgs)
    {
        if (debugLogChannelMessages) UnityEngine.Debug.Log("IpcIrcDebug: USER LEFT: " + userLeftArgs.User);
        if (debugEchoChannelMessages) IpcIrc.Instance.Message("IpcIrcDebug: USER LEFT: " + userLeftArgs.User);
    }

    // Receive a message from the server
    void OnServerMessage(string message)
    {
        if (debugLogServerMessages) UnityEngine.Debug.Log("IpcIrcDebug: SERVER: " + message);
        if (debugEchoServerMessages) IpcIrc.Instance.Message("IpcIrcDebug: SERVER: " + message);
    }

    // Receive a raw message from the server
    void OnServerRawMessage(string message)
    {
        if (debugLogServerRawMessages) UnityEngine.Debug.Log("IpcIrcDebug: SERVER RAW: " + message);
        if (debugEchoServerRawMessages) IpcIrc.Instance.Message("IpcIrcDebug: SERVER RAW: " + message);
    }

    void OnServerPong()
    {
        if (debugLogServerMessages) UnityEngine.Debug.Log("IpcIrcDebug: RESPONDED TO SERVER PONG: " + IpcIrc.Instance.ServerName);
        if (debugEchoServerMessages) IpcIrc.Instance.Message("IpcIrcDebug: RESPONDED TO SERVER PONG: " + IpcIrc.Instance.ServerName);
    }

    void OnConnectedToServer()
    {
        if (debugLogServerMessages) UnityEngine.Debug.Log("IpcIrcDebug: CONNECTED TO SERVER: " + IpcIrc.Instance.ServerName);
    }

    void OnDisconnectedFromServer()
    {
        if (debugLogServerMessages) UnityEngine.Debug.Log("IpcIrcDebug: DISCONNECTED FROM SERVER: " + IpcIrc.Instance.ServerName);
        //IpcIrc.Instance.Connect();
    }

    // Receive exception if something goes wrong
    private void OnExceptionThrown(Exception oops)
    {
        Debug.Log("IpcIrc: Debug logger caught an exception: " + oops);
        if (debugEchoServerMessages) IpcIrc.Instance.Message("IpcIrc: Debug logger caught an exception: " + oops);
    }
}
