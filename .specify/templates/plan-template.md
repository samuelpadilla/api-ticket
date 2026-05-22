# Plano de Implementação: [FEATURE]

**Branch**: `[###-nome-da-feature]` | **Data**: [DATA] | **Especificação**: [link]

**Entrada**: Especificação da feature em `/specs/[###-nome-da-feature]/spec.md`

**Nota**: Este template é preenchido pelo comando `/speckit.plan`. Veja `.specify/templates/plan-template.md` para o fluxo de execução.

## Resumo

[Extraído da especificação da feature: requisito principal + abordagem técnica da pesquisa]

## Contexto Técnico

<!--
  AÇÃO NECESSÁRIA: Substitua o conteúdo desta seção com os detalhes técnicos
  do projeto. A estrutura aqui é apresentada como orientação para guiar
  o processo de iteração.
-->

**Linguagem/Versão**: [ex.: .NET 8 (C# 12) ou NECESSITA ESCLARECIMENTO]

**Dependências Principais**: [ex.: ASP.NET Core Minimal APIs, Dapper, FluentValidation, Polly, OpenTelemetry]

**Armazenamento**: [se aplicável, ex.: SQL Server, Redis, arquivos ou N/A]

**Testes**: [ex.: xUnit + testes de integração quando aplicável]

**Plataforma Alvo**: [ex.: containers Linux no Kubernetes no Azure]

**Tipo de Projeto**: [ex.: backend-service/microservice]

**Metas de Desempenho**: [específico do domínio, ex.: 1000 req/s, 10k linhas/seg, 60 fps ou NECESSITA ESCLARECIMENTO]

**Restrições**: [específico do domínio, ex.: <200ms p95, <100MB memória, capacidade offline ou NECESSITA ESCLARECIMENTO]

**Escala/Escopo**: [específico do domínio, ex.: 10k usuários, 1M LOC, 50 telas ou NECESSITA ESCLARECIMENTO]

## Verificação da Constituição

*PORTÃO: Deve passar antes da pesquisa da Fase 0. Re-verificar após o design da Fase 1.*

- Integração e Operação: runtime .NET 8+, contratos de integração documentados, Health Checks, timeout <= 30s, CancellationToken em operações assíncronas.
- Persistência: SQL Server, consultas explícitas, sem SELECT *, paginação quando aplicável, mitigação de N+1.
- Mensageria: Azure Service Bus, DLQ ativa, consumidores idempotentes, retries com Polly, estratégia de Outbox.
- Observabilidade: OpenTelemetry, logs estruturados, CorrelationId, tracing distribuído e métricas.
- Plataforma: workload stateless, readiness/liveness probes, requests/limits declarados, avaliação de HPA.
- Estrutura de Solução: padrão Clean Architecture com src/NomeProjeto.Worker, src/NomeProjeto.Domain, src/NomeProjeto.Application, src/NomeProjeto.Infra e tests/NomeProjeto.Tests.
- Qualidade e Segurança: sem AutoMapper, sem lógica de negócio na camada de entrada, autenticação/autorização para interfaces protegidas, validação de entrada e higienização de logs sensíveis.
- Testes: cobertura de testes unitários obrigatória e plano de testes de integração para fluxos críticos.
