# Operação Pato – Front-end + Backend

Projeto full‑stack composto por uma API .NET (DDD + EF Core) e um front‑end React (Vite) para cadastro, visualização e simulação de Patos Primordiais.

## Arquitetura
- Backend (.NET 8)
  - API: `backend/OperacaoPato.Backend.API`
  - Camadas: `Domain`, `Application`, `Infrastructure`, `Shared`
  - ORM: EF Core (SQL Server LocalDB por padrão)
  - Migrations: `backend/OperacaoPato.Backend.Infrastructure/Migrations`
- Front-end (React + Vite)
  - App: `operacao-pato-frontend`
  - Integração: Axios com baseURL configurada em `operacao-pato-frontend/src/services/api.js`

## Requisitos
- .NET SDK 8.0+
- Node.js 18+ (recomendado 20+)
- NPM 9+ (ou PNPM/Yarn se preferir)
- SQL Server LocalDB (Windows) ou SQL Server acessível (ver seção de configuração)

## Primeira execução (desenvolvimento)
1) Backend – preparar banco e iniciar API
- Restaurar e compilar:
  - `dotnet build backend/OperacaoPato.Backend.API/OperacaoPato.Backend.API.csproj -c Debug`
- Criar/Recriar banco e aplicar migrations (com dados seed):
  - `dotnet ef database update --project backend/OperacaoPato.Backend.Infrastructure/OperacaoPato.Backend.Infrastructure.csproj --startup-project backend/OperacaoPato.Backend.API/OperacaoPato.Backend.API.csproj`
- Subir a API (porta padrão 5258):
  - `dotnet run --project backend/OperacaoPato.Backend.API/OperacaoPato.Backend.API.csproj --urls http://localhost:5258`

2) Front-end – instalar e iniciar
- Instalar dependências: `cd operacao-pato-frontend && npm install`
- Conferir baseURL em `operacao-pato-frontend/src/services/api.js` (padrão: `http://localhost:5258/api`)
- Rodar o dev server: `npm run dev`
- Acesse o app (Vite indicará a URL, ex.: http://localhost:5173)

## Fluxo principal
- Patos
  - Listagem/Mapa/Detalhes no front usando os endpoints da API.
  - Cadastro via modal envia `POST /api/patos` com validação no backend.
- Drones
  - Seeds incluem Drones `DRONE-001` … `DRONE-005` para vínculo no cadastro de patos.

## Endpoints relevantes (API)
- `GET /api/patos` – lista patos (DTO)
- `GET /api/patos/{id}` – detalhe
- `POST /api/patos` – cadastra pato (DTO de entrada)
- `POST /api/patos/{id}/assess` – avaliação de captura (risco)
- `GET /api/drones` – lista drones (para popular o select do front)

## Modelo de dados (resumo DTO de entrada POST /api/patos)
- `droneNumeroSerie` (string) – obrigatório
- Altura: `alturaValor` (double > 0), `alturaUnidade` (m|cm|km ou nomes do enum)
- Peso: `pesoValor` (double > 0), `pesoUnidade` (mg|g|kg|t ou nomes do enum)
- Localização: `pais` (string), `cidade` (string), `latitude` (-90..90), `longitude` (-180..180)
- Precisão: `precisao` (double > 0), `precisaoUnidade` (m|cm|km ou nomes do enum)
- Estado: `status` (Desperto|Transe|HibernacaoProfunda)
  - Se Transe/HibernacaoProfunda: `batimentosPorMinuto` > 0 obrigatório
- Mutações: `quantidadeMutacoes` (int >= 0)
- Poder: `poderNome` e `poderDescricao` obrigatórios (para dormente, usamos default como “Dormente”/“Em estado dormente”)
- `dataColetaUtc` (opcional; se omitido, assume UtcNow)

## Seeds padrão
- Aplicados pela migration `20251026134305_FixSeedData.cs` (Infrastructure/Migrations):
  - Drones: DRONE-001 … DRONE-005
  - Patos: 6 registros (inclui exemplos em Desperto, Transe e Hibernação Profunda)

## Variáveis e configurações
- API
  - Porta: editar `launchSettings.json` (projeto API) ou passar `--urls` ao `dotnet run`.
  - Banco: por padrão usa LocalDB. Para usar SQL Server diferente, configure o connection string em `backend/OperacaoPato.Backend.API/appsettings.json` e `appsettings.Development.json`.
- Front-end
  - Base de API: `operacao-pato-frontend/src/services/api.js` (ajuste se mudar host/porta da API).

## Implantação (produção)
- Backend
  - Publicar: `dotnet publish backend/OperacaoPato.Backend.API/OperacaoPato.Backend.API.csproj -c Release -o out`
  - Configurar connection string via variável de ambiente `ConnectionStrings__Default` (ou arquivo appsettings).
  - Executar: `dotnet OperacaoPato.Backend.API.dll` (em contêiner, IIS, Azure App Service etc.).
  - Preparar DB: executar `dotnet ef database update` no ambiente de produção (ou aplicar migrations via CI/CD/entrypoint).
- Front-end
  - Build: `cd operacao-pato-frontend && npm ci && npm run build`
  - Servir a pasta `operacao-pato-frontend/dist` em um servidor estático (Nginx, Apache, S3+CloudFront etc.).
  - Ajustar `baseURL` da API conforme o endpoint público do backend.

## Troubleshooting
- Erro 400 no cadastro
  - Verificar mensagem detalhada exibida no modal (validadores do backend informam o campo).
  - Conferir unidades (`alturaUnidade`, `pesoUnidade`, `precisaoUnidade`) e `status` conforme enum aceito.
- Build/lock de DLL durante o desenvolvimento
  - Fechar a API antes do rebuild (Windows: `taskkill /IM OperacaoPato.Backend.API.exe /F`).
- CORS
  - Se hospedar o front em domínio diferente, habilitar CORS no projeto API (Program.cs/Services).

## Scripts úteis
- Recriar DB (dev):
  - Drop: `dotnet ef database drop -f --project backend/OperacaoPato.Backend.Infrastructure/OperacaoPato.Backend.Infrastructure.csproj --startup-project backend/OperacaoPato.Backend.API/OperacaoPato.Backend.API.csproj`
  - Update: `dotnet ef database update --project backend/OperacaoPato.Backend.Infrastructure/OperacaoPato.Backend.Infrastructure.csproj --startup-project backend/OperacaoPato.Backend.API/OperacaoPato.Backend.API.csproj`

## Estrutura do repositório (resumo)
- `backend/` – solução .NET da API e camadas
- `operacao-pato-frontend/` – aplicação React/Vite
- `README.md` – este guia


