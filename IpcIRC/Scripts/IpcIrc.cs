using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Irc;

#region delegates
public delegate void UserJoined(UserJoinedEventArgs userJoinedArgs);
public delegate void UserLeft(UserLeftEventArgs userLeftArgs);
public delegate void UserMessage(UserMessageEventArgs userMessageArgs);
public delegate void UserMessageSent(UserMessageSentEventArgs userMessageSentArgs);
public delegate void UserAction(UserActionEventArgs userActionArgs);
public delegate void UserActionSent(UserActionSentEventArgs userActionSentArgs);
public delegate void UserNotice(UserNoticeEventArgs userNoticeArgs);
public delegate void UserNoticeSent(UserNoticeSentEventArgs userNoticeSentArgs);
public delegate void ChannelDirectedMessage(ChannelDirectedMessageEventArgs channelMessageArgs);
public delegate void ChannelMessage(ChannelMessageEventArgs channelMessageArgs);
public delegate void ChannelMessageSent(ChannelMessageSentEventArgs channelMessageSentArgs);
public delegate void ChannelAction(ChannelActionEventArgs channelActionArgs);
public delegate void ChannelActionSent(ChannelActionSentEventArgs channelActionSentArgs);
public delegate void ChannelNotice(ChannelNoticeEventArgs channelNoticeArgs);
public delegate void ChannelNoticeSent(ChannelNoticeSentEventArgs channelNoticeSentArgs);
public delegate void ServerRawMessage(string message);
public delegate void ServerMessage(string message);
public delegate void ServerPong();
public delegate void ServerConnected();
public delegate void Connected();
public delegate void Disconnected();
public delegate void ExceptionThrown(Exception exeption);
#endregion

public class IpcIrc : MonoBehaviour {
    #region delegate_instances
    public UserJoined OnUserJoined;
    public UserLeft OnUserLeft;
    public UserMessage OnUserMessage;
    public UserMessageSent OnUserMessageSent;
    public UserAction OnUserAction;
    public UserActionSent OnUserActionSent;
    public UserNotice OnUserNotice;
    public UserNoticeSent OnUserNoticeSent;
    public ChannelDirectedMessage OnChannelDirectedMessage;
    public ChannelMessage OnChannelMessage;
    public ChannelMessageSent OnChannelMessageSent;
    public ChannelAction OnChannelAction;
    public ChannelActionSent OnChannelActionSent;
    public ChannelNotice OnChannelNotice;
    public ChannelNoticeSent OnChannelNoticeSent;
    public ServerRawMessage OnServerRawMessage;
    public ServerMessage OnServerMessage;
    public ServerPong OnServerPong;
    public ServerConnected OnServerConnected;
    public Connected OnConnected;
    public Disconnected OnDisconnected;
    public ExceptionThrown OnExceptionThrown;
    #endregion delegate_instances

    #region classes
    [System.Serializable]
    public class IpcIrcServerList
    {
        // These are known beforehand and can be edited.
        public string serverName;  // What is the server's name?
        public string serverPort;  // What is the server's port?
        public string nickname; // What is the user's nickname?
        public string authString; // What is the user's authentifier?
        public bool serverSetUserInvisible; // Set invisible mode after connecting?
        public bool serverConnectAutomatically; // Connect automatically?
        // These are learned after we connect.
        public bool serverActive;  // Is the server currently active?
        public string serverIdent = "";  // What is the server's identification? (msgtype 002)
        public string serverAge = "";  // What is the server's age? (msgtype 003)
        public string serverVersion = "";  // What is the server's Version? (msgtype 004)
        public string serverCapabilities = "";  // What are the server's capabilities? (msgtype 005)
        public string serverConnectionCount = "";  // What is the server's Connection Count? (msgtype 250)
        public string serverUserCount = "";  // What is the server's User Count? (msgtype 251)
        public string serverOperCount = "";  // What is the server's Oper Count? (msgtype 252)
        public string serverChannelCount = "";  // What is the server's Channel Count? (msgtype 254)
        public string serverClientCount = "";  // What is the server's Client Count? (msgtype 255)
        public string serverLocalUserMax = "";  // What is the server's User Count? (msgtype 265)
        public string serverGlobalUserMax = "";  // What is the server's User Count? (msgtype 266)
    }

    [System.Serializable]
    public class IpcIrcChannelList
    {
        public string channelName;  // What is the channel's name?
        public bool channelActive;  // Is the channel currently active?
        public string channelTopic;  // What is the channel's topic?
        public string channelModes;  // What is the channel's modes?
        public string[] channelNicklist;  // Who is in the channel?
    }
    #endregion classes

