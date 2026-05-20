# Data Model - Worker de Topico e DLQ

## Entidade: MensagemTopico

- Descricao: Mensagem recebida do topico principal para processamento.
- Campos:
  - messageId (string, obrigatorio, unico)
  - correlationId (string, obrigatorio)
  - eventType (string, obrigatorio)
  - payload (json, obrigatorio)
  - enqueuedAtUtc (datetime, obrigatorio)
  - deliveryCount (int, obrigatorio)
- Validacoes:
  - messageId nao pode ser vazio
  - payload deve atender schema minimo por eventType

## Entidade: RegistroProcessamento

- Descricao: Registro persistido do resultado do processamento da mensagem.
- Campos:
  - processingId (guid, PK)
  - messageId (string, obrigatorio, indice)
  - correlationId (string, obrigatorio, indice)
  - status (enum: Received, Validated, Persisted, Failed, DeadLettered, Reprocessed)
  - attempt (int, obrigatorio)
  - failureReason (string, opcional)
  - processedAtUtc (datetime, obrigatorio)
- Relacionamentos:
  - 1 MensagemTopico -> N RegistroProcessamento (historico de tentativas)

## Entidade: ProcessedMessage (Inbox)

- Descricao: Controle de idempotencia para impedir duplicidade de efeito.
- Campos:
  - messageId (string, PK)
  - firstProcessedAtUtc (datetime, obrigatorio)
  - lastStatus (enum, obrigatorio)
- Validacoes:
  - inserir apenas uma vez por messageId

## Entidade: ItemDLQ

- Descricao: Representacao operacional de mensagem encaminhada para dead-letter.
- Campos:
  - messageId (string, obrigatorio)
  - correlationId (string, obrigatorio)
  - deadLetterReason (string, obrigatorio)
  - deadLetterDescription (string, opcional)
  - deadLetteredAtUtc (datetime, obrigatorio)
  - reprocessCount (int, obrigatorio, default 0)
  - lastReprocessAtUtc (datetime, opcional)
  - reprocessStatus (enum: Pending, Reprocessed, FailedAgain)

## State Transitions

- Fluxo principal:
  - Received -> Validated -> Persisted
- Falha transiente:
  - Received -> Validated -> Failed -> (retry) -> Persisted
- Falha definitiva:
  - Received -> Validated -> Failed -> DeadLettered
- Reprocessamento:
  - DeadLettered -> Reprocessed -> Persisted
  - DeadLettered -> Reprocessed -> FailedAgain -> DeadLettered
