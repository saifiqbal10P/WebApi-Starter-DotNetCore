﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Core.IService
{
    public interface IBackgroundJobService
    {
        public Task TestJob();
    }
}
