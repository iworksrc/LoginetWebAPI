using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginetWebAPI.DTO
{
    public class Album
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
    }
}