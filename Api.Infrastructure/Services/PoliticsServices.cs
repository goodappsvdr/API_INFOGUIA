using Api.Shared.Interface;

namespace Api.Infrastructure.Services
{
    public class PoliticsServices : IPoliticsServices
    {
        private readonly IRepository<Politic> _repository;
        private readonly IMapper _mapper;

        public PoliticsServices(IRepository<Politic> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Obtiene una politica por su tipo
        public async Task<Politic_Get> GetAsync(string Type) => _mapper.Map<Politic_Get>(await _repository.GetAsync(x => x.Type == Type));

        // Modifica la politica existente
        public async Task<bool> UpdateAsync(Politic_Update Model) => await _repository.UpdateAsync(_mapper.Map<Politic>(Model));

    }
}
