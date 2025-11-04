using Api.Shared.DTOs.Help;

namespace Api.Shared.Interface
{
    public interface IHelpServices
    {
        Task<Help_Create> Add(Help_Create Model);
        Task<bool> Delete(int Id);
        Task<bool> ExistQuestion(string Question, int Id);
        Task<Help_Get> Get(int Id);
        Task<List<Help_Get>> GetList();
        Task<bool> Update(Help_Update Model);
    }
}