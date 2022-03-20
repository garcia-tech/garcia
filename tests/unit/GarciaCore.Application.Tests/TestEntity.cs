using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarciaCore.Domain;

namespace GarciaCore.Application.Tests
{
    public class TestEntity : Entity<long>
    {
        public string Name { get; set; }
    }
}
