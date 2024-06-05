using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UploaderApi.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sheets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    middlename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    states = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    residencephone = table.Column<string>(name: "residence_phone", type: "nvarchar(max)", nullable: false),
                    alternatephone = table.Column<string>(name: "alternate_phone", type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    month = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    maritalstatus = table.Column<string>(name: "marital_status", type: "nvarchar(max)", nullable: false),
                    religion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    codmiscustcode1 = table.Column<string>(name: "cod_mis_cust_code_1", type: "nvarchar(max)", nullable: false),
                    codmiscustcode2 = table.Column<string>(name: "cod_mis_cust_code_2", type: "nvarchar(max)", nullable: false),
                    profession = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    groupmis = table.Column<string>(name: "group_mis", type: "nvarchar(max)", nullable: false),
                    profitcentre = table.Column<string>(name: "profit_centre", type: "nvarchar(max)", nullable: false),
                    accountofficercode = table.Column<string>(name: "account_officer_code", type: "nvarchar(max)", nullable: false),
                    accountofficername = table.Column<string>(name: "account_officer_name", type: "nvarchar(max)", nullable: false),
                    producttype = table.Column<string>(name: "product_type", type: "nvarchar(max)", nullable: false),
                    branchcode = table.Column<string>(name: "branch_code", type: "nvarchar(max)", nullable: false),
                    staffid = table.Column<string>(name: "staff_id", type: "nvarchar(max)", nullable: false),
                    debitcardrequired = table.Column<string>(name: "debit_card_required", type: "nvarchar(max)", nullable: false),
                    enrollmentid = table.Column<string>(name: "enrollment_id", type: "nvarchar(max)", nullable: false),
                    enrollmentno = table.Column<string>(name: "enrollment_no", type: "nvarchar(max)", nullable: false),
                    nbabranch = table.Column<string>(name: "nba_branch", type: "nvarchar(max)", nullable: false),
                    proftitle = table.Column<string>(name: "prof_title", type: "nvarchar(max)", nullable: false),
                    nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    datecalledtobar = table.Column<string>(name: "date_called_to_bar", type: "nvarchar(max)", nullable: false),
                    officestreet = table.Column<string>(name: "office_street", type: "nvarchar(max)", nullable: false),
                    officecity = table.Column<string>(name: "office_city", type: "nvarchar(max)", nullable: false),
                    officestate = table.Column<string>(name: "office_state", type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sheets", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sheets");
        }
    }
}
