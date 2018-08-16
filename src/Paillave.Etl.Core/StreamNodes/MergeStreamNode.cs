﻿using Paillave.Etl.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Paillave.RxPush.Operators;
using Paillave.Etl.Core.Streams;
using Paillave.Etl.Core.StreamNodes;

namespace Paillave.Etl.StreamNodes
{
    public class MergeStreamNode<TIn> : StreamNodeBase<IStream<TIn>, TIn, IStream<TIn>>, IStreamNodeOutput<TIn>
    {
        public IStream<TIn> Output { get; }
        public MergeStreamNode(IStream<TIn> input, string name, IEnumerable<string> parentNodeNamePath, IStream<TIn> arguments) : base(input, name, parentNodeNamePath, arguments)
        {
            this.Output = base.CreateStream(nameof(Output), input.Observable.Merge(arguments.Observable));
        }
    }
    public static partial class StreamEx
    {
        public static IStream<I> Merge<I>(this IStream<I> stream, string name, IStream<I> inputStream2)
        {
            return new MergeStreamNode<I>(stream, name, null, inputStream2).Output;
        }
    }
}