    #region variables
    public string IpcIrcHandshake = "IpcIRC Project 2501";
    /// <summary>
    /// This is our list we want to use to represent our Servers to unity's inspector
    /// </summary>
    public List<IpcIrcServerList> Servers = new List<IpcIrcServerList>(1);
    public void AddServer() { Servers.Add(new IpcIrcServerList()); } // Add a list element
    public void RemoveServer(int index) { Servers.RemoveAt(index); } // Remove a list element

    public string ServerName = "irc.ospnet.org";
    public int ServerPort = 6667;

    /// <summary>
    /// The central IpcIrc instance that any class can call methods on.
    /// </summary>
    public static IpcIrc Instance;

    /// <summary>
    /// Show ALL messages from server?
    /// </summary>
    public bool MessageDebug;
    /// <summary>
    /// Connect to IRC server and join channel automatically?
    /// </summary>
    public bool ConnectOnAwake;
    /// <summary>
    /// Set +i (invisible) when connecting to IRC?
    /// </summary>
    public bool SetInvisibleMode;
    /// <summary>
    /// Chosen Nickname of the User
    /// </summary>
    public string Nickname;
    /// <summary>
    /// Authentication string of the user's profile
    /// </summary>
    public string AuthString;
    /// <summary>
    /// Specific command channels to join
    /// </summary>
    public string CommandChannel;
    /// <summary>
    /// This is our list we want to use to represent our Channels to unity's inspector
    /// </summary>
    public List<IpcIrcChannelList> Channels = new List<IpcIrcChannelList>(1);
    public void AddChannel() { Channels.Add(new IpcIrcChannelList()); } // Add a list element
    public void RemoveChannel(int index) { Channels.RemoveAt(index); } // Remove a list element
    /// <summary>
    /// Automatically join Channels on connect?
    /// </summary>
    public bool AutoJoinChannels;

    [HideInInspector] public string[] joinedChannels; // Channels we've actually joined.
    private bool ircConnected; // used to prevent foolishness before joining channels
    private TcpClient irc; // private TcpClient used to talk to the server
    private NetworkStream stream; // private network stream used to read/write from/to
    private string inputLine; // global variable used to read input from the client
    private StreamReader reader; // stream reader to read from the network stream
    private StreamWriter writer; // stream writer to write to the stream
    #endregion variables

    #region public_methods
    /// <summary>
    /// Connect to IRC server
    /// </summary>
    public void Connect() {
        if (String.IsNullOrEmpty(Nickname))
            return;
        try {
            irc = new TcpClient(ServerName, ServerPort);
            stream = irc.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            // The nickname will be used as the IDENT string, and the handshake as the realname.
            if (SetInvisibleMode)
                SendLine("USER " + Nickname + " 8 * :" + IpcIrcHandshake);
            else // Mode is either 0 or 8 according to RFC2812
                SendLine("USER " + Nickname + " 0 * :" + IpcIrcHandshake);
            if (!String.IsNullOrEmpty(AuthString))
                SendLine("PASS " + AuthString);
            SendLine("NICK " + Nickname);
            StartCoroutine("Listen");
        }
        catch (Exception ex) {
            if (OnExceptionThrown != null)
                OnExceptionThrown(ex);
        }
    }

    /// <summary>
    /// Disconnect from IRC server
    /// </summary>
    public void Disconnect() {
        ircConnected = false;
        irc = null;
        StopCoroutine("Listen");

        if (stream != null)
            stream.Dispose();
        if (writer != null)
            writer.Dispose();
        if (reader != null)
            reader.Dispose();
        if (OnDisconnected != null)
            OnDisconnected();
    }

    /// <summary>
    /// Join to channel after succesful connection
    /// </summary>
    public void JoinChannel() {
        if (String.IsNullOrEmpty(CommandChannel))
            return;
        if (CommandChannel[0] != '#')
            CommandChannel = "#" + CommandChannel;
        if (irc != null && irc.Connected)
            SendLine("JOIN " + CommandChannel);
    }

    /// <summary>
    /// Leave current channel
    /// </summary>
    public void LeaveChannel() { SendLine("PART " + CommandChannel); }

    /// <summary>
    /// Sends a QUIT command to the server.
    /// </summary>
    /// <param name="message">raw line to send</param>    
    public void LeaveServer(string message) { SendLine("QUIT :" + message); }

