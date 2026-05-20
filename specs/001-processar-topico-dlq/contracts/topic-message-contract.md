# Contract - Mensagem de Entrada do Topico

## Objetivo

Definir o contrato minimo da mensagem consumida pelo worker no topico principal.

## Envelope

```json
{
  "messageId": "string",
  "correlationId": "string",
  "eventType": "string",
  "occurredAtUtc": "2026-05-20T12:30:00Z",
  "payload": {}
}
```

## Regras

- `messageId`: obrigatorio, unico por evento de negocio.
- `correlationId`: obrigatorio para rastreabilidade.
- `eventType`: obrigatorio, determina validacoes de payload.
- `payload`: obrigatorio, schema especifico por `eventType`.

## Comportamento esperado

- Mensagem valida -> processamento e persistencia.
- Mensagem invalida (schema/regra) -> tentativa conforme politica e dead-letter ao esgotar.
- Mensagem duplicada -> sem duplicidade de efeito (idempotencia).
