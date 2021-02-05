using UnityEngine.Events;
using UnityEngine;
using System.Collections.Generic;
using System;

public static class Events
{
    public static EventAggregator<Item> OnItemFinded = new EventAggregator<Item>();
    public static EventAggregator<Level> OnLevelChanged = new EventAggregator<Level>();
}

public class EventAggregator<T>
{
    private readonly List<Action<T>> _callbacks = new List<Action<T>>();

    public void Subscribe(Action<T> callback)
    {
        _callbacks.Add(callback);
    }

    public void Publish(T unit)
    {
        foreach (Action<T> callback in _callbacks)
            callback(unit);
    }

    public void UnSubscribe(Action<T> callback)
    {
        _callbacks.Remove(callback);
    }
}
