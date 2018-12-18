using Capstone2.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone2.Models.ViewModels
{
    public class ProConViewModel
    {
        public List<Pros> ProListItems { get; set; }
        public Pros ProEntry { get; set; }

        public List<Cons> ConListItems { get; set; }
        public Cons ConEntry { get; set; }

        public int totalPros { get; set; }
        public int totalCons { get; set; }

        public ProConViewModel(ApplicationDbContext context)
        {
            ProListItems = context.Pros.ToList();
            ConListItems = context.Cons.ToList();
             totalPros = ProListItems.Count();
            totalCons = ConListItems.Count();
           
          
        }
    }
}
