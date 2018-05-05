using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using GameManagers;

namespace Players
{
    class CheckPointPassenger : MonoBehaviour, ICheckPointPassenger
    {
        public IObservable<CheckPoint> PassedCheckPointAsObservable()
        {
            return this.OnTriggerEnterAsObservable()
                .Select(x => x.gameObject.GetComponent<CheckPoint>())
                .WhereNotNull();
        }
    }
}