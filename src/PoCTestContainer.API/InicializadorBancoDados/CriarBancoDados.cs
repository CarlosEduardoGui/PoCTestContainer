using PoCTestContainer.API.Interfaces;

namespace PoCTestContainer.API.InicializadorBancoDados;

public class CriarBancoDados : ICriarBancoDados
{
    private readonly ISqlInterface _sqlInterface;

    public CriarBancoDados(ISqlInterface sqlInterface)
    {
        _sqlInterface = sqlInterface;
    }

    public async void CriarBanco()
    {
        var sql = @"
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Usuario') AND type IN (N'U'))
            BEGIN
                DROP TABLE Usuario;
            END
            BEGIN
                CREATE TABLE Usuario (
                    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                    Nome VARCHAR(200) NOT NULL,
                    Sobrenome VARCHAR(200) NOT NULL,
                    CriadoEm DATETIME NOT NULL,
                    AtualizadoEm DATETIME NULL
                );
            END";

        await _sqlInterface.Execute(sql);
    }
}
