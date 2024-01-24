using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLK.Website.Model
{
    public class Uploadfl
    {
        QLKEntities db = new QLKEntities();
        public void Add(Uploadfile uploadfile)
        {
            var model = new Uploadfile();
           
            model.IDPath = uploadfile.IDPath;
            model.Path = uploadfile.Path;

            db.Uploadfiles.Add(model);
            db.SaveChanges();
        }
    }
}