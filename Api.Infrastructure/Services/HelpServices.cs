using Api.Shared.DTOs.Help;

namespace Api.Infrastructure.Services
{
    public class HelpServices : IHelpServices
    {
        private readonly IRepository<Help> _repository;
        private readonly IMapper _mapper;

        public HelpServices(IRepository<Help> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        // Existe la pregunta en la base
        public async Task<bool> ExistQuestion(string Question, int Id) => await _repository.GetAsync(x => x.Question == Question && x.Id != Id) == null ? false : true;

        // Obtiene una lista de pregunta con su respuestas
        public async Task<List<Help_Get>> GetList() => _mapper.Map<List<Help_Get>>(await _repository.GetAllAsync());

        // Obtiene una pregunta con su respuesta por su id
        public async Task<Help_Get> Get(int Id) => _mapper.Map<Help_Get>(await _repository.GetAsync(x => x.Id == Id));

        // Crea una nueva pregunta con su respuesta
        public async Task<Help_Create> Add(Help_Create Model) => _mapper.Map<Help_Create>(await _repository.AddAsync(_mapper.Map<Help>(Model)));

        // Modifica una pregunta con su respuesta
        public async Task<bool> Update(Help_Update Model) => await _repository.UpdateAsync(_mapper.Map<Help>(Model));
        
        // Elimina una pregunta con su respuesta
        public async Task<bool> Delete(int Id) => await _repository.Delete(await _repository.GetAsync(x => x.Id == Id));


    }
}
