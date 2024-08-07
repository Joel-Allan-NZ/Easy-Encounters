﻿using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Contracts.Services;

public interface ICreatureService
{
    /// <summary>
    /// Create a deep copy of a source creature
    /// </summary>
    /// <param name="toCopy"></param>
    CreatureBase DeepCopy(CreatureBase toCopy);

    Creature Create();

    /// <summary>
    /// Get the bonus value for a specified attribute on a specified creature.
    /// </summary>
    /// <param name="creature"></param>
    /// <param name="creatureAttributeType"></param>
    /// <returns>Bonus value of an attribute, ie +2</returns>
    int GetAttributeBonusValue(CreatureBase creature, CreatureAttributeType creatureAttributeType);

    /// <summary>
    /// Get the total value for a specified attribute for a specified creature.
    /// </summary>
    /// <param name="creature"></param>
    /// <param name="creatureAttributeType"></param>
    /// <returns></returns>
    int GetAttributeTypeValue(CreatureBase creature, CreatureAttributeType creatureAttributeType);

    int GetSkillBonusTotal(CreatureBase creature, CreatureSkills skill, CreatureSkillLevel proficiencyLevel);

    CreatureSkillLevel GetSkillProficiencyLevel(CreatureBase creature, CreatureSkills skill);
}