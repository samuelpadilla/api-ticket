# Plano de Implementação: Worker de Consumo de Tópico e DLQ

**Branch**: `[001-processar-topico-dlq]` | **Data**: 2026-05-20 | **Especificação**: [specs/001-processar-topico-dlq/spec.md](specs/001-processar-topico-dlq/spec.md)

**Entrada**: Especificação da feature em `/specs/001-processar-topico-dlq/spec.md`

## Resumo

Implementar um worker .NET 8 para consumir mensagens de tópico no Azure Service Bus,
persistir payload validado no SQL Server com Dapper e tratar erros com retry + DLQ,
garantindo idempotência, rastreabilidade ponta a ponta e reprocessamento operacional.

## Contexto Técnico

**Linguagem/Versão**: .NET 8 (C# 12)

**Dependências Principais**: Azure.Messaging.ServiceBus, Dapper, FluentValidation, Polly, OpenTelemetry, Serilog

**Armazenamento**: SQL Server

**Testes**: xUnit, FluentAssertions, Testcontainers para SQL Server, testes de integração para fluxo de mensageria

**Plataforma Alvo**: Containers Linux no Kubernetes no Azure

**Tipo de Projeto**: backend-service (worker orientado a eventos)

**Metas de Desempenho**:
- p95 de processamento de mensagem válida <= 2s (sem dependência externa degradada)
- throughput sustentado de 200 msg/min por réplica

**Restrições**:
- timeout máximo de 30s por operação externa
- processamento idempotente (at-least-once sem duplicidade de efeito)
- sem métodos síncronos para IO

**Escala/Escopo**:
- 100k mensagens/dia
- até 10 réplicas com escalonamento horizontal
- 3 fluxos principais: consumo, dead-letter, reprocessamento

## Verificação da Constituição

*PORTÃO: Deve passar antes da pesquisa da Fase 0. Re-verificar após o design da Fase 1.*

- Integração e Operação: **APROVADO**. Runtime .NET 8+, contratos de integração documentados para consumo/reprocessamento e health checks ativos; para eventual HTTP operacional, aplicar versionamento e OpenAPI.
- Persistência: **APROVADO**. SQL Server + Dapper com SQL explícito, sem SELECT *, com paginação em consultas de listagem operacional.
- Mensageria: **APROVADO**. Azure Service Bus com DLQ, retry com Polly, idempotência via Inbox e trilha de correlação.
- Observabilidade: **APROVADO**. OpenTelemetry, logs estruturados, CorrelationId e métricas de consumo/erro/reprocessamento.
- Plataforma: **APROVADO**. Worker stateless, readiness/liveness, requests/limits e estratégia com KEDA/HPA.
- Estrutura de Solução: **APROVADO**. Clean Architecture em projetos separados.
- Qualidade e Segurança: **APROVADO**. Sem AutoMapper, sem regra de negócio na camada de entrada, validação com FluentValidation, proteção de interfaces operacionais e mascaramento de dados sensíveis.
- Testes: **APROVADO**. Unit tests obrigatorios + plano de integração para tópico, persistência e DLQ.
- Evidência de PR: **APROVADO**. Checklist mínimo de aderência com contratos de integração, timeout/cancellation token, observabilidade, validação e separação de camadas será exigido no PR.

## Estrutura do Projeto

### Documentação (esta feature)

```text
specs/001-processar-topico-dlq/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
└── tasks.md
```

### Código Fonte (raiz do repositório)

```text
src/
├── TicketProcessor.Worker/
├── TicketProcessor.Domain/
├── TicketProcessor.Application/
└── TicketProcessor.Infra/

tests/
└── TicketProcessor.Tests/
```

**Decisão de Estrutura**: `TicketProcessor.Worker` hospedará o worker (`BackgroundService`) e interfaces operacionais internas (health/reprocess), enquanto regras de domínio e casos de uso permanecerão desacoplados nas camadas Domain/Application.

## Tracking de Complexidade

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| N/A | N/A | N/A |
