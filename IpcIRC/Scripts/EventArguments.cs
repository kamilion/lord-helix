using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irc
{
    public class UpdateUsersEventArgs : EventArgs
    {
        public string Channel { get; internal set; }
        public string[] UserList { get; internal set; }
        public UpdateUsersEventArgs(string Channel, string[] UserList)
        {
            this.Channel = Channel;
            this.UserList = UserList;
        }
    }

    public class UserJoinedEventArgs : EventArgs
    {
        public string Channel { get; internal set; }
        public string User { get; internal set; }
        public UserJoinedEventArgs(string Channel, string User)
        {
            this.Channel = Channel;
            this.User = User;
        }
    }

    public class UserLeftEventArgs : EventArgs
    {
        public string Channel { get; internal set; }
        public string User { get; internal set; }
        public UserLeftEventArgs(string Channel, string User)
        {
            this.Channel = Channel;
            this.User = User;
        }
    }

    public class UserMessageEventArgs : EventArgs
    {
        public string From { get; internal set; }
        public string Message { get; internal set; }
        public UserMessageEventArgs(string From, string Message)
        {
            this.From = From;
            this.Message = Message;
        }
    }

    public class UserMessageSentEventArgs : EventArgs
    {
        public string To { get; internal set; }
        public string Message { get; internal set; }
        public UserMessageSentEventArgs(string To, string Message)
        {
            this.To = To;
            this.Message = Message;
        }
    }

    public class UserActionEventArgs : EventArgs
    {
        public string From { get; internal set; }
        public string Message { get; internal set; }
        public UserActionEventArgs(string From, string Message)
        {
            this.From = From;
            this.Message = Message;
        }
    }

    public class UserActionSentEventArgs : EventArgs
    {
        public string To { get; internal set; }
        public string Message { get; internal set; }
        public UserActionSentEventArgs(string To, string Message)
        {
            this.To = To;
            this.Message = Message;
        }
    }

    public class UserNoticeEventArgs : EventArgs
    {
        public string From { get; internal set; }
        public string Message { get; internal set; }
        public UserNoticeEventArgs(string From, string Message)
        {
            this.From = From;
            this.Message = Message;
        }
    }

    public class UserNoticeSentEventArgs : EventArgs
    {
        public string To { get; internal set; }
        public string Message { get; internal set; }
        public UserNoticeSentEventArgs(string To, string Message)
        {
            this.To = To;
            this.Message = Message;
        }
    }

    public class ChannelDirectedMessageEventArgs : EventArgs
    {
        public string Channel { get; internal set; }
        public string From { get; internal set; }
        public string Message { get; internal set; }
        public ChannelDirectedMessageEventArgs(string Channel, string From, string Message)
        {
            this.Channel = Channel;
            this.From = From;
            this.Message = Message;
        }
    }

    public class ChannelMessageEventArgs : EventArgs
    {
        public string Channel { get; internal set; }
        public string From { get; internal set; }
        public string Message { get; internal set; }
        public ChannelMessageEventArgs(string Channel, string From, string Message)
        {
            this.Channel = Channel;
            this.From = From;
            this.Message = Message;
        }
    }

    public class ChannelMessageSentEventArgs : EventArgs
    {
        public string Channel { get; internal set; }
        public string Message { get; internal set; }
        public ChannelMessageSentEventArgs(string Channel, string Message)
        {
            this.Channel = Channel;
            this.Message = Message;
        }
    }

    public class ChannelActionEventArgs : EventArgs
    {
        public string Channel { get; internal set; }
        public string From { get; internal set; }
        public string Message { get; internal set; }
        public ChannelActionEventArgs(string Channel, string From, string Message)
        {
            this.Channel = Channel;
            this.From = From;
            this.Message = Message;
        }
    }

    public class ChannelActionSentEventArgs : EventArgs
    {
        public string Channel { get; internal set; }
        public string Message { get; internal set; }
        public ChannelActionSentEventArgs(string Channel, string Message)
        {
            this.Channel = Channel;
            this.Message = Message;
        }
    }

    public class ChannelNoticeEventArgs : EventArgs
    {
        public string Channel { get; internal set; }
        public string From { get; internal set; }
        public string Message { get; internal set; }
        public ChannelNoticeEventArgs(string Channel, string From, string Message)
        {
            this.Channel = Channel;
            this.From = From;
            this.Message = Message;
        }
    }

    public class ChannelNoticeSentEventArgs : EventArgs
    {
        public string Channel { get; internal set; }
        public string Message { get; internal set; }
        public ChannelNoticeSentEventArgs(string Channel, string Message)
        {
            this.Channel = Channel;
            this.Message = Message;
        }
    }

    public class StringEventArgs : EventArgs
    {
        public string Result { get; internal set; }
        public StringEventArgs(string s) { Result = s; }
        public override string ToString() { return Result; }
    }

    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; internal set; }
        public ExceptionEventArgs(Exception x) { Exception = x; }
        public override string ToString() { return Exception.ToString(); }
    }
}
