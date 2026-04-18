using System.Collections.Generic;
using System;

public sealed class ServiceLocator
{
    private readonly Dictionary<Type, IService> _services = new();

    public void Register<TService>(TService service) where TService : class, IService
    {
        if (service == null)
            throw new ArgumentNullException(nameof(service));

        _services[typeof(TService)] = service;
    }

    public TService Get<TService>() where TService : class, IService
    {
        if (_services.TryGetValue(typeof(TService), out var service))
            return (TService)service;

        throw new InvalidOperationException($"Service of type {typeof(TService).Name} is not registered.");
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