# Implementation Plan: [FEATURE]

**Branch**: `[###-feature-name]` | **Date**: [DATE] | **Spec**: [link]

**Input**: Feature specification from `/specs/[###-feature-name]/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/plan-template.md` for the execution workflow.

## Summary

[Extract from feature spec: primary requirement + technical approach from research]

## Technical Context

<!--
  ACTION REQUIRED: Replace the content in this section with the technical details
  for the project. The structure here is presented in advisory capacity to guide
  the iteration process.
-->

**Language/Version**: [e.g., .NET 8 (C# 12) or NEEDS CLARIFICATION]

**Primary Dependencies**: [e.g., ASP.NET Core Minimal APIs, Dapper, FluentValidation, Polly, OpenTelemetry]

**Storage**: [if applicable, e.g., SQL Server, Redis, files or N/A]

**Testing**: [e.g., xUnit + tests de integracao quando aplicavel]

**Target Platform**: [e.g., Linux containers em Kubernetes no Azure]

**Project Type**: [e.g., backend-service/microservice]

**Performance Goals**: [domain-specific, e.g., 1000 req/s, 10k lines/sec, 60 fps or NEEDS CLARIFICATION]

**Constraints**: [domain-specific, e.g., <200ms p95, <100MB memory, offline-capable or NEEDS CLARIFICATION]

**Scale/Scope**: [domain-specific, e.g., 10k users, 1M LOC, 50 screens or NEEDS CLARIFICATION]

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- Integracao e Operacao: runtime .NET 8+, contratos de integracao documentados, Health Checks, timeout <= 30s, CancellationToken em operacoes assincronas.
- Persistencia: SQL Server, consultas explicitas, sem SELECT *, paginacao quando aplicavel, mitigacao de N+1.
- Mensageria: Azure Service Bus, DLQ ativa, consumidores idempotentes, retries com Polly, estrategia de Outbox.
- Observabilidade: OpenTelemetry, logs estruturados, CorrelationId, tracing distribuido e metricas.
- Plataforma: workload stateless, readiness/liveness probes, requests/limits declarados, avaliacao de HPA.
- Estrutura de Solucao: padrao Clean Architecture com src/NomeProjeto.Worker, src/NomeProjeto.Domain, src/NomeProjeto.Application, src/NomeProjeto.Infra e tests/NomeProjeto.Tests.
- Qualidade e Seguranca: sem AutoMapper, sem logica de negocio na camada de entrada, autenticacao/autorizacao para interfaces protegidas, validacao de entrada e higienizacao de logs sensiveis.
- Testes: cobertura de unit tests obrigatoria e plano de integration tests para fluxos criticos.
- Evidencia de PR: checklist minimo com runtime .NET 8+, contratos de integracao documentados, timeout/cancellation token, observabilidade, validacao de input, politica de segredos e separacao de camadas.

## Project Structure

### Documentation (this feature)

```text
specs/[###-feature]/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
src/
├── NomeProjeto.Worker/
├── NomeProjeto.Domain/
├── NomeProjeto.Application/
└── NomeProjeto.Infra/

tests/
└── NomeProjeto.Tests/
```

**Structure Decision**: [Manter Clean Architecture obrigatoria; registrar apenas detalhes internos de pastas e modulos por feature]

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| [e.g., 4th project] | [current need] | [why 3 projects insufficient] |
| [e.g., Repository pattern] | [specific problem] | [why direct DB access insufficient] |
