using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EXAM_SYSTEM_API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class createInitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "activity_log",
                columns: table => new
                {
                    log_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    action = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__activity__9E2397E0E92B2ADF", x => x.log_id);
                });

            migrationBuilder.CreateTable(
                name: "es_choices",
                columns: table => new
                {
                    choice_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสตัวเลือก")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    question_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสคำถามที่ตัวเลือกนี้เป็นของมัน"),
                    choice_text = table.Column<string>(type: "text", nullable: false, comment: "เนื้อหาของตัวเลือก"),
                    is_correct = table.Column<bool>(type: "bit", nullable: false, comment: "ระบุว่าตัวเลือกนี้เป็นคำตอบที่ถูกต้องหรือไม่ (true/false)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__choices__33CAF83AF7B0737B", x => x.choice_id);
                });

            migrationBuilder.CreateTable(
                name: "es_exam_enrollments",
                columns: table => new
                {
                    enrollment_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสการลงทะเบียนสอบ")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    num_days = table.Column<int>(type: "int", nullable: false, comment: "จำนวนที่ระบุในการสอบ"),
                    person_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสบุคคล"),
                    examset_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสชุดข้อสอบ"),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_es_exam_enrollments", x => x.enrollment_id);
                });

            migrationBuilder.CreateTable(
                name: "es_exam_final_results",
                columns: table => new
                {
                    result_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสผลสอบ")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    person_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสนักเรียน"),
                    examset_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสชุดข้อสอบ"),
                    total_score = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0, comment: "คะแนนรวมจากทุก Part"),
                    total_days = table.Column<int>(type: "int", nullable: false, comment: "จำนวนวันที่สอบ"),
                    status = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true, comment: "สถานะสอบเสร็จหรือยัง"),
                    completed_date = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true, comment: "วันที่รวมคะแนนเสร็จ")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__exam_fin__AFB3C3164D8DA42D", x => x.result_id);
                });

            migrationBuilder.CreateTable(
                name: "es_exam_schedules",
                columns: table => new
                {
                    schedule_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสการจัดตารางสอบ")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    enrollment_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสบุคคล"),
                    part_no = table.Column<int>(type: "int", nullable: false, comment: "ครั้งที่สอบ เช่น ครั้งที่ 1, 2, 3 เป็นต้น"),
                    schedule_date = table.Column<DateOnly>(type: "date", nullable: false, comment: "วันที่สอบ"),
                    start_time = table.Column<TimeOnly>(type: "time", nullable: false, comment: "เวลาเริ่มสอบ"),
                    end_time = table.Column<TimeOnly>(type: "time", nullable: false, comment: "เวลาสิ้นสุดการสอบ"),
                    total_questions = table.Column<int>(type: "int", nullable: false, comment: "จำนวนข้อ"),
                    status = table.Column<int>(type: "int", nullable: false, comment: "สถานะ 0 คือ ยังไม่ได้สอบ , 1 คือ สอบแล้ว, 2 คือ ไม่ได้สอบ (ยกเลิการสอบอัตโนมัติ)"),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false, comment: "วันที่สร้างรายการ"),
                    updated_date = table.Column<DateTime>(type: "datetime", nullable: true, comment: "วันที่ปรับปรุงรายการ")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_schedules", x => x.schedule_id);
                });

            migrationBuilder.CreateTable(
                name: "es_exam_sets",
                columns: table => new
                {
                    examset_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสชุดข้อสอบ")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    examset_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, comment: "ชื่อชุดข้อสอบ"),
                    std_id = table.Column<int>(type: "int", nullable: true, comment: "รหัสมาตรฐาน"),
                    std_name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, comment: "ชื่อมาตรฐาน"),
                    description = table.Column<string>(type: "text", nullable: true, comment: "รายละเอียดของชุดข้อสอบ"),
                    total_questions = table.Column<int>(type: "int", nullable: false, defaultValue: 300, comment: "จำนวนข้อสอบทั้งหมด"),
                    num_date_exam = table.Column<int>(type: "int", nullable: false, comment: "จำนวนวันที่ใช้สอบ"),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false, comment: "วันที่เริ่มให้มีการสอบ"),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false, comment: "วันที่สิ้นสุดของการสอบ"),
                    time_per_question = table.Column<int>(type: "int", nullable: false, comment: "ระยะเวลาทำข้อสอบต่อ 1 ข้อ  (หน่วย: วินาที)"),
                    created_by = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "ผู้สร้างชุดข้อสอบ (เชื่อมกับ Users)"),
                    updated_by = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false, comment: "วันที่สร้าง"),
                    updated_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    is_used = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false, defaultValueSql: "((1))", collation: "Thai_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__exam_set__EA6772E4F5B0C197", x => x.examset_id);
                });

            migrationBuilder.CreateTable(
                name: "es_exam_taken",
                columns: table => new
                {
                    exam_taken_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสของการสอบ")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    selection_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสของรายการเลือก"),
                    generated_part_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสของ Part ที่กำลังสอบ"),
                    exam_taken_date = table.Column<DateOnly>(type: "date", nullable: false),
                    started_time = table.Column<TimeOnly>(type: "time", nullable: false, comment: "เวลาที่เริ่มสอบ"),
                    submitted_time = table.Column<TimeOnly>(type: "time", nullable: true, comment: "เวลาที่ส่งข้อสอบ"),
                    score = table.Column<double>(type: "float", nullable: true, defaultValueSql: "(NULL)"),
                    status = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true, comment: "สถานะการสอบ")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__exam_tak__1EC7A272006A86AA", x => x.exam_taken_id);
                });

            migrationBuilder.CreateTable(
                name: "es_exam_user_selection",
                columns: table => new
                {
                    selection_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสของรายการเลือก")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    person_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสผู้สอบ"),
                    examset_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสชุดข้อสอบ"),
                    status = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true, comment: "สถานะการสอบ"),
                    total_days = table.Column<int>(type: "int", nullable: false, comment: "จำนวนวันสอบที่เลือก (1-5 วัน)"),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: true, comment: "วันที่สร้าง"),
                    completed_at = table.Column<DateTime>(type: "datetime", nullable: true, comment: "วันที่สอบเสร็จ")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__exam_use__010BE539D8B988E3", x => x.selection_id);
                });

            migrationBuilder.CreateTable(
                name: "es_generated_exam_parts",
                columns: table => new
                {
                    generated_part_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสของ Part")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    selection_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสของรายการเลือก"),
                    part_number = table.Column<int>(type: "int", nullable: false, comment: "วันที่ของการสอบ"),
                    num_questions = table.Column<int>(type: "int", nullable: false, comment: "จำนวนข้อสอบใน Part นี้"),
                    scheduled_date = table.Column<DateOnly>(type: "date", nullable: false, comment: "วันที่กำหนดสอบของ Part นี้"),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false, comment: "วันที่สร้าง"),
                    person_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสบุคคล")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__generate__31E71B922A2D0DD1", x => x.generated_part_id);
                });

            migrationBuilder.CreateTable(
                name: "es_generated_exam_questions",
                columns: table => new
                {
                    generated_question_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสรายการ")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    generated_part_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสของ Part"),
                    question_id = table.Column<int>(type: "int", nullable: false, comment: "ข้อสอบที่ถูกสุ่ม")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__generate__5741C38A6543A9B1", x => x.generated_question_id);
                });

            migrationBuilder.CreateTable(
                name: "es_questions",
                columns: table => new
                {
                    question_id = table.Column<int>(type: "int", nullable: false, comment: "รหัสข้อสอบ")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    question_text = table.Column<string>(type: "text", nullable: false, comment: "เนื้อหาของคำถาม"),
                    question_type = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false, comment: "ประเภทของคำถาม"),
                    std_group = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: true, comment: "กลุ่มมาตรฐาน"),
                    std_type = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    ref_std_id = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    difficulty = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false, comment: "ระดับความยากของข้อสอบ"),
                    subject = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, comment: "วิชาของข้อสอบ"),
                    created_by = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "ผู้สร้างข้อสอบ"),
                    approved_by = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, defaultValueSql: "(NULL)", comment: "ผู้อนุมัติข้อสอบ"),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: true, comment: "วันที่สร้าง"),
                    approved_at = table.Column<DateTime>(type: "datetime", nullable: true, comment: "วันที่อนุมัติ")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__question__2EC21549CB6E8A7F", x => x.question_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    password_hash = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    full_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "UQ__users__AB6E6164810BD010",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__users__F3DBC572BC65037F",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activity_log");

            migrationBuilder.DropTable(
                name: "es_choices");

            migrationBuilder.DropTable(
                name: "es_exam_enrollments");

            migrationBuilder.DropTable(
                name: "es_exam_final_results");

            migrationBuilder.DropTable(
                name: "es_exam_schedules");

            migrationBuilder.DropTable(
                name: "es_exam_sets");

            migrationBuilder.DropTable(
                name: "es_exam_taken");

            migrationBuilder.DropTable(
                name: "es_exam_user_selection");

            migrationBuilder.DropTable(
                name: "es_generated_exam_parts");

            migrationBuilder.DropTable(
                name: "es_generated_exam_questions");

            migrationBuilder.DropTable(
                name: "es_questions");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
