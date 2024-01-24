using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLK.Website.Model
{
    public class Register
    {
        QLKEntities db = new QLKEntities();
        public void Add(SystemDatabase register)
        {
            db.SystemDatabases.Add(register);
            db.SaveChanges();
        }
    }
}