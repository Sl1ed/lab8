using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Models;


namespace MVC.Controllers
{
    public class HomeController : Controller
    {

        static List<Special> SpecialList = new List<Special>();
        static DataSet bd = new DataSet();
        static OleDbConnection Connection;
        static DataTable SpecialTable;
        static OleDbDataAdapter adapter = new OleDbDataAdapter();
        static OleDbCommandBuilder builder;

        public HomeController()
        {
            Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=d:\\Students.mdb;");
            bd.Clear();
            adapter.SelectCommand = new OleDbCommand("SELECT * FROM Специальности;", Connection);
            adapter.UpdateCommand = new OleDbCommand("UPDATE Специальности SET [Описание специальности]=@Info WHERE [Код специальности]=@Id", Connection);
            adapter.Fill(bd, "Специальности");
            SpecialTable = bd.Tables["Специальности"];
        }

        public ActionResult Index()
        {
            SpecialList.Clear();

            foreach (DataRow row in from row in SpecialTable.AsEnumerable() select row)
            {
                SpecialList.Add(new Special
                {
                    Id = row.Field<int>("Код специальности"),
                    Name = row.Field<string>("Наименование специальности"),
                    Opisanie = row.Field<string>("Описание специальности"),
                });
            }

            ViewBag.Specialnosti = SpecialList.AsEnumerable<Special>();

            return View();
        }
        
        [HttpGet]
        public ActionResult Change(int Id)
        {
            ViewBag.SpecId = Id;
            return View();
        }

        [HttpPost]
        public ActionResult Change(Special specialnost)
        {
            (from row in SpecialTable.AsEnumerable() where row.Field<int>("Код специальности") == specialnost.Id select row).First()[2] = specialnost.Opisanie;

            adapter.UpdateCommand.Parameters.Add("@Info", OleDbType.VarChar, 200, "Описание специальности");
            adapter.UpdateCommand.Parameters.Add("@Id", OleDbType.Integer, 200, "Код специальности");
            adapter.Update(SpecialTable);

            ViewBag.Success = $"Описание специальности измененено на {specialnost.Opisanie} ";

            return View();
        }
    }
}