    /// <summary>
    /// Send message to the current channel
    /// </summary>
    /// <param name="message">Message to send</param>
    public void Message(string message) {
        SendLine("PRIVMSG " + CommandChannel + " :" + message);
        // Notify other event listeners
        if (OnChannelMessageSent != null)
            OnChannelMessageSent(new ChannelMessageSentEventArgs(CommandChannel, message));
    }

    /// <summary>
    /// Send message to a user
    /// </summary>
    /// <param name="user">User to select</param>
    /// <param name="message">Message to send</param>
    public void MessageUserText(string user, string message) {
        SelectUser(user);
        MessageUser(message);
    }

    /// <summary>
    /// Send message to a selected user from a UI widget
    /// </summary>
    /// <param name="message">Message to send</param>
    public void MessageUser(string message) {
        if (String.IsNullOrEmpty(_currentUser))
            return;
        SendLine("PRIVMSG " + _currentUser + " :" + message);
        // Notify other event listeners
        if (OnUserMessageSent != null)
            OnUserMessageSent(new UserMessageSentEventArgs(_currentUser, message));
    }

    /// <summary>
    /// Select a user to Send message from a UI widget
    /// </summary>
    /// <param name="user">User to select</param>
    public void SelectUser(string user) {
        _currentUser = user;
    }
    #endregion public_methods
    private string _currentUser;

    #region private_methods

    /// <summary>
    /// Yield Coroutine: Listens for incoming message from the IRC server.
    /// This will dispatch events to the IRC Protocol data parser.
    /// Handles server disconnections, because we need to read from the socket to know
    /// that we've been disconnected from the server. Just the way sockets work.
    /// </summary>
    IEnumerator Listen() {
        while (true) {
            if(!irc.Connected)
                Disconnect();
            if (stream.DataAvailable)
                if ((inputLine = reader.ReadLine()) != null)
                    ParseData(inputLine);
            yield return null;
        }
    }

