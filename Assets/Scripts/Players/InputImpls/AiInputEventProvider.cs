using System;
using System.Linq;
using AIs;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Players.InputImpls
{
    public class AiInputEventProvider : MonoBehaviour, IInputEventProvider
    {
        private readonly ISubject<CoursePath> _coursePath = new AsyncSubject<CoursePath>();
        [SerializeField] Vector3ReactiveProperty _nextCorner = new Vector3ReactiveProperty();

        void Start()
        {
            _coursePath.Select(path => path.Corners.ToList())
                .SelectMany(corners => this.FixedUpdateAsObservable(), (x, _) => x)
                .WithLatestFrom(_nextCorner, Tuple.Create)
                .Where(t => Vector3.Distance(transform.position, t.Item2) < 50)
                .Select(t => t.Item1.ElementAt((t.Item1.IndexOf(t.Item2) + 1) % t.Item1.Count))
                .Subscribe(x => _nextCorner.Value = x)
                .AddTo(this);
        }

        public void Inject(CoursePath coursePath)
        {
            _coursePath.OnNext(coursePath);
            _coursePath.OnCompleted();
        }

        IObservable<bool> IInputEventProvider.GetAccelAsObservable()
        {
            return Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(500))
                .Select(_ => Random.Range(0, 10) > 0)
                .DistinctUntilChanged();
        }

        IObservable<float> IInputEventProvider.GetSteeringAsObservable()
        {
            return Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(50))
                .WithLatestFrom(_nextCorner, (_, x) => x)
                .Select(GetXAxisInput)
                .DistinctUntilChanged();
        }

        private float GetXAxisInput(Vector3 target)
        {
            var targetDir = target - transform.position;
            var dot = Vector3.Dot(transform.right.normalized, targetDir.normalized);
            if (Mathf.Abs(dot) < Random.Range(0f, 0.2f)) return 0;
            return dot;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(_nextCorner.Value, 50);
        }
    }
}