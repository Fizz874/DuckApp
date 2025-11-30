using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE;
using Strzelecki_Baranowski.DuckApp.INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.DAO
{
    internal class Duck : IDuck
    {
        public required string Name { get; set; }
        public int ID { get ; set ; }
        public int ProducerID { get ; set ; }
        public double Price { get ; set ; }
        public required string Photo { get; set; } 
        public required string Description { get; set; }
        public Category Category { get ; set ; }
    }
}
