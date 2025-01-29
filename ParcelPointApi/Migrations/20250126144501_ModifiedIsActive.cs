using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPointApi.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACTIVITY_LOGS",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    action_title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    action_context = table.Column<string>(type: "text", nullable: true),
                    module = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    sub_module = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ACTIVITY__3213E83F5766BD23", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GENDER",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, collation: "Latin1_General_CI_AS"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GENDER__3213E83FC97D2F73", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "INCOMING_PARCEL",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__INCOMING__3213E83FE8056C96", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PARCEL_LOGS",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    parcel_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    parcel_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true, collation: "Latin1_General_CI_AS"),
                    locker_number = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, collation: "Latin1_General_CI_AS"),
                    status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, collation: "Latin1_General_CI_AS"),
                    action = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, collation: "Latin1_General_CI_AS"),
                    arrived_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PARCEL_L__3213E83FE2F71526", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ROLES",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, collation: "Latin1_General_CI_AS"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ROLES__3213E83F0D62D1D5", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "USER_RELATIONSHIP",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, collation: "Latin1_General_CI_AS"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__USER_REL__3213E83F1E571D01", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "USERBIO_FP",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    fingerprint_data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    fingerprint_key = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true, collation: "Latin1_General_CI_AS"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__USERBIO___3213E83FB0A7F282", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    username = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, collation: "Latin1_General_CS_AS"),
                    password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, collation: "Latin1_General_CI_AS"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__USERS__3213E83FCEE53FC0", x => x.id);
                    table.ForeignKey(
                        name: "FK__USERS__role_id__5AEE82B9",
                        column: x => x.role_id,
                        principalTable: "ROLES",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "USER_GROUP",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    owner_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__USER_GRO__3213E83FD34E93E9", x => x.id);
                    table.ForeignKey(
                        name: "FK__USER_GROU__owner__6754599E",
                        column: x => x.owner_id,
                        principalTable: "USERS",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "USER_INFORMATION",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    first_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, collation: "Latin1_General_CI_AS"),
                    middle_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, collation: "Latin1_General_CI_AS"),
                    last_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, collation: "Latin1_General_CI_AS"),
                    suffix = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true, collation: "Latin1_General_CI_AS"),
                    birthdate = table.Column<DateOnly>(type: "date", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    contact_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true, collation: "Latin1_General_CI_AS"),
                    email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true, collation: "Latin1_General_CI_AS"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    gender_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__USER_INF__3213E83F826E9EAD", x => x.id);
                    table.ForeignKey(
                        name: "FK__USER_INFO__gende__60A75C0F",
                        column: x => x.gender_id,
                        principalTable: "GENDER",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__USER_INFO__user___619B8048",
                        column: x => x.user_id,
                        principalTable: "USERS",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "USER_LOGS",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    action = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, collation: "Latin1_General_CI_AS"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__USER_LOG__3213E83FA86CEBDC", x => x.id);
                    table.ForeignKey(
                        name: "FK__USER_LOGS__user___74AE54BC",
                        column: x => x.user_id,
                        principalTable: "USERS",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "USER_GROUP_MEMBERS",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    member_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    group_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    relationship_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    is_authorized = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__USER_GRO__3213E83FCEC9D398", x => x.id);
                    table.ForeignKey(
                        name: "FK__USER_GROU__group__6EF57B66",
                        column: x => x.group_id,
                        principalTable: "USER_GROUP",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__USER_GROU__membe__6E01572D",
                        column: x => x.member_id,
                        principalTable: "USERS",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__USER_GROU__relat__6FE99F9F",
                        column: x => x.relationship_id,
                        principalTable: "USER_RELATIONSHIP",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_USER_GROUP_owner_id",
                table: "USER_GROUP",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_USER_GROUP_MEMBERS_group_id",
                table: "USER_GROUP_MEMBERS",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_USER_GROUP_MEMBERS_member_id",
                table: "USER_GROUP_MEMBERS",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_USER_GROUP_MEMBERS_relationship_id",
                table: "USER_GROUP_MEMBERS",
                column: "relationship_id");

            migrationBuilder.CreateIndex(
                name: "IX_USER_INFORMATION_gender_id",
                table: "USER_INFORMATION",
                column: "gender_id");

            migrationBuilder.CreateIndex(
                name: "IX_USER_INFORMATION_user_id",
                table: "USER_INFORMATION",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_USER_LOGS_user_id",
                table: "USER_LOGS",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_role_id",
                table: "USERS",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "UQ__USERS__F3DBC57296B360BE",
                table: "USERS",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACTIVITY_LOGS");

            migrationBuilder.DropTable(
                name: "INCOMING_PARCEL");

            migrationBuilder.DropTable(
                name: "PARCEL_LOGS");

            migrationBuilder.DropTable(
                name: "USER_GROUP_MEMBERS");

            migrationBuilder.DropTable(
                name: "USER_INFORMATION");

            migrationBuilder.DropTable(
                name: "USER_LOGS");

            migrationBuilder.DropTable(
                name: "USERBIO_FP");

            migrationBuilder.DropTable(
                name: "USER_GROUP");

            migrationBuilder.DropTable(
                name: "USER_RELATIONSHIP");

            migrationBuilder.DropTable(
                name: "GENDER");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "ROLES");
        }
    }
}
