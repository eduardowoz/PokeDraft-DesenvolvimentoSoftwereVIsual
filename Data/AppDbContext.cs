using Microsoft.EntityFrameworkCore;
using Pokedraft.Models.Entities;

namespace Pokedraft.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Pokemon> Pokemons {get; set;}
    public DbSet<Jogador> Jogadores {get; set;}
    public DbSet<Partida> Partidas {get; set;}
    public DbSet<Conexao> PokemonsJogadores {get; set;}
    public DbSet<Item> Itens {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Partida>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Partida>()
            .HasOne<Jogador>().WithMany().HasForeignKey(p => p.JogadorUmId);
        modelBuilder.Entity<Partida>()
            .HasOne<Jogador>().WithMany().HasForeignKey(p => p.JogadorDoisId);

        modelBuilder.Entity<Conexao>()
            .HasKey(pj => pj.Id);
        modelBuilder.Entity<Conexao>()
            .HasOne<Pokemon>().WithMany().HasForeignKey(pj => pj.PokemonId);
        modelBuilder.Entity<Conexao>()
            .HasOne<Jogador>().WithMany().HasForeignKey(pj => pj.JogadorId);
        modelBuilder.Entity<Conexao>()
            .HasOne<Partida>().WithMany().HasForeignKey(pj => pj.PartidaId);
        modelBuilder.Entity<Conexao>()
            .HasOne<Item>().WithMany().HasForeignKey(pj => pj.ItemId);

        modelBuilder.Entity<Jogador>()
            .Property(p => p.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Partida>()
            .Property(p => p.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Conexao>()
            .Property(p => p.Id).ValueGeneratedOnAdd();
    }
}