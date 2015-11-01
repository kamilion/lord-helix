using System.Collections.Generic;
using Irc;
using UnityEngine;
using UnityEngine.Events;

public class IpcIrcEventHandler : MonoBehaviour {
    [System.Serializable]
    public class IpcIrcEventList {
        public string startsWith;
        public UnityEvent triggerEvent;
    }
    public string triggerPhrase = "EVENT";
    //This is our list we want to use to represent our events to unity's inspector
    public List<IpcIrcEventList> myEventList = new List<IpcIrcEventList>(1);
    void AddNew() { myEventList.Add(new IpcIrcEventList()); } // Add a list element
    void Remove(int index) { myEventList.RemoveAt(index); } // Remove a list element
    void Start() { IpcIrc.Instance.OnChannelMessage += OnChannelMessage; } // Subscribe

    // Receive a message from a channel and dispatch it
    void OnChannelMessage(ChannelMessageEventArgs channelMessageArgs) {
        if (channelMessageArgs.Message.StartsWith(triggerPhrase)) {
            OnChannelEvent(channelMessageArgs);
        }
    }

    // Receive an event from a channel and dispatch it
    void OnChannelEvent(ChannelMessageEventArgs channelMessageArgs) {
        string theEvent = channelMessageArgs.Message.Substring(triggerPhrase.Length + 1);
        // The +1 consumes the implicit space after the trigger phrase!
        foreach (var item in myEventList) {
            if (theEvent.StartsWith(item.startsWith)) {
                item.triggerEvent.Invoke();
            }
        }
    }
}
