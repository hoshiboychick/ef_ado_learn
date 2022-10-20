using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace ef_winforms.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
    }
}
