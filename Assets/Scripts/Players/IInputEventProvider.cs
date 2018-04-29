using System;
using UniRx;
using UnityEngine;

namespace Players
{
    interface IInputEventProvider
    {
        IObservable<bool> GetAccelAsObservable();
        IObservable<float> GetSteeringAsObservable();
    }
}