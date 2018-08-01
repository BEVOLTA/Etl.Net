﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paillave.RxPush.Core;
using System.Linq.Expressions;
using Paillave.RxPush.Disposables;

namespace Paillave.RxPush.Operators
{
    public class ChunkSubject<TIn> : PushSubject<IEnumerable<TIn>>
    {
        private IDisposable _subscription;
        private object syncLock = new object();
        private IList<TIn> _chunk = new List<TIn>();
        private readonly int _chunkSize;

        public ChunkSubject(IPushObservable<TIn> sourceS, int chunkSize)
        {
            lock (syncLock)
            {
                this._chunkSize = chunkSize;
                this._subscription = sourceS.Subscribe(this.HandleOnPush, this.HandleOnComplete, this.HandleOnException);
            }
        }

        private void HandleOnException(Exception ex)
        {
            this.PushException(ex);
        }

        private void HandleOnComplete()
        {
            lock (syncLock)
            {
                if (_chunk.Count > 0)
                    this.PushValue(_chunk);
                _chunk = new List<TIn>();
                this.Complete();
            }
        }

        private void HandleOnPush(TIn obj)
        {
            lock (syncLock)
            {
                _chunk.Add(obj);
                if (_chunk.Count == _chunkSize)
                {
                    this.PushValue(_chunk);
                    _chunk = new List<TIn>();
                }
            }
        }

        public override void Dispose()
        {
            lock (syncLock)
            {
                base.Dispose();
                _subscription.Dispose();
            }
        }
    }

    public static partial class ObservableExtensions
    {
        public static IPushObservable<IEnumerable<TIn>> Chunk<TIn>(this IPushObservable<TIn> sourceS, int chunkSize)
        {
            return new ChunkSubject<TIn>(sourceS, chunkSize);
        }
    }
}
