# Gestão de Empreendimentos — Instruções de execução

Este repositório contém dois projetos relacionados:
- Backend C# (.NET 8) em `GestaoDeEmpreendimentos` (API)
- Frontend Angular em `GestaoDeEmpreendimentosUI/gestao-ui`

Abaixo estão instruções para configurar e executar ambos localmente, incluindo como apontar o backend para um banco MySQL, gerar a primeira migration e executar o frontend.

**Pré-requisitos**
- .NET 8 SDK (https://dotnet.microsoft.com)
- MySQL Server (compatível com a versão do provider que o projeto usa)
- dotnet-ef (ferramenta EF Core): `dotnet tool install --global dotnet-ef` (se ainda não instalada)
- Node.js 18+ e npm
- (Opcional) Angular CLI: `npm i -g @angular/cli`

**Observação:** ajuste os caminhos abaixo se você tiver o projeto em local diferente.

**1) Configurar connection string para MySQL**

Abra o arquivo de configuração do backend `GestaoDeEmpreendimentos/appsettings.json` (ou `appsettings.Development.json` se preferir ambiente dev) e localize a seção `ConnectionStrings`.

Exemplo de `appsettings.json` (trecho):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=gestao_db;Uid=seu_usuario;Pwd=sua_senha;"
  }
}
```

- Substitua `Server`, `Port`, `Database`, `Uid` e `Pwd` pelos seus dados.
- Se preferir não versionar segredos, utilize `appsettings.Development.json` ou variáveis de ambiente.

**2) Restaurar dependências e preparar a database (migrations)**

Abra um terminal e navegue até o diretório do backend:

```powershell
cd "E:\Visual Studio\repos\GestaoDeEmpreendimentos\GestaoDeEmpreendimentos"
```

Restore e build:

```powershell
dotnet restore
dotnet build
```

Se o projeto ainda não contém migrations ou se você quiser criar a primeira migration manualmente, execute:

```powershell
# (instale a ferramenta se necessário)
dotnet tool install --global dotnet-ef

# Cria a migration inicial (nome: InitialCreate)
dotnet ef migrations add InitialCreate

# Aplica a migration no banco configurado
dotnet ef database update
```

Observações:
- Se o projeto de API está em uma pasta ou solução diferente, aponte `--project`/`--startup-project` conforme necessário.
- Erros comuns: provider MySQL não instalado/registrado, credenciais incorretas, porta bloqueada.

**3) Executar o backend**

Rode a API localmente:

```powershell
# Executa no diretório do projeto API
dotnet run
```

Por padrão a API costuma subir em `https://localhost:7052` (verifique o output do `dotnet run`).

**4) Configurar e subir o frontend (Angular)**

Instale dependências e inicie o servidor de desenvolvimento:

```powershell
npm install
npm start
```

Observações:
- O projeto já contém um `proxy.conf.json` que encaminha chamadas para `/api` para o backend (`https://localhost:7052`). Certifique-se de que o backend está rodando e que o `proxy.conf.json` aponta para a URL correta.
- Se o backend estiver em outra porta, atualize `proxy.conf.json` ou ajuste o `baseUrl` no serviço do frontend.

**5) Testando fluxos principais**

- Acesse `http://localhost:4200` (endereço padrão do dev server Angular).
- Verifique a listagem de empreendimentos; crie/edite/exclua um registro e veja o backend receber as requisições.
