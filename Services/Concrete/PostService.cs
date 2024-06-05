using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Collections;
using Uploader_Api.Context;
using Uploader_Api.Dtos;
using Uploader_Api.Models;
using Uploader_Api.Services.Contract;

namespace Uploader_Api.Services.Concrete
{

    public class PostService: IPostService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        public PostService(IWebHostEnvironment hostingEnvironment, ApplicationDbContext context) 
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        public async Task<IEnumerable<Sheet>> GetFile()
        {
            var data =  await _context.Sheets.ToListAsync();
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            else
            {
                return data;
            }
        }

        public string ReadFile(IFormFile file)
        {
            //string webroot = _hostingEnvironment.WebRootPath;

            string uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            string filePath = Path.Combine(uploadsFolder, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            FileInfo existingfile = new FileInfo(filePath);
            List<User> excelFileInfo = new List<User>();
            List<Dictionary<string, string>> excelFile = new List<Dictionary<string, string>>();
            using (ExcelPackage package = new ExcelPackage(existingfile))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int colCount = worksheet.Dimension.End.Column;  //get Column Count
                int noOfRow = worksheet.Dimension.End.Row;     //get row count
                List<string> headers = Enumerable.Range(1, colCount).Select(col => worksheet.Cells[1, col].Value.ToString().Trim()).ToList();
                for (int row = 2; row <= noOfRow; row++)
                {
                    #nullable disable
                    Dictionary<string, string> rowData = new Dictionary<string, string>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        //string key = worksheet.Cells[1, col].Value?.ToString()?.Trim() ?? "";
                        rowData[headers[col - 1]] = worksheet.Cells[row, col].Value?.ToString() ?? "";

                    }
                    excelFile.Add(rowData);
                }
            }
            var json = JsonConvert.SerializeObject(excelFile);
            return json.ToString();
        }

        public async Task<bool> SetFile(string jsonString)
        {
            var data = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonString);
            if (data != null)
            {
                  for (int i = 0; i < data.Count; i++)
            {
                Sheet newSheet = new Sheet()
                {
                    firstname = data[i]["First Name"],
                    middlename = data[i]["Middle Name"],
                    lastname = data[i]["Last Name"],
                    prefix = data[i]["Prefix"],
                    address1 = data[i]["Address1"],
                    address2 = data[i]["Address2"],
                    address3 = data[i]["Address3"],
                    city = data[i]["City"],
                    states = data[i]["State"],
                    residence_phone = data[i]["Residence Phone"],
                    alternate_phone = data[i]["Alternate Phone"],
                    email = data[i]["Email"],
                    day = data[i]["Day"],
                    month = data[i]["Month"],
                    year = data[i]["Year"],
                    gender = data[i]["Gender"],
                    marital_status = data[i]["Marital Status"],
                    religion = data[i]["Religion"],
                    cod_mis_cust_code_1 = data[i]["Cod_mis_cust_code_1"],
                    cod_mis_cust_code_2 = data[i]["Cod_mis_cust_code_2"],
                    profession = data[i]["Profession"],
                    group_mis = data[i]["Group Mis"],
                    profit_centre = data[i]["Profit Centre"],
                    account_officer_code = data[i]["Account Officer Code"],
                    account_officer_name = data[i]["Account Officer Name"],
                    product_type = data[i]["Product Type"],
                    branch_code = data[i]["Branch Code"],
                    staff_id = data[i]["Staff ID"],
                    debit_card_required = data[i]["Debit card required"],
                    enrollment_id = data[i]["EnrollmentID"],
                    enrollment_no = data[i]["EnrollmentNo"],
                    nba_branch = data[i]["NBABranch"],
                    prof_title = data[i]["Prof Title"],
                    nationality = data[i]["Nationality"],
                    date_called_to_bar = data[i]["Date Called to Bar"],
                    office_street = data[i]["Office Street"],
                    office_city = data[i]["Office City"],
                    office_state = data[i]["Office State"],
                };
                _context.Sheets.Add(newSheet);
                _context.SaveChanges();
              }
            }
          
            return true;
        } 

        public async Task<bool> DeleteFile (string UserId)
        {
            var data = UserId.Split(",");
            foreach(var item in data)
            {
                var findId = await _context.Sheets.FindAsync(Int32.Parse(item));
                if (findId != null)
                {
                    _context.Sheets.Remove(findId);
                }
                else
                {
                    return false;
                }
            }
            await _context.SaveChangesAsync();
            return true; 
        }
    }
}
