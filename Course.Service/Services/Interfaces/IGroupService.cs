using Course.Service.Dtos.GroupDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Service.Services.Interfaces
{
    public interface IGroupService
    {
        int Create(GroupCreateDto createDto);
        List<GroupGetDto> GetAll();
        void Edit(int id, GroupEditDto editDto);
        void Delete(int id);
        GroupGetDto GetById(int id);
    }
}
