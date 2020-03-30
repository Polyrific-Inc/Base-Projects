using System;
using System.Collections.Generic;
using System.Text;

namespace Polyrific.Project.Core
{

    /// <summary>
    /// Filter class for grid
    /// </summary>
    public class Filter
    {
        public string PropertyName { get; set; }
        public Op Operation { get; set; }
        public object Value { get; set; }
    }

    public enum Op
    {
        Equals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        StartsWith,
        EndsWith
    }
}
