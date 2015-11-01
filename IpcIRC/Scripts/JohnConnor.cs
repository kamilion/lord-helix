using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using Irc;


public class JohnConnor : MonoBehaviour {
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

    public void Skynet() {
        IpcIrc.Instance.LeaveServer("This IpcIRC Instance was terminated normally.");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void Start() { // Subscribe for events
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

    // Get the name of the user who joined the channel 
    void OnUserJoined(UserJoinedEventArgs userJoinedArgs) {
        if (debugLogChannelMessages) UnityEngine.Debug.Log("IpcIrc: JohnConnor:  WATCHED TERMINATOR JOIN: " + userJoinedArgs.User);
        if (debugEchoChannelMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  WATCHED TERMINATOR JOIN: " + userJoinedArgs.User);
        Debug.Log("IpcIrc:JohnConnor: WATCHED USER JOIN: " + userJoinedArgs.User);
    }

    // Get the name of the user who left the channel.
    void OnUserLeft(UserLeftEventArgs userLeftArgs) {
        if (debugLogChannelMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  WATCHED TERMINATOR DIE: " + userLeftArgs.User);
        if (debugEchoChannelMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  WATCHED TERMINATOR DIE: " + userLeftArgs.User);
    }

    // Sent a message to a user
    void OnUserMessageSent(UserMessageSentEventArgs userMessageSentArgs) {
        if (debugLogOutgoingMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  TRANSMIT PRIVATE MESSAGE: " + userMessageSentArgs.To + ": " + userMessageSentArgs.Message);
        if (debugEchoOutgoingMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  TRANSMIT PRIVATE MESSAGE: " + userMessageSentArgs.To + ": " + userMessageSentArgs.Message);
    }

    // Receive a message from a user
    void OnUserMessage(UserMessageEventArgs userMessageArgs) {
        if (debugLogPrivateMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  RECEIVE PRIVATE MESSAGE: " + userMessageArgs.From + ": " + userMessageArgs.Message);
        if (debugEchoPrivateMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  RECEIVE PRIVATE MESSAGE: " + userMessageArgs.From + ": " + userMessageArgs.Message);
        if (userMessageArgs.Message.StartsWith("terminated")) {
            Skynet();
        }
    }

    // Receive a action from a user
    void OnUserAction(UserActionEventArgs userActionArgs)
    {
        if (debugLogPrivateMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  PRIVATE ACTION: " + userActionArgs.From + ": " + userActionArgs.Message);
        if (debugEchoPrivateMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  PRIVATE ACTION: " + userActionArgs.From + ": " + userActionArgs.Message);
    }

    // Sent a action to a user
    void OnUserActionSent(UserActionSentEventArgs userActionSentArgs)
    {
        if (debugLogOutgoingMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  SENT PRIVATE ACTION: " + userActionSentArgs.To + ": " + userActionSentArgs.Message);
        if (debugEchoOutgoingMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:   SENT PRIVATE ACTION: " + userActionSentArgs.To + ": " + userActionSentArgs.Message);
    }

    // Receive a notice from a user
    void OnUserNotice(UserNoticeEventArgs userNoticeArgs)
    {
        if (debugLogPrivateNotices) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  RECEIVE PRIVATE NOTICE: " + userNoticeArgs.From + ": " + userNoticeArgs.Message);
        if (debugEchoPrivateNotices) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  RECEIVE PRIVATE NOTICE: " + userNoticeArgs.From + ": " + userNoticeArgs.Message);
    }

    // Sent a notice to a user
    void OnUserNoticeSent(UserNoticeSentEventArgs userNoticeSentArgs)
    {
        if (debugLogOutgoingNotices) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  SENT PRIVATE NOTICE: " + userNoticeSentArgs.To + ": " + userNoticeSentArgs.Message);
        if (debugEchoOutgoingNotices) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  SENT PRIVATE NOTICE: " + userNoticeSentArgs.To + ": " + userNoticeSentArgs.Message);
    }

    // Receive a notice from a channel
    void OnChannelNotice(ChannelNoticeEventArgs channelNoticeArgs)
    {
        if (debugLogChannelNotices) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  RECEIVE PUBLIC NOTICE: " + channelNoticeArgs.From + ": " + channelNoticeArgs.Message);
        if (debugEchoChannelNotices) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  RECEIVE PUBLIC NOTICE: " + channelNoticeArgs.From + ": " + channelNoticeArgs.Message);
    }

    // Sent a notice to a channel
    void OnChannelNoticeSent(ChannelNoticeSentEventArgs channelNoticeSentArgs)
    {
        if (debugLogOutgoingNotices) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  SENT PUBLIC NOTICE: " + channelNoticeSentArgs.Channel + ": " + channelNoticeSentArgs.Message);
        if (debugEchoOutgoingNotices) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  SENT PUBLIC NOTICE: " + channelNoticeSentArgs.Channel + ": " + channelNoticeSentArgs.Message);
    }

    // Receive an action from a channel
    void OnChannelAction(ChannelActionEventArgs channelActionMessageArgs)
    {
        if (debugLogChannelMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  RECEIVED ACTION ON " + channelActionMessageArgs.Channel + ": " + channelActionMessageArgs.From + ": " + channelActionMessageArgs.Message);
        if (debugEchoChannelMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  RECEIVED ACTION ON " + channelActionMessageArgs.Channel + ": " + channelActionMessageArgs.From + ": " + channelActionMessageArgs.Message);
    }

    // Sent an action to a channel
    void OnChannelActionSent(ChannelActionSentEventArgs channelActionMessageSentArgs)
    {
        if (debugLogOutgoingMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  SENT ACTION ON " + channelActionMessageSentArgs.Channel + ": " + channelActionMessageSentArgs.Message);
        if (debugEchoOutgoingMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  SENT ACTION ON " + channelActionMessageSentArgs.Channel + ": " + channelActionMessageSentArgs.Message);
    }

    // Sent a message to a channel
    void OnChannelMessageSent(ChannelMessageSentEventArgs channelMessageSentArgs) {
        if (debugLogOutgoingMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  TRANSMIT PUBLIC MESSAGE ON " + channelMessageSentArgs.Channel + ": " + channelMessageSentArgs.Message);
        if (debugEchoOutgoingMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  TRANSMIT PUBLIC MESSAGE ON " + channelMessageSentArgs.Channel + ": " + channelMessageSentArgs.Message);
    }

    // Receive a directed message from a channel
    void OnChannelDirectedMessage(ChannelDirectedMessageEventArgs channelDirectedMessageArgs)
    {
        if (debugLogDirectedMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  RECEIVE DIRECT MESSAGE ON " + channelDirectedMessageArgs.Channel + ": " + channelDirectedMessageArgs.From + ": " + channelDirectedMessageArgs.Message);
        if (debugEchoDirectedMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  RECEIVE DIRECT MESSAGE ON " + channelDirectedMessageArgs.Channel + ": " + channelDirectedMessageArgs.From + ": " + channelDirectedMessageArgs.Message);
        channelDirectedMessageArgs.Message = channelDirectedMessageArgs.Message.Substring(IpcIrc.Instance.Nickname.Length + 2); // Trim the nickname off.
        if (channelDirectedMessageArgs.Message.StartsWith("time travel"))
        {
            if (debugLogDirectedMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  RECEIVE TRAVEL COMMAND ON " + channelDirectedMessageArgs.Channel + ": " + channelDirectedMessageArgs.From + ": " + channelDirectedMessageArgs.Message);
            if (debugEchoDirectedMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  RECEIVE TRAVEL COMMAND ON " + channelDirectedMessageArgs.Channel + ": " + channelDirectedMessageArgs.From + ": " + channelDirectedMessageArgs.Message);
            IpcIrc.Instance.Disconnect();
        }
        if (channelDirectedMessageArgs.Message.StartsWith("you are terminated"))
        {
            if (debugLogDirectedMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  RECEIVE TERMINATE COMMAND ON " + channelDirectedMessageArgs.Channel + ": " + channelDirectedMessageArgs.From + ": " + channelDirectedMessageArgs.Message);
            if (debugEchoDirectedMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  RECEIVE TERMINATE COMMAND ON " + channelDirectedMessageArgs.Channel + ": " + channelDirectedMessageArgs.From + ": " + channelDirectedMessageArgs.Message);
            Skynet();
        }
    }

    // Receive a message from a channel
    void OnChannelMessage(ChannelMessageEventArgs channelMessageArgs) {
        if (debugLogChannelMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  RECEIVE PUBLIC MESSAGE ON " + channelMessageArgs.Channel + ": " + channelMessageArgs.From + ": " + channelMessageArgs.Message);
        if (debugEchoChannelMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  RECEIVE PUBLIC MESSAGE ON " + channelMessageArgs.Channel + ": " + channelMessageArgs.From + ": " + channelMessageArgs.Message);
        if (channelMessageArgs.Message.StartsWith("I'm not your ")) {
            OnChannelCommand(channelMessageArgs);
        }
    }

    // Receive a command from a channel
    void OnChannelCommand(ChannelMessageEventArgs channelMessageArgs) {
        if (debugLogChannelMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  RECEIVE PUBLIC COMMAND ON " + channelMessageArgs.Channel + ": " + channelMessageArgs.From + ": " + channelMessageArgs.Message);
        if (debugEchoChannelMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  RECEIVE PUBLIC COMMAND ON " + channelMessageArgs.Channel + ": " + channelMessageArgs.From + ": " + channelMessageArgs.Message);
        string triggerPhrase = "I'm not your ";
        if (channelMessageArgs.Message.StartsWith(triggerPhrase)) {
            string theCommand = channelMessageArgs.Message.Substring(triggerPhrase.Length);
            // react according to the incoming IRC message
            switch (theCommand) {
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
        if (debugLogServerMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  RADIO ALERT: " + message);
        if (debugEchoServerMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  RADIO ALERT: " + message);
    }

    // Receive a raw message from the server
    void OnServerRawMessage(string message)
    {
        if (debugLogServerRawMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  RADIO ALERT RAW: " + message);
        if (debugEchoServerRawMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  RADIO ALERT RAW: " + message);
    }

    void OnServerPong() {
        if (debugLogServerMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor:  CHECKED IN WITH SQUAD: " + IpcIrc.Instance.ServerName);
        if (debugEchoServerMessages) IpcIrc.Instance.Message("IpcIrc:JohnConnor:  CHECKED IN WITH SQUAD: " + IpcIrc.Instance.ServerName);
    }

    void OnConnectedToServer() {
        if (debugLogServerMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor: UPLINK ESTABLISHED: " + IpcIrc.Instance.ServerName);
    }

    void OnDisconnectedFromServer() {
        if (debugLogServerMessages) UnityEngine.Debug.Log("IpcIrc:JohnConnor: UPLINK SEVERED: " + IpcIrc.Instance.ServerName);
        Reconnect();
    }

    IEnumerator Reconnect() { // Reconnect to the server after some time.
        yield return new WaitForSeconds(2);
        IpcIrc.Instance.Connect();
    }

    // Receive exception if something goes wrong
    private void OnExceptionThrown(Exception oops) {
        Debug.Log(oops);
    }
}