    /// <summary>
    /// Parses an incoming message from the IRC server.
    /// This will dispatch events based on the basic content of the message.
    /// Handles server & channel joins, parts, quits, and classifies incoming messages.
    /// </summary>
    /// <param name="data">The complete line from the IRCd</param>
    private void ParseData(string data) {
        // split the data into parts
        string[] ircData = data.Split(' ');
        if (OnServerRawMessage != null)
            OnServerRawMessage(data);
        // PING messages need to be responded with PONG as quickly as possible.
        if (data.Length > 4) {
            if (data.Substring(0, 4) == "PING") {
                SendLine("PONG " + ircData[1]);
                if (OnServerPong != null) OnServerPong(); // Notify subscribers that a keepalive happened.
                return;
            }
        }

        // react according to the incoming IRC message
        switch (ircData[1]) {
            case "001": // server welcome message, after this we can send commands
                SendLine("MODE " + Nickname + " +R");  // Prevents receiving private messages from unidentified users
                //SendLine("MODE " + Nickname + " +i");  // Set this client 'invisible'
                if (OnServerConnected != null) OnServerConnected(); // Notify any subscribers that we connected to the server successfully.
                break;
            case "002": // ignore
            case "003": // ignore
            case "004": // ignore
            case "005": // ignore
            case "250": // ignore
            case "251": // ignore
            case "252": // ignore
            case "254": // ignore
            case "255": // ignore
            case "265": // ignore
            case "266": // ignore
                break;
            case "353": // names reply
                Debug.Log("Got some names.");
                break;
            case "366": // end of names reply
                Debug.Log("Got all names.");
                break;
            case "372": // server MOTD message, we can ignore this.
            case "375": // server MOTD message begins, we can ignore this.
                break;
            case "376": // server MOTD message finished, after this we can join channels.
                SendLine("MODE " + Nickname + " +R");  // Prevents receiving private messages from unidentified users
                //SendLine("MODE " + Nickname + " +i");  // Set this client 'invisible'
                ircConnected = true; // At this point we're ready to join channels and interact normally.
                if (OnConnected != null) OnConnected(); // Notify any subscribers that we are ready.
                break;
            case "404": // Could not send to channel, we can ignore this for now.
                break;
            case "JOIN": // someone joined
                if (MessageDebug) Instance.Message("HELLO MESSAGE: |" + ircData[2] + "|" + ircData[1] + "|" + ircData[0] + "|");
                if (Instance.OnUserJoined != null)
                    Instance.OnUserJoined(new UserJoinedEventArgs(ircData[2], ircData[0].Substring(1, ircData[0].IndexOf("!") - 1)));
                break;
            case "NOTICE": // Server notices, ignore for now
                if (ircConnected) { // We get some notices during early connect phase, ignore them.
                    if (MessageDebug) Instance.Message("NOTICE: |" + JoinArray(ircData, 3) + "|" + ircData[2] + "|" + ircData[1] + "|" + ircData[0] + "|");
                    if (ircData[2].ToLower() != Nickname.ToLower()) { // Doesn't match our nickname...
                        if (OnChannelNotice != null)
                            OnChannelNotice(new ChannelNoticeEventArgs(ircData[2], ircData[0].Substring(1, ircData[0].IndexOf('!') - 1), JoinArray(ircData, 3)));
                    } else {
                        if (OnUserNotice != null)
                            OnUserNotice(new UserNoticeEventArgs(ircData[0].Substring(1, ircData[0].IndexOf('!') - 1), JoinArray(ircData, 3)));
                    }
                }
                break;
            case "PRIVMSG": // message was sent to the channel or as private
                if (MessageDebug) Instance.Message("PRIVMSG: |" + JoinArray(ircData, 3) + "|" + ircData[2] + "|" + ircData[1] + "|" + ircData[0] + "|");
                if (ircData[2].ToLower() != Nickname.ToLower()) { // Doesn't match our nickname...
                    if (ircData[3].Contains("\x01")) { // This is a special message. like an ACTION or VERSION
                        if (MessageDebug) Instance.Message("SPECIAL: |" + JoinArray(ircData, 3) + "|" + ircData[2] + "|" + ircData[1] + "|" + ircData[0] + "|");
                        if (ircData[3].StartsWith(":" + "\x01")) { // This is a ACTION message.
                            if (OnChannelAction != null)
                                OnChannelAction(new ChannelActionEventArgs(ircData[2], ircData[0].Substring(1, ircData[0].IndexOf('!') - 1), JoinArray(ircData, 3)));
                            break;
                        } else {
                            string[] ctcpWrapper = ircData[3].Split('\x01');
                            string[] ctcpData = ctcpWrapper[1].Split(' ');
                            ParseCTCP(ctcpData);
                            break;
                        }
                    }
                    if (ircData[3] == ":" + IpcIrc.Instance.Nickname + ":") { // it's a directed public message
                        if (OnChannelDirectedMessage != null)
                            OnChannelDirectedMessage(new ChannelDirectedMessageEventArgs(ircData[2], ircData[0].Substring(1, ircData[0].IndexOf('!') - 1), JoinArray(ircData, 3)));
                        break;
                    }
                    else { // it's a public message
                        if (OnChannelMessage != null)
                            OnChannelMessage(new ChannelMessageEventArgs(ircData[2], ircData[0].Substring(1, ircData[0].IndexOf('!') - 1), JoinArray(ircData, 3)));
                        break;
                    }
                }
                else {  // it's a private message
                    if (ircData[3].Contains("\x01")) { // This is a special message. like an ACTION or VERSION
                        if (MessageDebug) Instance.Message("PRIVATE SPECIAL: |" + JoinArray(ircData, 3) + "|" + ircData[2] + "|" + ircData[1] + "|" + ircData[0] + "|");
                        if (ircData[3].StartsWith(":" + "\x01")) { // This is a ACTION message.
                            if (OnUserAction != null)
                                OnUserAction(new UserActionEventArgs(ircData[0].Substring(1, ircData[0].IndexOf('!') - 1), JoinArray(ircData, 3)));
                            break;
                        } else {
                            string[] ctcpWrapper = ircData[3].Split('\x01');
                            string[] ctcpData = ctcpWrapper[1].Split(' ');
                            ParseCTCP(ctcpData);
                            break;
                        }
                    }
                    // Nothing special, just a normal user private message.
                    if (OnUserMessage != null)
                        OnUserMessage(new UserMessageEventArgs(ircData[0].Substring(1, ircData[0].IndexOf('!') - 1), JoinArray(ircData, 3)));
                    break;
                }
            case "PART":
            case "QUIT":// someone left
                if (MessageDebug) Instance.Message("GOODBYE MESSAGE: |" + JoinArray(ircData, 3) + "|" + ircData[2] + "|" + ircData[1] + "|" + ircData[0] + "|");
                if (OnUserLeft != null)
                    OnUserLeft(new UserLeftEventArgs(ircData[2], ircData[0].Substring(1, data.IndexOf("!") - 1)));
                break;
            default:
                // still using this while debugging    String.Join("|", ircData)
                if (MessageDebug) Instance.Message("UNKNOWN MESSAGE: |" + JoinArray(ircData, 3) + "|"+ ircData[2] + "|" + ircData[1] + "|" + ircData[0] + "|");
                if (MessageDebug) Debug.Log("UNKNOWN MESSAGE: |" + JoinArray(ircData, 3) + "|" + ircData[2] + "|" + ircData[1] + "|" + ircData[0] + "|");
                //if (ircData.Length > 3) if (OnServerMessage != null) OnServerMessage(JoinArray(ircData, 3)); // Cut string at index 3 if we've got four or more elements.
                if (ircData.Length > 1) if (OnServerMessage != null) OnServerMessage(JoinArray(ircData, 0)); // Cut string at index 3 if we've got four or more elements.
                break;
        }

    }

