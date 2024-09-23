using Dapper;
using PoCTestContainer.API.Interfaces;
using System.Data;

namespace PoCTestContainer.API.InicializadorBancoDados;

public class CriarBancoDados : ICriarBancoDados
{
    private readonly IDbConnection _sqlInterface;

    public CriarBancoDados(IDbConnection sqlInterface)
    {
        _sqlInterface = sqlInterface;
    }

    public void CriarBanco()
    {
        var sql = @"
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Usuario') AND type IN (N'U'))
            BEGIN
                DROP TABLE Usuario;
            END

            CREATE TABLE Usuario (
                Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                Nome VARCHAR(200) NOT NULL,
                Sobrenome VARCHAR(200) NOT NULL,
                CriadoEm DATETIME NOT NULL,
                AtualizadoEm DATETIME NULL
            );";

        _sqlInterface.ExecuteScalar(sql);
    }
}
