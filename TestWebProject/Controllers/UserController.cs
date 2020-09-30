using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.WindowsRuntime;
using TestWebProject.Models;

namespace TestWebProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly Database _context;
        public UserController(Database context) => _context = context;

        // /api/user/info/2
        [HttpGet]
        [Route("info/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.Include(e => e.Logs).FirstOrDefaultAsync(e => e.Id == id);

            if (user == null)
                return NotFound();
            return user;
        }

        // /api/user/logs/2?page=2&pageSize=2
        [HttpGet]
        [Route("logs/{id}")]
        public async Task<ActionResult<GetLogByUserIdResponse>> GetLog(int page, int pageSize, int id)
        {
            var validFilter = new PaginationFilter(page, pageSize);
            var pageData = await _context.Logs
                .Skip((validFilter.Page - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .Where(e => e.UserId == id)
                .ToListAsync();
               
            var totalRecords = await _context.Logs.CountAsync();

            GetLogByUserIdResponse logs = new GetLogByUserIdResponse();

            logs.TotalRecords = totalRecords;
            logs.Logs = pageData;
            
            return logs;      
        }

        // /api/user/create
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            /*
            var u = new User
            {
                userLogs = []
            };
            _context.Users.Add(u);
            */

            if (user == null)
                return BadRequest();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
    }

}
