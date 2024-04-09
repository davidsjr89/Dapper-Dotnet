using Azure.Core;
using Dapper;
using Dapper_Dotnet.Models;
using Microsoft.Data.SqlClient;

namespace Dapper_Dotnet.Repository;

public class FilmeRepository : IFilmeRepository
{
    private readonly IConfiguration _configuration;
    private readonly string connectionString;

    public FilmeRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    public async Task<IEnumerable<FilmeResponse>> BuscaFilmesAsync()
    {
        using var con = new SqlConnection(connectionString);
        
        string sql = @"SELECT
                           	   f.Id Id
                                 ,f.Nome Nome
                                 ,f.Ano Ano
                                 ,p.nome Produtora
                           FROM tb_produtora p
                           JOIN tb_filme f ON p.Id = f.id_produtora";
        return await con.QueryAsync<FilmeResponse>(sql);
    }

    public async Task<FilmeResponse> BuscaFilmeAsync(int id)
    {
        using var con = new SqlConnection(connectionString);

        string sql = @"SELECT
	                     f.Id Id
                        ,f.Nome Nome
                        ,f.Ano Ano
                        ,p.nome Produtora
                      FROM tb_produtora p
                      JOIN tb_filme f ON p.Id = f.id_produtora
                      WHERE f.id = @id";
        return await con.QueryFirstOrDefaultAsync<FilmeResponse>(sql, new { Id = id});
    }

    public async Task<bool> AdicionarAsync(FilmeRequest request)
    {
        using var con = new SqlConnection(connectionString);
        string sql = @"INSERT INTO TB_FILME(Nome, Ano, Id_Produtora)
                       VALUES(@Nome, @Ano, @ProdutoraId)";

        return await con.ExecuteAsync(sql, request) > 0; 
    }

    public async Task<bool> AtualizarAsync(FilmeRequest request, int id)
    {
        using var con = new SqlConnection(connectionString);
        string sql = @"UPDATE TB_FILME SET Nome = @Nome, Ano = @Ano
                       WHERE id = @Id";

        var parametros = new DynamicParameters();
        parametros.Add("Ano", request.Ano);
        parametros.Add("Nome", request.Nome);
        parametros.Add("Id", id);

        return await con.ExecuteAsync(sql, parametros) > 0;
    }

    public async Task<bool> DeletarAsync(int id)
    {
        using var con = new SqlConnection(connectionString);
        string sql = @"DELETE FROM TB_FILME
                       WHERE id = @Id";

        return await con.ExecuteAsync(sql, new { Id = id }) > 0;
    }
}
