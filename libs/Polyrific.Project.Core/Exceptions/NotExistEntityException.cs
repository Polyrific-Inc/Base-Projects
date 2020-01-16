using System;
using System.Collections.Generic;
using System.Text;

namespace Polyrific.Project.Core.Exceptions
{
    public class NotExistEntityException : Exception
    {
        public NotExistEntityException(int id)
            : base($"Entity with Id = {id} does not exist.")
        {

        }
    }
}
