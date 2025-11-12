using Api.Shared.DTOs.Province;
using Api.Shared.Models;
using System.ComponentModel;

namespace Api.Infrastructure.Services.Province
{
    public class ProvinceServices : IProvinceServices
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        public ProvinceServices(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Province_Get>> GetAsync()
        {
            var result = await _context.Provinces.ToListAsync();

            return _mapper.Map<List<Province_Get>>(result);
        }


    }
}
