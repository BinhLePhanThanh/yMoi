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


        public async Task SyncAddress()
        {
            try
            {
                var url = "https://vietnamlabs.com/api/vietnamprovince";
                var req = HttpWebRequest.Create(string.Format(url));
                req.Method = "GET";
                var response = (HttpWebResponse)req.GetResponse();
                var jsonResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();

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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}