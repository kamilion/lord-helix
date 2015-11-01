using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Irc;

public class IpcIrcUIConnectPanel : MonoBehaviour {
    // Fields for connection
    public InputField UsernameText;
    public InputField AuthStringText;
    public InputField ChannelText;

    public void Connect() {
        // Check the instance to see if it already has connection info.
        if (String.IsNullOrEmpty(IpcIrc.Instance.AuthString)) {
            // Provide values to the core IpcIrc instance.
            IpcIrc.Instance.Nickname = UsernameText.text;
            IpcIrc.Instance.AuthString = AuthStringText.text;
            IpcIrc.Instance.CommandChannel = ChannelText.text;
        }

        IpcIrc.Instance.Connect();
    }
}
