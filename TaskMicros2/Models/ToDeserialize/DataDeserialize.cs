using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TaskMicros2.Models.ToDeserialize
{
    public class DataDeserialize
    {

        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime LastDate { get; set; }

        public double Amount { get; set; }

        public string? Commentary { get; set; }

        public string Type { get; set; }

        public int TypeId { get; set; }

        public string Category { get; set; }

        public int CategoryId { get; set; }

        public string User { get; set; }

        public int UserId { get; set; }


    }
}
