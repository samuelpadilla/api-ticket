# Tasks: Worker de Consumo de Topico e DLQ

**Input**: Design documents from `/specs/001-processar-topico-dlq/`

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Unit tests sao obrigatorios em toda feature. Integration tests sao recomendados para fluxos criticos, contratos e integracoes externas.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- **Clean Architecture obrigatoria**:
  - `src/TicketProcessor.Worker`
  - `src/TicketProcessor.Domain`
  - `src/TicketProcessor.Application`
  - `src/TicketProcessor.Infra`
  - `tests/TicketProcessor.Tests`

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Inicializar solucao .NET 8 e estrutura base de projetos da feature.

- [ ] T001 Criar solution e projetos base em `TicketProcessor.slnx`, `src/TicketProcessor.Worker/TicketProcessor.Worker.csproj`, `src/TicketProcessor.Domain/TicketProcessor.Domain.csproj`, `src/TicketProcessor.Application/TicketProcessor.Application.csproj`, `src/TicketProcessor.Infra/TicketProcessor.Infra.csproj`, `tests/TicketProcessor.Tests/TicketProcessor.Tests.csproj`
- [ ] T002 Configurar referencias entre projetos e pacotes principais em `src/TicketProcessor.Worker/TicketProcessor.Worker.csproj`, `src/TicketProcessor.Application/TicketProcessor.Application.csproj`, `src/TicketProcessor.Infra/TicketProcessor.Infra.csproj`, `tests/TicketProcessor.Tests/TicketProcessor.Tests.csproj`
- [ ] T003 [P] Criar configuracoes iniciais de ambiente e observabilidade em `src/TicketProcessor.Worker/appsettings.json`, `src/TicketProcessor.Worker/appsettings.Development.json`, `src/TicketProcessor.Worker/Properties/launchSettings.json`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Entregar infraestrutura comum obrigatoria antes de qualquer historia de usuario.

**⚠️ CRITICAL**: Nenhuma tarefa de US pode iniciar antes desta fase.

- [ ] T004 Criar contratos de configuracao e options para Service Bus/SQL/Retry em `src/TicketProcessor.Application/Configuration/ServiceBusOptions.cs`, `src/TicketProcessor.Application/Configuration/SqlOptions.cs`, `src/TicketProcessor.Application/Configuration/RetryOptions.cs`
- [ ] T005 [P] Implementar bootstrap de DI por camada em `src/TicketProcessor.Application/DependencyInjection.cs`, `src/TicketProcessor.Infra/DependencyInjection.cs`, `src/TicketProcessor.Worker/DependencyInjection.cs`
- [ ] T006 [P] Configurar OpenTelemetry, logs estruturados e correlation middleware em `src/TicketProcessor.Worker/Observability/TelemetrySetup.cs`, `src/TicketProcessor.Worker/Observability/CorrelationContextMiddleware.cs`
- [ ] T007 Implementar abstrações de mensageria e persistencia em `src/TicketProcessor.Application/Abstractions/Messaging/IMessageConsumer.cs`, `src/TicketProcessor.Application/Abstractions/Messaging/IDeadLetterPublisher.cs`, `src/TicketProcessor.Application/Abstractions/Persistence/IProcessingRepository.cs`
- [ ] T008 [P] Criar infraestrutura de SQL com Dapper e fabrica de conexao em `src/TicketProcessor.Infra/Persistence/SqlConnectionFactory.cs`, `src/TicketProcessor.Infra/Persistence/DapperProcessingRepository.cs`
- [ ] T009 [P] Criar cliente de Service Bus e processor base em `src/TicketProcessor.Infra/Messaging/ServiceBusProcessorFactory.cs`, `src/TicketProcessor.Infra/Messaging/ServiceBusTopicConsumer.cs`
- [ ] T010 Implementar health checks e readiness/liveness em `src/TicketProcessor.Worker/Health/ServiceBusHealthCheck.cs`, `src/TicketProcessor.Worker/Health/SqlServerHealthCheck.cs`, `src/TicketProcessor.Worker/Program.cs`
- [ ] T011 Implementar esquema SQL inicial (Inbox + historico de processamento) em `src/TicketProcessor.Infra/Persistence/Scripts/001_init_processing.sql`
- [ ] T012 Definir politicas de retry com Polly e timeout global de operacoes externas em `src/TicketProcessor.Infra/Resilience/RetryPolicies.cs`, `src/TicketProcessor.Infra/Resilience/TimeoutPolicies.cs`

**Checkpoint**: Fundacao pronta - historias de usuario podem ser implementadas em paralelo.

---

## Phase 3: User Story 1 - Processar mensagens validas do topico (Priority: P1) 🎯 MVP

**Goal**: Consumir mensagens validas do topico e persistir no banco com idempotencia e rastreabilidade.

**Independent Test**: Publicar mensagem valida no topico e confirmar persistencia unica com correlationId rastreavel.

### Tests for User Story 1

