﻿using ConsoleApp.Domain.Entities;
using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Persistence.Dapper.Mapping
{
    public class StudentMap : DommelEntityMap<Student>
    {
        public StudentMap()
        {
            ToTable("student", "dbo");

            Map(p => p.Id).ToColumn("id")
                            .IsKey()
                            .SetGeneratedOption(DatabaseGeneratedOption.None)
                            .IsIdentity();
        }
    }
}
