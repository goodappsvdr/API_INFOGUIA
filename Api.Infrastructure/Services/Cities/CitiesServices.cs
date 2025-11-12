using Api.Shared.DTOs.Cities;
using Api.Shared.DTOs.Province;

namespace Api.Infrastructure.Services.Cities
{
    public class CitiesServices : ICitiesServices
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        public CitiesServices(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<Cities_Get>> GetAsync(int provinceId)
        {
            var result = await _context.Cities.Where(x => x.ProvinceId == provinceId).OrderBy(x => x.Name).ToListAsync();

            return _mapper.Map<List<Cities_Get>>(result);
        }

    }
}
