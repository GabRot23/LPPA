using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPIUAI.Migrations
{
    /// <inheritdoc />
    public partial class MateriasFacultades_MateriasProfesores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MateriasFacultades",
                columns: table => new
                {
                    MateriaId = table.Column<int>(type: "int", nullable: false),
                    FacultadId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriasFacultades", x => new { x.MateriaId, x.FacultadId });
                    table.ForeignKey(
                        name: "FK_MateriasFacultades_Facultades_FacultadId",
                        column: x => x.FacultadId,
                        principalTable: "Facultades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MateriasFacultades_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MateriasProfesores",
                columns: table => new
                {
                    MateriaId = table.Column<int>(type: "int", nullable: false),
                    ProfesorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriasProfesores", x => new { x.MateriaId, x.ProfesorId });
                    table.ForeignKey(
                        name: "FK_MateriasProfesores_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MateriasProfesores_Profesores_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MateriasFacultades_FacultadId",
                table: "MateriasFacultades",
                column: "FacultadId");

            migrationBuilder.CreateIndex(
                name: "IX_MateriasProfesores_ProfesorId",
                table: "MateriasProfesores",
                column: "ProfesorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MateriasFacultades");

            migrationBuilder.DropTable(
                name: "MateriasProfesores");
        }
    }
}
