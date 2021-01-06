using System;
using System.Collections.Generic;
using System.Text;

namespace Atributes
{
    public class Test : Attribute
    {
        public Type Expected { get; set; }

        public string Ignore { get; set; }
    }
}
