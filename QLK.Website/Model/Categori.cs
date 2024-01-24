using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLK.Website.Model
{
    public class Categori
    {
        QLKEntities db = new QLKEntities();

        public IEnumerable<Category> List()
        {
            return db.Categories.ToList();
        }
        public Category listCategoryID(int id)
        {
            return db.Categories.First(m => m.CategoryID.CompareTo(id) == 0);
        }
        public void Add(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
        }
        public void Edit(Category category)
        {
          
                Category p = listCategoryID(category.CategoryID);
                p.CategoryID = category.CategoryID;
                p.CategoryName = category.CategoryName;
                p.Description = category.Description;
                db.SaveChanges();
             
           
        }
        //public void Delete(int id)
        //{
            
        //        Category p = listCategoryID(id);
        //        db.Categories.Remove(p);
        //        db.SaveChanges();
            
           
        //}
        //public void DetailCategory( int id)
        //{

        //}
    }
}