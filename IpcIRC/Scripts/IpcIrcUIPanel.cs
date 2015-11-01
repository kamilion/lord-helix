using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Irc;


public class IpcIrcUIPanel : MonoBehaviour {
    public Scrollbar ChatScrollbar;
    public Text ChatText;
    public InputField ChatInputText;

    void Start() {
        // Subscribe for events
        IpcIrc.Instance.OnChannelMessage += OnChannelMessage;
        IpcIrc.Instance.OnChannelMessageSent += OnChannelMessageSent;
        IpcIrc.Instance.OnUserLeft += OnUserLeft;
        IpcIrc.Instance.OnUserJoined += OnUserJoined;
        IpcIrc.Instance.OnUserMessage += OnUserMessage;
        IpcIrc.Instance.OnUserMessageSent += OnUserMessageSent;
        IpcIrc.Instance.OnServerMessage += OnServerMessage;
        IpcIrc.Instance.OnConnected += OnConnectedToServer;
        IpcIrc.Instance.OnDisconnected += OnDisconnectedFromServer;
        IpcIrc.Instance.OnServerPong += OnServerPong;
        IpcIrc.Instance.OnExceptionThrown += OnExceptionThrown;
    }

    // Send message
    public void MessageSend() {
        if (String.IsNullOrEmpty(ChatInputText.text))
            return;

        IpcIrc.Instance.Message(ChatInputText.text);
        ChatInputText.text = "";
    }

    // Send message from an object
    public void ObjectMessageSend(string text) {
        if (String.IsNullOrEmpty(text))
            return;

        IpcIrc.Instance.Message(text);
    }

    // Open URL
    public void GoUrl(string url) {
        Application.OpenURL(url);
    }

    // Scroll chat view, 1.0f is Beginning, 0.0f is End.
    public void ScrollTo(float val = 0.0f) {
        ChatScrollbar.value = val;
    }

    // Scroll chat view, 1.0f is Beginning, 0.0f is End.
    public void ScrollToEnd() {
        ScrollTo(0.0f); // Defaults to 0.0f
    }

    // Receive a line, append it to the view, scroll the view.
    public void AppendAndScroll(string message) {
        ChatText.text += message + "\n";
        ScrollToEnd();
    }

    // Incoming Events from IpcIrc

    // Receive a message from server
    void OnServerMessage(string message) {
        AppendAndScroll("<b>SERVER:</b> " + message);
    }

    // Sent a message to a user
    void OnUserMessageSent(UserMessageSentEventArgs userMessageSentArgs) {
        AppendAndScroll("<b>PM:" + IpcIrc.Instance.Nickname + "</b>:" + userMessageSentArgs.To + ": " + userMessageSentArgs.Message);
    }

    // Receive a message from a user
    void OnUserMessage(UserMessageEventArgs userMessageArgs) {
        AppendAndScroll("<b>PM:" + userMessageArgs.From + "</b>: " + userMessageArgs.Message);
    }

    // Sent a message to a channel
    void OnChannelMessageSent(ChannelMessageSentEventArgs channelMessageSentArgs) {
        AppendAndScroll("<b>" + IpcIrc.Instance.Nickname + "</b>: " + channelMessageSentArgs.Channel + ": " + channelMessageSentArgs.Message);
    }

    // Receive a message from a channel
    void OnChannelMessage(ChannelMessageEventArgs channelMessageArgs) {
        AppendAndScroll("<b>" + channelMessageArgs.Channel + ": " + channelMessageArgs.From + ":</b> " + channelMessageArgs.Message);
    }

    // Get the name of the user who joined the channel 
    void OnUserJoined(UserJoinedEventArgs userJoinedArgs) {
        AppendAndScroll("<b>" + "USER JOINED" + ":</b> " + userJoinedArgs.User);
    }

    // Get the name of the user who left the channel.
    void OnUserLeft(UserLeftEventArgs userLeftArgs) {
        AppendAndScroll("<b>" + "USER LEFT" + ":</b> " + userLeftArgs.User);
    }

    void OnServerPong()
    {
        AppendAndScroll("<b>" + "REPONDED TO SERVER KEEPALIVE FROM" + ":</b> " + IpcIrc.Instance.ServerName);
    }

    void OnConnectedToServer() {
        AppendAndScroll("<b>" + "CONNECTED TO SERVER" + ":</b> " + IpcIrc.Instance.ServerName);
    }

    void OnDisconnectedFromServer() {
        AppendAndScroll("<b>" + "DISCONNECTED FROM SERVER" + ":</b> " + IpcIrc.Instance.ServerName);
        Reconnect();
    }

    IEnumerator Reconnect()
    { // Reconnect to the server after some time.
        yield return new WaitForSeconds(2);
        IpcIrc.Instance.Connect();
    }

    // Receive exception if something goes wrong
    private void OnExceptionThrown(Exception oops) {
        Debug.Log(oops);
    }
}
