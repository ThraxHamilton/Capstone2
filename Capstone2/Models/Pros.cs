using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone2.Models
{
    public class Pros
    {
        [Key]
        public int ProId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public ApplicationUser User { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string ProEntry { get; set; }

    }
}
