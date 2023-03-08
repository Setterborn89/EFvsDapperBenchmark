using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using ConsoleApp.DataProviders;
using ConsoleApp.Persistence.Dapper.Mapping;
using ConsoleApp.Persistence.EF.Context;
using ConsoleApp.Tests;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace ConsoleApp
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var config = new ManualConfig()
                .WithOptions(ConfigOptions.DisableOptimizationsValidator)
                .AddValidator(JitOptimizationsValidator.DontFailOnError)
                .AddLogger(ConsoleLogger.Default)
                .AddColumnProvider(DefaultColumnProviders.Instance);

            var context = new ApplicationDbContext(InitEf());

            context.Database.EnsureDeleted();
            context.Database.Migrate();


            BenchmarkRunner.Run<InsertTest>(config);
            BenchmarkRunner.Run<UpdateTest>(config);
            BenchmarkRunner.Run<InsertManyTest>(config);
            BenchmarkRunner.Run<DeleteTest>(config);
            BenchmarkRunner.Run<SelectTest>(config);
            BenchmarkRunner.Run<FunctionsTest>(config);
            BenchmarkRunner.Run<SearchTest>(config);

            Console.ReadLine();
        }

        public static void InitDapper()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            FluentMapper.Initialize(config =>
            {
                config.AddMap(new StudentMap());
                config.ForDommel();
            });
        }

        public static DbContextOptions InitEf()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(Constants.ConnectionStringEF);
            builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            return builder.Options;
        }
    }
}