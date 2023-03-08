using BenchmarkDotNet.Attributes;
using ConsoleApp.DataProviders;
using ConsoleApp.Persistence.EF.Context;
using Dapper;
using Dommel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp.Tests
{
    [SimpleJob(BenchmarkDotNet.Engines.RunStrategy.ColdStart, BenchmarkDotNet.Jobs.RuntimeMoniker.Net60, launchCount: 1, id: "Insert Test")]
    [MemoryDiagnoser]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MarkdownExporterAttribute.Default]
    public class InsertTest
    {
        private readonly string rawSqlDP = @"INSERT INTO student (FirstName, LastName, BirthDate) 
                                            VALUES (@FirstName, @LastName, @BirthDate)";

        private readonly string rawSqlEF = @"INSERT INTO student (FirstName, LastName, BirthDate) 
                                            VALUES ({0}, {1}, {2})";

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

        [Benchmark(Description = "EF Single Insert")]
        public async Task InsertEF()
        {
            var student = StudentDataProvider.GetStudentEF();

            await context.AddAsync(student);
            await context.SaveChangesAsync();
        }


        [Benchmark(Description = "DP Single Insert")]
        public async Task InsertDP()
        {
            var student = StudentDataProvider.GetStudentDP();

            await connection.InsertAsync(student);
        }


        [Benchmark(Description = "EF Single Raw")]
        public async Task InsertEFRaw()
        {
            var student = StudentDataProvider.GetStudentEF();

            await context.Database.ExecuteSqlRawAsync(rawSqlEF, student.FirstName, student.LastName, student.BirthDate);
        }

        [Benchmark(Description = "DP Single Raw")]
        public async Task InsertDPRaw()
        {
            var student = StudentDataProvider.GetStudentDP();

            await connection.ExecuteAsync(rawSqlDP, student);
        }
    }
}
