using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Customer.API.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "geodata",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_geodata", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "identityrole",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identityrole", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "openiddictentityframeworkcoreapplication",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    client_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    client_secret = table.Column<string>(type: "text", nullable: true),
                    concurrency_token = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    consent_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    display_name = table.Column<string>(type: "text", nullable: true),
                    display_names = table.Column<string>(type: "text", nullable: true),
                    permissions = table.Column<string>(type: "text", nullable: true),
                    post_logout_redirect_uris = table.Column<string>(type: "text", nullable: true),
                    properties = table.Column<string>(type: "text", nullable: true),
                    redirect_uris = table.Column<string>(type: "text", nullable: true),
                    requirements = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_openiddictentityframeworkcoreapplication", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "openiddictentityframeworkcorescope",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    concurrency_token = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    descriptions = table.Column<string>(type: "text", nullable: true),
                    display_name = table.Column<string>(type: "text", nullable: true),
                    display_names = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    properties = table.Column<string>(type: "text", nullable: true),
                    resources = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_openiddictentityframeworkcorescope", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "address",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    country = table.Column<string>(type: "text", nullable: false),
                    state = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    near_by_area = table.Column<string>(type: "text", nullable: false),
                    house_shop_plot_no = table.Column<string>(type: "text", nullable: false),
                    addressline1 = table.Column<string>(type: "text", nullable: false),
                    geo_data_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_address", x => x.id);
                    table.ForeignKey(
                        name: "fk_address_geo_data_geo_data_id",
                        column: x => x.geo_data_id,
                        principalSchema: "public",
                        principalTable: "geodata",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "identityroleclaim<string>",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identityroleclaim_string", x => x.id);
                    table.ForeignKey(
                        name: "fk_identityroleclaim_string_identityrole_role_id",
                        column: x => x.role_id,
                        principalSchema: "public",
                        principalTable: "identityrole",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "openiddictentityframeworkcoreauthorization",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    application_id = table.Column<string>(type: "text", nullable: true),
                    concurrency_token = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    creation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    properties = table.Column<string>(type: "text", nullable: true),
                    scopes = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    subject = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_openiddictentityframeworkcoreauthorization", x => x.id);
                    table.ForeignKey(
                        name: "fk_openiddictentityframeworkcoreauthorization_openiddictentity",
                        column: x => x.application_id,
                        principalSchema: "public",
                        principalTable: "openiddictentityframeworkcoreapplication",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "applicationuser",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    user_status = table.Column<int>(type: "integer", nullable: false),
                    order_type = table.Column<int>(type: "integer", nullable: false),
                    address_id = table.Column<int>(type: "integer", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    added_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_applicationuser", x => x.id);
                    table.ForeignKey(
                        name: "fk_applicationuser_address_address_id",
                        column: x => x.address_id,
                        principalSchema: "public",
                        principalTable: "address",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "openiddictentityframeworkcoretoken",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    application_id = table.Column<string>(type: "text", nullable: true),
                    authorization_id = table.Column<string>(type: "text", nullable: true),
                    concurrency_token = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    creation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    expiration_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    payload = table.Column<string>(type: "text", nullable: true),
                    properties = table.Column<string>(type: "text", nullable: true),
                    redemption_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reference_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    subject = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_openiddictentityframeworkcoretoken", x => x.id);
                    table.ForeignKey(
                        name: "fk_openiddictentityframeworkcoretoken_openiddictentityframewor",
                        column: x => x.application_id,
                        principalSchema: "public",
                        principalTable: "openiddictentityframeworkcoreapplication",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_openiddictentityframeworkcoretoken_openiddictentityframewor1",
                        column: x => x.authorization_id,
                        principalSchema: "public",
                        principalTable: "openiddictentityframeworkcoreauthorization",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "identityuserclaim<string>",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identityuserclaim_string", x => x.id);
                    table.ForeignKey(
                        name: "fk_identityuserclaim_string_applicationuser_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "applicationuser",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identityuserlogin<string>",
                schema: "public",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    provider_key = table.Column<string>(type: "text", nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identityuserlogin_string", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_identityuserlogin_string_applicationuser_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "applicationuser",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identityuserrole<string>",
                schema: "public",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identityuserrole_string", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_identityuserrole_string_applicationuser_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "applicationuser",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_identityuserrole_string_identityrole_role_id",
                        column: x => x.role_id,
                        principalSchema: "public",
                        principalTable: "identityrole",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identityusertoken<string>",
                schema: "public",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identityusertoken_string", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_identityusertoken_string_applicationuser_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "applicationuser",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_address_geo_data_id",
                schema: "public",
                table: "address",
                column: "geo_data_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "public",
                table: "applicationuser",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "ix_applicationuser_address_id",
                schema: "public",
                table: "applicationuser",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "public",
                table: "applicationuser",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "public",
                table: "identityrole",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_identityroleclaim_string_role_id",
                schema: "public",
                table: "identityroleclaim<string>",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_identityuserclaim_string_user_id",
                schema: "public",
                table: "identityuserclaim<string>",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_identityuserlogin_string_user_id",
                schema: "public",
                table: "identityuserlogin<string>",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_identityuserrole_string_role_id",
                schema: "public",
                table: "identityuserrole<string>",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_openiddictentityframeworkcoreapplication_client_id",
                schema: "public",
                table: "openiddictentityframeworkcoreapplication",
                column: "client_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_openiddictentityframeworkcoreauthorization_application_id_s",
                schema: "public",
                table: "openiddictentityframeworkcoreauthorization",
                columns: new[] { "application_id", "status", "subject", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_openiddictentityframeworkcorescope_name",
                schema: "public",
                table: "openiddictentityframeworkcorescope",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_openiddictentityframeworkcoretoken_application_id_status_su",
                schema: "public",
                table: "openiddictentityframeworkcoretoken",
                columns: new[] { "application_id", "status", "subject", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_openiddictentityframeworkcoretoken_authorization_id",
                schema: "public",
                table: "openiddictentityframeworkcoretoken",
                column: "authorization_id");

            migrationBuilder.CreateIndex(
                name: "ix_openiddictentityframeworkcoretoken_reference_id",
                schema: "public",
                table: "openiddictentityframeworkcoretoken",
                column: "reference_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "identityroleclaim<string>",
                schema: "public");

            migrationBuilder.DropTable(
                name: "identityuserclaim<string>",
                schema: "public");

            migrationBuilder.DropTable(
                name: "identityuserlogin<string>",
                schema: "public");

            migrationBuilder.DropTable(
                name: "identityuserrole<string>",
                schema: "public");

            migrationBuilder.DropTable(
                name: "identityusertoken<string>",
                schema: "public");

            migrationBuilder.DropTable(
                name: "openiddictentityframeworkcorescope",
                schema: "public");

            migrationBuilder.DropTable(
                name: "openiddictentityframeworkcoretoken",
                schema: "public");

            migrationBuilder.DropTable(
                name: "identityrole",
                schema: "public");

            migrationBuilder.DropTable(
                name: "applicationuser",
                schema: "public");

            migrationBuilder.DropTable(
                name: "openiddictentityframeworkcoreauthorization",
                schema: "public");

            migrationBuilder.DropTable(
                name: "address",
                schema: "public");

            migrationBuilder.DropTable(
                name: "openiddictentityframeworkcoreapplication",
                schema: "public");

            migrationBuilder.DropTable(
                name: "geodata",
                schema: "public");
        }
    }
}
