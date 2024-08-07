﻿namespace EasyEncounters.Core.Models;
#nullable enable

internal class Data
{
    /// <summary>
    /// Laziest way to manage persistence
    /// </summary>
    public Data()
    {
        Parties = new List<Party>();
        Creatures = new List<Creature>();
        Encounters = new List<Encounter>();
        Campaigns = new List<Campaign>();
        Abilities = new List<Ability>();
        DamageInstancesDone = new Dictionary<Creature, List<int>>();
    }

    public List<Ability> Abilities
    {
        get; set;
    }

    ////public ActiveEncounter? ActiveEncounter
    //{
    //    get; set;
    //}

    public List<Campaign> Campaigns
    {
        get; set;
    }

    public List<Creature> Creatures
    {
        get; set;
    }

    public Dictionary<Creature, List<int>> DamageInstancesDone
    {
        get; set;
    }

    public List<Encounter> Encounters
    {
        get; set;
    }

    public List<Party> Parties
    {
        get; set;
    }
}