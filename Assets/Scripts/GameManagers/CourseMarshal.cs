using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using Zenject;

namespace GameManagers
{
    public class CourseMarshal : MonoBehaviour, IRaceTerminator
    {
        [SerializeField] private int lapCount;

        [Inject] private ICheckPointProvider checkPointProvider;
        [Inject] private ICheckPointPassengersProvider checkPointPassengersProvider;

        private List<CheckPoint> shiftedCheckPoints;

        private readonly ISubject<Unit> terminateRace = new AsyncSubject<Unit>();

        public IObservable<Unit> TerminateRaceAsObservable()
        {
            return terminateRace;
        }

        void OnValidate()
        {
            lapCount = Mathf.Clamp(lapCount, 1, int.MaxValue);
        }

        void Start()
        {
            shiftedCheckPoints = checkPointProvider.CheckPoints
                .Skip(1)
                .Append(checkPointProvider.CheckPoints.First())
                .ToList();

            checkPointPassengersProvider
                .CheckPointPassengersAsObservable()
                .Select(x => ToSomeoneReceivedCheckerObservable(x))
                .Switch()
                .First()
                .Subscribe(_ =>
                {
                    terminateRace.OnNext(Unit.Default);
                    terminateRace.OnCompleted();
                })
                .AddTo(this);
        }

        private IObservable<Unit> ToSomeoneReceivedCheckerObservable(List<ICheckPointPassenger> checkPointPassengers)
        {
            return checkPointPassengers
                .Select(x => ToReceivedCheckerObservable(x))
                .Merge();
        }

        private IObservable<Unit> ToReceivedCheckerObservable(ICheckPointPassenger checkPointPassenger)
        {
            return checkPointPassenger
                .PassedCheckPointAsObservable()
                .Scan(new List<CheckPoint>(), (checkPoints, checkPoint) => {
                    checkPoints.Add(checkPoint);
                    return checkPoints;
                })
                .Where(x => x.TakeLast(shiftedCheckPoints.Count).SequenceEqual(shiftedCheckPoints))
                .Buffer(lapCount)
                .AsUnitObservable();
        }
        
    }
}