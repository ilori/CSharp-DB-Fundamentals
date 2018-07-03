using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Products.Data.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE Categories ADD CONSTRAINT CK_Category_Name CHECK (LEN(Name) >=3 AND LEN(Name) <= 15);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}