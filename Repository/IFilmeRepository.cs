using Dapper_Dotnet.Models;

namespace Dapper_Dotnet.Repository;

public interface IFilmeRepository
{
    Task<IEnumerable<FilmeResponse>> BuscaFilmesAsync();
    Task<FilmeResponse> BuscaFilmeAsync(int id);
    Task<bool> AdicionarAsync(FilmeRequest request);
    Task<bool> AtualizarAsync(FilmeRequest request, int id);
    Task<bool> DeletarAsync(int id);
}
