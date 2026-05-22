<!--
Relatório de Impacto de Sincronização
Mudança de versão: 2.0.0 -> 2.0.1
Princípios modificados:
- III. Controle de Versão e Padrões de Ignore -> VI. Controle de Versão e Padrões de Ignore
- Governança -> Governança (relatório de sincronização e status dos templates corrigidos)
Seções adicionadas:
- Nenhuma
Seções removidas:
- Nenhuma
Templates que requerem atualização:
- .specify/templates/plan-template.md ✅ validado
- .specify/templates/spec-template.md ✅ validado
- .specify/templates/tasks-template.md ✅ validado
- .specify/templates/commands/*.md N/A (diretório inexistente neste repositório)
- README.md N/A (arquivo inexistente neste repositório)
- docs/quickstart.md N/A (arquivo inexistente neste repositório)
Tarefas pendentes:
- Nenhuma
-->

# Constituição de Arquitetura Backend

## Convenções do Projeto

Idioma obrigatório: Português do Brasil (pt-BR)
Comentários de código: pt-BR
Documentação: pt-BR
Mensagens para usuário: pt-BR

## Princípios Fundamentais

### I. Contratos de Integração e Operação
Todos os workers em .NET 8+ DEVEM definir contratos explícitos de integração para entrada e saída
(mensageria, arquivos, agendadores ou HTTP quando aplicável).
Quando houver superfície HTTP operacional, contrato OpenAPI e versionamento DEVEM ser aplicados.
Cada serviço DEVE expor Health Checks e DEVE aplicar timeout padrão máximo de 30 segundos
em operações externas. Toda operação assíncrona DEVE receber e propagar CancellationToken.
Justificativa: contratos claros e comportamento operacional previsível reduzem regressão,
aceleram diagnóstico e melhoram confiabilidade em produção.

### II. Persistência Explícita e Eficiente
Persistência transacional DEVE usar SQL Server. Acesso a dados DEVE preferir Dapper,
com queries explícitas e projeção intencional; SELECT * é PROIBIDO.
Toda consulta paginável DEVE implementar paginação e o desenho de consultas DEVE evitar
N+1 por meio de joins/projeções adequadas.
Justificativa: controle explícito da camada de dados melhora performance, previsibilidade e custo.

### III. Mensageria Confiável e Consistente
Integrações assíncronas DEVEM usar Azure Service Bus. Toda fila ou tópico DEVE ter
Dead Letter Queue ativa. Consumidores DEVEM ser idempotentes e DEVEM aplicar política de retries
com Polly. Fluxos que combinam escrita em banco e publicação de evento DEVEM usar Outbox Pattern
ou alternativa equivalente com garantia de consistência. Mensagens DEVEM manter rastreabilidade
com identificadores de correlação.
Justificativa: consistência e recuperação controlada evitam perda de eventos e duplicidade de efeitos.

### IV. Observabilidade e Resiliência por Padrão
OpenTelemetry, logs estruturados, CorrelationId, tracing distribuído e métricas DEVEM estar
presentes em todos os serviços. Endpoints e consumidores DEVEM registrar falhas com contexto
operacional suficiente sem expor dados sensíveis.
Justificativa: sem telemetria padronizada não há operação segura nem melhoria contínua baseada em dados.

### V. Qualidade de Código, Segurança e Performance
Código DEVE aplicar SOLID e Clean Architecture, mantendo responsabilidades pequenas e
separação entre transporte, domínio e infraestrutura. Lógica de negócio na camada de entrada
é proibida. Inputs DEVEM ser validados com FluentValidation ou estratégia equivalente.
Interfaces operacionais protegidas DEVEM aplicar autenticação e autorização conforme padrão corporativo.
Operações de IO DEVEM ser assíncronas, com minimização de alocação e serialização desnecessária.
Testes unitários são obrigatórios; testes de integração são recomendados para fluxos críticos e contratos externos.
Justificativa: qualidade estrutural, segurança e eficiência sustentam evolução de longo prazo.

### VI. Controle de Versão e Padrões de Ignore
Todo repositório DEVE incluir um arquivo `.gitignore` configurado para projetos C# e .NET.
Padrões obrigatórios incluem:
- `bin/`, `obj/` para artefatos de build.
- Arquivos de configuração de IDEs como `.vs/`, `.vscode/`, `.idea/`.
- Logs (`*.log`) e arquivos temporários (`*.tmp`, `*.swp`).
- Diretórios de dependências como `node_modules/`.
- Arquivos de cobertura de testes (`coverage/`).

Justificativa: um `.gitignore` bem configurado reduz ruído no controle de versão e melhora a colaboração.

## Plataforma e Operação em Kubernetes

Aplicações DEVEM ser stateless e configuradas via variáveis de ambiente.
Readiness e Liveness probes são obrigatórios para workloads em cluster.
Todo deployment DEVE declarar resource requests e limits.
HPA DEVE ser configurado para cargas críticas e serviços com variação de throughput.

## Arquitetura de Solução e Estrutura de Projetos

A solução DEVE seguir Clean Architecture com separação explícita por projetos.
A estrutura padrão obrigatória da raiz do repositório é:

- src/NomeProjeto.Worker
- src/NomeProjeto.Domain
- src/NomeProjeto.Application
- src/NomeProjeto.Infra
- tests/NomeProjeto.Tests

Regras de dependência entre camadas:
- NomeProjeto.Worker DEVE depender de NomeProjeto.Application e PODE referenciar NomeProjeto.Infra somente para composition root (registro de DI e wiring de adaptadores), sem conter lógica de negócio.
- NomeProjeto.Application DEVE conter casos de uso, contratos e regras de orquestração da aplicação.
- NomeProjeto.Domain DEVE conter regras de negócio centrais e NÃO DEVE depender de infraestrutura.
- NomeProjeto.Infra DEVE implementar adaptadores externos (banco, mensageria, cache, clientes externos) e NÃO DEVE conter regra de negócio de domínio.
- NomeProjeto.Tests DEVE cobrir, no mínimo, casos de uso e regras de domínio críticas, mantendo testes determinísticos.

## Fluxo de Desenvolvimento e Qualidade

Cada mudança DEVE demonstrar conformidade com esta constituição no plano de implementação,
incluindo evidências de integração, persistência, mensageria, observabilidade, segurança e testes.
Code review DEVE bloquear merges com violações não justificadas.
Exceções arquiteturais DEVEM conter justificativa técnica, risco assumido e plano de remediação.

Proibições não negociáveis:
- Nunca utilizar AutoMapper.
- Nunca implementar lógica de negócio na camada de entrada (worker host, endpoints operacionais, gatilhos).
- Nunca utilizar métodos síncronos para IO.
- Nunca ignorar CancellationToken em operações assíncronas.
- Nunca criar serviços com múltiplas responsabilidades.
- Nunca acessar banco diretamente da camada de entrada.

## Governança

Esta constituição prevalece sobre convenções locais de feature quando houver conflito.
Mudanças neste documento DEVEM ser propostas via PR com justificativa e análise de impacto
nos templates e comandos do Spec Kit.

Política de versionamento da constituição:
- MAJOR: remoção ou redefinição incompatível de princípios ou gates obrigatórios.
- MINOR: adição de novo princípio, seção ou obrigatoriedade material.
- PATCH: clarificações editoriais sem alterar exigências normativas.

Revisão de conformidade:
- Todo plano DEVE registrar Constitution Check com evidências verificáveis.
- Todo conjunto de tarefas DEVE refletir testes obrigatórios, observabilidade e segurança.
- Toda PR DEVE incluir checklist de aderência com, no mínimo: runtime .NET 8+, contratos de integração documentados, timeout e CancellationToken, observabilidade, validação de input, política de segredos e separação de camadas.
- Toda PR DEVE registrar desvios aprovados explicitamente.

**Versão**: 2.0.1 | **Ratificado**: 2026-05-19 | **Última Emenda**: 2026-05-21
