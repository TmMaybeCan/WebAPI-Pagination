using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

namespace TestWebProject
{
    public class User
    { 
        [JsonProperty("userId")]
        public int Id { get; set; }
        [JsonProperty("userName")]
        public string Name { get; set; }
        [JsonProperty("userLogs")]
        public List<UserLog> Logs { get; set; }
    }

    public class UserLog
    { 
        public int Id { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public int UserId { get; set; }
        public string Action { get; set; }
        [ForeignKey("UserId")]
        [Newtonsoft.Json.JsonIgnore]
        public User User { get; set; }
    }

    public class Database : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserLog> Logs { get; set; }

        public Database(){}
        public Database(DbContextOptions<Database> options): base(options){}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=data.db"); 
        }
    }
}
