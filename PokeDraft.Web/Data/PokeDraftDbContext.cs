using Microsoft.EntityFrameworkCore;
using PokeDraft.Web.Models.Entities;

namespace PokeDraft.Web.Data;

/// <summary>
/// Ponte entre as classes do domínio e o banco SQLite.
/// Cada <see cref="DbSet{T}"/> vira uma tabela.
/// </summary>
public class PokeDraftDbContext : DbContext
{
    public PokeDraftDbContext(DbContextOptions<PokeDraftDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pokemon> Pokemons => Set<Pokemon>();
    public DbSet<Item> Itens => Set<Item>();
    public DbSet<Jogador> Jogadores => Set<Jogador>();
    public DbSet<Configuracao> Configuracoes => Set<Configuracao>();
    public DbSet<Partida> Partidas => Set<Partida>();
    public DbSet<PokemonNoPool> PokemonNoPool => Set<PokemonNoPool>();
    public DbSet<ItemNoPool> ItensNoPool => Set<ItemNoPool>();
    public DbSet<EscolhaPokemon> EscolhasPokemon => Set<EscolhaPokemon>();
    public DbSet<MovimentacaoDraft> MovimentacoesDraft => Set<MovimentacaoDraft>();
    public DbSet<ConfrontoBatalha> ConfrontosBatalha => Set<ConfrontoBatalha>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---- Enums guardados como texto (mais fácil de ler direto no banco) ----
        modelBuilder.Entity<Pokemon>()
            .Property(p => p.Tipo1).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<Pokemon>()
            .Property(p => p.Tipo2).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<Partida>()
            .Property(p => p.Status).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<MovimentacaoDraft>()
            .Property(m => m.Tipo).HasConversion<string>().HasMaxLength(20);

        // ---- Índices ----
        modelBuilder.Entity<Pokemon>()
            .HasIndex(p => p.PokeApiId).IsUnique();
        modelBuilder.Entity<Configuracao>()
            .HasIndex(c => c.Nome).IsUnique();

        // ---- Partida: três referências para Jogador, sem exclusão em cascata ----
        modelBuilder.Entity<Partida>(e =>
        {
            e.HasOne(p => p.JogadorA)
                .WithMany()
                .HasForeignKey(p => p.JogadorAId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(p => p.JogadorB)
                .WithMany()
                .HasForeignKey(p => p.JogadorBId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(p => p.VencedorJogador)
                .WithMany()
                .HasForeignKey(p => p.VencedorJogadorId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(p => p.Configuracao)
                .WithMany(c => c.Partidas)
                .HasForeignKey(p => p.ConfiguracaoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ---- Filhos da Partida: apagar a partida apaga os registros dependentes ----
        modelBuilder.Entity<PokemonNoPool>(e =>
        {
            e.HasOne(x => x.Partida).WithMany(p => p.PokemonNoPool)
                .HasForeignKey(x => x.PartidaId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Pokemon).WithMany(p => p.AparicoesEmPools)
                .HasForeignKey(x => x.PokemonId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(x => new { x.PartidaId, x.NumeroSample });
        });

        modelBuilder.Entity<ItemNoPool>(e =>
        {
            e.HasOne(x => x.Partida).WithMany(p => p.ItensNoPool)
                .HasForeignKey(x => x.PartidaId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Item).WithMany(i => i.AparicoesEmPools)
                .HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<EscolhaPokemon>(e =>
        {
            e.HasOne(x => x.Partida).WithMany(p => p.Escolhas)
                .HasForeignKey(x => x.PartidaId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Jogador).WithMany(j => j.Escolhas)
                .HasForeignKey(x => x.JogadorId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Pokemon).WithMany(p => p.Escolhas)
                .HasForeignKey(x => x.PokemonId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Item).WithMany(i => i.EscolhasEquipadas)
                .HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<MovimentacaoDraft>(e =>
        {
            e.HasOne(x => x.Partida).WithMany(p => p.Movimentacoes)
                .HasForeignKey(x => x.PartidaId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Jogador).WithMany(j => j.Movimentacoes)
                .HasForeignKey(x => x.JogadorId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Pokemon).WithMany()
                .HasForeignKey(x => x.PokemonId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Item).WithMany()
                .HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ConfrontoBatalha>(e =>
        {
            e.HasOne(x => x.Partida).WithMany(p => p.Confrontos)
                .HasForeignKey(x => x.PartidaId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.EscolhaPokemonA).WithMany()
                .HasForeignKey(x => x.EscolhaPokemonAId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.EscolhaPokemonB).WithMany()
                .HasForeignKey(x => x.EscolhaPokemonBId).OnDelete(DeleteBehavior.Restrict);
        });

        // ---- Dados iniciais: uma configuração padrão para começar ----
        modelBuilder.Entity<Configuracao>().HasData(new Configuracao
        {
            Id = 1,
            Nome = "Padrão",
            BudgetPontos = 100,
            QuantidadeSamples = 5,
            PokemonPorSample = 6,
            TamanhoTime = 6,
            BansPorJogador = 2,
            FatorVenda = 0.5,
            CriadaEm = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}
