using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace QLK.Website.Model
{
    public class excel
    {
            public static DataTable Import(DataTable dt, XSSFWorkbook workbook, QLKEntities db)
            {
                NPOI.SS.UserModel.ISheet sheet = workbook.GetSheetAt(0);
                IEnumerator rows = sheet.GetRowEnumerator();
                for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
                {
                    dt.Columns.Add(sheet.GetRow(0).Cells[j].ToString());
                }
                while (rows.MoveNext())
                {
                    XSSFRow row = (XSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        NPOI.SS.UserModel.ICell cell = row.GetCell(i);
                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            dr[i] = cell.ToString();
                        }
                    }
                    dt.Rows.Add(dr);
                }
                dt.Rows.RemoveAt(0);
                if (dt != null && dt.Rows.Count != 0)
                {
                //add product
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                       Product b = new Product();
                        b.ProductID = dt.Rows[i]["ProductID"].ToString();
                        b.CategoryID = (int)dt.Rows[i]["CategoryID"];
                        b.SupplierID = (int)dt.Rows[i]["SupplierID"];
                        b.ProductName = dt.Rows[i]["ProductName"].ToString();
                         b.Unit = dt.Rows[i]["Unit"].ToString();
                    //THIẾU ẢNH
                         b.Price = (int)dt.Rows[i]["Price"];
                    //var flag = db.Products.Where(x => x. == b.BO).FirstOrDefault();
                    //    if (flag != null && flag.paymentstatus != b.paymentstatus)
                    //    {
                    //        flag.paymentstatus = b.paymentstatus;
                    //        db.Products.AddOrUpdate(flag);
                    //    }
                    //    if (flag != null)
                    //    {
                    //        db.Products.AddOrUpdate(flag);
                    //    }
                    //    else
                    //    {
                    //        db.Products.AddOrUpdate(b);
                    //    }
                    }
                }
                db.SaveChanges();
                return dt;
            }
            public static DataTable Import(DataTable dt, HSSFWorkbook workbook, QLKEntities db)
            {
                NPOI.SS.UserModel.ISheet sheet = workbook.GetSheetAt(0);
                IEnumerator rows = sheet.GetRowEnumerator();
                for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
                {
                    dt.Columns.Add(sheet.GetRow(0).Cells[j].ToString());
                }
                while (rows.MoveNext())
                {
                    HSSFRow row = (HSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        NPOI.SS.UserModel.ICell cell = row.GetCell(i);
                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            dr[i] = cell.ToString();
                        }
                    }
                    dt.Rows.Add(dr);
                }
                dt.Rows.RemoveAt(0);
                if (dt != null && dt.Rows.Count != 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                    Product b = new Product();
                    b.ProductID = dt.Rows[i]["ProductID"].ToString();
                    b.CategoryID = (int)dt.Rows[i]["CategoryID"];
                    b.SupplierID = (int)dt.Rows[i]["SupplierID"];
                    b.ProductName = dt.Rows[i]["ProductName"].ToString();
                    b.Unit = dt.Rows[i]["Unit"].ToString();
                    //THIẾU ẢNH
                    b.Price = (int)dt.Rows[i]["Price"];
                    //var flag = db.Products.Where(x => x.BO == b.BO).FirstOrDefault();
                    //    if (flag != null && flag.paymentstatus != b.paymentstatus)
                    //    {
                    //        flag.paymentstatus = b.paymentstatus;
                    //        db.Products.AddOrUpdate(flag);
                    //    }
                    //    if (flag != null)
                    //    {
                    //        db.Products.AddOrUpdate(flag);
                    //    }
                    //    else
                    //    {
                    //        db.Products.AddOrUpdate(b);
                    //    }
                    }
                }
                db.SaveChanges();
                return dt;
            }
        }
   
}