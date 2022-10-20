using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Configuration;

namespace ef_winforms.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base(ConfigurationManager.ConnectionStrings["ConnectionLocalDb"].ToString())
        {

        }

        public virtual DbSet<User> Users { get; set; }
    }
}
