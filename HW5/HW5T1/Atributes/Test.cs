﻿using System;

namespace Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Test : Attribute
    {
        public Type Expected { get; set; }

        public string Ignore { get; set; }
    }
}
