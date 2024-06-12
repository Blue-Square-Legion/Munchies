using System;

namespace Util
{
    public interface ITimer
    {
        public void Tick(float time);
        public void Stop();
        public void Start();
    }


    public abstract class BaseTimer : ITimer
    {
        public Action OnStart, OnComplete;
        public bool isRunning = true;

        public float currentTime { get; protected set; } = 0;

        public virtual void Start() { isRunning = true; Reset(); OnStart?.Invoke(); }
        public virtual void Stop() { isRunning = false; Reset(); }

        public virtual void Pause() { isRunning = false; }
        public virtual void Resume() { isRunning = true; }

        public virtual void Reset() { currentTime = 0; }

        public virtual void Tick(float time)
        {
            if (!isRunning)
            {
                return;
            }

            OnTickHandler(time);
        }

        protected abstract void OnTickHandler(float time);

    }


    public class Timeout : BaseTimer
    {
        public float targetTime;

        public Timeout(float targetTime)
        {
            this.targetTime = targetTime;
        }

        public Timeout(float targetTime, bool isStarted = true)
        {
            this.targetTime = targetTime;
            this.isRunning = isStarted;
        }

        public Timeout(Action OnComplete, float TargetTime)
        {
            targetTime = TargetTime;
            this.OnComplete += OnComplete;
        }

        protected override void OnTickHandler(float time)
        {
            currentTime += time;
            if (currentTime >= targetTime)
            {
                Stop();
                OnComplete?.Invoke();
            }
        }
    }


    public class Interval : BaseTimer
    {
        public float interval;

        public Interval(float interval)
        {
            this.interval = interval;
        }

        public Interval(Action OnInterval, float interval)
        {
            this.interval = interval;
            OnComplete += OnInterval;
        }

        protected override void OnTickHandler(float time)
        {
            currentTime += time;
            if (currentTime >= interval)
            {
                OnComplete?.Invoke();
                currentTime -= interval;
            }
        }
    }


    public class TimeoutTick : Timeout
    {
        public Action<float> OnTick;

        public TimeoutTick(float time) : base(time)
        {

        }

        protected override void OnTickHandler(float time)
        {
            currentTime += time;
            if (currentTime >= targetTime)
            {
                Stop();
                OnComplete?.Invoke();
            }
            else
            {
                OnTick.Invoke(currentTime / targetTime);
            }
        }
    }
}
