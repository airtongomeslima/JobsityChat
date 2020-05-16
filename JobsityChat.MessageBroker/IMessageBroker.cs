using System;

namespace JobsityChat.MessageBroker
{
    public interface IMessageBroker: IDisposable
    {
        void Subscribe(string exchange, string routingKey, Action<string> onReceive, string type = "direct");
        void SendMessage(string exchange, string routingKey, string message, string type = "direct");
    }
}