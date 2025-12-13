using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE;
using Strzelecki_Baranowski.DuckApp.INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.DAO
{
    public class Duck : IDuck
    {
        
        public string Name { get; set; } = string.Empty;  //TODO zakładamy że nazwa może się powtarzać?

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ProducerID { get; set; }
        public double Price { get; set; }
        public string Photo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Category Category { get; set; }

        [ForeignKey("ProducerID")]
        public virtual Producer? Producer { get; set; }

    }
}
