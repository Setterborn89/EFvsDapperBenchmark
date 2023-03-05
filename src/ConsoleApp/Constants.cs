using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public static class Constants
    {
        public static String ConnectionStringDapper { get; } = "Server=(localdb)\\mssqllocaldb;Database=efdapperbenchmark;Trusted_Connection=True;MultipleActiveResultSets=true";
        public static String ConnectionStringEF { get; } = "Server=(localdb)\\mssqllocaldb;Database=efdapperbenchmark;Trusted_Connection=True;MultipleActiveResultSets=true";
    }
}
