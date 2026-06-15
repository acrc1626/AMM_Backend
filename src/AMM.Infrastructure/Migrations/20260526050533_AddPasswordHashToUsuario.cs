using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordHashToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DEPARTAMENTO",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigoDANE = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    departamento = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEPARTAMENTO", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ESTADO",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false),
                    nombre = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESTADO", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ESTADO_USUARIO",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false),
                    nombre = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESTADO_USUARIO", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ETNIA",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    codigo = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETNIA", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "EVENTO_TIPO",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false),
                    codigo = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    nombre = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTO_TIPO", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FORMA_FARMACEUTICA",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORMA_FARMACEUTICA", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MENU",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false),
                    ruta = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENU", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MOTIVO_NO_TRATAMIENTO",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descripcion = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTIVO_NO_TRATAMIENTO", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PARENTESCO",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PARENTESCO", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PERMISO",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERMISO", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PRESENCIA_NOVEDAD",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRESENCIA_NOVEDAD", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PUEBLO_INDIGENA",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descripcion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PUEBLO_INDIGENA", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ROL",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROL", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SEXO",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEXO", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TIPO_DOCUMENTO",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPO_DOCUMENTO", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TIPO_ENTORNO",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPO_ENTORNO", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TIPO_NOVEDAD",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPO_NOVEDAD", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TIPO_SUPERVISION",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPO_SUPERVISION", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UBICACION",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    lat = table.Column<double>(type: "float", nullable: true),
                    lon = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UBICACION", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MUNICIPIO",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    departamento_id = table.Column<short>(type: "smallint", nullable: false),
                    codigoDANE_Dpto = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    municipio = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MUNICIPIO", x => x.id);
                    table.ForeignKey(
                        name: "FK_MUNICIPIO_DEPARTAMENTO_departamento_id",
                        column: x => x.departamento_id,
                        principalTable: "DEPARTAMENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    azure_ad_object_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    correo = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false),
                    nombre_completo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    password_hash = table.Column<string>(type: "varchar(512)", unicode: false, maxLength: 512, nullable: true),
                    entidad_id = table.Column<int>(type: "int", nullable: true),
                    estado_usuario_id = table.Column<byte>(type: "tinyint", nullable: false),
                    creado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "sysutcdatetime()"),
                    creado_por = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    modificado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.id);
                    table.ForeignKey(
                        name: "FK_USUARIO_ESTADO_USUARIO_estado_usuario_id",
                        column: x => x.estado_usuario_id,
                        principalTable: "ESTADO_USUARIO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "MEDICAMENTO",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_generico = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    forma_farmaceutica_id = table.Column<short>(type: "smallint", nullable: false),
                    concentracion = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    codigo_cum = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDICAMENTO", x => x.id);
                    table.ForeignKey(
                        name: "FK_MEDICAMENTO_FORMA_FARMACEUTICA_forma_farmaceutica_id",
                        column: x => x.forma_farmaceutica_id,
                        principalTable: "FORMA_FARMACEUTICA",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ROL_MENU",
                columns: table => new
                {
                    rol_id = table.Column<int>(type: "int", nullable: false),
                    menu_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROL_MENU", x => new { x.rol_id, x.menu_id });
                    table.ForeignKey(
                        name: "FK_ROL_MENU_MENU_menu_id",
                        column: x => x.menu_id,
                        principalTable: "MENU",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ROL_MENU_ROL_rol_id",
                        column: x => x.rol_id,
                        principalTable: "ROL",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ROL_PERMISO",
                columns: table => new
                {
                    rol_id = table.Column<int>(type: "int", nullable: false),
                    permiso_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROL_PERMISO", x => new { x.rol_id, x.permiso_id });
                    table.ForeignKey(
                        name: "FK_ROL_PERMISO_PERMISO_permiso_id",
                        column: x => x.permiso_id,
                        principalTable: "PERMISO",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ROL_PERMISO_ROL_rol_id",
                        column: x => x.rol_id,
                        principalTable: "ROL",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PACIENTE",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo_documento_id = table.Column<byte>(type: "tinyint", nullable: false),
                    documento = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    primer_nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    segundo_nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    primer_apellido = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    segundo_apellido = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    sexo_id = table.Column<byte>(type: "tinyint", nullable: false),
                    fecha_nacimiento = table.Column<DateTime>(type: "date", nullable: true),
                    pertenencia_etnica_id = table.Column<byte>(type: "tinyint", nullable: true),
                    pueblo_indigena_id = table.Column<short>(type: "smallint", nullable: true),
                    creado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "sysutcdatetime()"),
                    creado_por = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    modificado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    estado_id = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PACIENTE", x => x.id);
                    table.ForeignKey(
                        name: "FK_PACIENTE_ESTADO_estado_id",
                        column: x => x.estado_id,
                        principalTable: "ESTADO",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PACIENTE_ETNIA_pertenencia_etnica_id",
                        column: x => x.pertenencia_etnica_id,
                        principalTable: "ETNIA",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PACIENTE_PUEBLO_INDIGENA_pueblo_indigena_id",
                        column: x => x.pueblo_indigena_id,
                        principalTable: "PUEBLO_INDIGENA",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PACIENTE_SEXO_sexo_id",
                        column: x => x.sexo_id,
                        principalTable: "SEXO",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PACIENTE_TIPO_DOCUMENTO_tipo_documento_id",
                        column: x => x.tipo_documento_id,
                        principalTable: "TIPO_DOCUMENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TERRITORIO",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    municipio_id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TERRITORIO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TERRITORIO_MUNICIPIO_municipio_id",
                        column: x => x.municipio_id,
                        principalTable: "MUNICIPIO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "USUARIO_ROL",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    rol_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO_ROL", x => new { x.usuario_id, x.rol_id });
                    table.ForeignKey(
                        name: "FK_USUARIO_ROL_ROL_rol_id",
                        column: x => x.rol_id,
                        principalTable: "ROL",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_USUARIO_ROL_USUARIO_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "USUARIO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CENSO",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo_entorno_id = table.Column<byte>(type: "tinyint", nullable: false),
                    paciente_id = table.Column<long>(type: "bigint", nullable: true),
                    ubicacion_id = table.Column<long>(type: "bigint", nullable: true),
                    departamento_id = table.Column<short>(type: "smallint", nullable: true),
                    municipio_id = table.Column<int>(type: "int", nullable: true),
                    fecha = table.Column<DateTime>(type: "date", nullable: false),
                    observacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    creado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "sysutcdatetime()"),
                    creado_por = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    modificado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    estado_id = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CENSO", x => x.id);
                    table.ForeignKey(
                        name: "FK_CENSO_DEPARTAMENTO_departamento_id",
                        column: x => x.departamento_id,
                        principalTable: "DEPARTAMENTO",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CENSO_ESTADO_estado_id",
                        column: x => x.estado_id,
                        principalTable: "ESTADO",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CENSO_MUNICIPIO_municipio_id",
                        column: x => x.municipio_id,
                        principalTable: "MUNICIPIO",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CENSO_PACIENTE_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "PACIENTE",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CENSO_TIPO_ENTORNO_tipo_entorno_id",
                        column: x => x.tipo_entorno_id,
                        principalTable: "TIPO_ENTORNO",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CENSO_UBICACION_ubicacion_id",
                        column: x => x.ubicacion_id,
                        principalTable: "UBICACION",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "EVENTO",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    paciente_id = table.Column<long>(type: "bigint", nullable: false),
                    evento_tipo_id = table.Column<byte>(type: "tinyint", nullable: false),
                    fecha = table.Column<DateTime>(type: "date", nullable: false),
                    observacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    creado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "sysutcdatetime()"),
                    creado_por = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    modificado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    estado_id = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTO", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_EVENTO_ESTADO_estado_id",
                        column: x => x.estado_id,
                        principalTable: "ESTADO",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_EVENTO_EVENTO_TIPO_evento_tipo_id",
                        column: x => x.evento_tipo_id,
                        principalTable: "EVENTO_TIPO",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_EVENTO_PACIENTE_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "PACIENTE",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MICROTERRITORIO",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    territorio_id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MICROTERRITORIO", x => x.id);
                    table.ForeignKey(
                        name: "FK_MICROTERRITORIO_TERRITORIO_territorio_id",
                        column: x => x.territorio_id,
                        principalTable: "TERRITORIO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ESCABIOSIS",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    tratado = table.Column<bool>(type: "bit", nullable: true),
                    cierre_control = table.Column<bool>(type: "bit", nullable: true)
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
                    tratado = table.Column<bool>(type: "bit", nullable: true),
                    cierre_control = table.Column<bool>(type: "bit", nullable: true),
                    laboratorio = table.Column<bool>(type: "bit", nullable: true)
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
                    tratado = table.Column<bool>(type: "bit", nullable: true),
                    cierre_control = table.Column<bool>(type: "bit", nullable: true)
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
                name: "TRATAMIENTO",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    supervisado_por_id = table.Column<int>(type: "int", nullable: true),
                    requiere_tratamiento = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    motivo_no_tratamiento_id = table.Column<short>(type: "smallint", nullable: true),
                    observacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    fecha = table.Column<DateTime>(type: "date", nullable: false),
                    creado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "sysutcdatetime()"),
                    creado_por = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    modificado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    estado_id = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRATAMIENTO", x => x.id);
                    table.ForeignKey(
                        name: "FK_TRATAMIENTO_ESTADO_estado_id",
                        column: x => x.estado_id,
                        principalTable: "ESTADO",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TRATAMIENTO_EVENTO_evento_id",
                        column: x => x.evento_id,
                        principalTable: "EVENTO",
                        principalColumn: "evento_id");
                    table.ForeignKey(
                        name: "FK_TRATAMIENTO_MOTIVO_NO_TRATAMIENTO_motivo_no_tratamiento_id",
                        column: x => x.motivo_no_tratamiento_id,
                        principalTable: "MOTIVO_NO_TRATAMIENTO",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TRATAMIENTO_USUARIO_supervisado_por_id",
                        column: x => x.supervisado_por_id,
                        principalTable: "USUARIO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TUBERCULOSIS",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    sintomatico = table.Column<bool>(type: "bit", nullable: true),
                    resultado = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "AREA",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    microterritorio_id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AREA", x => x.id);
                    table.ForeignKey(
                        name: "FK_AREA_MICROTERRITORIO_microterritorio_id",
                        column: x => x.microterritorio_id,
                        principalTable: "MICROTERRITORIO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CENSO_NOVEDAD",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    censo_id = table.Column<long>(type: "bigint", nullable: false),
                    paciente_id = table.Column<long>(type: "bigint", nullable: true),
                    tipo_novedad_id = table.Column<byte>(type: "tinyint", nullable: false),
                    presencia_novedad_id = table.Column<byte>(type: "tinyint", nullable: true),
                    tratamiento_id = table.Column<long>(type: "bigint", nullable: true),
                    observacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    fecha = table.Column<DateTime>(type: "date", nullable: false),
                    creado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "sysutcdatetime()"),
                    creado_por = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    modificado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    estado_id = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CENSO_NOVEDAD", x => x.id);
                    table.ForeignKey(
                        name: "FK_CENSO_NOVEDAD_CENSO_censo_id",
                        column: x => x.censo_id,
                        principalTable: "CENSO",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CENSO_NOVEDAD_ESTADO_estado_id",
                        column: x => x.estado_id,
                        principalTable: "ESTADO",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CENSO_NOVEDAD_PACIENTE_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "PACIENTE",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CENSO_NOVEDAD_PRESENCIA_NOVEDAD_presencia_novedad_id",
                        column: x => x.presencia_novedad_id,
                        principalTable: "PRESENCIA_NOVEDAD",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CENSO_NOVEDAD_TIPO_NOVEDAD_tipo_novedad_id",
                        column: x => x.tipo_novedad_id,
                        principalTable: "TIPO_NOVEDAD",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CENSO_NOVEDAD_TRATAMIENTO_tratamiento_id",
                        column: x => x.tratamiento_id,
                        principalTable: "TRATAMIENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PRM",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tratamiento_id = table.Column<long>(type: "bigint", nullable: false),
                    paciente_id = table.Column<long>(type: "bigint", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    severidad = table.Column<byte>(type: "tinyint", nullable: true),
                    creado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "sysutcdatetime()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRM", x => x.id);
                    table.ForeignKey(
                        name: "FK_PRM_PACIENTE_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "PACIENTE",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PRM_TRATAMIENTO_tratamiento_id",
                        column: x => x.tratamiento_id,
                        principalTable: "TRATAMIENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "RAFA",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tratamiento_id = table.Column<long>(type: "bigint", nullable: false),
                    paciente_id = table.Column<long>(type: "bigint", nullable: false),
                    fecha_evento = table.Column<DateTime>(type: "date", nullable: false),
                    severidad = table.Column<byte>(type: "tinyint", nullable: true),
                    sintomas = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    enlace_reporte_invima = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    creado_en = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "sysutcdatetime()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAFA", x => x.id);
                    table.ForeignKey(
                        name: "FK_RAFA_PACIENTE_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "PACIENTE",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_RAFA_TRATAMIENTO_tratamiento_id",
                        column: x => x.tratamiento_id,
                        principalTable: "TRATAMIENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TRATAMIENTO_DETALLE",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tratamiento_id = table.Column<long>(type: "bigint", nullable: false),
                    medicamento_id = table.Column<int>(type: "int", nullable: false),
                    dosis = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    via = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    forma_farmaceutica_id = table.Column<short>(type: "smallint", nullable: true),
                    supervision_id = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRATAMIENTO_DETALLE", x => x.id);
                    table.ForeignKey(
                        name: "FK_TRATAMIENTO_DETALLE_FORMA_FARMACEUTICA_forma_farmaceutica_id",
                        column: x => x.forma_farmaceutica_id,
                        principalTable: "FORMA_FARMACEUTICA",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TRATAMIENTO_DETALLE_MEDICAMENTO_medicamento_id",
                        column: x => x.medicamento_id,
                        principalTable: "MEDICAMENTO",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TRATAMIENTO_DETALLE_TIPO_SUPERVISION_supervision_id",
                        column: x => x.supervision_id,
                        principalTable: "TIPO_SUPERVISION",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TRATAMIENTO_DETALLE_TRATAMIENTO_tratamiento_id",
                        column: x => x.tratamiento_id,
                        principalTable: "TRATAMIENTO",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AREA_microterritorio_id",
                table: "AREA",
                column: "microterritorio_id");

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_departamento_id",
                table: "CENSO",
                column: "departamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_estado_id",
                table: "CENSO",
                column: "estado_id");

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_municipio_id",
                table: "CENSO",
                column: "municipio_id");

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_paciente_id",
                table: "CENSO",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_tipo_entorno_id",
                table: "CENSO",
                column: "tipo_entorno_id");

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_ubicacion_id",
                table: "CENSO",
                column: "ubicacion_id");

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_NOVEDAD_censo_id",
                table: "CENSO_NOVEDAD",
                column: "censo_id");

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_NOVEDAD_estado_id",
                table: "CENSO_NOVEDAD",
                column: "estado_id");

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_NOVEDAD_paciente_id",
                table: "CENSO_NOVEDAD",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_NOVEDAD_presencia_novedad_id",
                table: "CENSO_NOVEDAD",
                column: "presencia_novedad_id");

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_NOVEDAD_tipo_novedad_id",
                table: "CENSO_NOVEDAD",
                column: "tipo_novedad_id");

            migrationBuilder.CreateIndex(
                name: "IX_CENSO_NOVEDAD_tratamiento_id",
                table: "CENSO_NOVEDAD",
                column: "tratamiento_id");

            migrationBuilder.CreateIndex(
                name: "IX_EVENTO_estado_id",
                table: "EVENTO",
                column: "estado_id");

            migrationBuilder.CreateIndex(
                name: "IX_EVENTO_evento_tipo_id",
                table: "EVENTO",
                column: "evento_tipo_id");

            migrationBuilder.CreateIndex(
                name: "IX_EVENTO_paciente_id",
                table: "EVENTO",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "IX_MEDICAMENTO_forma_farmaceutica_id",
                table: "MEDICAMENTO",
                column: "forma_farmaceutica_id");

            migrationBuilder.CreateIndex(
                name: "IX_MICROTERRITORIO_territorio_id",
                table: "MICROTERRITORIO",
                column: "territorio_id");

            migrationBuilder.CreateIndex(
                name: "IX_MUNICIPIO_departamento_id",
                table: "MUNICIPIO",
                column: "departamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_PACIENTE_estado_id",
                table: "PACIENTE",
                column: "estado_id");

            migrationBuilder.CreateIndex(
                name: "IX_PACIENTE_pertenencia_etnica_id",
                table: "PACIENTE",
                column: "pertenencia_etnica_id");

            migrationBuilder.CreateIndex(
                name: "IX_PACIENTE_pueblo_indigena_id",
                table: "PACIENTE",
                column: "pueblo_indigena_id");

            migrationBuilder.CreateIndex(
                name: "IX_PACIENTE_sexo_id",
                table: "PACIENTE",
                column: "sexo_id");

            migrationBuilder.CreateIndex(
                name: "IX_PACIENTE_tipo_documento_id",
                table: "PACIENTE",
                column: "tipo_documento_id");

            migrationBuilder.CreateIndex(
                name: "IX_PRM_paciente_id",
                table: "PRM",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "IX_PRM_tratamiento_id",
                table: "PRM",
                column: "tratamiento_id");

            migrationBuilder.CreateIndex(
                name: "IX_RAFA_paciente_id",
                table: "RAFA",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "IX_RAFA_tratamiento_id",
                table: "RAFA",
                column: "tratamiento_id");

            migrationBuilder.CreateIndex(
                name: "IX_ROL_MENU_menu_id",
                table: "ROL_MENU",
                column: "menu_id");

            migrationBuilder.CreateIndex(
                name: "IX_ROL_PERMISO_permiso_id",
                table: "ROL_PERMISO",
                column: "permiso_id");

            migrationBuilder.CreateIndex(
                name: "IX_TERRITORIO_municipio_id",
                table: "TERRITORIO",
                column: "municipio_id");

            migrationBuilder.CreateIndex(
                name: "IX_TRATAMIENTO_estado_id",
                table: "TRATAMIENTO",
                column: "estado_id");

            migrationBuilder.CreateIndex(
                name: "IX_TRATAMIENTO_evento_id",
                table: "TRATAMIENTO",
                column: "evento_id");

            migrationBuilder.CreateIndex(
                name: "IX_TRATAMIENTO_motivo_no_tratamiento_id",
                table: "TRATAMIENTO",
                column: "motivo_no_tratamiento_id");

            migrationBuilder.CreateIndex(
                name: "IX_TRATAMIENTO_supervisado_por_id",
                table: "TRATAMIENTO",
                column: "supervisado_por_id");

            migrationBuilder.CreateIndex(
                name: "IX_TRATAMIENTO_DETALLE_forma_farmaceutica_id",
                table: "TRATAMIENTO_DETALLE",
                column: "forma_farmaceutica_id");

            migrationBuilder.CreateIndex(
                name: "IX_TRATAMIENTO_DETALLE_medicamento_id",
                table: "TRATAMIENTO_DETALLE",
                column: "medicamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_TRATAMIENTO_DETALLE_supervision_id",
                table: "TRATAMIENTO_DETALLE",
                column: "supervision_id");

            migrationBuilder.CreateIndex(
                name: "IX_TRATAMIENTO_DETALLE_tratamiento_id",
                table: "TRATAMIENTO_DETALLE",
                column: "tratamiento_id");

            migrationBuilder.CreateIndex(
                name: "IX_TUBERCULOSIS_CONTACTO_index_id",
                table: "TUBERCULOSIS_CONTACTO",
                column: "index_id");

            migrationBuilder.CreateIndex(
                name: "IX_TUBERCULOSIS_CONTACTO_parentesco_id",
                table: "TUBERCULOSIS_CONTACTO",
                column: "parentesco_id");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_estado_usuario_id",
                table: "USUARIO",
                column: "estado_usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_ROL_rol_id",
                table: "USUARIO_ROL",
                column: "rol_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AREA");

            migrationBuilder.DropTable(
                name: "CENSO_NOVEDAD");

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
                name: "PRM");

            migrationBuilder.DropTable(
                name: "RAFA");

            migrationBuilder.DropTable(
                name: "ROL_MENU");

            migrationBuilder.DropTable(
                name: "ROL_PERMISO");

            migrationBuilder.DropTable(
                name: "TRATAMIENTO_DETALLE");

            migrationBuilder.DropTable(
                name: "TUBERCULOSIS");

            migrationBuilder.DropTable(
                name: "TUBERCULOSIS_CONTACTO");

            migrationBuilder.DropTable(
                name: "USUARIO_ROL");

            migrationBuilder.DropTable(
                name: "MICROTERRITORIO");

            migrationBuilder.DropTable(
                name: "CENSO");

            migrationBuilder.DropTable(
                name: "PRESENCIA_NOVEDAD");

            migrationBuilder.DropTable(
                name: "TIPO_NOVEDAD");

            migrationBuilder.DropTable(
                name: "MENU");

            migrationBuilder.DropTable(
                name: "PERMISO");

            migrationBuilder.DropTable(
                name: "MEDICAMENTO");

            migrationBuilder.DropTable(
                name: "TIPO_SUPERVISION");

            migrationBuilder.DropTable(
                name: "TRATAMIENTO");

            migrationBuilder.DropTable(
                name: "PARENTESCO");

            migrationBuilder.DropTable(
                name: "ROL");

            migrationBuilder.DropTable(
                name: "TERRITORIO");

            migrationBuilder.DropTable(
                name: "TIPO_ENTORNO");

            migrationBuilder.DropTable(
                name: "UBICACION");

            migrationBuilder.DropTable(
                name: "FORMA_FARMACEUTICA");

            migrationBuilder.DropTable(
                name: "EVENTO");

            migrationBuilder.DropTable(
                name: "MOTIVO_NO_TRATAMIENTO");

            migrationBuilder.DropTable(
                name: "USUARIO");

            migrationBuilder.DropTable(
                name: "MUNICIPIO");

            migrationBuilder.DropTable(
                name: "EVENTO_TIPO");

            migrationBuilder.DropTable(
                name: "PACIENTE");

            migrationBuilder.DropTable(
                name: "ESTADO_USUARIO");

            migrationBuilder.DropTable(
                name: "DEPARTAMENTO");

            migrationBuilder.DropTable(
                name: "ESTADO");

            migrationBuilder.DropTable(
                name: "ETNIA");

            migrationBuilder.DropTable(
                name: "PUEBLO_INDIGENA");

            migrationBuilder.DropTable(
                name: "SEXO");

            migrationBuilder.DropTable(
                name: "TIPO_DOCUMENTO");
        }
    }
}
