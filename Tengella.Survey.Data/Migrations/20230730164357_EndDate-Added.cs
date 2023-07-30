﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tengella.Survey.Data.Migrations
{
    /// <inheritdoc />
    public partial class EndDateAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Surveys",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Surveys");
        }
    }
}
