using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public abstract class BaseViewModel
    {
        public int Id { get; set; }
        public abstract string DisplayName { get; }
    }
}
