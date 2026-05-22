---

description: "Template de lista de tarefas para implementação de features"
---

# Tarefas: [NOME DA FEATURE]

**Entrada**: Documentos de design em `/specs/[###-nome-da-feature]/`

**Pré-requisitos**: plan.md (obrigatório), spec.md (obrigatório para histórias de usuário), research.md, data-model.md, contracts/

**Testes**: Testes unitários são obrigatórios em toda feature. Testes de integração são recomendados para fluxos críticos, contratos e integrações externas.

**Organização**: As tarefas são agrupadas por história de usuário para permitir implementação e testes independentes de cada história.

## Formato: `[ID] [P?] [História] Descrição`

- **[P]**: Pode ser executado em paralelo (arquivos diferentes, sem dependências)
- **[História]**: A qual história de usuário esta tarefa pertence (ex.: US1, US2, US3)
- Inclua caminhos exatos dos arquivos nas descrições

## Convenções de Caminho

- **Clean Architecture obrigatória**:
  - `src/NomeProjeto.Worker`
  - `src/NomeProjeto.Domain`
  - `src/NomeProjeto.Application`
  - `src/NomeProjeto.Infra`
  - `tests/NomeProjeto.Tests`
- Organize tarefas por camada e mantenha dependências de acordo com a constituição.

<!--
  ============================================================================
  IMPORTANTE: As tarefas abaixo são EXEMPLOS ILUSTRATIVOS apenas.

  O comando /speckit.tasks DEVE substituir estas tarefas por tarefas reais baseadas em:
  - Histórias de usuário de spec.md (com suas prioridades P1, P2, P3...)
  - Requisitos da feature de plan.md
  - Entidades de data-model.md
  - Endpoints de contracts/

  As tarefas DEVEM ser organizadas por história de usuário para que cada história possa ser:
  - Implementada independentemente
  - Testada independentemente
  - Entregue como um incremento de MVP

  NÃO mantenha estas tarefas de exemplo no arquivo tasks.md gerado.
  ============================================================================
-->

## Fase 1: Setup (Infraestrutura Compartilhada)

**Propósito**: Projeto inicialização e estrutura básica

- [ ] T001 Criar estrutura do projeto conforme o plano de implementação
- [ ] T002 Inicializar [linguagem] projeto com [framework] dependências
- [ ] T003 [P] Configurar ferramentas de linting e formatação

---

## Fase 2: Fundamentação (Pré-requisitos Bloqueadores)

**Propósito**: Infraestrutura fundamental que DEVE estar completa antes de QUALQUER história de usuário poder ser implementada

**⚠️ CRÍTICO**: Nenhum trabalho de história de usuário pode começar até que esta fase seja completa

Exemplos de tarefas fundamentais (ajuste conforme seu projeto):

- [ ] T004 Criar esquema de banco de dados e framework de migrações
- [ ] T005 [P] Implementar framework de autenticação/autorização
- [ ] T006 [P] Criar pipeline de processamento de worker e estrutura de host middleware
- [ ] T007 Criar modelos base que todas as histórias dependem
- [ ] T008 Configurar infraestrutura de tratamento de erros e logging
- [ ] T009 Criar gerenciamento de configuração de ambiente

**Checkpoint**: Fundação pronta - implementação de histórias de usuário pode começar em paralelo

---

## Fase 3: História de Usuário 1 - [Título] (Prioridade: P1) 🎯 MVP

**Objetivo**: [Breve descrição de o que esta história entrega]

**Teste Independente**: [Como verificar se esta história funciona sozinha]

### Testes para História de Usuário 1 (Testes unitários obrigatorios; testes de integração recomendados) ⚠️

> **NOTE: Escreva estes testes PRIMEIRO, certifique-se de que falham antes da implementação**

- [ ] T010 [P] [US1] Contrato test para [endpoint] em tests/contract/test_[name].py
- [ ] T011 [P] [US1] Teste de integração para [user journey] em tests/integration/test_[name].py

### Implementação para História de Usuário 1

- [ ] T012 [P] [US1] Criar [Entity1] modelo em src/models/[entity1].py
- [ ] T013 [P] [US1] Criar [Entity2] modelo em src/models/[entity2].py
- [ ] T014 [US1] Implementar [Service] em src/services/[service].py (depende de T012, T013)
- [ ] T015 [US1] Implementar [worker flow/feature] em src/[location]/[file].py
- [ ] T016 [US1] Adicionar validação e tratamento de erros
- [ ] T017 [US1] Adicionar logging para operações de história 1

**Checkpoint**: Neste ponto, a História de Usuário 1 deve ser totalmente funcional e testável independentemente

---

## Fase 4: História de Usuário 2 - [Título] (Prioridade: P2)

**Objetivo**: [Breve descrição de o que esta história entrega]

**Teste Independente**: [Como verificar se esta história funciona sozinha]

### Testes para História de Usuário 2 (Testes unitários obrigatorios; testes de integração recomendados) ⚠️

- [ ] T018 [P] [US2] Contrato test para [endpoint] em tests/contract/test_[name].py
- [ ] T019 [P] [US2] Teste de integração para [user journey] em tests/integration/test_[name].py

### Implementação para História de Usuário 2

- [ ] T020 [P] [US2] Criar [Entity] modelo em src/models/[entity].py
- [ ] T021 [US2] Implementar [Service] em src/services/[service].py
- [ ] T022 [US2] Implementar [worker flow/feature] em src/[location]/[file].py
- [ ] T023 [US2] Integrar com componentes da História de Usuário 1 (se necessário)

