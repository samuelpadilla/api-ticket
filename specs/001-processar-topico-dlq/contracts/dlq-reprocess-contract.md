# Contract - Reprocessamento de DLQ

## Objetivo

Definir o contrato operacional para reprocessamento seletivo de mensagens da DLQ.

## Requisicao (operacional)

```json
{
  "correlationId": "string",
  "items": [
    {
      "messageId": "string",
      "reason": "string"
    }
  ],
  "requestedBy": "string"
}
```

## Resposta

```json
{
  "correlationId": "string",
  "accepted": 10,
  "rejected": 0,
  "details": [
    {
      "messageId": "string",
      "status": "Accepted|Rejected",
      "reason": "string|null"
    }
  ]
}
```

## Regras

- Reprocessamento deve ser auditavel por `correlationId`.
- Mensagens aceitas retornam ao fluxo principal para nova tentativa.
- Mensagens rejeitadas devem manter motivo explicito e permanecer na DLQ.
- Reprocessamento nao pode burlar validacoes de negocio e idempotencia.
