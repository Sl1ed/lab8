using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;

namespace MVC.Models
{
    public class Special
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Opisanie { get; set; }
    }
}