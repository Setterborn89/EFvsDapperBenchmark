using BenchmarkDotNet.Attributes;
using ConsoleApp.DataProviders;
using ConsoleApp.Domain.Entities;
using ConsoleApp.Persistence.EF.Context;
using Dommel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.Dapper.Plus;

namespace ConsoleApp.Tests
{
    [SimpleJob(
        BenchmarkDotNet.Engines.RunStrategy.ColdStart,
        BenchmarkDotNet.Jobs.RuntimeMoniker.Net60,
        launchCount: 1,
        id: "Insert Many Test")]
    [MemoryDiagnoser]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class InsertManyTest
    {
        [Params(10, 100, 1000)]
        public int insertRowCount { get; set; }

        private SqlConnection connection;
        private ApplicationDbContext context;

        [GlobalSetup]
        public void Init()
        {
            Program.InitDapper();
            var dbContextOptions = Program.InitEf();

            connection = new SqlConnection(Constants.ConnectionStringDapper);
            context = new ApplicationDbContext(dbContextOptions);

            // let it call modelcreating method
            context.Student.Count();
        }

        [Benchmark(Description = "EF Insert Many")]
        public void InsertEF()
        {
            var students = StudentDataProvider.GetStudentsDP(insertRowCount).ToList();
            context.BulkInsert(students);
        }

        [Benchmark(Description = "DP Insert Many")]
        public void InsertDP()
        {
            var students = StudentDataProvider.GetStudentsDP(insertRowCount).ToList();
            connection.BulkInsert(students);
        }
    }
}
