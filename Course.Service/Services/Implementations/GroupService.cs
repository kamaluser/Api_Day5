using Course.Core.Entities;
using Course.Data;
using Course.Service.Dtos.GroupDtos;
using Course.Service.Exceptions;
using Course.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Service.Services.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GroupService> _logger;

        public GroupService(AppDbContext context, ILogger<GroupService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public int Create(GroupCreateDto dto)
        {
            _logger.LogInformation("Create method called with No: {No}", dto.No);

            if (_context.Groups.Any(x => x.No == dto.No))
            {
                _logger.LogWarning("Group with No: {No} already exists", dto.No);
                throw new RestException(StatusCodes.Status400BadRequest, "No", "Group is already exists with given No");
            }

            Group group = new Group
            {
                No = dto.No,
                Limit = dto.Limit,
            };

            _context.Groups.Add(group);
            _context.SaveChanges();

            _logger.LogInformation("Group created with Id: {Id}", group.Id);
            return group.Id;
        }

        public List<GroupGetDto> GetAll()
        {
            _logger.LogInformation("GetAll method called");

            var result = _context.Groups.Where(x => !x.IsDeleted).Select(x => new GroupGetDto
            {
                Id = x.Id,
                No = x.No,
                Limit = x.Limit,
            }).ToList();

            _logger.LogInformation("GetAll method completed");
            return result;
        }

        public void Edit(int id, GroupEditDto dto)
        {
            _logger.LogInformation("Edit method called for Id: {Id}", id);

            Group group = _context.Groups.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (group == null)
            {
                _logger.LogWarning("Group not found with Id: {Id}", id);
                throw new RestException(StatusCodes.Status404NotFound, "Group not found by given Id!");
            }

            group.No = dto.No;
            group.Limit = dto.Limit;

            _context.SaveChanges();
            _logger.LogInformation("Group edited successfully for Id: {Id}", id);
        }

        public void Delete(int id)
        {
            _logger.LogInformation("Delete method called for Id: {Id}", id);

            Group group = _context.Groups.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (group == null)
            {
                _logger.LogWarning("Group not found with Id: {Id}", id);
                throw new RestException(StatusCodes.Status404NotFound, "Group not found by given Id!");
            }

            group.IsDeleted = true;
            _context.Groups.Remove(group);
            _context.SaveChanges();

            _logger.LogInformation("Group deleted successfully for Id: {Id}", id);
        }

        public GroupGetDto GetById(int id)
        {
            _logger.LogInformation("GetById method called for Id: {Id}", id);

            Group group = _context.Groups.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (group == null)
            {
                _logger.LogWarning("Group not found with Id: {Id}", id);
                throw new RestException(StatusCodes.Status404NotFound, "Group not found by given Id!");
            }

            var result = new GroupGetDto
            {
                Id = group.Id,
                No = group.No,
                Limit = group.Limit,
            };

            _logger.LogInformation("GetById method completed for Id: {Id}", id);
            return result;
        }
    }
}