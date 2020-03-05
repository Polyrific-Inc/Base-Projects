using Polyrific.Project.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polyrific.Project.Test.Core
{
    public class DummyTestEntity : BaseEntity
    {
        public string Name { get; set; }

        public override void UpdateValueFrom(BaseEntity source)
        {
            var sourceEntity = (DummyTestEntity)source;

            Name = sourceEntity.Name;
        }
    }
}
