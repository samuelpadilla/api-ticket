# Fase 0 - Research

## 1) Consumo de mensagens do topico

- Decision: Usar `ServiceBusProcessor` com `AutoCompleteMessages = false` e confirmacao manual.
- Rationale: Garante controle de ack somente apos persistencia bem-sucedida e reduz risco de perda silenciosa.
- Alternatives considered: `ServiceBusReceiver` com loop manual (mais complexo); framework de mensageria adicional (abstracao extra sem necessidade imediata).

## 2) Idempotencia de processamento

- Decision: Implementar padrao Inbox com tabela `ProcessedMessages` no SQL Server, na mesma transacao do dado de negocio.
- Rationale: Modelo de entrega at-least-once pode reenviar mensagens; Inbox impede duplicidade de efeito.
- Alternatives considered: Duplicate detection do broker apenas (janela limitada); lock distribuido externo (complexidade maior e menor atomicidade).

## 3) Persistencia com SQL Server

- Decision: Usar Dapper com SQL explicito e transacoes (`IDbTransaction`) para escrita de negocio + marca de idempotencia.
- Rationale: Atende diretriz de performance e previsibilidade de query, mantendo controle fino de transacao.
- Alternatives considered: ORM completo (mais abstracao e menor controle de SQL).

## 4) Falhas, retry e DLQ

- Decision: Retry com Polly (backoff exponencial + jitter) para falhas transientes e envio para DLQ em falhas nao recuperaveis.
- Rationale: Diferencia falhas transientes de falhas definitivas e evita loops agressivos de reprocessamento.
- Alternatives considered: Retry apenas nativo do SDK (menor flexibilidade para politicas compostas).

## 5) Reprocessamento de DLQ

- Decision: Expor acao operacional de reprocessamento seletivo, com trilha de tentativas e motivo de retorno a DLQ.
- Rationale: Permite recuperacao controlada apos correcao de causa raiz sem operacao manual ad-hoc.
- Alternatives considered: Reprocessamento totalmente manual via ferramenta externa (baixo controle e baixa auditabilidade).

## 6) Observabilidade

- Decision: OpenTelemetry + logs estruturados + propagacao de CorrelationId em propriedades de mensagem.
- Rationale: Rastreabilidade ponta a ponta para diagnostico rapido de falhas e auditoria.
- Alternatives considered: Telemetria proprietaria isolada sem padrao OTel (menor portabilidade).

## 7) Seguranca operacional

- Decision: Autenticacao por identidade gerenciada quando em Azure; fallback com cofre de segredos equivalente fora do Azure.
- Rationale: Reduz exposicao de credenciais e facilita rotacao/auditoria de segredos.
- Alternatives considered: Segredo em variavel sem cofre gerenciado (nao atende governanca).

## 8) Deploy e escalabilidade

- Decision: Deployment stateless com readiness/liveness e escalonamento por carga de fila (KEDA/HPA).
- Rationale: Worker deve escalar por backlog e manter shutdown gracioso sem perder mensagem em processamento.
- Alternatives considered: Escala fixa (baixo aproveitamento e maior risco em picos).
