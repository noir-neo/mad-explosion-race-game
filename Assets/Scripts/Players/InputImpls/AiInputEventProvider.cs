using System;
using AIs;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Players.InputImpls
{
    public class AiInputEventProvider : MonoBehaviour, IInputEventProvider
    {
        private readonly ISubject<CoursePath> _coursePath = new AsyncSubject<CoursePath>();
        private readonly ReactiveProperty<Vector3> _nextCorner = new ReactiveProperty<Vector3>();

        void Start()
        {
            var targetCornerIndex = 0;
            _coursePath.Select(path => path.Corners)
                .SelectMany(corners => this.FixedUpdateAsObservable(), (x, _) => x)
                .Where(corners => Vector3.Distance(transform.position, corners[targetCornerIndex]) < 10f)
                .Subscribe(corners => _nextCorner.Value = corners[++targetCornerIndex % corners.Length])
                .AddTo(this);
        }

        public void Inject(CoursePath coursePath)
        {
            _coursePath.OnNext(coursePath);
            _coursePath.OnCompleted();
        }

        IObservable<bool> IInputEventProvider.GetAccelAsObservable()
        {
            return Observable.Interval(TimeSpan.FromMilliseconds(100))
                .CombineLatest(_nextCorner, (_, x) => x)
                .Select(x => Vector3.Distance(transform.position, x) > 20f);
        }

        IObservable<float> IInputEventProvider.GetSteeringAsObservable()
        {
            return Observable.Interval(TimeSpan.FromMilliseconds(100))
                .CombineLatest(_nextCorner, (_, x) => x)
                .Select(GetXAxisInput);
        }

        private float GetXAxisInput(Vector3 target)
        {
            var targetDir = target - transform.position;
            var angle = Vector3.Angle(transform.forward, targetDir);
            if (angle > 20)
            {
                return 0 < targetDir.y ? 1 : -1;
            }
            return 0;
        }
    }
}