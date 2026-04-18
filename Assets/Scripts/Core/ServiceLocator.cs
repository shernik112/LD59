using System.Collections.Generic;
using System;

public sealed class ServiceLocator
{
    private static readonly Lazy<ServiceLocator> _instance = 
        new Lazy<ServiceLocator>(() => new ServiceLocator());

    public static ServiceLocator Instance => _instance.Value;

    private readonly Dictionary<Type, IService> _services = new();
    
    private ServiceLocator() { }

    public void Register<TService>(TService service) where TService : class, IService
    {
        if (service == null)
            throw new ArgumentNullException(nameof(service));

        Type type = typeof(TService);
        if (_services.ContainsKey(type))
        {
            UnityEngine.Debug.LogWarning($"[ServiceLocator] Service of type {type.Name} is already registered.");
        }

        _services[type] = service;
    }

    public TService Get<TService>() where TService : class, IService
    {
        if (_services.TryGetValue(typeof(TService), out var service))
            return (TService)service;

        throw new InvalidOperationException($"[ServiceLocator] Service of type {typeof(TService).Name} is not registered.");
    }

    public bool Remove<TService>() where TService : class, IService
    {
        return _services.Remove(typeof(TService));
    }

    public void Clear()
    {
        _services.Clear();
    }
}