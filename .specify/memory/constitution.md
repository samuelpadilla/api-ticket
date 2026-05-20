<!--
Sync Impact Report
Version change: 1.2.0 -> 2.0.0
Modified principles:
- I. Contratos de API e Operacao -> I. Contratos de Integracao e Operacao (worker-first)
- V. Qualidade de Codigo, Seguranca e Performance -> V. Qualidade de Codigo, Seguranca e Performance (seguranca de interfaces operacionais)
- Arquitetura de Solucao e Estrutura de Projetos (Api -> Worker)
Added sections:
- Nenhuma
Removed sections:
- Nenhuma
Templates requiring updates:
- .specify/templates/plan-template.md ✅ updated
- .specify/templates/spec-template.md ✅ updated
- .specify/templates/tasks-template.md ✅ updated
- .specify/templates/commands/*.md ⚠ pending (diretorio nao existe neste repositorio)
Follow-up TODOs:
- Nenhum
-->

# Backend Architecture Constitution

## Core Principles

### I. Contratos de Integracao e Operacao
Todos os workers em .NET 8+ DEVE definir contratos explicitos de integracao para entrada e saida
(mensageria, arquivos, agendadores ou HTTP quando aplicavel).
Quando houver superficie HTTP operacional, contrato OpenAPI e versionamento DEVE ser aplicado.
Cada servico DEVE expor Health Checks e DEVE aplicar timeout padrao maximo de 30 segundos
em operacoes externas. Toda operacao assincrona DEVE receber e propagar CancellationToken.
Justificativa: contratos claros e comportamento operacional previsivel reduzem regressao,
aceleram diagnostico e melhoram confiabilidade em producao.

### II. Persistencia Explicita e Eficiente
Persistencia transacional DEVE usar SQL Server. Acesso a dados DEVE preferir Dapper,
com queries explicitas e projection intencional; SELECT * e PROIBIDO.
Toda consulta paginavel DEVE implementar paginacao e o desenho de consultas DEVE evitar
N+1 por meio de joins/projecoes adequadas.
Justificativa: controle explicito da camada de dados melhora performance, previsibilidade e custo.

### III. Mensageria Confiavel e Consistente
Integracoes assincronas DEVE usar Azure Service Bus. Toda fila ou topico DEVE ter
Dead Letter Queue ativa. Consumidores DEVE ser idempotentes e DEVE aplicar retry policy
com Polly. Fluxos que combinam escrita em banco e publicacao de evento DEVE usar Outbox Pattern
ou alternativa equivalente com garantia de consistencia. Mensagens DEVE manter rastreabilidade
com identificadores de correlacao.
Justificativa: consistencia e recuperacao controlada evitam perda de eventos e duplicidade de efeitos.

### IV. Observabilidade e Resiliencia por Padrao
OpenTelemetry, logs estruturados, CorrelationId, tracing distribuido e metricas DEVE estar
presentes em todos os servicos. Endpoints e consumidores DEVE registrar falhas com contexto 
operacional suficiente sem expor dados sensiveis.
Justificativa: sem telemetria padronizada nao ha operacao segura nem melhoria continua baseada em dados.

### V. Qualidade de Codigo, Seguranca e Performance
Codigo DEVE aplicar SOLID e Clean Architecture, mantendo responsabilidades pequenas e
separacao entre transporte, dominio e infraestrutura. Logica de negocio na camada de entrada
e proibida. Inputs DEVE ser validados com FluentValidation ou estrategia equivalente.
Interfaces operacionais protegidas DEVE aplicar autenticacao e autorizacao conforme padrao corporativo.
Operacoes de IO DEVE ser assincronas,
com minimizacao de alocacao e serializacao desnecessaria. Unit tests sao obrigatorios;
integration tests sao recomendados para fluxos criticos e contratos externos.
Justificativa: qualidade estrutural, seguranca e eficiencia sustentam evolucao de longo prazo.

## Plataforma e Operacao em Kubernetes

Aplicacoes DEVE ser stateless e configuradas via variaveis de ambiente.
Readiness e Liveness probes sao obrigatorios para workloads em cluster.
Todo deployment DEVE declarar resource requests e limits.
HPA DEVE ser configurado para cargas criticas e servicos com variacao de throughput.

## Arquitetura de Solucao e Estrutura de Projetos

A solucao DEVE seguir Clean Architecture com separacao explicita por projetos.
A estrutura padrao obrigatoria da raiz do repositorio e:

- src/NomeProjeto.Worker
- src/NomeProjeto.Domain
- src/NomeProjeto.Application
- src/NomeProjeto.Infra
- tests/NomeProjeto.Tests

Regras de dependencia entre camadas:
- NomeProjeto.Worker DEVE depender de NomeProjeto.Application e PODE referenciar NomeProjeto.Infra somente para composition root (registracao de DI e wiring de adaptadores), sem conter logica de negocio.
- NomeProjeto.Application DEVE conter casos de uso, contratos e regras de orquestracao da aplicacao.
- NomeProjeto.Domain DEVE conter regras de negocio centrais e nao DEVE depender de infraestrutura.
- NomeProjeto.Infra DEVE implementar adaptadores externos (banco, mensageria, cache, clientes externos) e nao DEVE conter regra de negocio de dominio.
- NomeProjeto.Tests DEVE cobrir, no minimo, casos de uso e regras de dominio criticas, mantendo testes deterministas.

## Fluxo de Desenvolvimento e Qualidade

Cada mudanca DEVE demonstrar conformidade com esta constituicao no plano de implementacao,
incluindo evidencias de API, persistencia, mensageria, observabilidade, seguranca e testes.
Code review DEVE bloquear merges com violacoes nao justificadas.
Excecoes arquiteturais DEVE conter justificativa tecnica, risco assumido e plano de remediacao.

Proibicoes nao negociaveis:
- Nunca utilizar AutoMapper.
- Nunca implementar logica de negocio na camada de entrada (worker host, endpoints operacionais, gatilhos).
- Nunca utilizar metodos sincronos para IO.
- Nunca ignorar CancellationToken em operacoes assincronas.
- Nunca criar servicos com multiplas responsabilidades.
- Nunca acessar banco diretamente da camada de entrada.

## Governance

Esta constituicao prevalece sobre convencoes locais de feature quando houver conflito.
Mudancas neste documento DEVE ser propostas via PR com justificativa e analise de impacto
nos templates e comandos do Spec Kit.

Politica de versionamento da constituicao:
- MAJOR: remocao ou redefinicao incompativel de principios/gates obrigatorios.
- MINOR: adicao de novo principio, secao ou obrigatoriedade material.
- PATCH: clarificacoes editoriais sem alterar exigencias normativas.

Revisao de compliance:
- Todo plano DEVE registrar Constitution Check com evidencias verificaveis.
- Todo conjunto de tarefas DEVE refletir testes obrigatorios, observabilidade e seguranca.
- Toda PR DEVE incluir checklist de aderencia com, no minimo: runtime .NET 8+, contratos de integracao documentados, timeout/cancellation token, observabilidade (logs+traces+metricas), validacao de input, politica de segredos e separacao de camadas.
- Toda PR DEVE registrar desvios aprovados explicitamente.

**Version**: 2.0.0 | **Ratified**: 2026-05-19 | **Last Amended**: 2026-05-20
