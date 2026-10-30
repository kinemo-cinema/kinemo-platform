using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace KinemoPlatform.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dashboards",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    generated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    period_start = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    period_end = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_dashboards", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "equipment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    equipment_type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    installation_date = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    useful_life_hours = table.Column<int>(type: "int", nullable: false),
                    accumulated_hours = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_equipment", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "maintenance_orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    equipment_id = table.Column<int>(type: "int", nullable: false),
                    incident_id = table.Column<int>(type: "int", nullable: true),
                    maintenance_type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    scheduled_date = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    completed_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    completion_notes = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_maintenance_orders", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "movies",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    duration_minutes = table.Column<int>(type: "int", nullable: false),
                    genre = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_movies", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "operational_reports",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    report_type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    generated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    period_start = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    period_end = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    export_format = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_operational_reports", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    capacity = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_rooms", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seat_allocations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    show_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_seat_allocations", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "show_executions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    show_id = table.Column<int>(type: "int", nullable: false),
                    sensory_content_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    finished_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_show_executions", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shows",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    movie_id = table.Column<int>(type: "int", nullable: false),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    end_datetime = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    start_datetime = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_shows", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "subscription_plans",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    plan_type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    max_screens = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_subscription_plans", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "subscriptions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cinema_id = table.Column<int>(type: "int", nullable: false),
                    plan_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    start_date = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    end_date = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_subscriptions", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ticketing_integrations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cinema_id = table.Column<int>(type: "int", nullable: false),
                    provider_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    last_connection_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_ticketing_integrations", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "incident_summaries",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    dashboard_id = table.Column<int>(type: "int", nullable: false),
                    total_incidents = table.Column<int>(type: "int", nullable: false),
                    low_count = table.Column<int>(type: "int", nullable: false),
                    medium_count = table.Column<int>(type: "int", nullable: false),
                    high_count = table.Column<int>(type: "int", nullable: false),
                    critical_count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_incident_summaries", x => x.id);
                    table.ForeignKey(
                        name: "fk_incident_sum_dashboards",
                        column: x => x.dashboard_id,
                        principalTable: "dashboards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "operational_metrics",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    dashboard_id = table.Column<int>(type: "int", nullable: false),
                    metric_type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    metric_value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    period_start = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    period_end = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_operational_metrics", x => x.id);
                    table.ForeignKey(
                        name: "fk_metrics_dashboards",
                        column: x => x.dashboard_id,
                        principalTable: "dashboards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "room_performances",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    dashboard_id = table.Column<int>(type: "int", nullable: false),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    operating_hours = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_shows = table.Column<int>(type: "int", nullable: false),
                    incident_count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_room_performances", x => x.id);
                    table.ForeignKey(
                        name: "fk_room_perf_dashboards",
                        column: x => x.dashboard_id,
                        principalTable: "dashboards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "equipment_tests",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    equipment_id = table.Column<int>(type: "int", nullable: false),
                    test_date = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    result = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    observations = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_equipment_tests", x => x.id);
                    table.ForeignKey(
                        name: "fk_equipment_tests_equipment",
                        column: x => x.equipment_id,
                        principalTable: "equipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "incidents",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    equipment_id = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    severity = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    reported_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    resolved_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_incidents", x => x.id);
                    table.ForeignKey(
                        name: "fk_incidents_equipment",
                        column: x => x.equipment_id,
                        principalTable: "equipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sensory_contents",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    movie_id = table.Column<int>(type: "int", nullable: false),
                    intensity_level = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sensory_contents", x => x.id);
                    table.ForeignKey(
                        name: "fk_sensory_contents_movies",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "room_blocks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    reason = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    start_date_time = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    end_date_time = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_room_blocks", x => x.id);
                    table.ForeignKey(
                        name: "fk_room_blocks_rooms",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "room_inspections",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    inspected_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    observations = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    result = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_room_inspections", x => x.id);
                    table.ForeignKey(
                        name: "fk_room_inspections_rooms",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "manual_interventions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seat_allocation_id = table.Column<int>(type: "int", nullable: false),
                    seat_number = table.Column<int>(type: "int", nullable: false),
                    operator_id = table.Column<int>(type: "int", nullable: false),
                    reason = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    intervention_date = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_manual_interventions", x => x.id);
                    table.ForeignKey(
                        name: "fk_manual_interventions_seat_allocations",
                        column: x => x.seat_allocation_id,
                        principalTable: "seat_allocations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "show_seats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seat_allocation_id = table.Column<int>(type: "int", nullable: false),
                    seat_number = table.Column<int>(type: "int", nullable: false),
                    sold = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_show_seats", x => x.id);
                    table.ForeignKey(
                        name: "fk_show_seats_seat_allocations",
                        column: x => x.seat_allocation_id,
                        principalTable: "seat_allocations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "emergency_events",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    execution_id = table.Column<int>(type: "int", nullable: false),
                    reason = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    triggered_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    resolved_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_emergency_events", x => x.id);
                    table.ForeignKey(
                        name: "fk_emergency_events_executions",
                        column: x => x.execution_id,
                        principalTable: "show_executions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "hardware_execution_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    execution_id = table.Column<int>(type: "int", nullable: false),
                    occurred_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    message = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    severity = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_hardware_execution_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_hardware_logs_executions",
                        column: x => x.execution_id,
                        principalTable: "show_executions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sensory_sequences",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    execution_id = table.Column<int>(type: "int", nullable: false),
                    sensory_track_id = table.Column<int>(type: "int", nullable: false),
                    current_time_code = table.Column<long>(type: "bigint", nullable: false),
                    actual_time_code = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sensory_sequences", x => x.id);
                    table.ForeignKey(
                        name: "fk_sensory_sequences_executions",
                        column: x => x.execution_id,
                        principalTable: "show_executions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "synchronization_events",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    execution_id = table.Column<int>(type: "int", nullable: false),
                    expected_time_code = table.Column<long>(type: "bigint", nullable: false),
                    actual_time_code = table.Column<long>(type: "bigint", nullable: false),
                    drift_milliseconds = table.Column<long>(type: "bigint", nullable: false),
                    occurred_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    event_type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_synchronization_events", x => x.id);
                    table.ForeignKey(
                        name: "fk_sync_events_executions",
                        column: x => x.execution_id,
                        principalTable: "show_executions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "schedule_conflicts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    show_id = table.Column<int>(type: "int", nullable: false),
                    conflicting_show_id = table.Column<int>(type: "int", nullable: false),
                    detected_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    resolved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    resolved_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_schedule_conflicts", x => x.id);
                    table.ForeignKey(
                        name: "fk_schedule_conflicts_shows_conflicting_id",
                        column: x => x.conflicting_show_id,
                        principalTable: "shows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_schedule_conflicts_shows_show_id",
                        column: x => x.show_id,
                        principalTable: "shows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payment_transactions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    subscription_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    processed_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_payment_transactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_payment_transactions_subscriptions",
                        column: x => x.subscription_id,
                        principalTable: "subscriptions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "subscription_upgrades",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    subscription_id = table.Column<int>(type: "int", nullable: false),
                    old_plan_id = table.Column<int>(type: "int", nullable: false),
                    new_plan_id = table.Column<int>(type: "int", nullable: false),
                    changed_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_subscription_upgrades", x => x.id);
                    table.ForeignKey(
                        name: "fk_subscription_upgrades_subscriptions",
                        column: x => x.subscription_id,
                        principalTable: "subscriptions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "show_occupancies",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    integration_id = table.Column<int>(type: "int", nullable: false),
                    show_id = table.Column<int>(type: "int", nullable: false),
                    total_seats = table.Column<int>(type: "int", nullable: false),
                    occupied_seats = table.Column<int>(type: "int", nullable: false),
                    synchronized_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_show_occupancies", x => x.id);
                    table.ForeignKey(
                        name: "fk_show_occupancies_integrations",
                        column: x => x.integration_id,
                        principalTable: "ticketing_integrations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sync_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    integration_id = table.Column<int>(type: "int", nullable: false),
                    show_id = table.Column<int>(type: "int", nullable: false),
                    executed_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    error_details = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sync_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_sync_logs_integrations",
                        column: x => x.integration_id,
                        principalTable: "ticketing_integrations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "channel_tests",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    equipment_test_id = table.Column<int>(type: "int", nullable: false),
                    effect_channel = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    intensity = table.Column<int>(type: "int", nullable: false),
                    successful = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_channel_tests", x => x.id);
                    table.ForeignKey(
                        name: "fk_channel_tests_equipment_tests",
                        column: x => x.equipment_test_id,
                        principalTable: "equipment_tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "configuration_history_entries",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    sensory_content_id = table.Column<int>(type: "int", nullable: false),
                    previous_intensity = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    new_intensity = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    changed_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_configuration_history_entries", x => x.id);
                    table.ForeignKey(
                        name: "fk_configuration_history_sensory_contents",
                        column: x => x.sensory_content_id,
                        principalTable: "sensory_contents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sensory_tracks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    sensory_content_id = table.Column<int>(type: "int", nullable: false),
                    effect_type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    file_path = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    duration_seconds = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sensory_tracks", x => x.id);
                    table.ForeignKey(
                        name: "fk_sensory_tracks_sensory_contents",
                        column: x => x.sensory_content_id,
                        principalTable: "sensory_contents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_channel_tests_equipment_test_id",
                table: "channel_tests",
                column: "equipment_test_id");

            migrationBuilder.CreateIndex(
                name: "i_x_configuration_history_entries_sensory_content_id",
                table: "configuration_history_entries",
                column: "sensory_content_id");

            migrationBuilder.CreateIndex(
                name: "i_x_emergency_events_execution_id",
                table: "emergency_events",
                column: "execution_id");

            migrationBuilder.CreateIndex(
                name: "i_x_equipment_tests_equipment_id",
                table: "equipment_tests",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "i_x_hardware_execution_logs_execution_id",
                table: "hardware_execution_logs",
                column: "execution_id");

            migrationBuilder.CreateIndex(
                name: "i_x_incident_summaries_dashboard_id",
                table: "incident_summaries",
                column: "dashboard_id");

            migrationBuilder.CreateIndex(
                name: "i_x_incidents_equipment_id",
                table: "incidents",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "i_x_manual_interventions_seat_allocation_id",
                table: "manual_interventions",
                column: "seat_allocation_id");

            migrationBuilder.CreateIndex(
                name: "i_x_movies_genre",
                table: "movies",
                column: "genre");

            migrationBuilder.CreateIndex(
                name: "i_x_movies_status",
                table: "movies",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "i_x_movies_title",
                table: "movies",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_operational_metrics_dashboard_id",
                table: "operational_metrics",
                column: "dashboard_id");

            migrationBuilder.CreateIndex(
                name: "i_x_payment_transactions_subscription_id",
                table: "payment_transactions",
                column: "subscription_id");

            migrationBuilder.CreateIndex(
                name: "i_x_room_blocks_room_id",
                table: "room_blocks",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "i_x_room_inspections_room_id",
                table: "room_inspections",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "i_x_room_performances_dashboard_id",
                table: "room_performances",
                column: "dashboard_id");

            migrationBuilder.CreateIndex(
                name: "i_x_rooms_name",
                table: "rooms",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_schedule_conflicts_conflicting_show_id",
                table: "schedule_conflicts",
                column: "conflicting_show_id");

            migrationBuilder.CreateIndex(
                name: "i_x_schedule_conflicts_show_id",
                table: "schedule_conflicts",
                column: "show_id");

            migrationBuilder.CreateIndex(
                name: "i_x_seat_allocations_show_id",
                table: "seat_allocations",
                column: "show_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_sensory_contents_movie_id",
                table: "sensory_contents",
                column: "movie_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_sensory_sequences_execution_id",
                table: "sensory_sequences",
                column: "execution_id");

            migrationBuilder.CreateIndex(
                name: "i_x_sensory_tracks_sensory_content_id",
                table: "sensory_tracks",
                column: "sensory_content_id");

            migrationBuilder.CreateIndex(
                name: "i_x_show_occupancies_integration_id",
                table: "show_occupancies",
                column: "integration_id");

            migrationBuilder.CreateIndex(
                name: "i_x_show_seats_seat_allocation_id",
                table: "show_seats",
                column: "seat_allocation_id");

            migrationBuilder.CreateIndex(
                name: "i_x_subscription_plans_name",
                table: "subscription_plans",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_subscription_upgrades_subscription_id",
                table: "subscription_upgrades",
                column: "subscription_id");

            migrationBuilder.CreateIndex(
                name: "i_x_sync_logs_integration_id",
                table: "sync_logs",
                column: "integration_id");

            migrationBuilder.CreateIndex(
                name: "i_x_synchronization_events_execution_id",
                table: "synchronization_events",
                column: "execution_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "channel_tests");

            migrationBuilder.DropTable(
                name: "configuration_history_entries");

            migrationBuilder.DropTable(
                name: "emergency_events");

            migrationBuilder.DropTable(
                name: "hardware_execution_logs");

            migrationBuilder.DropTable(
                name: "incident_summaries");

            migrationBuilder.DropTable(
                name: "incidents");

            migrationBuilder.DropTable(
                name: "maintenance_orders");

            migrationBuilder.DropTable(
                name: "manual_interventions");

            migrationBuilder.DropTable(
                name: "operational_metrics");

            migrationBuilder.DropTable(
                name: "operational_reports");

            migrationBuilder.DropTable(
                name: "payment_transactions");

            migrationBuilder.DropTable(
                name: "room_blocks");

            migrationBuilder.DropTable(
                name: "room_inspections");

            migrationBuilder.DropTable(
                name: "room_performances");

            migrationBuilder.DropTable(
                name: "schedule_conflicts");

            migrationBuilder.DropTable(
                name: "sensory_sequences");

            migrationBuilder.DropTable(
                name: "sensory_tracks");

            migrationBuilder.DropTable(
                name: "show_occupancies");

            migrationBuilder.DropTable(
                name: "show_seats");

            migrationBuilder.DropTable(
                name: "subscription_plans");

            migrationBuilder.DropTable(
                name: "subscription_upgrades");

            migrationBuilder.DropTable(
                name: "sync_logs");

            migrationBuilder.DropTable(
                name: "synchronization_events");

            migrationBuilder.DropTable(
                name: "equipment_tests");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "dashboards");

            migrationBuilder.DropTable(
                name: "shows");

            migrationBuilder.DropTable(
                name: "sensory_contents");

            migrationBuilder.DropTable(
                name: "seat_allocations");

            migrationBuilder.DropTable(
                name: "subscriptions");

            migrationBuilder.DropTable(
                name: "ticketing_integrations");

            migrationBuilder.DropTable(
                name: "show_executions");

            migrationBuilder.DropTable(
                name: "equipment");

            migrationBuilder.DropTable(
                name: "movies");
        }
    }
}
