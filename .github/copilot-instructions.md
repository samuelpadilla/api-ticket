<!-- SPECKIT START -->
Plano ativo da feature: `specs/001-processar-topico-dlq/plan.md`.
Use este plano como referencia principal de tecnologia, estrutura,
comandos e decisoes de implementacao.
<!-- SPECKIT END -->

## Regra de Idioma (Obrigatoria)

- Todos os agentes do Copilot neste repositorio devem responder em portugues (pt-BR).
- Toda documentacao gerada pelos agentes deve ser escrita em portugues (pt-BR).
- Quando houver termos tecnicos, comandos, nomes de arquivos, APIs, mensagens de erro, ou trechos de codigo, preserve o original e explique em portugues quando necessario.
- Nao alternar para ingles, exceto quando o usuario solicitar explicitamente outro idioma.

## Instruções para Mensagens de Commit
Todos os commits DEVE seguir o formato do Conventional Commits, com as seguintes regras adicionais:
- O título do commit DEVE ter no máximo 256 caracteres (ideal) e NÃO pode exceder 512 caracteres.
- O corpo do commit DEVE ser usado para detalhes adicionais, quando necessário, e seguir boas práticas de escrita técnica.
- Exemplos de títulos válidos:
  - `feat(worker): adicionar suporte a DLQ com retry configurável`
  - `fix(domain): corrigir validação de payload para mensagens inválidas`
  - `chore: atualizar dependências do projeto`

Justificativa: Um padrão consistente de commits melhora a rastreabilidade, facilita revisões e integrações contínuas.