using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyEncounters.Persistence.SQLLite;
public class DataContext : DbContext
{
    public DbSet<Ability> Abilities
    {
        get; set;
    }

    public DbSet<Encounter> Encounters
    {
        get; set;
    }

    public DbSet<Campaign> Campaigns
    {
        get; set;
    }

    public DbSet<Party> Parties
    {
        get; set;
    }

    //active encounter persistance:

    public DbSet<ActiveEncounter> ActiveEncounters
    {
        get; set;
    }

    public DbSet<ActiveEncounterCreature> ActiveEncounterCreatures
    {
        get; set;
    }

    public DbSet<ActiveAbility> ActiveAbilities
    {
        get; set;
    }

    public DbSet<Creature> Creatures
    {
        get; set;
    }

    public DbSet<EncounterCreatures> EncounterCreatures
    {
        get; set;
    }

    public string DbPath
    {
        get;
    }
    public DataContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        path = Path.Join(path, @"/EasyEncounters/ApplicationData");
        DbPath = System.IO.Path.Join(path, "EasyEncounters.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={DbPath}").EnableSensitiveDataLogging();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Creature>().HasMany(x => x.Abilities).WithMany();
        modelBuilder.Entity<Encounter>().HasMany(x => x.CreaturesByCount);
        modelBuilder.Entity<EncounterCreatures>().HasOne(x => x.Creature).WithMany();
        modelBuilder.Entity<Encounter>().HasMany(x => x.Creatures).WithMany();
        modelBuilder.Entity<Party>().HasMany(x => x.Members).WithMany();

        modelBuilder.Ignore<ActiveAbility>();
        modelBuilder.Ignore<ICollection<ActiveAbility>>();
        modelBuilder.Entity<ActiveEncounterCreature>().Ignore(x => x.ActiveAbilities);

    }

}
