# Especificação da Feature: Worker de Consumo de Tópico e DLQ

**Branch da Feature**: `[001-processar-topico-dlq]`

**Criado em**: 2026-05-20

**Status**: Rascunho

**Entrada**: Descrição do usuário: "Criar um worker para ler um tópico, processar e inserir no banco de dados e tratar a DLQ"

## Cenários de Usuário e Testes *(obrigatório)*

### História de Usuário 1 - Processar mensagens válidas do tópico (Prioridade: P1)

Como operador da plataforma, quero que o worker consuma mensagens de um tópico e persista os dados válidos no banco para garantir processamento confiável e rastreável.

**Por que esta prioridade**: Este é o fluxo principal de valor; sem ele, a solução não atende o objetivo de negócio.

**Teste Independente**: Publicar mensagem válida no tópico e verificar que o worker processa, persiste os dados e registra rastreabilidade da operação.

**Cenários de Aceitação**:

1. **Dado** uma mensagem válida publicada no tópico, **Quando** o worker consumir a mensagem, **Então** os dados devem ser persistidos no banco com confirmação de processamento.
2. **Dado** várias mensagens válidas, **Quando** o worker consumir em sequência, **Então** cada mensagem deve ser processada uma única vez sem duplicidade de registros.

---

### História de Usuário 2 - Tratar falhas e encaminhar para DLQ (Prioridade: P2)

Como operador da plataforma, quero que mensagens com falha de processamento sejam encaminhadas para DLQ após tentativas controladas para evitar perda silenciosa de dados.

**Por que esta prioridade**: Garante resiliência operacional e recuperação segura de erros sem bloquear o fluxo principal.

**Teste Independente**: Publicar mensagem inválida e validar que, após tentativas configuradas, a mensagem é direcionada para DLQ com motivo de falha.

**Cenários de Aceitação**:

1. **Dado** uma mensagem inválida, **Quando** o worker falhar no processamento após o número máximo de tentativas, **Então** a mensagem deve ser encaminhada para DLQ com metadados de erro.
2. **Dado** uma falha temporária de dependência, **Quando** houver nova tentativa dentro da política de retry, **Então** a mensagem deve ser reprocessada antes de ser descartada para DLQ.

---

### História de Usuário 3 - Reprocessar itens da DLQ com segurança (Prioridade: P3)

Como operador da plataforma, quero iniciar reprocessamento de mensagens da DLQ para recuperar eventos falhos após correção de causa raiz.

**Por que esta prioridade**: Complementa a operação com capacidade de recuperação e reduz trabalho manual em incidentes.

**Teste Independente**: Selecionar item da DLQ, solicitar reprocessamento e validar que ele volta ao fluxo, com resultado auditável de sucesso ou nova falha.

**Cenários de Aceitação**:

1. **Dado** mensagens disponíveis na DLQ, **Quando** o operador solicitar reprocessamento, **Então** cada mensagem selecionada deve ser reenviada para processamento com novo registro de tentativa.
2. **Dado** uma mensagem que falha novamente no reprocessamento, **Quando** o processamento terminar, **Então** a mensagem deve retornar a DLQ com historico atualizado.

---

### Casos de Fronteira

- Mensagem duplicada recebida mais de uma vez deve manter idempotencia e nao gerar insercao duplicada.
- Mensagem com schema inesperado deve falhar com motivo claro sem impactar o processamento das demais.
- Indisponibilidade temporaria do banco deve acionar retry e telemetria de falha sem perda de rastreabilidade.
- Pico de mensagens acima da taxa nominal deve manter consistencia de processamento, ainda que com aumento controlado de latencia.

## Requisitos *(obrigatório)*

### Requisitos Funcionais

