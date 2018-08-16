﻿using Paillave.Etl.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Paillave.RxPush.Operators;
using Paillave.Etl.Core.Streams;
using Paillave.RxPush.Core;
using Paillave.Etl.Core.StreamNodes;

namespace Paillave.Etl.StreamNodes
{
    public class ToActionStreamNode<TIn> : AwaitableStreamNodeBase<IStream<TIn>, TIn, Action<TIn>>
    {
        public ToActionStreamNode(IStream<TIn> input, string name, IEnumerable<string> parentNodeNamePath, Action<TIn> arguments) : base(input, name, parentNodeNamePath, arguments)
        {
        }
        protected override void ProcessValue(TIn value)
        {
            this.Arguments(value);
        }
    }

    //public class SkipSortedStreamNode<TIn> : StreamNodeBase<ISortedStream<TIn>, TIn, int>, ISortedStreamNodeOutput<TIn>
    //{
    //    public ISortedStream<TIn> Output { get; }
    //    public SkipSortedStreamNode(ISortedStream<TIn> input, string name, IEnumerable<string> parentNodeNamePath, int arguments) : base(input, name, parentNodeNamePath, arguments)
    //    {
    //        this.Output = base.CreateSortedStream(nameof(Output), input.Observable.Skip(arguments), input.SortCriterias);
    //    }
    //}

    //public class SkipKeyedStreamNode<TIn> : StreamNodeBase<IKeyedStream<TIn>, TIn, int>, IKeyedStreamNodeOutput<TIn>
    //{
    //    public IKeyedStream<TIn> Output { get; }
    //    public SkipKeyedStreamNode(IKeyedStream<TIn> input, string name, IEnumerable<string> parentNodeNamePath, int arguments) : base(input, name, parentNodeNamePath, arguments)
    //    {
    //        this.Output = base.CreateKeyedStream(nameof(Output), input.Observable.Skip(arguments), input.SortCriterias);
    //    }
    //}

    public static partial class StreamEx
    {
        public static IStream<TIn> ToAction<TIn>(this IStream<TIn> stream, string name, Action<TIn> action)
        {
            return new ToActionStreamNode<TIn>(stream, name, null, action).Output;
        }
        //public static ISortedStream<TIn> Skip<TIn>(this ISortedStream<TIn> stream, string name, int count)
        //{
        //    return new SkipSortedStreamNode<TIn>(stream, name, null, count).Output;
        //}
        //public static IKeyedStream<TIn> Skip<TIn>(this IKeyedStream<TIn> stream, string name, int count)
        //{
        //    return new SkipKeyedStreamNode<TIn>(stream, name, null, count).Output;
        //}
    }
}
