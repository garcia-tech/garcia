using GarciaCore.Domain.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarciaCore.Persistence.Tests
{
    public class TestMongoEntity : MongoDbEntity
    {
        public string Name { get; set; }
        public int Indicator { get; set; }
    }
}
