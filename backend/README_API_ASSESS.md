Operação Pato — API: Avaliação de Captura (Capture Assessment)

O que foi adicionado

- Endpoint: POST /api/patos/{id}/assess
  - Recebe o Id (GUID) de um `PatoPrimordial` cadastrado e retorna um `CaptureAssessmentResult` com:
    - OperationalCostScore, CaptureRiskScore, ScientificValueScore (0-100)
    - RequiredForceLevel (Recon/Team/Armored/Heavy)
    - EstimatedTransportKm, EstimatedTransportCost, EstimatedSequencingCost
    - Recommendation (Go / Consider / Defer/Observe)

Como testar localmente (PowerShell)

1. Garanta que o banco de dados foi criado e a migration aplicada. O projeto já contém uma migration e seed que insere patos de exemplo.

2. Inicie a API (na pasta `OperacaoPato.Backend.API`):

```powershell
cd c:\ProjetosCS\Front_End_Desafio_Coder_2025_\backend\OperacaoPato.Backend.API
dotnet run
```

3. Faça uma chamada ao endpoint de avaliação para um pato existente (exemplo com pato semeado):

```powershell
Invoke-RestMethod -Method Post -Uri 'http://localhost:5258/api/patos/11111111-1111-1111-1111-111111111111/assess' -UseBasicParsing | ConvertTo-Json
```

(Substitua a porta se a aplicação expor outra porta.)

Resposta esperada (exemplo):

{
  "patoId": "11111111-1111-1111-1111-111111111111",
  "operationalCostScore": 45.2,
  "captureRiskScore": 62.3,
  "scientificValueScore": 78.9,
  "requiredForceLevel": "Team",
  "estimatedTransportKm": 12.34,
  "estimatedTransportCost": 148.08,
  "estimatedSequencingCost": 6000.0,
  "recommendation": "Consider",
  "notes": ""
}

Observações e próximos passos

- Os cálculos são heurísticos; os pesos e thresholds podem ser calibrados.
- Há testes unitários em `OperacaoPato.Backend.Tests` que cobrem o `CaptureAssessmentService`.
- Recomendado: adicionar testes de integração do endpoint e documentar o contrato OpenAPI/Swagger se necessário.
