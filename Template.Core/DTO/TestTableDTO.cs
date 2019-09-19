using Recipe.NetCore.Base.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using Template.Core.Entity;

namespace Template.Core.DTO
{
    public class TestTableDTO : Dto<TestTable, long>
    {
        public string Classification { get; set; }
    }
}
