﻿using BenchmarkDotNet.Attributes;
using ConsoleApp.Domain.Entities;
using ConsoleApp.Persistence.EF.Context;
using Dommel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp.Tests
{
    [SimpleJob(
        BenchmarkDotNet.Engines.RunStrategy.ColdStart,
        BenchmarkDotNet.Jobs.RuntimeMoniker.Net60,
        launchCount: 1,
        id: "Insert Many Test")]
    [MemoryDiagnoser]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class FunctionsTest
    {
        private SqlConnection connection;
        private ApplicationDbContext context;

        [GlobalSetup]
        public void Init()
        {
            Program.InitDapper();
            var dbContextOptions = Program.InitEf();

            context = new ApplicationDbContext(dbContextOptions);
            connection = new SqlConnection(Constants.ConnectionStringDapper);

            // let it call modelcreating method
            context.Student.Count();
        }

        [Benchmark(Description = "DP Count")]
        public async Task CountDP()
        {
            await connection.CountAsync<Student>();
        }

        [Benchmark(Description = "EF Count")]
        public async Task CountEF()
        {
            await context.Student.CountAsync();
        }




        [Benchmark(Description = "DP Paged 1,50")]
        public async Task PagedDP()
        {
            (await connection.GetPagedAsync<Student>(1, 50)).ToList();
        }

        [Benchmark(Description = "EF Paged 1,50")]
        public async Task PagedEF()
        {
            await context.Student.Take(50).ToListAsync();
        }




        [Benchmark(Description = "DP Paged 3,75")]
        public async Task Pagedv2DP()
        {
            (await connection.GetPagedAsync<Student>(3, 75)).ToList();
        }

        [Benchmark(Description = "EF Paged 3,75")]
        public async Task Pagedv2EF()
        {
            await context.Student.Skip(75 * 2).Take(75).ToListAsync();
        }
    }
}

