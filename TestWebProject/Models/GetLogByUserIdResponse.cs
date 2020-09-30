using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebProject.Models
{
    public class GetLogByUserIdResponse
    {
        public int TotalRecords { get; set; }
        public List<UserLog> Logs { get; set; }
    }
}