- [ ] T013 [P] [US1] Criar unit tests de validacao e idempotencia em `tests/TicketProcessor.Tests/Unit/Application/MessageProcessingServiceTests.cs`
- [ ] T014 [P] [US1] Criar integration test de consumo e persistencia no SQL em `tests/TicketProcessor.Tests/Integration/Us1ConsumeAndPersistTests.cs`
- [ ] T015 [P] [US1] Criar contract test da mensagem de topico com base em `contracts/topic-message-contract.md` em `tests/TicketProcessor.Tests/Contract/TopicMessageContractTests.cs`

### Implementation for User Story 1

- [ ] T016 [P] [US1] Implementar entidades de dominio de mensagem e processamento em `src/TicketProcessor.Domain/Entities/TopicMessage.cs`, `src/TicketProcessor.Domain/Entities/ProcessingRecord.cs`, `src/TicketProcessor.Domain/Entities/ProcessedMessage.cs`
- [ ] T017 [P] [US1] Implementar validadores de payload e regras de dominio em `src/TicketProcessor.Application/Validation/TopicMessageValidator.cs`, `src/TicketProcessor.Domain/Rules/MessageSchemaRule.cs`
- [ ] T018 [US1] Implementar caso de uso de processamento principal com CancellationToken em `src/TicketProcessor.Application/UseCases/ProcessTopicMessage/ProcessTopicMessageHandler.cs`
- [ ] T019 [US1] Implementar escrita transacional com Inbox (idempotencia) em `src/TicketProcessor.Infra/Persistence/DapperProcessingRepository.cs`
- [ ] T020 [US1] Implementar worker de consumo principal e ack manual em `src/TicketProcessor.Worker/Workers/TopicConsumerWorker.cs`
- [ ] T021 [US1] Adicionar metrica e logs estruturados de sucesso no fluxo de consumo em `src/TicketProcessor.Worker/Observability/ProcessingMetrics.cs`, `src/TicketProcessor.Worker/Workers/TopicConsumerWorker.cs`

**Checkpoint**: US1 funcional e testavel de forma independente.

---

## Phase 4: User Story 2 - Tratar falhas e encaminhar para DLQ (Priority: P2)

**Goal**: Tratar falhas transientes e nao recuperaveis com retries e encaminhamento confiavel para DLQ.

**Independent Test**: Forcar falha de processamento e verificar retry + dead-letter com motivo registrado.

### Tests for User Story 2

- [ ] T022 [P] [US2] Criar unit tests de classificacao de falhas e retry policy em `tests/TicketProcessor.Tests/Unit/Infra/RetryAndFailureClassificationTests.cs`
- [ ] T023 [P] [US2] Criar integration test de dead-letter apos esgotar tentativas em `tests/TicketProcessor.Tests/Integration/Us2DeadLetterFlowTests.cs`

### Implementation for User Story 2

- [ ] T024 [P] [US2] Implementar modelo de falha e dead-letter no dominio em `src/TicketProcessor.Domain/Entities/DeadLetterItem.cs`, `src/TicketProcessor.Domain/Enums/ProcessingStatus.cs`
- [ ] T025 [US2] Implementar caso de uso de tratamento de erro e decisao retry/dead-letter em `src/TicketProcessor.Application/UseCases/HandleProcessingFailure/HandleProcessingFailureHandler.cs`
- [ ] T026 [US2] Implementar publisher de DLQ e metadados de erro em `src/TicketProcessor.Infra/Messaging/DeadLetterPublisher.cs`
- [ ] T027 [US2] Integrar retry com Polly no worker e registrar historico de tentativas em `src/TicketProcessor.Worker/Workers/TopicConsumerWorker.cs`, `src/TicketProcessor.Infra/Resilience/RetryPolicies.cs`
- [ ] T028 [US2] Persistir motivo de falha e tentativa no historico em `src/TicketProcessor.Infra/Persistence/DapperProcessingRepository.cs`

**Checkpoint**: US1 e US2 funcionais independentemente.

---

## Phase 5: User Story 3 - Reprocessar itens da DLQ com seguranca (Priority: P3)

**Goal**: Permitir reprocessamento seletivo de itens da DLQ com trilha auditavel.

**Independent Test**: Solicitar reprocessamento de itens da DLQ e validar retorno ao fluxo principal com historico atualizado.

### Tests for User Story 3

- [ ] T029 [P] [US3] Criar contract test do reprocessamento com base em `contracts/dlq-reprocess-contract.md` em `tests/TicketProcessor.Tests/Contract/DlqReprocessContractTests.cs`
- [ ] T030 [P] [US3] Criar integration test de reprocessamento seletivo de DLQ em `tests/TicketProcessor.Tests/Integration/Us3DlqReprocessTests.cs`

### Implementation for User Story 3

