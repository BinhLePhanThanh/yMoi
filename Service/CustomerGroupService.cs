using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using yMoi.Dto;
using yMoi.Dto.CustomerGroup;
using yMoi.Dto.Department;
using yMoi.Enums;
using yMoi.Model;
using yMoi.Service.Interfaces;
using yMoi.Util;

namespace yMoi.Service
{
    public class CustomerGroupService : ICustomerGroupService
    {
        private readonly ApplicationDbContext _dbContext;
        public CustomerGroupService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<JsonResponseModel> CreateCustomerGroup(CreateCustomerGroupModel dto, int createById)
        {
            var existCustomerGroup = await _dbContext.CustomerGroups.Where(a => a.IsActive == true && !string.IsNullOrEmpty(a.Code) && a.Code == dto.Code).FirstOrDefaultAsync();

            if (existCustomerGroup != null)
            {
                return JsonResponse.Error(0, "Mã nhóm đã tồn tại");
            }

            var newCustomerGroup = new CustomerGroup
            {
                Phone = dto.Phone,
                Name = dto.Name,
                Code = dto.Code,
                Address = dto.Address,
                BankCode = dto.BankCode,
                AccountHolder = dto.AccountHolder,
                AccountNumber = dto.AccountNumber,
                CreatedById = createById
            };

            await _dbContext.CustomerGroups.AddAsync(newCustomerGroup);
            await _dbContext.SaveChangesAsync();

            var action = new CustomerAction
            {
                Action = ActionHistoryEnum.Create,
                UserId = createById,
                CustomerGroupId = newCustomerGroup.Id
            };

            await _dbContext.CustomerActions.AddAsync(action);
            await _dbContext.SaveChangesAsync();

            foreach (PropertyInfo propertyInfo in newCustomerGroup.GetType().GetProperties())
            {
                string value = propertyInfo.GetValue(newCustomerGroup) != null ? propertyInfo.GetValue(newCustomerGroup).ToString() : null;
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

        public async Task<JsonResponseModel> DeleteCustomerGroup(int id)
        {
            var customerGroup = await _dbContext.CustomerGroups.Where(a => a.Id == id).FirstOrDefaultAsync();

            if (customerGroup != null)
            {
                customerGroup.IsActive = false;
                await _dbContext.SaveChangesAsync();
            }

            return JsonResponse.Success(new { });
        }

        public async Task<JsonResponseModel> EditCustomerGroup(int id, CreateCustomerGroupModel dto, int updatedById)
        {
            var existCustomerGroup = await _dbContext.CustomerGroups.Where(a => a.Id != id && a.IsActive == true && !string.IsNullOrEmpty(a.Code) && a.Code == dto.Code).FirstOrDefaultAsync();

            if (existCustomerGroup != null)
            {
                return JsonResponse.Error(0, "Mã nhóm đã tồn tại");
            }

            var customerGroup = await _dbContext.CustomerGroups.Where(a => a.Id == id).FirstOrDefaultAsync();

            var oldCustomerGroup = customerGroup.DeepCopy();

            customerGroup.Name = dto.Name;
            customerGroup.Phone = dto.Phone;
            customerGroup.Code = dto.Code;
            customerGroup.Address = dto.Address;
            customerGroup.BankCode = dto.BankCode;
            customerGroup.AccountHolder = dto.AccountHolder;
            customerGroup.AccountNumber = dto.AccountNumber;

            await _dbContext.SaveChangesAsync();

            var action = new CustomerAction
            {
                Action = ActionHistoryEnum.Update,
                UserId = updatedById,
                CustomerGroupId = customerGroup.Id
            };

            await _dbContext.CustomerActions.AddAsync(action);
            await _dbContext.SaveChangesAsync();

            foreach (PropertyInfo f in oldCustomerGroup.GetType().GetProperties())
            {
                Variance v = new Variance();
                v.Field = f.Name;

                string value = null;

                v.FromValue = f.GetValue(oldCustomerGroup) != null ? f.GetValue(oldCustomerGroup).ToString() : null;
                v.ToValue = f.GetValue(customerGroup) != null ? f.GetValue(customerGroup).ToString() : null;

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

        public async Task<JsonResponseModel> GetCustomerGroupByCode(string code)
        {
            var model = await _dbContext.CustomerGroups.Where(a => a.Code == code).Select(a => new GetListCustomerGroupModel
            {
                Id = a.Id,
                Status = a.Status,
                Name = a.Name,
                Code = a.Code,
                Phone = a.Phone,
                Address = a.Address,
                BankCode = a.BankCode,
                AccountHolder = a.AccountHolder,
                AccountNumber = a.AccountHolder,
                CreatedDate = a.CreatedDate,
                CreatedById = a.CreatedById,
                CreatedByName = a.CreatedBy.Name,
                UpdatedDate = a.UpdatedDate
            }).FirstOrDefaultAsync();

            return JsonResponse.Success(model);
        }

        public async Task<JsonResponseModel> GetCustomerGroupCustomers(int id, int page, int limit)
        {
            var query = _dbContext.Customers.Where(a => a.IsActive == true && a.CustomerGroupId == id
            );

            var count = await query.CountAsync();
            var list = await query.OrderByDescending(a => a.Id).Select(a => new GetListCustomerGroupCustomer
            {
                Id = a.Id,
                Name = a.Name,
                Code = a.Code
            }).Skip((page - 1) * limit).Take(limit).ToListAsync();

            return JsonResponse.Success(list, new PagingModel
            {
                Page = page,
                Limit = limit,
                TotalItemCount = count
            });
        }

        public async Task<JsonResponseModel> GetCustomerGroupDetails(int id)
        {
            var model = await _dbContext.CustomerGroups.Where(a => a.Id == id).Select(a => new GetListCustomerGroupModel
            {
                Id = a.Id,
                Status = a.Status,
                Name = a.Name,
                Code = a.Code,
                Phone = a.Phone,
                Address = a.Address,
                BankCode = a.BankCode,
                AccountHolder = a.AccountHolder,
                AccountNumber = a.AccountHolder,
                CreatedDate = a.CreatedDate,
                CreatedById = a.CreatedById,
                CreatedByName = a.CreatedBy.Name,
                UpdatedDate = a.UpdatedDate
            }).FirstOrDefaultAsync();

            return JsonResponse.Success(model);
        }

        public async Task<JsonResponseModel> GetCustomerGroupHistory(int id, int page, int limit)
        {
            var query = _dbContext.CustomerActions.Where(a => a.IsActive == true && a.CustomerGroupId == id
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

        public async Task<JsonResponseModel> GetListCustomerGroup(int page, int limit, bool? status, string? search, DateTime? fromDate, DateTime? toDate)
        {
            var query = _dbContext.CustomerGroups.Where(a => a.IsActive == true
                && (status.HasValue ? a.Status == status : true)
                && (!string.IsNullOrEmpty(search) ? a.Name.Contains(search) : true)
                && (fromDate.HasValue ? a.CreatedDate >= fromDate : true)
                && (toDate.HasValue ? a.CreatedDate <= toDate : true)
            );
            var count = await query.CountAsync();
            var list = await query.OrderByDescending(a => a.Id).Select(a => new GetListCustomerGroupModel
            {
                Id = a.Id,
                Status = a.Status,
                Name = a.Name,
                Code = a.Code,
                Phone = a.Phone,
                Address = a.Address,
                BankCode = a.BankCode,
                AccountHolder = a.AccountHolder,
                AccountNumber = a.AccountHolder,
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
            var customerGroup = await _dbContext.CustomerGroups.Where(a => a.Id == id).FirstOrDefaultAsync();

            if (customerGroup != null)
            {
                customerGroup.Status = !customerGroup.Status;
                await _dbContext.SaveChangesAsync();
            }

            return JsonResponse.Success(new { });
        }
    }
}
