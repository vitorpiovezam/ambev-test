# Projeto de Avaliação de Desenvolvedor - API de Vendas

Esta é a API de Vendas desenvolvida para o teste técnico.

## Requisitos

* Docker e Docker Compose
* .NET 8 SDK

## Como Executar o Projeto do Zero

Siga os passos abaixo para configurar e rodar o ambiente completo.

### 1. Preparar o Ambiente

Clone o repositório e navegue até a pasta `backend` do projeto.

### 2. Subir o Banco de Dados

Para aplicar as migrações do banco de dados, precisamos que o serviço do PostgreSQL esteja no ar.

```bash
docker compose up -d ambev.developerevaluation.database
```

Aguarde cerca de 15 segundos para o banco de dados iniciar completamente.

### 3. Aplicar as Migrações (Criar as Tabelas)

As migrações do Entity Framework criam a estrutura de tabelas no banco de dados.

Primeiro, certifique-se de que a `ConnectionStrings` no arquivo `src/Ambev/DeveloperEvaluation.WebApi/appsettings.Development.json` esteja apontando para `Host=localhost`.

Depois, navegue até a pasta da API e execute o comando:

```bash
cd src/Ambev.DeveloperEvaluation.WebApi
dotnet ef database update
```

Após a conclusão, **lembre-se de reverter** a `ConnectionStrings` no `appsettings.Development.json` para `Host=ambev.developerevaluation.database` para que a API possa se comunicar com o banco de dados de dentro do contêiner.

### 4. Subir a Aplicação Completa

Volte para a pasta `backend` e use o Docker Compose para construir e iniciar todos os serviços.

```bash
# Na pasta /backend
docker compose up --build -d
```

### 5. Testar

A API estará disponível na porta `8080`.

* **Endpoint de Criação de Venda:** `POST http://localhost:8080/api/sales`

A collection do Postman com exemplos de requisição pode ser importada a partir dos comandos cURL disponíveis no projeto.