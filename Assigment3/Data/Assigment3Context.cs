using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Assigment3.Models;

namespace Assigment3.Data
{
    public class Assigment3Context : DbContext
    {
        public Assigment3Context (DbContextOptions<Assigment3Context> options)
            : base(options)
        {
        }

        public DbSet<Assigment3.Models.WheatherDato> WheatherDato { get; set; }
    }
}
