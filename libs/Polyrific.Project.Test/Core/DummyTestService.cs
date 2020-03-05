using Microsoft.Extensions.Logging;
using Polyrific.Project.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polyrific.Project.Test.Core
{
    public class DummyTestService : BaseService<DummyTestEntity>, IDummyTestService
    {
        public DummyTestService(IRepository<DummyTestEntity> repository, ILogger logger) : base(repository, logger)
        {
        }
    }
}
