# PoC TestContainers

Este repositório contém uma Prova de Conceito (PoC) para explorar o uso de **Testcontainers** em um ambiente de testes automatizados. A aplicação é construída em .NET e utiliza o **NUnit** para a execução dos testes de integração, demonstrando como criar e gerenciar containers Docker durante os testes.

## Tecnologias Utilizadas

- **.NET 6**
- **C#**
- **Testcontainers**
- **Docker**
- **Docker Compose**
- **NUnit**

## O que é Testcontainers?

[Testcontainers](https://www.testcontainers.org/) é uma biblioteca leve que suporta a execução de containers Docker durante testes automatizados, permitindo a criação de ambientes reais de banco de dados e outros serviços. Isso facilita a configuração de testes de integração com alta confiabilidade e simplicidade.

### Vantagens do Testcontainers:
- **Isolamento**: Cada teste tem seu próprio ambiente de execução, com containers independentes.
- **Flexibilidade**: Suporta diversos serviços como bancos de dados, filas, entre outros.
- **Simplicidade**: Gerencia os containers automaticamente, sem a necessidade de configurações manuais.

## Como Funciona o Projeto

Este projeto demonstra o uso de Testcontainers para:
1. Criar e gerenciar containers Docker dinamicamente durante os testes.
2. Executar testes de integração que interagem com um **Microsoft SQL Server** rodando em um container.
3. Limpar os containers após a execução dos testes, garantindo que o ambiente permaneça consistente.

### Exemplo de Teste de Integração com Testcontainers (NUnit)

Aqui está um exemplo de como os testes de integração foram configurados usando NUnit e Testcontainers:

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

            // Exemplo de teste: Verificar se a conexão com o banco de dados está aberta
            connection.State.Should().Be(System.Data.ConnectionState.Open);
        }
    }
}
```

Neste exemplo, usamos o Testcontainers para iniciar um container com SQL Server para um teste de conexão ao banco de dados. O container é iniciado antes dos testes e finalizado automaticamente após a execução.

### Executando o Projeto

## Subindo o SQL Server com Docker Compose
Antes de rodar o projeto, é necessário subir o SQL Server localmente usando o docker-compose. Para isso, execute o seguinte comando na raiz do projeto:
```bash
    docker-compose up -d
```
O arquivo docker-compose.yml já está configurado para subir uma instância do SQL Server.

## Executando os Testes
1. Certifique-se de ter o **Docker** instalado e em execução.
2. Clone o repositório:
```bash
    git clone https://github.com/CarlosEduardoGui/PoCTestContainer.git
```
3. Navegue até o diretório do projeto:
```bash
    cd PoCTestContainer
```
4. Restaure as dependências do projeto:
```bash
   dotnet restore
```
5. Execute os testes:
```bash
   dotnet test
```

### Estrutura do Projeto
```bash
├── PoCTestContainer/
│   ├── src/              # Código da aplicação
│   ├── docker-compose.yml # Configuração do SQL Server local
│   └── tests/            # Testes automatizados
│       ├── Integration/  # Testes de integração com uso de containers Docker
│       └── Unit/         # Testes unitários
```

### Requisitos
- [.NET 6 SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/6.0)
- [Docker](https://docs.docker.com/desktop/install/windows-install/)
- [Docker Compose](https://docs.docker.com/compose/)

### Contribuições
Sinta-se à vontade para abrir um PR ou enviar sugestões de melhorias.

### Alterações:
- **NUnit** substituiu o **xUnit** nos exemplos de teste.
- Adicionei a instrução de subir o **SQL Server** localmente usando **docker-compose** antes de rodar os testes.
