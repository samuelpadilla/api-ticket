# Implementation Plan: Worker de Consumo de Topico e DLQ

**Branch**: `[001-processar-topico-dlq]` | **Date**: 2026-05-20 | **Spec**: [specs/001-processar-topico-dlq/spec.md](specs/001-processar-topico-dlq/spec.md)

**Input**: Feature specification from `/specs/001-processar-topico-dlq/spec.md`

## Summary

Implementar um worker .NET 8 para consumir mensagens de topico no Azure Service Bus,
persistir payload validado no SQL Server com Dapper e tratar erros com retry + DLQ,
garantindo idempotencia, rastreabilidade ponta a ponta e reprocessamento operacional.

## Technical Context

**Language/Version**: .NET 8 (C# 12)

**Primary Dependencies**: Azure.Messaging.ServiceBus, Dapper, FluentValidation, Polly, OpenTelemetry, Serilog

**Storage**: SQL Server

**Testing**: xUnit, FluentAssertions, Testcontainers para SQL Server, testes de integracao para fluxo de mensageria

**Target Platform**: Linux containers em Kubernetes no Azure

**Project Type**: backend-service (worker orientado a eventos)

**Performance Goals**:
- p95 de processamento de mensagem valida <= 2s (sem dependencia externa degradada)
- throughput sustentado de 200 msg/min por replica

**Constraints**:
- timeout maximo de 30s por operacao externa
- processamento idempotente (at-least-once sem duplicidade de efeito)
- sem metodos sincronos para IO

**Scale/Scope**:
- 100k mensagens/dia
- ate 10 replicas com escalonamento horizontal
- 3 fluxos principais: consumo, dead-letter, reprocessamento

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- Integracao e Operacao: **PASS**. Runtime .NET 8+, contratos de integracao documentados para consumo/reprocessamento e health checks ativos; para eventual HTTP operacional, aplicar versionamento e OpenAPI.
- Persistencia: **PASS**. SQL Server + Dapper com SQL explicito, sem SELECT *, com paginacao em consultas de listagem operacional.
- Mensageria: **PASS**. Azure Service Bus com DLQ, retry com Polly, idempotencia via Inbox e trilha de correlacao.
- Observabilidade: **PASS**. OpenTelemetry, logs estruturados, CorrelationId e metricas de consumo/erro/reprocessamento.
- Plataforma: **PASS**. Worker stateless, readiness/liveness, requests/limits e estrategia com KEDA/HPA.
- Estrutura de Solucao: **PASS**. Clean Architecture em projetos separados.
- Qualidade e Seguranca: **PASS**. Sem AutoMapper, sem regra de negocio na camada de entrada, validacao com FluentValidation, protecao de interfaces operacionais e mascaramento de dados sensiveis.
- Testes: **PASS**. Unit tests obrigatorios + plano de integracao para topico, persistencia e DLQ.
- Evidencia de PR: **PASS**. Checklist minimo de aderencia com contratos de integracao, timeout/cancellation token, observabilidade, validacao e separacao de camadas sera exigido no PR.

## Project Structure

### Documentation (this feature)

```text
specs/001-processar-topico-dlq/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
└── tasks.md
```

### Source Code (repository root)

```text
src/
├── TicketProcessor.Worker/
├── TicketProcessor.Domain/
├── TicketProcessor.Application/
└── TicketProcessor.Infra/

tests/
└── TicketProcessor.Tests/
```

**Structure Decision**: `TicketProcessor.Worker` hospedara o worker (`BackgroundService`) e interfaces operacionais internas (health/reprocess), enquanto regras de dominio e casos de uso permanecem desacoplados nas camadas Domain/Application.

## Complexity Tracking

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| N/A | N/A | N/A |
