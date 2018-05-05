using System;
using UniRx;
using UnityDebug = UnityEngine.Debug;

public static class ObservableExtensions
{
    public static IObservable<T> WhereNotNull<T>(this IObservable<T> observable) where T : class
    {
        return observable.Where(x => x != null);
    }
}