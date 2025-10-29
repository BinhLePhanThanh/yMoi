using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using yMoi.Dto;
using yMoi.Dto.Customer;
using yMoi.Dto.CustomerGroup;
using yMoi.Enums;
using yMoi.Migrations;
using yMoi.Model;
using yMoi.Service.Interfaces;
using yMoi.Util;

namespace yMoi.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _dbContext;
        public CustomerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<JsonResponseModel> CreateCustomer(CreateCustomerModel dto, int createById)
        {
            var existCode = await _dbContext.Customers.Where(a => a.IsActive == true && !string.IsNullOrEmpty(dto.Code) && a.Code == dto.Code).FirstOrDefaultAsync();

            if (existCode != null)
            {
                return JsonResponse.Error(0, "Mã khách hàng đã tồn tại");
            }

            if (string.IsNullOrEmpty(dto.Code))
            {
                do
                {
                    dto.Code = Utils.GenerateBNCode();
                    existCode = await _dbContext.Customers.Where(a => a.Code == dto.Code && a.IsActive == true).FirstOrDefaultAsync();
                } while (existCode != null);
            }

            var customer = new Customer
            {
                Name = dto.Name,
                Code = dto.Code,
                Phone = dto.Phone,
                Dob = dto.Dob.HasValue? dto.Dob.Value.Date : (DateTime?)null,
                Email = dto.Email,
                Job = dto.Job,
                ProvinceId = dto.ProvinceId,
                DistrictId = dto.DistrictId,
                WardId = dto.WardId,
                Address = dto.Address,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                Language = dto.Language,
                Religion = dto.Religion,
                Nationality = dto.Nationality,
                IdentityCardNumber = dto.IdentityCardNumber,
                EducationalLevel = dto.EducationalLevel,
                MaritalStatus = dto.MaritalStatus,
                Gender = dto.Gender,

                RelativeName = dto.RelativeName,
                Relationship = dto.Relationship,
                RelationshipAddress = dto.RelationshipAddress,
                RelativeProvinceId = dto.RelativeProvinceId,
                RelativeDistrictId = dto.RelativeDistrictId,
                RelativeWardId = dto.RelativeWardId,
                RelativeCountry = dto.RelativeCountry,
                RelativePostalCode = dto.RelativePostalCode,
                RelativePhone = dto.RelativePhone,

                BankCode = dto.BankCode,
                AccountNumber = dto.AccountNumber,
                AccountHolder = dto.AccountHolder,

                CustomerGroupId = dto.CustomerGroupId,
                CreatedById = createById
            };

            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();

            var action = new CustomerAction
            {
                Action = ActionHistoryEnum.Create,
                UserId = createById,
                CustomerId = customer.Id
            };

            await _dbContext.CustomerActions.AddAsync(action);
            await _dbContext.SaveChangesAsync();

            foreach (PropertyInfo propertyInfo in customer.GetType().GetProperties())
            {
                string value = propertyInfo.GetValue(customer) != null ? propertyInfo.GetValue(customer).ToString() : null;
                var fieldType = propertyInfo.PropertyType.ToString();

                var userActionPropertyArticleLog = new CustomerActionHistory
                {
                    CustomerActionId = action.Id,
                    Field = Utils.CamelCaseFromTitleCase(propertyInfo.Name),
                    FieldType = fieldType,
                    ToValue = value
                };

                await _dbContext.CustomerActionHistories.AddAsync(userActionPropertyArticleLog);
            }

            await _dbContext.SaveChangesAsync();

            return JsonResponse.Success(new { });
        }

        public async Task<JsonResponseModel> DeleteCustomer(int id)
        {
            var customer = await _dbContext.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (customer != null)
            {
                customer.IsActive = false;
                await _dbContext.SaveChangesAsync();
            }

            return JsonResponse.Success(new { });
        }

        public async Task<JsonResponseModel> EditCustomer(int id, CreateCustomerModel dto, int updatedById)
        {
            var existCode = await _dbContext.Customers.Where(a => a.Id != id && a.IsActive == true && !string.IsNullOrEmpty(a.Code) && a.Code == dto.Code).FirstOrDefaultAsync();

            if (existCode != null)
            {
                return JsonResponse.Error(0, "Mã khách hàng đã tồn tại");
            }

            var customer = await _dbContext.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();


            if (customer == null)
            {
                return JsonResponse.Error(0, "Khách hàng không tồn tại");
            }

            var oldCustomer = customer.DeepCopy();


            customer.Name = dto.Name;
            customer.Code = dto.Code;
            customer.Phone = dto.Phone;
            customer.Dob = dto.Dob.HasValue ? dto.Dob.Value.Date : (DateTime?)null;
            customer.Email = dto.Email;
            customer.Job = dto.Job;
            customer.ProvinceId = dto.ProvinceId;
            customer.DistrictId = dto.DistrictId;
            customer.WardId = dto.WardId;
            customer.Address = dto.Address;
            customer.PostalCode = dto.PostalCode;
            customer.Country = dto.Country;
            customer.Language = dto.Language;
            customer.Religion = dto.Religion;
            customer.Nationality = dto.Nationality;
            customer.IdentityCardNumber = dto.IdentityCardNumber;
            customer.EducationalLevel = dto.EducationalLevel;
            customer.MaritalStatus = dto.MaritalStatus;
            customer.Gender = dto.Gender;

            customer.RelativeName = dto.RelativeName;
            customer.Relationship = dto.Relationship;
            customer.RelationshipAddress = dto.RelationshipAddress;
            customer.RelativeProvinceId = dto.RelativeProvinceId;
            customer.RelativeDistrictId = dto.RelativeDistrictId;
            customer.RelativeWardId = dto.RelativeWardId;
            customer.RelativeCountry = dto.RelativeCountry;
            customer.RelativePostalCode = dto.RelativePostalCode;
            customer.RelativePhone = dto.RelativePhone;

            customer.BankCode = dto.BankCode;
            customer.AccountNumber = dto.AccountNumber;
            customer.AccountHolder = dto.AccountHolder;

            customer.CustomerGroupId = dto.CustomerGroupId;
            await _dbContext.SaveChangesAsync();


            var action = new CustomerAction
            {
                Action = ActionHistoryEnum.Update,
                UserId = updatedById,
                CustomerId = customer.Id
            };

            await _dbContext.CustomerActions.AddAsync(action);
            await _dbContext.SaveChangesAsync();

            foreach (PropertyInfo f in oldCustomer.GetType().GetProperties())
            {
                Variance v = new Variance();
                v.Field = f.Name;

                string value = null;

                v.FromValue = f.GetValue(oldCustomer) != null ? f.GetValue(oldCustomer).ToString() : null;
                v.ToValue = f.GetValue(customer) != null ? f.GetValue(customer).ToString() : null;

                if (!Equals(v.FromValue, v.ToValue))
                {
                    var customerActionHistory = new CustomerActionHistory
                    {
                        CustomerActionId = action.Id,
                        Field = Utils.CamelCaseFromTitleCase(v.Field),
                        FieldType = f.PropertyType.ToString(),
                        FromValue = v.FromValue,
                        ToValue = v.ToValue
                    };

                    await _dbContext.CustomerActionHistories.AddAsync(customerActionHistory);
                }
            }
            await _dbContext.SaveChangesAsync();
            return JsonResponse.Success(new { });

        }

        public async Task<JsonResponseModel> GetCustomerByCode(string code)
        {
            var found = await _dbContext.Customers.Where(a => a.Code == code && !string.IsNullOrEmpty(a.Code)).AsNoTracking().FirstOrDefaultAsync();

            if (found != null)
            {

                return await GetCustomerDetails(found.Id);
            }

            return JsonResponse.Success(new { });
        }

        public async Task<JsonResponseModel> GetCustomerDetails(int id)
        {
            var customer = await _dbContext.Customers.Where(a => a.Id == id).Select(a => new GetCustomerDetailsModel
            {
                Id = a.Id,
                Name = a.Name,
                Code = a.Code,
                Phone = a.Phone,
                Dob = a.Dob,
                Email = a.Email,
                Job = a.Job,
                ProvinceId = a.ProvinceId,
                DistrictId = a.DistrictId,
                WardId = a.WardId,
                Address = a.Address,
                PostalCode = a.PostalCode,
                Country = a.Country,
                Language = a.Language,
                Religion = a.Religion,
                Nationality = a.Nationality,
                IdentityCardNumber = a.IdentityCardNumber,
                EducationalLevel = a.EducationalLevel,
                MaritalStatus = a.MaritalStatus,
                Gender = a.Gender,

                RelativeName = a.RelativeName,
                Relationship = a.Relationship,
                RelationshipAddress = a.RelationshipAddress,
                RelativeProvinceId = a.RelativeProvinceId,
                RelativeDistrictId = a.RelativeDistrictId,
                RelativeWardId = a.RelativeWardId,
                RelativeCountry = a.RelativeCountry,
                RelativePostalCode = a.RelativePostalCode,
                RelativePhone = a.RelativePhone,

                BankCode = a.BankCode,
                AccountNumber = a.AccountNumber,
                AccountHolder = a.AccountHolder,

                CustomerGroupId = a.CustomerGroupId,
                CreatedById = a.CreatedById,
                CreatedByName = a.CreatedBy.Name,
                CreatedDate = a.CreatedDate,
                UpdatedDate = a.UpdatedDate,
            }).FirstOrDefaultAsync();

            return JsonResponse.Success(customer);
        }

        public async Task<JsonResponseModel> GetCustomerHistory(int id, int page, int limit)
        {
            var query = _dbContext.CustomerActions.Where(a => a.IsActive == true && a.CustomerId == id
                && a.CustomerActionHistories.Where(b => b.IsActive == true).FirstOrDefault() != null
            );

            var count = await query.CountAsync();
            var list = await query.OrderByDescending(a => a.Id).Select(a => new CustomerGroupHistoryModel
            {
                Id = a.Id,
                UserId = a.UserId,
                UserName = a.User.Name,
                CreatedDate = a.CreatedDate,
                Action = a.Action,
                Details = a.CustomerActionHistories.Where(b => b.IsActive == true).Select(b => new CustomerGroupHistoryDetails
                {
                    Field = b.Field,
                    FieldType = b.FieldType,
                    FromValue = b.FromValue,
                    ToValue = b.ToValue,
                }).ToList()
            }).Skip((page - 1) * limit).Take(limit).ToListAsync();

            return JsonResponse.Success(list, new PagingModel
            {
                Page = page,
                Limit = limit,
                TotalItemCount = count
            });
        }

        public async Task<JsonResponseModel> GetListCustomer(int page, int limit, bool? status, string? search, DateTime? fromDate, DateTime? toDate)
        {
            var query = _dbContext.Customers.Where(a => a.IsActive == true
                           && (status.HasValue ? a.Status == status : true)
                           && (!string.IsNullOrEmpty(search) ? a.Name.Contains(search) || a.Code.Contains(search) : true)
                           && (fromDate.HasValue ? a.CreatedDate >= fromDate : true)
                           && (toDate.HasValue ? a.CreatedDate <= toDate : true)
                       );
            var count = await query.CountAsync();
            var list = await query.OrderByDescending(a => a.Id).Select(a => new GetListCustomerModel
            {
                Id = a.Id,
                Status = a.Status,
                Name = a.Name,
                Code = a.Code,
                Phone = a.Phone,
                Dob = a.Dob,
                Address = a.Address,
                Gender = a.Gender,
                CreatedDate = a.CreatedDate,
                CreatedById = a.CreatedById,
                CreatedByName = a.CreatedBy.Name
            }).Skip((page - 1) * limit).Take(limit).ToListAsync();

            return JsonResponse.Success(list, new PagingModel
            {
                Page = page,
                Limit = limit,
                TotalItemCount = count
            });
        }

        public async Task<JsonResponseModel> GetListSearchCustomer(string? code, string? name, string? phone, DateTime? dob, string? nationality, string? identityCardNumber, int page = 1, int limit = 12)
        {
            var query = _dbContext.Customers.Where(a => a.IsActive == true
                    && (!string.IsNullOrEmpty(code) ? a.Code.Contains(code) : true)
                    && (!string.IsNullOrEmpty(name) ? a.Name.Contains(name) : true)
                    && (!string.IsNullOrEmpty(nationality) ? a.Nationality.Contains(nationality) : true)
                    && (!string.IsNullOrEmpty(identityCardNumber) ? a.IdentityCardNumber.Contains(identityCardNumber) : true)
                    && (!string.IsNullOrEmpty(phone) ? a.Phone.Contains(phone) : true)
                    && (dob.HasValue ? a.Dob.Value.Date == dob.Value.Date : true)
            );

            var count = await query.CountAsync();

            var list = await query.OrderByDescending(a => a.Id).Select(a => new GetListCustomerModel
            {
                Id = a.Id,
                Status = a.Status,
                Name = a.Name,
                Code = a.Code,
                Phone = a.Phone,
                Dob = a.Dob,
                Address = a.Address,
                Gender = a.Gender,
                CreatedDate = a.CreatedDate,
                CreatedById = a.CreatedById,
                CreatedByName = a.CreatedBy.Name,
                BankCode = a.BankCode,
                IdentityCardNumber = a.IdentityCardNumber,
                AccountHolder = a.AccountHolder,
                AccountNumber = a.AccountNumber
            }).Skip((page - 1) * limit).Take(limit).ToListAsync();

            return JsonResponse.Success(list, new PagingModel
            {
                Page = page,
                Limit = limit,
                TotalItemCount = count
            });
        }

        public async Task<JsonResponseModel> ToggleStatus(int id)
        {
            var customer = await _dbContext.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (customer != null)
            {
                customer.Status = !customer.Status;
                await _dbContext.SaveChangesAsync();
            }
            return JsonResponse.Success(new { });
        }

        public async Task<JsonResponseModel> ImportCustomer(IFormFile file)
        {
            try
            {
                using var memStrem = new MemoryStream();

                await file.CopyToAsync(memStrem);

                using var workbook = new XLWorkbook(memStrem);
                var worksheet = workbook.Worksheets.First();

                var row1 = worksheet.Rows().First();

                var celles = row1.Cells().Where(a => a.Value.ToString().ToLower() == "mã khách hàng").FirstOrDefault();

                var hoVaTenCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "họ và tên")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var maKhachHangCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "mã khách hàng")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var maNhomCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "mã nhóm")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var ngaySinhCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "ngày sinh")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var gioiTinhCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "giới tính")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var tinhTrangHonNhanCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "tình trạng hôn nhân")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var ngheNghiepCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "nghề nghiệp")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var ngonNguCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "ngôn ngữ")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var trinhDoHocVanCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "trình độ học vấn")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var tonGiaoCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "tôn giáo")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var quocTichCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "quốc tịch")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var soCccdCol = row1.Cells().Where(a => a.Value.ToString().ToLower().Contains("cccd")
                    || a.Value.ToString().ToLower().Contains("hộ chiếu")
                    || a.Value.ToString().ToLower().Contains("cmt"))
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var emailCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "email")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var diaChiCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "địa chỉ")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var phuongXaCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "phường/xã")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var tinhCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "tỉnh")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var quocGiaCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "quốc gia")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var maBuuDienCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "mã bưu điện")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var soDienThoaiCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "số điện thoại")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var tenNguoiThanCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "tên người thân")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var kieuQuanHeCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "kiểu quan hệ")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var diaChiNguoiThanCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "địa chỉ người thân")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var tinhNguoiThanCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "tỉnh người thân")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var quocGiaNguoiThanCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "quốc gia người thân")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var maBuuDienNguoiThanCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "mã bưu điện người thân")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var phuongXaNguoiThanCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "phường/ xã người thân")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();


                var soDienThoaiNguoiThanCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "số điện thoại người thân")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var nganHangCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "ngân hàng")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var soTaiKhoanCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "số tài khoản")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                var chuTaiKhoanCol = row1.Cells().Where(a => a.Value.ToString().ToLower() == "chủ tài khoản")
                    .Select(a => a.Address.ColumnNumber).FirstOrDefault();

                foreach (var row in worksheet.RowsUsed())
                {
                    if (row.RowNumber() == 1) continue;

                    var code = maKhachHangCol != 0 ? row.Cell(maKhachHangCol).Value.ToString() : string.Empty;

                    var customerGroupCode = maNhomCol != 0 ? row.Cell(maNhomCol).Value.ToString() : null;

                    int? customerGroupId = null;

                    if (!string.IsNullOrEmpty(customerGroupCode))
                    {
                        var customerGroup = await _dbContext.CustomerGroups.Where(a => a.Code == customerGroupCode && a.IsActive == true).FirstOrDefaultAsync();
                        if (customerGroup != null)
                        {
                            customerGroupId = customerGroup.Id;
                        }
                    }

                    var existCode = await _dbContext.Customers.Where(a => a.Code == code && a.IsActive == true).FirstOrDefaultAsync();

                    while (existCode != null)
                    {
                        code = Utils.GenerateBNCode();
                        existCode = await _dbContext.Customers.Where(a => a.Code == code && a.IsActive == true).FirstOrDefaultAsync();
                    }

                    var province = await _dbContext.ProvinceV2s.Where(a => a.IsActive == true && a.Name.ToLower() == (tinhCol != 0 ? row.Cell(tinhCol).Value.ToString() : string.Empty)).FirstOrDefaultAsync();
                    var ward = await _dbContext.WardV2s.Where(a => a.IsActive == true && a.Name.ToLower() == (phuongXaCol != 0 ? row.Cell(phuongXaCol).Value.ToString() : string.Empty)).FirstOrDefaultAsync();

                    var relativeProvince = await _dbContext.ProvinceV2s.Where(a => a.IsActive == true && a.Name.ToLower() == (tinhNguoiThanCol != 0 ? row.Cell(tinhNguoiThanCol).Value.ToString() : string.Empty)).FirstOrDefaultAsync();
                    var relativeWward = await _dbContext.WardV2s.Where(a => a.IsActive == true && a.Name.ToLower() == (phuongXaNguoiThanCol != 0 ? row.Cell(phuongXaNguoiThanCol).Value.ToString() : string.Empty)).FirstOrDefaultAsync();

                    var date = row.Cell(ngaySinhCol).Value;

                    var newCustomer = new Customer
                    {
                        Code = code,
                        Name = hoVaTenCol != 0 ? row.Cell(hoVaTenCol).Value.ToString() : null,
                        Phone = soDienThoaiCol != 0 ? row.Cell(soDienThoaiCol).Value.ToString() : null,
                        Dob = ngaySinhCol != 0 && row.Cell(ngaySinhCol).Value.IsDateTime ? row.Cell(ngaySinhCol).Value.GetDateTime().Date : (DateTime?)null,
                        Email = emailCol != 0 ? row.Cell(emailCol).Value.ToString() : null,
                        Job = ngheNghiepCol != 0 ? row.Cell(ngheNghiepCol).Value.ToString() : null,
                        Address = diaChiCol != 0 ? row.Cell(diaChiCol).Value.ToString() : null,
                        PostalCode = maBuuDienCol != 0 ? row.Cell(maBuuDienCol).Value.ToString() : null,
                        Country = quocGiaCol != 0 ? Constants.Contants.nationalities.Where(a => a.Name.ToLower() == row.Cell(quocGiaCol).Value.ToString()).Select(a => a.Code).FirstOrDefault() : null,
                        Language = ngonNguCol != 0 ? Constants.Contants.languages.Where(a => a.Name.ToLower() == row.Cell(ngonNguCol).Value.ToString()).Select(a => a.Code).FirstOrDefault() : null,
                        Religion = tonGiaoCol != 0 ? row.Cell(tonGiaoCol).Value.ToString() : null,
                        Nationality = quocTichCol != 0 ? Constants.Contants.nationalities.Where(a => a.Name.ToLower() == row.Cell(quocTichCol).Value.ToString()).Select(a => a.Code).FirstOrDefault() : null,
                        IdentityCardNumber = soCccdCol != 0 ? row.Cell(soCccdCol).Value.ToString() : null,
                        EducationalLevel = trinhDoHocVanCol != 0 ? Constants.Contants.educationLevels.Where(a => a.Name.ToLower() == row.Cell(trinhDoHocVanCol).Value.ToString().ToLower()).Select(a => a.Code).FirstOrDefault() : null,
                        MaritalStatus = tinhTrangHonNhanCol != 0 ? row.Cell(tinhTrangHonNhanCol).Value.ToString() : null,
                        Gender = gioiTinhCol != 0 ? row.Cell(gioiTinhCol).Value.ToString().ToLower() == "nam" ? GenderEnum.Male : row.Cell(gioiTinhCol).Value.ToString().ToLower() == "nữ" ? GenderEnum.Female : null : null,

                        ProvinceId = province?.Id,
                        WardId = ward?.Id,

                        RelativeName = tenNguoiThanCol != 0 ? row.Cell(tenNguoiThanCol).Value.ToString() : null,
                        Relationship = kieuQuanHeCol != 0 ? row.Cell(kieuQuanHeCol).Value.ToString() : null,
                        RelationshipAddress = diaChiNguoiThanCol != 0 ? row.Cell(diaChiNguoiThanCol).Value.ToString() : null,

                        RelativeWardId = relativeWward?.Id,
                        RelativeProvinceId = relativeProvince?.Id,

                        RelativeCountry = quocGiaNguoiThanCol != 0 ? row.Cell(quocGiaNguoiThanCol).Value.ToString() : null,
                        RelativePostalCode = maBuuDienNguoiThanCol != 0 ? row.Cell(maBuuDienNguoiThanCol).Value.ToString() : null,
                        RelativePhone = soDienThoaiNguoiThanCol != 0 ? row.Cell(soDienThoaiNguoiThanCol).Value.ToString() : null,

                        BankCode = nganHangCol != 0 ? row.Cell(nganHangCol).Value.ToString() : null,
                        AccountNumber = soTaiKhoanCol != 0 ? row.Cell(soTaiKhoanCol).Value.ToString() : null,
                        AccountHolder = chuTaiKhoanCol != 0 ? row.Cell(chuTaiKhoanCol).Value.ToString() : null,
                        CustomerGroupId = customerGroupId
                    };

                    await _dbContext.Customers.AddAsync(newCustomer);
                    await _dbContext.SaveChangesAsync();
                }

                return JsonResponse.Success(new { });
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
