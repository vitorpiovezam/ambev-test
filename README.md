# Projeto de Avaliação de Desenvolvedor - API de Vendas

Video Demo: https://youtu.be/m8v1JmBDCbU?si=9n2Wk7SvxwDP7lBE

Postman: https://sadasdsad-9363.postman.co/workspace/Teste-Ambev~1817fbb4-8637-4a86-8abb-f8c6108b8e56/collection/5150766-4d77f5d6-287c-4a0a-a2ca-84828367eac1?action=share&creator=5150766

Esta é a API de Vendas desenvolvida para o teste técnico.

## Requisitos

* Docker e Docker Compose
* .NET 8 SDK

## Como Executar o Projeto do Zero

Siga os passos abaixo para configurar e rodar o ambiente completo.

### 1. Subir o Banco de Dados

Para aplicar as migrações do banco de dados, precisamos que o serviço do PostgreSQL esteja no ar.

```bash
docker compose up -d ambev.developerevaluation.database
```

### 2. Aplicar as Migrações (Criar as Tabelas)

As migrações do Entity Framework criam a estrutura de tabelas no banco de dados.

Primeiro, certifique-se de que a `ConnectionStrings` no arquivo `src/Ambev/DeveloperEvaluation.WebApi/appsettings.Development.json` esteja apontando para `Host=localhost`.

Depois, navegue até a pasta da API e execute o comando:

```bash
cd src/Ambev.DeveloperEvaluation.WebApi
dotnet ef database update
```

Após a conclusão, **lembre-se de reverter** a `ConnectionStrings` no `appsettings.Development.json` para `Host=ambev.developerevaluation.database` para que a API possa se comunicar com o banco de dados de dentro do contêiner.

### 3. Subir a Aplicação Completa

Use o Docker Compose para construir e iniciar todos os serviços.

```bash
# Na pasta /backend
docker compose up --build -d
```

### 4. Testar

A API estará disponível na porta `8080`.

* **Endpoint de Criação de Venda:** `POST http://localhost:8080/api/sales`

A collection do Postman pode ser conferida aqui (não consegui exportar o arquivo) -> https://sadasdsad-9363.postman.co/workspace/Teste-Ambev~1817fbb4-8637-4a86-8abb-f8c6108b8e56/request/5150766-0030e99c-d36e-4e3b-b545-5271052f9f10?action=share&creator=5150766&ctx=documentation

## Rodando os Testes Automatizados

O projeto possui uma suíte de testes de unidade para validar as regras de negócio. Na pasta raíz rode:

```bash
cd src/Ambev.DeveloperEvaluation.Tests/
dotnet test
```

Todos os testes devem passar com sucesso.


----------------------------------------------------------------------

Esse projeto foi feito e rodado em ambiente WSL2 com Debian, qualquer problema me dá um toque no e-mail.

Obrigado 
Made with ❤ by [Vitor Piovezam](mailto::vitor@piovezam.ru)
