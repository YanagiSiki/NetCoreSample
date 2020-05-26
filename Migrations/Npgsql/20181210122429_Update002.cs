using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NetCoreSample.Migrations.Npgsql
{
    public partial class Update002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "public",
                table: "User",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "PostContent",
                schema: "public",
                table: "Post",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostContent",
                schema: "public",
                table: "Post");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "public",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
