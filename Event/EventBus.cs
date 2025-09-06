using System.Collections.Generic;
using System;
public class EventBus : IEventBus
{
    private readonly Dictionary<Type, List<object>> subscribers = new ();
    public void Subscribe<T>(Action<T> handler) where T : struct
    {
        var eventType = typeof(T);

        if (!subscribers.ContainsKey(eventType))
        {
            subscribers[eventType] = new List<object> ();
        }

        subscribers[eventType].Add(handler);
    }

    public void Unsubscribe<T>(Action<T> handler) where T : struct
    {
        var eventType = typeof(T);

        if (subscribers.ContainsKey(eventType))
        {
            subscribers.Remove(eventType);
        }
        
    }
    public void Publish<T>(T eventData) where T : struct
    {
        var enentType = typeof(T);

        if (subscribers.ContainsKey(enentType))
        {
            foreach (var handler in subscribers[enentType])
            {
                if (handler is Action<T> typeHandler)
                {
                    typeHandler.Invoke(eventData);
                }
            }
        }
    }
}

