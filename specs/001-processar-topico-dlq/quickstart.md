# Quickstart - Worker de Topico e DLQ

## 1. Pre-requisitos

- .NET SDK 8
- Acesso a um namespace Azure Service Bus com topico/subscricao e DLQ
- SQL Server acessivel
- Variaveis de ambiente configuradas para conexao com mensageria e banco

## 2. Estrutura esperada da solucao

- src/TicketProcessor.Worker
- src/TicketProcessor.Domain
- src/TicketProcessor.Application
- src/TicketProcessor.Infra
- tests/TicketProcessor.Tests

## 3. Configuracao minima

- Definir credenciais por identidade gerenciada ou cofre de segredos
- Definir nome de topico/subscricao e parametros de retry
- Definir connection string de SQL Server com politicas de timeout

## 4. Executar localmente

1. Subir dependencias (SQL Server e acesso ao Service Bus).
2. Iniciar o worker host.
3. Publicar mensagens validas no topico.
4. Verificar registros no banco e telemetria de processamento.

## 5. Validar user story 1 (consumo + persistencia)

1. Publicar uma mensagem valida.
2. Confirmar status Persisted no historico.
3. Confirmar ausencia de duplicidade com reenvio da mesma mensagem.

## 6. Validar user story 2 (retry + DLQ)

1. Publicar mensagem invalida.
2. Confirmar tentativas conforme politica de retry.
3. Confirmar envio para DLQ com motivo registrado.

## 7. Validar user story 3 (reprocessamento)

1. Selecionar item da DLQ.
2. Acionar rotina operacional de reprocessamento.
3. Confirmar sucesso (Persisted) ou retorno para DLQ com historico atualizado.

## 8. Observabilidade e saude

- Verificar logs estruturados com correlationId.
- Verificar traces distribuidos e metricas de throughput/erro.
- Verificar readiness/liveness no ambiente de execucao.
