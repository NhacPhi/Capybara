using System;
using UnityEngine;

namespace Stats.Status_Effect
{
    public abstract class StatusEffect : IEquatable<StatusEffect>
    {
        public bool IsStop { get; protected set; }
        public StatsController Target { get; protected set; }
        protected float timer;
        public abstract EffectData Data { get;}
        public abstract string ID { get;}
        /// <summary>
        /// Can Change With Other Value Not In Readonly Data
        /// </summary>
        public virtual int CurStack => 1;
        public virtual int MaxStack => Data.MaxStack;
        public virtual float Duration => Data.Duration;
        public virtual bool IsUnique => Data.IsUnique;
        public virtual bool IsStackable => Data.IsStackable;
        
        public StatusEffect(StatsController target)
        {
            this.Target = target;
        }
        
        internal void StartEffect()
        {
            if(!this.Target) return;
            
            IsStop = false;
            timer = 0f;
            OnStart();
        }

        public void Update()
        {
            timer += Time.deltaTime;
            
            if (timer > Duration)
            {
                Stop();
            }
            WhileActive();
        }

        public void Stop()
        {
            IsStop = true;
            OnStop();
        }
        
        public virtual void Reset()
        {
            IsStop = true;
        }
        protected virtual void OnStart(){}
        protected virtual void WhileActive(){}
        protected virtual void OnStop(){}
        public virtual void AddStack(){}
        
        /// <summary>
        /// Return Stack After Remove Stack
        /// Default Is Not Stackable So Return 0
        /// </summary>
        /// <returns></returns>
        public virtual int RemoveStack() => 0;
        public abstract StatusEffect Clone();
        
        public virtual string GetID() => this.ID;
        
        //Can Add Source From Other To Compart If Effect Difference From Source
        public virtual bool Equals(StatusEffect other)
        {
            return this.GetID() == other.GetID()
                && this.GetType() == other.GetType();
        }
    }
}
