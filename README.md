# PoC TestContainers

Este reposit�rio cont�m uma Prova de Conceito (PoC) para explorar o uso de **Testcontainers** em um ambiente de testes automatizados. A aplica��o � constru�da em .NET e utiliza o **NUnit** para a execu��o dos testes de integra��o, demonstrando como criar e gerenciar containers Docker durante os testes.

## Tecnologias Utilizadas

- **.NET 6**
- **C#**
- **Testcontainers**
- **Docker**
- **Docker Compose**
- **NUnit**

## O que � Testcontainers?

[Testcontainers](https://www.testcontainers.org/) � uma biblioteca leve que suporta a execu��o de containers Docker durante testes automatizados, permitindo a cria��o de ambientes reais de banco de dados e outros servi�os. Isso facilita a configura��o de testes de integra��o com alta confiabilidade e simplicidade.

### Vantagens do Testcontainers:
- **Isolamento**: Cada teste tem seu pr�prio ambiente de execu��o, com containers independentes.
- **Flexibilidade**: Suporta diversos servi�os como bancos de dados, filas, entre outros.
- **Simplicidade**: Gerencia os containers automaticamente, sem a necessidade de configura��es manuais.

## Como Funciona o Projeto

Este projeto demonstra o uso de Testcontainers para:
1. Criar e gerenciar containers Docker dinamicamente durante os testes.
2. Executar testes de integra��o que interagem com um **Microsoft SQL Server** rodando em um container.
3. Limpar os containers ap�s a execu��o dos testes, garantindo que o ambiente permane�a consistente.

### Exemplo de Teste de Integra��o com Testcontainers (NUnit)

Aqui est� um exemplo de como os testes de integra��o foram configurados usando NUnit e Testcontainers:

```csharp
using NUnit.Framework;
using Testcontainers.MsSql;
using FluentAssertions;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class DatabaseTests
    {
        private MsSqlContainer _mssqlContainer;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _mssqlContainer = new MsSqlBuilder().Build();
            await _mssqlContainer.StartAsync();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _mssqlContainer.DisposeAsync();
        }

        [Test]
        public async Task TestDatabaseConnection()
        {
            using var connection = new SqlConnection(_mssqlContainer.GetConnectionString());
            await connection.OpenAsync();

            // Exemplo de teste: Verificar se a conex�o com o banco de dados est� aberta
            connection.State.Should().Be(System.Data.ConnectionState.Open);
        }
    }
}
```

Neste exemplo, usamos o Testcontainers para iniciar um container com SQL Server para um teste de conex�o ao banco de dados. O container � iniciado antes dos testes e finalizado automaticamente ap�s a execu��o.

### Executando o Projeto

## Subindo o SQL Server com Docker Compose
Antes de rodar o projeto, � necess�rio subir o SQL Server localmente usando o docker-compose. Para isso, execute o seguinte comando na raiz do projeto:
```bash
    docker-compose up -d
```
O arquivo docker-compose.yml j� est� configurado para subir uma inst�ncia do SQL Server.

## Executando os Testes
1. Certifique-se de ter o **Docker** instalado e em execu��o.
2. Clone o reposit�rio:
```bash
    git clone https://github.com/CarlosEduardoGui/PoCTestContainer.git
```
3. Navegue at� o diret�rio do projeto:
```bash
    cd PoCTestContainer
```
4. Restaure as depend�ncias do projeto:
```bash
   dotnet restore
```
5. Execute os testes:
```bash
   dotnet test
```

### Estrutura do Projeto
```bash
??? PoCTestContainer/
?   ??? src/              # C�digo da aplica��o
?   ??? docker-compose.yml # Configura��o do SQL Server local
?   ??? tests/            # Testes automatizados
?       ??? Integration/  # Testes de integra��o com uso de containers Docker
?       ??? Unit/         # Testes unit�rios
```

### Requisitos
- [.NET 6 SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/6.0)
- [Docker](https://docs.docker.com/desktop/install/windows-install/)
- [Docker Compose](https://docs.docker.com/compose/)

### Contribui��es
Sinta-se � vontade para abrir um PR ou enviar sugest�es de melhorias.

### Altera��es:
- **NUnit** substituiu o **xUnit** nos exemplos de teste.
- Adicionei a instru��o de subir o **SQL Server** localmente usando **docker-compose** antes de rodar os testes.
