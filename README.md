# PokéDraft

Simulador de batalha Pokémon entre dois jogadores. Cada jogador monta o time
através de um **draft** de Pokémon e itens (com orçamento em pontos, banimentos,
compra e venda) e depois o sistema **simula a batalha** 1×1 entre os times.

Projeto da disciplina **Desenvolvimento de Software Visual**.

## Tecnologias

| Camada | Tecnologia |
|---|---|
| Backend | C# / ASP.NET Core MVC (.NET 8) |
| API | Controllers REST (CRUD) retornando JSON |
| Banco de dados | SQLite + Entity Framework Core 8 |
| Frontend | HTML + CSS + JavaScript (`fetch`), em `wwwroot/` |
| Dados de Pokémon | Importados da [PokéAPI](https://pokeapi.co/) |
| Versionamento | Git + GitHub |
| Gestão | Trello (Kanban) |

## Pré-requisitos (configuração do ambiente em cada máquina)

1. **.NET SDK 8.0** — https://dotnet.microsoft.com/download/dotnet/8.0
   Teste: `dotnet --version` (deve mostrar `8.x`).
   > O arquivo `global.json` trava o projeto no SDK 8, mesmo que a máquina
   > também tenha versões mais novas instaladas.
2. **Git** — https://git-scm.com/downloads
3. **Visual Studio Code** — https://code.visualstudio.com/ com as extensões
   **C#** e **C# Dev Kit**.
4. **Ferramenta de linha de comando do EF Core** (uma vez por máquina):
   ```bash
   dotnet tool install --global dotnet-ef --version 8.*
   ```
   Teste: `dotnet ef --version`.

## Como rodar o projeto

```bash
git clone https://github.com/eduardowoz/PokeDraft-DesenvolvimentoSoftwereVIsual.git
cd PokeDraft-DesenvolvimentoSoftwereVIsual
dotnet restore
dotnet ef database update --project PokeDraft.Web   # cria o arquivo pokedraft.db
dotnet run --project PokeDraft.Web
```

Abra no navegador o endereço mostrado no terminal (algo como `http://localhost:5xxx`).

> O banco (`pokedraft.db`) **não** é versionado — cada integrante gera o seu
> localmente a partir das migrations. O que vai para o Git são os arquivos
> em `PokeDraft.Web/Migrations/`.

## Estrutura da solução

```
PokeDraft.sln                  Solução (agrupa os projetos)
global.json                    Trava o SDK em .NET 8
PokeDraft.Web/                  Projeto web (backend + views + estáticos)
  Program.cs                    Configuração da aplicação e do EF Core
  appsettings.json              Connection string do SQLite
  Controllers/                  Recebem as requisições (MVC + API)
  Models/
    Entities/                   Classes do domínio (viram tabelas)
    Enums/                      Tipos fixos (status, tipo de Pokémon, etc.)
  Data/
    PokeDraftDbContext.cs       Ponte entre as classes e o banco
  Migrations/                   Histórico de alterações do banco (versionado)
  Views/                        HTML (.cshtml)
  wwwroot/                      CSS, JS e imagens servidos ao navegador
```

## Modelo de dados (primeira versão)

| Entidade | Papel |
|---|---|
| `Pokemon` | Catálogo de Pokémon (status base, tipos, custo) — vem da PokéAPI |
| `Item` | Catálogo de itens de batalha (bônus de overall, custo) |
| `Jogador` | Participante de partidas |
| `Configuracao` | Preset de regras do draft (budget, tamanho do pool/time, fator de venda) |
| `Partida` | Disputa entre dois jogadores; amarra tudo abaixo |
| `PokemonNoPool` | Pokémon sorteados para os samples do draft de uma partida |
| `ItemNoPool` | Itens disponíveis no sample de itens de uma partida |
| `EscolhaPokemon` | Pokémon comprado por um jogador (item, natureza/IVs, ordem, venda) |
| `MovimentacaoDraft` | Histórico de ban / compra / venda / equipar item |
| `ConfrontoBatalha` | Cada duelo 1×1 da batalha simulada e seu resultado |

## Trabalhando em equipe (Git)

- `main` fica sempre funcionando. Ninguém faz commit direto nela.
- Para cada tarefa: crie um branch (`git checkout -b feat/importador-pokeapi`),
  faça os commits, suba (`git push -u origin <branch>`) e abra um Pull Request.
- Rode `dotnet build` antes de commitar.

### Migrations (quem mexer no modelo)

```bash
# depois de alterar/adicionar classes em Models/Entities:
dotnet ef migrations add DescricaoDaMudanca --project PokeDraft.Web
dotnet ef database update --project PokeDraft.Web
```

Commite a pasta `Migrations/` junto com a mudança nas entidades. Os colegas só
precisam rodar `dotnet ef database update` depois do `git pull`.

## Cronograma

| Data | Entrega |
|---|---|
| 28/08 | Ambiente, repositório, estrutura da solução, modelagem do banco + 1ª migration |
| 04/09 | Importador da PokéAPI + CRUD de Pokémon na API |
| 11/09 | CRUD de Jogador, Partida e Configuração + esqueleto do frontend |
| 18/09 | **Marco:** protótipo navegável — criação de partida e sorteio do pool |
| 02/10 | Motor do draft: ban, compra, validações, controle de saldo |
| 09/10 | Venda + trava anti-beco-sem-saída + histórico de movimentações |
| 16/10 | **Marco:** MVP integrado — tela do draft ponta a ponta |
| 23/10 | Polimento visual, sprites, feedback de erro, responsividade |
| 30/10 | **Marco:** versão candidata — testes, casos de borda, bugs |
| 06/11 | Congelamento do código, README final, ensaio da defesa |
