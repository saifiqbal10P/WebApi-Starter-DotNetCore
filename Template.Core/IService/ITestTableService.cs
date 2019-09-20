using Recipe.NetCore.Base.Abstract;
using Recipe.NetCore.Base.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Template.Core.DTO;
using Template.Core.Entity;
using Template.Core.IRepository;

namespace Template.Core.IService
{
    public interface ITestTableService: IService<ITestTableRepository, TestTable, TestTableDTO, long>
    {
        Task<DataTransferObject<List<TestTableDTO>>> GetByName(DataTransferObject<TestTableDTO> model);

        Task<DataTransferObject<List<TestTableDTO>>> GetAll();

    }
}
