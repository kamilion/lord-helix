IpcIRC v1.0

--- DESCRIPTION

IpcIRC lets you simply connect to IRC channels and send/receive messages. 
Tested on standalone Windows, linux, linux headless, and mobile platforms.

NOTE: IpcIRC does not work in Web builds without setting up a cross-domain policy responder.

Available commands:

- Connect to server
- Disconnect from server
- Join channel
- Leave channel
- Send message to channel
- Send message to user privately
- Select a user to send a message to (For UI eventselectors)
- Send a message to a selected user (For UI eventselectors)

Available responses from server:

- On connected to server
- On disconnected from server
- On user joined
- On user left
- On sent user private message
- On receive private message from user
- On sent channel message
- On receive channel message
- On server message

--- QUICK START

1. Place IpcIRC prefab on the scene and select it. 
   Before connecting, you must fill these fields in IpcIRC component:

* Nickname: Your Nickname 
* AuthString: Your authentication string
* Channel: Name of the primary channel to join on connection

2. Enable ConnectOnAwake and start the scene. 
   If connection was successful you will be able to send and receive messages.

3. Read comments in IpcIrcUI.cs script to see how IpcIRC works as a GUI chat system.

4. Read comments in JohnConnor.cs script to see how IPC works for GameObjects.

---
