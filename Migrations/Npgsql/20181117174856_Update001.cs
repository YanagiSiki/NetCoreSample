using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NetCoreSample.Migrations.Npgsql
{
    public partial class Update001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LoginAt",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "VerifyCode",
                schema: "public",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "public",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "public",
                table: "User",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                schema: "public",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "public",
                table: "User",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LoginAt",
                schema: "public",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerifyCode",
                schema: "public",
                table: "User",
                nullable: true);
        }
    }
}