**Checkpoint**: Neste ponto, as Histórias de Usuário 1 e 2 devem funcionar independentemente

---

## Fase 5: História de Usuário 3 - [Título] (Prioridade: P3)

**Objetivo**: [Breve descrição de o que esta história entrega]

**Teste Independente**: [Como verificar se esta história funciona sozinha]

### Testes para História de Usuário 3 (Testes unitários obrigatorios; testes de integração recomendados) ⚠️

- [ ] T024 [P] [US3] Contrato test para [endpoint] em tests/contract/test_[name].py
- [ ] T025 [P] [US3] Teste de integração para [user journey] em tests/integration/test_[name].py

### Implementação para História de Usuário 3

- [ ] T026 [P] [US3] Criar [Entity] modelo em src/models/[entity].py
- [ ] T027 [US3] Implementar [Service] em src/services/[service].py
- [ ] T028 [US3] Implementar [worker flow/feature] em src/[location]/[file].py

**Checkpoint**: Todas as histórias de usuário devem ser agora funcionalmente independentes

---

[Adicionar mais fases de história de usuário conforme necessário]

---

## Fase N: Polishing & Cross-Cutting Concerns

**Propósito**: Melhorias que afetam múltiplas histórias de usuário

- [ ] TXXX [P] Documentação atualizada em docs/
- [ ] TXXX Code cleanup e refactoring
- [ ] TXXX Performance optimization across all stories
- [ ] TXXX [P] Additional unit tests in tests/unit/
- [ ] TXXX Security hardening
- [ ] TXXX Run quickstart.md validation

---

## Dependências & Execução

### Dependências de Fase

- **Setup (Fase 1)**: Sem dependências - pode começar imediatamente
- **Foundational (Fase 2)**: Depende de Setup completo - BLOCKS todos os stories
- **Histórias de Usuário (Fase 3+)**: Todas dependem da Fase Fundamentação
  - Histórias de usuário podem então prosseguir em paralelo (se staffed)
  - Ou sequencialmente em ordem de prioridade (P1 → P2 → P3)
- **Polishing (Fase Final)**: Depende de todas as histórias desejadas serem completas

### Dependências de História de Usuário

- **História de Usuário 1 (P1)**: Pode começar após Fundamentação (Fase 2) - Sem dependências de outras histórias
- **História de Usuário 2 (P2)**: Pode começar após Fundamentação (Fase 2) - Pode integrar com US1 mas deve ser testável independentemente
- **História de Usuário 3 (P3)**: Pode começar após Fundamentação (Fase 2) - Pode integrar com US1/US2 mas deve ser testável independentemente

### Dentro de Cada História de Usuário

- Testes (se incluídos) DEVEM ser escritos e FAIL antes da implementação
- Modelos antes de serviços
- Serviços antes de endpoints
- Implementação central antes de integração
- História completa antes de passar para a próxima prioridade

### Oportunidades em Paralelo

- Todos os Setup tarefas marcadas [P] podem ser executados em paralelo
- Todos os Foundational tarefas marcadas [P] podem ser executados em paralelo (dentro da Fase 2)
- Uma vez que a Fase Fundamentação termine, todas as histórias podem começar em paralelo (se a capacidade do time permitir)
- Todos os testes para uma história de usuário marcados [P] podem ser executados em paralelo
- Modelos dentro de uma história marcados [P] podem ser executados em paralelo
- Histórias de usuário diferentes podem ser trabalhadas em paralelo por diferentes membros da equipe

---

## Exemplo em Paralelo: História de Usuário 1

```bash
# Lançar todos os testes para História de Usuário 1 juntos (se testes solicitados):
Tarefa: "Contrato test para [endpoint] em tests/contract/test_[name].py"
Tarefa: "Teste de integração para [user journey] em tests/integration/test_[name].py"

# Lançar todos os modelos para História de Usuário 1 juntos:
Tarefa: "Criar [Entity1] modelo em src/models/[entity1].py"
Tarefa: "Criar [Entity2] modelo em src/models/[entity2].py"
```

---

## Estratégia de Implementação

### MVP First (História de Usuário 1 Apenas)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational (CRÍTICO - bloqueia todas as histórias)
3. Complete Phase 3: User Story 1
4. **STOP and VALIDATE**: Test User Story 1 independentemente
5. Deploy/demo se estiver pronto

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 1 → Test independently → Deploy/Demo (MVP!)
3. Add User Story 2 → Test independently → Deploy/Demo
4. Add User Story 3 → Test independently → Deploy/Demo
5. Cada história adiciona valor sem quebrar as histórias anteriores

### Estratégia em Paralelo

Com múltiplos desenvolvedores:

1. Equipe completa Setup + Foundational juntos
2. Uma vez que a Fundamentação estiver completa:
   - Desenvolvedor A: História de Usuário 1
   - Desenvolvedor B: História de Usuário 2
   - Desenvolvedor C: História de Usuário 3
3. Histórias completas e integradas independentemente

---

## Notas

- [P] tarefas = arquivos diferentes, sem dependências
- [Story] label mapeia a tarefa para uma história de usuário para rastreabilidade
- Cada história de usuário deve ser independentemente completável e testável
- Verifique que os testes falham antes da implementação
- Commit após cada tarefa ou grupo lógico
- Pare em qualquer checkpoint para validar a história independentemente
- Evite: tarefas vagas, conflitos de mesmo arquivo, dependências entre histórias que quebrem a independência
