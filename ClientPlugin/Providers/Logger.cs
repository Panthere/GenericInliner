using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientPlugin.Models;

namespace ClientPlugin.Providers
{
    /// <summary>
    /// Universal logger to display log messages
    /// </summary>
    public static class Logger
    {
        public static event LogReceived MessageReceived;

        public delegate void LogReceived(object sender, LogReceivedEventArgs e);

        /// <summary>
        /// Log to the log without specifying a plugin
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="msg">Message to display</param>
        public static void Log(string sender, string msg)
        {
            MessageReceived(null, new LogReceivedEventArgs() { Message = msg, AltSender = sender });
        }
        /// <summary>
        /// Log to log specifying the plugin that caused the message
        /// </summary>
        /// <param name="sender">The <see cref="ClientPlugin.Models.IGenericInliner"/> plugin that called this</param>
        /// <param name="msg">Message to display</param>
        public static void Log(IGenericInliner sender, string msg)
        {
            MessageReceived(null, new LogReceivedEventArgs() { Message = msg, Sender = sender });
        }
    }
    public class LogReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// The plugin that sent the message
        /// </summary>
        public IGenericInliner Sender;
        /// <summary>
        /// The message sent
        /// </summary>
        public string Message;
        /// <summary>
        /// If no plugin sent the message, this will be filled
        /// </summary>
        public string AltSender;
    }
}
