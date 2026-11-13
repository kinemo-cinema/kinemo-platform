using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KinemoPlatform.API.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class PersistentSensoryFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "stored_sensory_blobs",
                columns: table => new
                {
                    path = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    content = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stored_sensory_blobs", x => x.path);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "stored_sensory_blobs");
        }
    }
}