- **FR-001**: O sistema DEVE consumir mensagens de um topico de mensageria de forma continua, com limite configuravel de concorrencia por instancia (padrao: 10 mensagens em processamento simultaneo) e controle de backpressure quando o lag exceder 1.000 mensagens pendentes.
- **FR-002**: O sistema DEVE validar a estrutura e os campos obrigatorios da mensagem antes da persistencia.
- **FR-003**: O sistema DEVE persistir os dados processados com garantia de idempotencia para evitar duplicidade.
- **FR-004**: O sistema DEVE aplicar politica de tentativas de reprocessamento para falhas transientes.
- **FR-005**: O sistema DEVE encaminhar para DLQ mensagens que excederem o limite de tentativas ou possuirem falha nao recuperavel.
- **FR-006**: O sistema DEVE registrar motivo de falha, identificador de correlacao e historico de tentativas para cada mensagem em erro.
- **FR-007**: O sistema DEVE permitir acao operacional para reprocessar mensagens da DLQ de forma seletiva.
- **FR-008**: O sistema DEVE disponibilizar sinalizacao de saude e status de processamento para suporte operacional.

### Requisitos Não Funcionais

- **NFR-001**: A solucao DEVE manter rastreabilidade ponta a ponta por mensagem, incluindo correlacao entre consumo, persistencia e falhas.
- **NFR-002**: A solucao DEVE suportar processamento assincrono com cancelamento cooperativo e timeout operacional maximo de 30 segundos por operacao externa.
- **NFR-003**: A solucao DEVE registrar logs estruturados, metricas e traces distribuidos para observabilidade.
- **NFR-004**: A solucao DEVE manter confidencialidade de dados sensiveis em logs e trilhas de erro.
- **NFR-005**: A solucao DEVE manter taxa de sucesso de processamento de mensagens validas de, no minimo, 99%, medida em janelas moveis de 24 horas, com base nas metricas de processamento emitidas pelo worker. Para este requisito, "condicoes operacionais normais" significa indisponibilidade acumulada de dependencias externas inferior a 5 minutos por hora.

### Restrições Arquitetônicas

- A implementacao backend DEVE seguir Clean Architecture com separacao entre Worker, Application, Domain, Infra e projeto de testes.
- O worker DEVE operar de forma assincrona e desacoplada de superficie HTTP para o fluxo principal de consumo.
- O fluxo assincrono DEVE prever retries e DLQ para falhas de processamento.
- A persistencia DEVE ser explicita, com consultas paginadas quando aplicavel para operacoes de leitura/listagem.

### Entidades Chave *(include if feature involves data)*

- **MensagemTopico**: representa a mensagem recebida do topico com identificador unico, payload, timestamp e metadados de correlacao.
- **RegistroProcessamento**: representa o resultado do processamento da mensagem, incluindo status, tentativas, motivo de falha e referencia de persistencia.
- **ItemDLQ**: representa a mensagem encaminhada para DLQ com causa da falha, historico de tentativas e estado de reprocessamento.

## Critérios de Sucesso *(obrigatório)*

### Resultados Mensuráveis

- **SC-001**: Em condicoes operacionais normais, o p95 do tempo entre consumo e persistencia DEVE ser menor ou igual a 2 segundos, e 95% das mensagens validas DEVE ser processadas e persistidas em ate 60 segundos apos publicacao no topico.
- **SC-002**: 100% das mensagens com falha nao recuperavel devem ser encaminhadas para DLQ com motivo de erro registrado.
- **SC-003**: 100% das mensagens processadas (sucesso ou falha) devem conter identificador de correlacao em logs e trilhas de observabilidade.
- **SC-004**: O tempo medio para reprocessar um lote de ate 100 mensagens da DLQ deve ser inferior a 5 minutos.

## Assumptions

- O topico de entrada e a DLQ ja existem e possuem permissoes de acesso configuradas para a aplicacao.
- Existe um esquema de dados definido para persistencia das mensagens processadas no banco.
- Operadores terao permissao para executar reprocessamento da DLQ em ambiente operacional.
- Reprocessamento de DLQ sera acionado sob demanda operacional, nao de forma automatica continua.