    /// <summary>
    /// Parses an incoming CTCP message (\x01) from the IRC server.
    /// This will dispatch events based on the basic content of the message.
    /// Handles CTCP version, clientinfo and more.
    /// </summary>
    /// <param name="data">The contents of the CTCP</param>
    private void ParseCTCP(string[] data)
    {
        if (MessageDebug) Instance.Message("CTCP: |" + data + "|");
        switch (data[0])
        {
            case "ACTION": // it was an action, figure out if it was public or private.
                Instance.Message("Got action'd.");
                break;
            case "VERSION": // we were sent a VERSION request. Reply. Required!
                Instance.Message("Got verison'd.");
                break;
            case "CLIENTINFO": // we were sent a CLIENTINFO request. Reply.
                Instance.Message("Got clientinfo'd.");
                break;
            case "USERINFO": // we were sent a USERINFO request. Reply.
                Instance.Message("Got userinfo'd.");
                break;
            case "AVATAR": // server MOTD message begins, we can ignore this.
                Instance.Message("Got Avatar'd.");
                break;
        }
    }

    /// <summary>
    /// Converts a unicode string to a hexadecimal encoded string
    /// </summary>
    /// <param name="str">string containing unicode representations</param>
    public static string ToHexString(string str) {
        var sb = new StringBuilder();
        var bytes = Encoding.Unicode.GetBytes(str);
        foreach (var t in bytes) { sb.Append(t.ToString("X2")); }
        return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
    }

    /// <summary>
    /// Converts a hexadecimal encoded string to a unicode string
    /// </summary>
    /// <param name="hexString">string containing hexadecimal representations</param>
    public static string FromHexString(string hexString) {
        var bytes = new byte[hexString.Length / 2];
        for (var i = 0; i < bytes.Length; i++) { bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16); }
        return Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
    }

    /// <summary>
    /// Strips a word of unnecessary characters
    /// This will remove any mIRC color codes.
    /// </summary>
    /// <param name="message">Single word as a string</param>
    private string StripMessage(string message) {
        // remove mIRC Color Codes
        foreach (Match m in new Regex((char)3 + @"(?:\d{1,2}(?:,\d{1,2})?)?").Matches(message))
            message = message.Replace(m.Value, "");

        // if there is nothing to strip
        if (message == "")
            return "";
        if (message.Substring(0, 1) == ":" && message.Length > 2)
            return message.Substring(1, message.Length - 1);
        return message;
    }

    /// <summary>
    /// Joins the message array into a string after a specific index
    /// Essentially this is myString.Substring(myString.length) for string arrays.
    /// </summary>
    /// <param name="strArray">String array input</param>
    /// <param name="startIndex">Array Index to begin outputting space delimited string at (0 indexed)</param>
    private string JoinArray(string[] strArray, int startIndex) {
        return StripMessage(String.Join(" ", strArray, startIndex, strArray.Length - startIndex));
    }

    /// <summary>
    /// Sends a raw line directly to the server.
    /// This will require understanding of the IRC protocol.
    /// </summary>
    /// <param name="message">raw line to send</param>    
    private void SendLine(string message) {
        writer.WriteLine(message);
        writer.Flush();
    }
    #endregion private_methods

    #region default_methods
    void Awake() { Instance = this;
        if (AutoJoinChannels) { OnConnected += JoinChannel; }
        if (ConnectOnAwake) { Connect(); } }

    void OnDisable() { if (OnDisconnected != null) { Disconnect(); } }
    #endregion default_methods
}
