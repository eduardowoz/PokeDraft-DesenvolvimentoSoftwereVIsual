# PokéDraft

Simulador de batalha Pokémon entre dois jogadores. Cada jogador monta o time
através de um **draft** de Pokémon e itens (com orçamento em pontos, compra e
venda) e depois o sistema **simula a batalha** 1×1 entre os times.

Projeto da disciplina **Desenvolvimento de Software Visual**.

## Tecnologias

| Camada | Tecnologia |
|---|---|
| Backend | C# / ASP.NET Core MVC (**.NET 10**) |
| API | Controllers REST retornando JSON |
| Banco de dados | SQLite + Entity Framework Core 10 |
| Frontend | HTML + CSS + JavaScript (`fetch`), em `wwwroot/` |
| Dados de Pokémon | Importados da [PokéAPI](https://pokeapi.co/) |
| Documentação da API | Scalar (`/scalar/v1`) |

## Pré-requisitos (em cada máquina)

1. **.NET SDK 10.0** — https://dotnet.microsoft.com/download
   Teste: `dotnet --version` (deve mostrar `10.x`).
2. **Visual Studio Code** — https://code.visualstudio.com/ com as extensões
   **C#** e **C# Dev Kit**.
3. **Ferramenta de linha de comando do EF Core** (só quem for mexer em migration):
   ```bash
   dotnet tool install --global dotnet-ef --version 10.*
   ```
   > Se você já tinha a versão 8 instalada, atualize:
   > `dotnet tool update --global dotnet-ef --version 10.*`
   > A versão do `dotnet-ef` precisa bater com a do EF Core do projeto.

## Como rodar

```bash
dotnet restore
dotnet run
```

Abra no navegador o endereço que o **terminal imprimir** na linha `Now listening on:`.
A porta vem de `Properties/launchSettings.json` e pode mudar — sempre confie no terminal,
não em um número decorado. A documentação da API fica em `/scalar/v1`.

Para parar o servidor: `Ctrl+C` no terminal.

> O banco (`pokedraft.db`) **é versionado junto com o projeto**, já populado com
> os 1025 Pokémon e os 36 itens. Não é preciso rodar migration para começar.

## Estrutura

```
Pokedraft.sln                  Solução (abra esta no VS Code)
Pokedraft.csproj               Projeto e pacotes NuGet
Program.cs                     Configuração da aplicação, serviços e rotas
appsettings.json               Configuração
pokedraft.db                   Banco SQLite (versionado)
Controllers/                   Endpoints da API e as páginas (HomeController)
Data/AppDbContext.cs           Ponte entre as classes e o banco
Migrations/                    Histórico de alterações do banco
Models/
  Entities/                    Classes do domínio (viram tabelas)
  Requests/                    Objetos de entrada dos endpoints
  Dto/                         Desserialização da PokéAPI
Services/                      Regras de negócio (draft, itens, resultado)
Views/Home/                    As telas (.cshtml)
wwwroot/                       CSS, JS, sprites e bibliotecas
```

## Modelo de dados

| Entidade | Papel |
|---|---|
| `Pokemon` | Catálogo de Pokémon (tipos, overall, custo) — vem da PokéAPI |
| `Item` | Catálogo de itens (tipo, tier de overall, custo) |
| `Jogador` | Participante, com o budget em pontos |
| `Partida` | Disputa entre dois jogadores (budget, geração, turno) |
| `Conexao` | Liga Partida + Jogador + Pokémon (+ Item equipado) — é o time montado |

## Telas e fluxo

1. **Index** — cadastro dos jogadores e criação da partida
2. **Draft** — compra de Pokémon alternando o turno, gastando o budget
3. **Itens** — compra e equipe de itens nos Pokémon do time
4. **Batalha** — cada jogador escala os 6 Pokémon na ordem de combate
5. **Resultado** — simulação 1×1, placar e vencedor

### Como o overall é calculado

```
overall = Pokemon.Overall (soma dos base stats)
        + IV sorteado        (0 a 310)
        + natureza sorteada  (-100 a +100)
        + bônus do item
```

O bônus do item usa `Item.Overall` como **tier**, e cada tier vale 10%:

- itens **com tipo** (plates, tier 1, custo 10) → só valem se o tipo casar com o
  `PrimeiroTipo` ou `SegundoTipo` do Pokémon;
- itens **sem tipo** (Leftovers tier 2, Life Orb tier 3) → valem sempre.

Na batalha, cada vitória de rodada vale 1 ponto. **Em caso de empate no placar**,
vence quem tiver a maior média de overall do time.

## Migrations

```bash
# depois de alterar/adicionar classes em Models/Entities:
dotnet ef migrations add DescricaoDaMudanca
dotnet ef database update
```

Commite a pasta `Migrations/` junto com a mudança nas entidades.

## Documentação

Na pasta `docs/`:

- **Manual Técnico** — arquitetura, função de cada arquivo, estrutura do banco
- **Lista de Exercícios** — 72 exercícios de C#, MVC, EF Core e API para treino
