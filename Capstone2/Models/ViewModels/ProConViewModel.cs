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

        public List<PCTotals> PCTotals { get; set; }


        public ProConViewModel(ApplicationDbContext context)
        {
            Dictionary<Pros, string> proByDate = new Dictionary<Pros, string>();
            
            ProListItems = context.Pros.Where(d => d.Date == DateTime.Today.ToString("MM/dd/yy")).ToList();
            ConListItems = context.Cons.Where(d => d.Date == DateTime.Today.ToString("MM/dd/yy")).ToList();
            totalPros = ProListItems.Count();
            totalCons = ConListItems.Count();

            PCTotals = new List<PCTotals>();
            



           var pTotal = from Pros in context.Pros
            group Pros by Pros.Date
            into PCTotals
            select new PCTotals
            {
                Date = PCTotals.Key,
                totalPros = PCTotals.Count()
            };

            var cTotal = from Cons in context.Cons
                         group Cons by Cons.Date
                         into PCTotals
                         select new PCTotals
                         {
                             Date = PCTotals.Key,
                             totalCons = PCTotals.Count()
                         };

            foreach (var pro in pTotal)
            {
                pro.totalCons = cTotal.FirstOrDefault(c => c.Date == pro.Date)?.totalCons ?? 0;
                PCTotals.Add(pro);

            }

 

        }
    }
}
    
