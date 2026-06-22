using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoToTipoDocumento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CENSO_PACIENTE_paciente_id",
                table: "CENSO");

            migrationBuilder.DropForeignKey(
                name: "FK_EVENTO_EVENTO_TIPO_evento_tipo_id",
                table: "EVENTO");

            migrationBuilder.DropForeignKey(
                name: "FK_EVENTO_PACIENTE_paciente_id",
                table: "EVENTO");

            migrationBuilder.DropTable(
                name: "ESCABIOSIS");

            migrationBuilder.DropTable(
                name: "GEOHELMINTIASIS");

            migrationBuilder.DropTable(
                name: "LESHMANIASIS_CUTANEA");

            migrationBuilder.DropTable(
                name: "MALARIA");

            migrationBuilder.DropTable(
                name: "PEDICULOSIS");

            migrationBuilder.DropTable(
                name: "TUBERCULOSIS");

            migrationBuilder.DropTable(
                name: "TUBERCULOSIS_CONTACTO");

            migrationBuilder.DropIndex(
                name: "IX_CENSO_paciente_id",
                table: "CENSO");

            migrationBuilder.DropColumn(
                name: "observacion",
                table: "EVENTO");

            migrationBuilder.DropColumn(
                name: "paciente_id",
                table: "CENSO");

            migrationBuilder.RenameColumn(
                name: "fecha",
                table: "EVENTO",
                newName: "fecha_evento");

            migrationBuilder.RenameColumn(
                name: "evento_tipo_id",
                table: "EVENTO",
                newName: "tipo_id");

            migrationBuilder.RenameColumn(
                name: "evento_id",
                table: "EVENTO",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_EVENTO_evento_tipo_id",
                table: "EVENTO",
                newName: "IX_EVENTO_tipo_id");

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                table: "TIPO_DOCUMENTO",
                type: "varchar(60)",
                unicode: false,
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "tipo",
                table: "TIPO_DOCUMENTO",
                type: "varchar(10)",
                unicode: false,
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ENFERMEDAD",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false),
                    codigo = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    nombre = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENFERMEDAD", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ESTADO_PERSONA",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false),
                    nombre = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESTADO_PERSONA", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ETNIA_PUEBLO_INDIGENA",
                columns: table => new
                {
                    etnia_id = table.Column<byte>(type: "tinyint", nullable: false),
                    pueblo_indigena_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETNIA_PUEBLO_INDIGENA", x => new { x.etnia_id, x.pueblo_indigena_id });
                    table.ForeignKey(
                        name: "FK_ETNIA_PUEBLO_INDIGENA_ETNIA_etnia_id",
                        column: x => x.etnia_id,
                        principalTable: "ETNIA",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ETNIA_PUEBLO_INDIGENA_PUEBLO_INDIGENA_pueblo_indigena_id",
                        column: x => x.pueblo_indigena_id,
                        principalTable: "PUEBLO_INDIGENA",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "EVENTO_ESCABIOSIS",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    severidad = table.Column<byte>(type: "tinyint", nullable: true),
                    localizacion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTO_ESCABIOSIS", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_EVENTO_ESCABIOSIS_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "EVENTO_GEOHELMINTIASIS",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    tipo_prueba = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true),
                    resultado = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTO_GEOHELMINTIASIS", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_EVENTO_GEOHELMINTIASIS_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "EVENTO_LEISHMANIASIS_CUTANEA",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    tipo_lesion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTO_LEISHMANIASIS_CUTANEA", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_EVENTO_LEISHMANIASIS_CUTANEA_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "EVENTO_MALARIA",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    especie = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTO_MALARIA", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_EVENTO_MALARIA_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "EVENTO_PEDICULOSIS",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    grado = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTO_PEDICULOSIS", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_EVENTO_PEDICULOSIS_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "EVENTO_PIAN",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    estadio = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTO_PIAN", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_EVENTO_PIAN_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "EVENTO_TENIASIS_CISTICERCOSIS",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    teniasis = table.Column<bool>(type: "bit", nullable: true),
                    cisticercosis = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTO_TENIASIS_CISTICERCOSIS", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_EVENTO_TENIASIS_CISTICERCOSIS_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "EVENTO_TRACOMA",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    criterio_exclusion_id = table.Column<int>(type: "int", nullable: true),
                    examinado_tt = table.Column<bool>(type: "bit", nullable: true),
                    triquiasis = table.Column<byte>(type: "tinyint", nullable: true),
                    opacidad_corneal = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTO_TRACOMA", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_EVENTO_TRACOMA_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "EVENTO_TUBERCULOSIS",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    tipo_tuberculosis = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTO_TUBERCULOSIS", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_EVENTO_TUBERCULOSIS_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "EVENTO_TUBERCULOSIS_CONTACTO",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    observacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTO_TUBERCULOSIS_CONTACTO", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_EVENTO_TUBERCULOSIS_CONTACTO_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "EVENTO_TUNGIASIS",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    numero_lesiones = table.Column<int>(type: "int", nullable: true),
                    complicaciones = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTO_TUNGIASIS", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_EVENTO_TUNGIASIS_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CENSO_PERSONA",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    censo_id = table.Column<long>(type: "bigint", nullable: false),
                    paciente_id = table.Column<long>(type: "bigint", nullable: false),
                    parentesco_id = table.Column<byte>(type: "tinyint", nullable: true),
                    es_jefe_hogar = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    comunidad = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    grupo_poblacion_especial = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    estado_persona_id = table.Column<byte>(type: "tinyint", nullable: false),
                    creado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "sysutcdatetime()"),
                    creado_por = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    modificado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CENSO_PERSONA", x => x.id);
                    table.ForeignKey(
                        name: "FK_CENSO_PERSONA_CENSO_censo_id",
                        column: x => x.censo_id,
                        principalTable: "CENSO",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CENSO_PERSONA_ESTADO_PERSONA_estado_persona_id",
                        column: x => x.estado_persona_id,
                        principalTable: "ESTADO_PERSONA",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CENSO_PERSONA_PACIENTE_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "PACIENTE",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CENSO_PERSONA_PARENTESCO_parentesco_id",
                        column: x => x.parentesco_id,
                        principalTable: "PARENTESCO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PERSONA_ENFERMEDAD",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    censo_persona_id = table.Column<long>(type: "bigint", nullable: false),
                    enfermedad_id = table.Column<byte>(type: "tinyint", nullable: false),
                    estado_id = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    creado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "sysutcdatetime()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERSONA_ENFERMEDAD", x => x.id);
                    table.ForeignKey(
                        name: "FK_PERSONA_ENFERMEDAD_CENSO_PERSONA_censo_persona_id",
                        column: x => x.censo_persona_id,
                        principalTable: "CENSO_PERSONA",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PERSONA_ENFERMEDAD_ENFERMEDAD_enfermedad_id",
                        column: x => x.enfermedad_id,
                        principalTable: "ENFERMEDAD",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PERSONA_ENFERMEDAD_ESTADO_estado_id",
                        column: x => x.estado_id,
                        principalTable: "ESTADO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_PERSONA_censo_id",
                table: "CENSO_PERSONA",
                column: "censo_id");

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_PERSONA_estado_persona_id",
                table: "CENSO_PERSONA",
                column: "estado_persona_id");

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_PERSONA_paciente_id",
                table: "CENSO_PERSONA",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_PERSONA_parentesco_id",
                table: "CENSO_PERSONA",
                column: "parentesco_id");

            migrationBuilder.CreateIndex(
                name: "IX_ETNIA_PUEBLO_INDIGENA_pueblo_indigena_id",
                table: "ETNIA_PUEBLO_INDIGENA",
                column: "pueblo_indigena_id");

            migrationBuilder.CreateIndex(
                name: "IX_PERSONA_ENFERMEDAD_censo_persona_id",
                table: "PERSONA_ENFERMEDAD",
                column: "censo_persona_id");

            migrationBuilder.CreateIndex(
                name: "IX_PERSONA_ENFERMEDAD_enfermedad_id",
                table: "PERSONA_ENFERMEDAD",
                column: "enfermedad_id");

            migrationBuilder.CreateIndex(
                name: "IX_PERSONA_ENFERMEDAD_estado_id",
                table: "PERSONA_ENFERMEDAD",
                column: "estado_id");

            migrationBuilder.AddForeignKey(
                name: "FK_EVENTO_EVENTO_TIPO_tipo_id",
                table: "EVENTO",
                column: "tipo_id",
                principalTable: "EVENTO_TIPO",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_EVENTO_PACIENTE_paciente_id",
                table: "EVENTO",
                column: "paciente_id",
                principalTable: "PACIENTE",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EVENTO_EVENTO_TIPO_tipo_id",
                table: "EVENTO");

            migrationBuilder.DropForeignKey(
                name: "FK_EVENTO_PACIENTE_paciente_id",
                table: "EVENTO");

            migrationBuilder.DropTable(
                name: "ETNIA_PUEBLO_INDIGENA");

            migrationBuilder.DropTable(
                name: "EVENTO_ESCABIOSIS");

            migrationBuilder.DropTable(
                name: "EVENTO_GEOHELMINTIASIS");

            migrationBuilder.DropTable(
                name: "EVENTO_LEISHMANIASIS_CUTANEA");

            migrationBuilder.DropTable(
                name: "EVENTO_MALARIA");

            migrationBuilder.DropTable(
                name: "EVENTO_PEDICULOSIS");

            migrationBuilder.DropTable(
                name: "EVENTO_PIAN");

            migrationBuilder.DropTable(
                name: "EVENTO_TENIASIS_CISTICERCOSIS");

            migrationBuilder.DropTable(
                name: "EVENTO_TRACOMA");

            migrationBuilder.DropTable(
                name: "EVENTO_TUBERCULOSIS");

            migrationBuilder.DropTable(
                name: "EVENTO_TUBERCULOSIS_CONTACTO");

            migrationBuilder.DropTable(
                name: "EVENTO_TUNGIASIS");

            migrationBuilder.DropTable(
                name: "PERSONA_ENFERMEDAD");

            migrationBuilder.DropTable(
                name: "CENSO_PERSONA");

            migrationBuilder.DropTable(
                name: "ENFERMEDAD");

            migrationBuilder.DropTable(
                name: "ESTADO_PERSONA");

            migrationBuilder.DropColumn(
                name: "tipo",
                table: "TIPO_DOCUMENTO");

            migrationBuilder.RenameColumn(
                name: "tipo_id",
                table: "EVENTO",
                newName: "evento_tipo_id");

            migrationBuilder.RenameColumn(
                name: "fecha_evento",
                table: "EVENTO",
                newName: "fecha");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "EVENTO",
                newName: "evento_id");

            migrationBuilder.RenameIndex(
                name: "IX_EVENTO_tipo_id",
                table: "EVENTO",
                newName: "IX_EVENTO_evento_tipo_id");

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                table: "TIPO_DOCUMENTO",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(60)",
                oldUnicode: false,
                oldMaxLength: 60);

            migrationBuilder.AddColumn<string>(
                name: "observacion",
                table: "EVENTO",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "paciente_id",
                table: "CENSO",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ESCABIOSIS",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    cierre_control = table.Column<bool>(type: "bit", nullable: true),
                    tratado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESCABIOSIS", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_ESCABIOSIS_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "evento_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GEOHELMINTIASIS",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    cierre_control = table.Column<bool>(type: "bit", nullable: true),
                    laboratorio = table.Column<bool>(type: "bit", nullable: true),
                    tratado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GEOHELMINTIASIS", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_GEOHELMINTIASIS_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "evento_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LESHMANIASIS_CUTANEA",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    cicatriz = table.Column<bool>(type: "bit", nullable: true),
                    numero_lesiones = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LESHMANIASIS_CUTANEA", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_LESHMANIASIS_CUTANEA_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "evento_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MALARIA",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    gota = table.Column<bool>(type: "bit", nullable: true),
                    resultado = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MALARIA", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_MALARIA_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "evento_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PEDICULOSIS",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    cierre_control = table.Column<bool>(type: "bit", nullable: true),
                    tratado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PEDICULOSIS", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_PEDICULOSIS_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "evento_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TUBERCULOSIS",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    resultado = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    sintomatico = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TUBERCULOSIS", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_TUBERCULOSIS_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "evento_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TUBERCULOSIS_CONTACTO",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    index_id = table.Column<long>(type: "bigint", nullable: true),
                    parentesco_id = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TUBERCULOSIS_CONTACTO", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_TUBERCULOSIS_CONTACTO_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "evento_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TUBERCULOSIS_CONTACTO_PACIENTE_index_id",
                        column: x => x.index_id,
                        principalTable: "PACIENTE",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TUBERCULOSIS_CONTACTO_PARENTESCO_parentesco_id",
                        column: x => x.parentesco_id,
                        principalTable: "PARENTESCO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_paciente_id",
                table: "CENSO",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "IX_TUBERCULOSIS_CONTACTO_index_id",
                table: "TUBERCULOSIS_CONTACTO",
                column: "index_id");

            migrationBuilder.CreateIndex(
                name: "IX_TUBERCULOSIS_CONTACTO_parentesco_id",
                table: "TUBERCULOSIS_CONTACTO",
                column: "parentesco_id");

            migrationBuilder.AddForeignKey(
                name: "FK_CENSO_PACIENTE_paciente_id",
                table: "CENSO",
                column: "paciente_id",
                principalTable: "PACIENTE",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_EVENTO_EVENTO_TIPO_evento_tipo_id",
                table: "EVENTO",
                column: "evento_tipo_id",
                principalTable: "EVENTO_TIPO",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_EVENTO_PACIENTE_paciente_id",
                table: "EVENTO",
                column: "paciente_id",
                principalTable: "PACIENTE",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
