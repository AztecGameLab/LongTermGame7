﻿using System;
using System.Runtime.Serialization;

namespace Application.Core.Serialization
{
    public struct SurrogateData
    {
        public ISerializationSurrogate Surrogate;
        public Type Type;
    }
}