# Preparando o ambiente

## Criando o banco de dados a estrutura e dados iniciais do Identity
Abra o terminal, navegue para a pasta onde baixou o projeto e execute o comando abaixo:
```
dotnet ef database update --context IdentityDbContext --project .\src\ContainRs.Api\ContainRs.Api.csproj --startup-project .\src\ContainRs.Api\ContainRs.Api.csproj
```

## Criando o banco de dados a estrutura e dados iniciais de negócio
Abra o terminal, navegue para a pasta onde baixou o projeto e execute o comando abaixo:
```
dotnet ef database update --context AppDbContext --project .\src\ContainRs.Api\ContainRs.Api.csproj --startup-project .\src\ContainRs.Api\ContainRs.Api.csproj
```

## Rodando o projeto

O comando deverá criar o banco de dados e as tabelas necessárias para o funcionamento do projeto.