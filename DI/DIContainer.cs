using UnityEngine;
using System.Collections.Generic;
using System;
public class DIContainer : MonoBehaviour
{
    public static DIContainer Instance { get; private set; }

    private readonly Dictionary<Type, object> services = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            RegisterDefaultService();
        }
        
    }

    void RegisterDefaultService()
    {
        Register<IEventBus>(new EventBus());
    }


    public void Register<TInterface>(object implementation)
    {
        services[typeof(TInterface)] = implementation;
    }

    public T Resolove<T>() where T : class
    {
        if (services.ContainsKey(typeof(T)))
        {
            return services[typeof(T)] as T;
        }

        throw new InvalidOperationException($"Service of type {typeof(T)} not registered");
    }

    public bool TryResolve<T>(out T service) where T : class
    {
        service = default(T);

        if (services.ContainsKey(typeof(T)))
        {
            service = services[typeof(T)] as T;
            return true;
        }

        return false;
    }
}
