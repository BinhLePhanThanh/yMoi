using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using yMoi.Dto;
using yMoi.Dto.Address;
using yMoi.Model;
using yMoi.Service.Interfaces;

namespace yMoi.Service
{
    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext _dbContext;

        public AddressService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<JsonResponseModel> GetProvinceV2()
        {
            var provinces = await _dbContext.ProvinceV2s.Where(a => a.IsActive == true).ToListAsync();
            return JsonResponse.Success(provinces);
        }

        public async Task<JsonResponseModel> GetWardV2(int provinceId)
        {
            var wards = await _dbContext.WardV2s.Where(a => a.IsActive == true && a.ProvinceV2Id == provinceId).ToListAsync();
            return JsonResponse.Success(wards);
        }


        public async Task<JsonResponseModel> SyncAddress()
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36");

                var url = "https://vietnamlabs.com/api/vietnamprovince";
                var jsonResponse = await httpClient.GetStringAsync(url);

                var provinces = JsonConvert.DeserializeObject<VietnamLabResponseModel>(jsonResponse).data;

                foreach (var province in provinces)
                {
                    var existProvince = await _dbContext.ProvinceV2s.Where(x => province.province.ToLower().Contains(x.Name.ToLower())).FirstOrDefaultAsync();

                    if (existProvince == null)
                    {
                        existProvince = new ProvinceV2
                        {
                            Name = province.province,
                            Code = province.id,
                        };

                        await _dbContext.ProvinceV2s.AddAsync(existProvince);
                    }
                    else
                    {
                        existProvince.Name = province.province;
                        existProvince.Code = province.id;
                    }

                    await _dbContext.SaveChangesAsync();
                    foreach (var ward in province.wards)
                    {
                        var existWard = await _dbContext.WardV2s.Where(x => x.Name.ToLower() == ward.name.ToLower() && x.ProvinceV2Id == existProvince.Id).FirstOrDefaultAsync();
                        if (existWard == null)
                        {
                            existWard = new WardV2
                            {
                                Name = ward.name,
                                ProvinceV2Id = existProvince.Id
                            };

                            await _dbContext.WardV2s.AddAsync(existWard);
                        }
                        else
                        {
                            existWard.Name = ward.name;
                            existWard.ProvinceV2Id = existProvince.Id;
                        }
                        await _dbContext.SaveChangesAsync();
                    }
                }

                return JsonResponse.Success(new { });
            }
            catch (Exception ex)
            {
                return JsonResponse.Error(0, ex.ToString());
            }
        }
    }
}