- [ ] T031 [P] [US3] Implementar caso de uso de reprocessamento seletivo em `src/TicketProcessor.Application/UseCases/ReprocessDlqMessages/ReprocessDlqMessagesHandler.cs`
- [ ] T032 [US3] Implementar leitor de DLQ e reenvio ao topico principal em `src/TicketProcessor.Infra/Messaging/DlqReader.cs`, `src/TicketProcessor.Infra/Messaging/TopicMessageRepublisher.cs`
- [ ] T033 [US3] Implementar endpoint operacional interno de reprocessamento em `src/TicketProcessor.Worker/Endpoints/Operational/ReprocessDlqEndpoint.cs`, `src/TicketProcessor.Worker/Program.cs`
- [ ] T034 [US3] Registrar auditoria de reprocessamento e status final em `src/TicketProcessor.Infra/Persistence/DapperProcessingRepository.cs`

**Checkpoint**: Todas as historias funcionais e testaveis de forma independente.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Consolidar qualidade operacional, seguranca e prontidao de entrega.

- [ ] T035 [P] Atualizar guia operacional e exemplos de execucao em `specs/001-processar-topico-dlq/quickstart.md`
- [ ] T036 [P] Ajustar configuracoes de deploy Kubernetes (readiness/liveness/resources) em `deploy/k8s/ticket-processor-deployment.yaml`
- [ ] T037 Revisar mascaramento de dados sensiveis em logs e tratamento de excecoes em `src/TicketProcessor.Worker/Observability/LogSanitizer.cs`, `src/TicketProcessor.Worker/Workers/TopicConsumerWorker.cs`
- [ ] T038 Executar validacao fim a fim do quickstart e registrar evidencias em `specs/001-processar-topico-dlq/quickstart.md`
- [ ] T039 [P] Definir e documentar contrato de status operacional (saude e processamento) em `specs/001-processar-topico-dlq/contracts/operational-status-contract.md`
- [ ] T040 [P] Criar teste de performance para validar SC-001 (latencia de processamento) em `tests/TicketProcessor.Tests/Performance/Sc001ProcessingLatencyTests.cs`
- [ ] T041 [P] Criar teste de reprocessamento em lote para validar SC-004 em `tests/TicketProcessor.Tests/Performance/Sc004DlqBatchReprocessingTests.cs`
- [ ] T042 Consolidar relatorio de taxa de sucesso (NFR-005) com evidencias de execucao em `specs/001-processar-topico-dlq/quickstart.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: sem dependencias.
- **Foundational (Phase 2)**: depende da conclusao da Phase 1 e bloqueia todas as US.
- **User Stories (Phase 3+)**: dependem da conclusao da Phase 2.
- **Polish (Phase 6)**: depende das historias que forem selecionadas para release.

### User Story Dependencies

- **US1 (P1)**: inicia apos Foundational; entrega MVP funcional.
- **US2 (P2)**: inicia apos Foundational; depende conceitualmente do fluxo de consumo da US1.
- **US3 (P3)**: inicia apos Foundational; depende da existencia de itens em DLQ (US2), mas permanece testavel isoladamente.

### Within Each User Story

- Testes primeiro (unit + integration/contract).
- Entidades/validadores antes dos casos de uso.
- Casos de uso antes de adaptadores/host.
- Integracao e observabilidade por ultimo em cada historia.

### Parallel Opportunities

- T003 pode rodar em paralelo com T001/T002.
- T005, T006, T008 e T009 podem rodar em paralelo dentro da Foundational.
- Em US1: T013, T014, T015 e T016/T017 podem iniciar em paralelo.
- Em US2: T022, T023 e T024 podem iniciar em paralelo.
- Em US3: T029, T030 e T031 podem iniciar em paralelo.
- T035 e T036 podem rodar em paralelo na fase de Polish.
- T039, T040 e T041 podem rodar em paralelo na fase de Polish.

---

## Parallel Example: User Story 1

```bash
# Testes em paralelo
Task: "T013 [US1] MessageProcessingServiceTests"
Task: "T014 [US1] Us1ConsumeAndPersistTests"
Task: "T015 [US1] TopicMessageContractTests"

# Modelagem/validacao em paralelo
Task: "T016 [US1] Entidades de dominio"
Task: "T017 [US1] Validadores e regras"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Concluir Phase 1 e Phase 2.
2. Concluir US1 (T013-T021).
3. Validar processamento com idempotencia e observabilidade.
4. Disponibilizar MVP.

### Incremental Delivery

1. MVP com US1.
2. Adicionar US2 para resiliencia e DLQ.
3. Adicionar US3 para operacao de reprocessamento.
4. Fechar com Polish e validacao operacional.

### Parallel Team Strategy

1. Dev A: base de worker + consumo (US1).
2. Dev B: resiliencia/retry/dead-letter (US2).
3. Dev C: reprocessamento e endpoint operacional (US3).
4. Integracao conjunta em checkpoints por fase.

---

## Notes

- Todos os itens seguem o formato obrigatorio de checklist com ID.
- Tarefas com [P] nao compartilham o mesmo arquivo de implementacao direta.
- Cada US possui criterio de teste independente.
- Tarefas refletem contratos em `contracts/` e entidades em `data-model.md`.
- SC-001, SC-004 e NFR-005 possuem tarefas dedicadas de validacao quantitativa.
