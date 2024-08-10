using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestVadarod.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Cur_ID = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Cur_Abbreviation = table.Column<string>(type: "text", nullable: false),
                    Cur_Scale = table.Column<int>(type: "integer", nullable: false),
                    Cur_Name = table.Column<string>(type: "text", nullable: false),
                    Cur_OfficialRate = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rates");
        }
    }
}
