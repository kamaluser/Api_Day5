using Course.Core.Entities;
using Course.Data;
using Course.Service.Dtos.GroupDtos;
using Course.Service.Dtos.StudentDtos;
using Course.Service.Exceptions;
using Course.Service.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Group = Course.Core.Entities.Group;

namespace Course.Service.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<GroupService> _logger;

        public StudentService(AppDbContext context, IWebHostEnvironment environment, ILogger<GroupService> logger)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
        }
        public int Create(StudentCreateDto dto)
        {
            _logger.LogInformation("Create method called with Email: {Email}", dto.Email);
            Group group = _context.Groups.Include(x => x.Students).FirstOrDefault(x => x.Id == dto.GroupId && !x.IsDeleted);

            if (group == null)
            {
                _logger.LogWarning("Group not found by given GroupId", dto.GroupId);
                throw new RestException(StatusCodes.Status404NotFound, "GroupId", "Group not found by given GroupId");
            }

            if (group.Limit <= group.Students.Count)
            {
                _logger.LogWarning("Group is full.");
                throw new RestException(StatusCodes.Status400BadRequest, "Group is full");
            }

            if (_context.Students.Any(x => x.Email.ToUpper() == dto.Email.ToUpper() && !x.IsDeleted))
            {
                _logger.LogWarning("Student with Email: {Email} already exists", dto.Email);
                throw new RestException(StatusCodes.Status400BadRequest, "Email", "Student already exists by given Email");
            }

            string file = null;
            if (dto.File != null)
            {
                file = SaveFile(dto.File);
            }

            Student student = new Student
            {
                FullName = dto.FullName,
                Email = dto.Email,
                BirthDate = dto.BirthDate,
                GroupId = dto.GroupId,
                Photo = file
            };

            _context.Students.Add(student);
            _context.SaveChanges();

            _logger.LogInformation("Student created with Id: {Id}", student.Id);
            return student.Id;
        }

        public List<StudentGetDto> GetAll()
        {
            _logger.LogInformation("GetAll method called");

            var students = _context.Students.Include(x => x.Group).Where(x=>!x.IsDeleted).Select(x => new StudentGetDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                BirthDate = x.BirthDate,
                GroupId = x.GroupId,
                GroupName = x.Group.No,
                PhotoUrl = x.Photo!=null?null:$"/uploads/student/{x.Photo}"
            }).ToList();
            _logger.LogInformation("GetAll method completed");

            return students;
        }

        public StudentGetDto GetById(int id)
        {
            _logger.LogInformation("GetById method called for Id: {Id}", id);
            var student = _context.Students.Include(x => x.Group).FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (student == null)
                throw new RestException(StatusCodes.Status404NotFound, $"Student with {id} ID not found.");

            _logger.LogInformation("GetById method completed for Id: {Id}", id);
            return new StudentGetDto
            {
                Id = student.Id,
                FullName = student.FullName,
                Email = student.Email,
                BirthDate = student.BirthDate,
                GroupId = student.GroupId,
                GroupName = student.Group.No,
                PhotoUrl = student.Photo != null ? null : $"uploads/student/{student.Photo}"
            };
        }
        public void Edit(int id, StudentEditDto dto)
        {
            _logger.LogInformation("Edit method called for Id: {Id}", id);

            var student = _context.Students.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (student == null)
                throw new RestException(StatusCodes.Status404NotFound,$"Student with {id} ID not found.");

            Group group = _context.Groups.Include(x => x.Students).FirstOrDefault(x => x.Id == dto.GroupId && !x.IsDeleted);

            if (group == null)
                throw new EntityNotFoundException($"Group with ID {dto.GroupId} not found.");

            if (group.Limit <= group.Students.Count)
                throw new GroupLimitException($"Group is full!");

            if (dto.File != null)
            {
                string path = SaveFile(dto.File);
                student.Photo = path;
            }

            student.FullName = dto.FullName;
            student.Email = dto.Email;
            student.BirthDate = dto.BirthDate;
            student.GroupId = dto.GroupId;

            _context.SaveChanges();
            _logger.LogInformation("Group edited successfully for Id: {Id}", id);
        }

        public void Delete(int id)
        {
            _logger.LogInformation("Delete method called for Id: {Id}", id);

            var student = _context.Students.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (student == null)
                throw new RestException(StatusCodes.Status404NotFound, $"Student with {id} ID not found.");

            student.IsDeleted = true;
            _context.Students.Remove(student);
            _context.SaveChanges();

            _logger.LogInformation("Group deleted successfully for Id: {Id}", id);
        }

        private string SaveFile(IFormFile file)
        {
            string uploadDir = Path.Combine(_environment.WebRootPath, "uploads/student");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadDir, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return fileName;
        }
    }
}
