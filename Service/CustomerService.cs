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
            var existCode = await _dbContext.Customers.Where(a => a.IsActive == true && !string.IsNullOrEmpty(a.Code) && a.Code == dto.Code).FirstOrDefaultAsync();

            if (existCode != null)
            {
                return JsonResponse.Error(0, "Mã khách hàng đã tồn tại");
            }

            var customer = new Customer
            {
                Name = dto.Name,
                Code = dto.Code,
                Phone = dto.Phone,
                Dob = dto.Dob,
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
            customer.Dob = dto.Dob;
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
    }
}
