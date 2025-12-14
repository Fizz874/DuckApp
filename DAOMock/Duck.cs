using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Strzelecki_Baranowski.DuckApp.CORE;
using Strzelecki_Baranowski.DuckApp.INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.DAO
{
    internal class Duck : IDuck
    {
        public string Name { get; set; } = string.Empty;
        public int ID { get ; set ; }
        public int ProducerID { get ; set ; }
        public double Price { get ; set ; }
        public string Photo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Category Category { get ; set ; }
    }
}
