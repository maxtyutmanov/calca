using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Calca.Model
{
    public class Tran
    {
        [Required]
        public string CollectionId { get; set; } = "default";

        public long Id { get; set; }

        public DateTime AddedAt { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public List<string> Contributors { get; set; }

        [Required]
        public List<string> Consumers { get; set; }
    }
}
