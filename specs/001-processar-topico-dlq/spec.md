# Feature Specification: Worker de Consumo de Topico e DLQ

**Feature Branch**: `[001-processar-topico-dlq]`

**Created**: 2026-05-20

**Status**: Draft

**Input**: User description: "Criar um worker para ler um topico, processar e inserir no banco de dados e tratar a DLQ"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Processar mensagens validas do topico (Priority: P1)

Como operador da plataforma, quero que o worker consuma mensagens de um topico e persista os dados validos no banco para garantir processamento confiavel e rastreavel.

**Why this priority**: Este e o fluxo principal de valor; sem ele, a solucao nao atende o objetivo de negocio.

**Independent Test**: Publicar mensagem valida no topico e verificar que o worker processa, persiste os dados e registra rastreabilidade da operacao.

**Acceptance Scenarios**:

1. **Given** uma mensagem valida publicada no topico, **When** o worker consumir a mensagem, **Then** os dados devem ser persistidos no banco com confirmacao de processamento.
2. **Given** varias mensagens validas, **When** o worker consumir em sequencia, **Then** cada mensagem deve ser processada uma unica vez sem duplicidade de registros.

---

### User Story 2 - Tratar falhas e encaminhar para DLQ (Priority: P2)

Como operador da plataforma, quero que mensagens com falha de processamento sejam encaminhadas para DLQ apos tentativas controladas para evitar perda silenciosa de dados.

**Why this priority**: Garante resiliencia operacional e recuperacao segura de erros sem bloquear o fluxo principal.

**Independent Test**: Publicar mensagem invalida e validar que, apos tentativas configuradas, a mensagem e direcionada para DLQ com motivo de falha.

**Acceptance Scenarios**:

1. **Given** uma mensagem invalida, **When** o worker falhar no processamento apos o numero maximo de tentativas, **Then** a mensagem deve ser encaminhada para DLQ com metadados de erro.
2. **Given** uma falha temporaria de dependencia, **When** houver nova tentativa dentro da politica de retry, **Then** a mensagem deve ser reprocessada antes de ser descartada para DLQ.

---

### User Story 3 - Reprocessar itens da DLQ com seguranca (Priority: P3)

Como operador da plataforma, quero iniciar reprocessamento de mensagens da DLQ para recuperar eventos falhos apos correcao de causa raiz.

**Why this priority**: Complementa a operacao com capacidade de recuperacao e reduz trabalho manual em incidentes.

**Independent Test**: Selecionar item da DLQ, solicitar reprocessamento e validar que ele volta ao fluxo, com resultado auditavel de sucesso ou nova falha.

**Acceptance Scenarios**:

1. **Given** mensagens disponiveis na DLQ, **When** o operador solicitar reprocessamento, **Then** cada mensagem selecionada deve ser reenviada para processamento com novo registro de tentativa.
2. **Given** uma mensagem que falha novamente no reprocessamento, **When** o processamento terminar, **Then** a mensagem deve retornar a DLQ com historico atualizado.

---

### Edge Cases

- Mensagem duplicada recebida mais de uma vez deve manter idempotencia e nao gerar insercao duplicada.
- Mensagem com schema inesperado deve falhar com motivo claro sem impactar o processamento das demais.
- Indisponibilidade temporaria do banco deve acionar retry e telemetria de falha sem perda de rastreabilidade.
- Pico de mensagens acima da taxa nominal deve manter consistencia de processamento, ainda que com aumento controlado de latencia.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: O sistema DEVE consumir mensagens de um topico de mensageria de forma continua e controlada.
- **FR-002**: O sistema DEVE validar a estrutura e os campos obrigatorios da mensagem antes da persistencia.
- **FR-003**: O sistema DEVE persistir os dados processados com garantia de idempotencia para evitar duplicidade.
- **FR-004**: O sistema DEVE aplicar politica de tentativas de reprocessamento para falhas transientes.
- **FR-005**: O sistema DEVE encaminhar para DLQ mensagens que excederem o limite de tentativas ou possuirem falha nao recuperavel.
- **FR-006**: O sistema DEVE registrar motivo de falha, identificador de correlacao e historico de tentativas para cada mensagem em erro.
- **FR-007**: O sistema DEVE permitir acao operacional para reprocessar mensagens da DLQ de forma seletiva.
- **FR-008**: O sistema DEVE disponibilizar sinalizacao de saude e status de processamento para suporte operacional.

### Non-Functional Requirements

- **NFR-001**: A solucao DEVE manter rastreabilidade ponta a ponta por mensagem, incluindo correlacao entre consumo, persistencia e falhas.
- **NFR-002**: A solucao DEVE suportar processamento assincrono com cancelamento cooperativo e timeout operacional maximo de 30 segundos por operacao externa.
- **NFR-003**: A solucao DEVE registrar logs estruturados, metricas e traces distribuidos para observabilidade.
- **NFR-004**: A solucao DEVE manter confidencialidade de dados sensiveis em logs e trilhas de erro.
- **NFR-005**: A solucao DEVE manter taxa de sucesso de processamento de mensagens validas de, no minimo, 99% em condicoes operacionais normais.

### Architectural Constraints

- A implementacao backend DEVE seguir Clean Architecture com separacao entre Api, Application, Domain, Infra e projeto de testes.
- O worker DEVE operar de forma assincrona e desacoplada de superficie HTTP para o fluxo principal de consumo.
- O fluxo assincrono DEVE prever retries e DLQ para falhas de processamento.
- A persistencia DEVE ser explicita, com consultas paginadas quando aplicavel para operacoes de leitura/listagem.

### Key Entities *(include if feature involves data)*

- **MensagemTopico**: representa a mensagem recebida do topico com identificador unico, payload, timestamp e metadados de correlacao.
- **RegistroProcessamento**: representa o resultado do processamento da mensagem, incluindo status, tentativas, motivo de falha e referencia de persistencia.
- **ItemDLQ**: representa a mensagem encaminhada para DLQ com causa da falha, historico de tentativas e estado de reprocessamento.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 95% das mensagens validas devem ser processadas e persistidas em ate 60 segundos apos publicacao no topico.
- **SC-002**: 100% das mensagens com falha nao recuperavel devem ser encaminhadas para DLQ com motivo de erro registrado.
- **SC-003**: 100% das mensagens processadas (sucesso ou falha) devem conter identificador de correlacao em logs e trilhas de observabilidade.
- **SC-004**: O tempo medio para reprocessar um lote de ate 100 mensagens da DLQ deve ser inferior a 5 minutos.

## Assumptions

- O topico de entrada e a DLQ ja existem e possuem permissoes de acesso configuradas para a aplicacao.
- Existe um esquema de dados definido para persistencia das mensagens processadas no banco.
- Operadores terao permissao para executar reprocessamento da DLQ em ambiente operacional.
- Reprocessamento de DLQ sera acionado sob demanda operacional, nao de forma automatica continua.
