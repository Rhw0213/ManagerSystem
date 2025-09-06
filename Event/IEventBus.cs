using System;
public interface IEventBus
{ 
    void Subscribe<T>(Action<T> handler) where T : struct;
    void Unsubscribe<T>(Action<T> handler) where T : struct;
    void Publish<T>(T handler) where T : struct;
}
