using System;
using System.Collections.Generic;
using System.Text;

namespace Polyrific.Project.Test.Core
{
    public class DummyException : Exception
    {
        public DummyException() : base(DefaultMessage)
        {

        }

        public const string DefaultMessage = "This is a dummy exception";
    }
}
