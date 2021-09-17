using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Feature
{
    public string Name { get; set; }

    public string LabelName { get; set; }

    public string Description { get; set; }

    public bool IsAvailable { get; set; }

    public int NeededWorld { get; set; } = 1;

    public bool IsBlocked { get; set; } = false;

    public bool CanBeChosen { get; set; } = false;

    public bool IsChosen { get; set; } = true;

    public int Cost { get; set; }

    public string NeededFeature { get; set; } = "";

    public static List<Feature> GetFeaturesList()
    {
        List<Feature> res = new List<Feature>();

        #region StandardFeatures
        res.Add(
            new Feature()
            {
                Name = "jump",
                LabelName = "Jump",
                Description = "Jump on a SPACE." + Environment.NewLine + Environment.NewLine + "This costs 10 meds.",
                Cost = 10
            });

        res.Add(
            new Feature()
            {
                Name = "dbljump",
                LabelName = "Double Jump",
                Description = "Jump in air by SPACE + SPACE." + Environment.NewLine + Environment.NewLine + "This costs 15 meds.",
                Cost = 15,
                NeededFeature = "jump"
            });

        res.Add(
            new Feature()
            {
                Name = "combojump",
                LabelName = "Combo Jump",
                Description = "Jump in air for infinity time by pressing SPACE." + Environment.NewLine + Environment.NewLine + "This costs 30 meds.",
                Cost = 30,
                NeededFeature = "dbljump"
            });

        res.Add(
            new Feature()
            {
                Name = "snatch",
                LabelName = "Snatch",
                Description = "Short snatch on E." + Environment.NewLine + Environment.NewLine + "This costs 15 meds.",
                Cost = 15
            });

        res.Add(
            new Feature()
            {
                Name = "shift",
                LabelName = "Shift",
                Description = "Hold SHIFT to move faster." + Environment.NewLine + Environment.NewLine + "This costs 10 meds.",
                Cost = 10,
                NeededFeature = "snatch"
            });

        res.Add(
            new Feature()
            {
                Name = "shot",
                LabelName = "Shot",
                Description = "Shooting by pressing LMB. Every shot costs 1 med." + Environment.NewLine + Environment.NewLine + "This costs 15 meds.",
                Cost = 15
            });

        res.Add(
            new Feature()
            {
                Name = "freeshot",
                LabelName = "Free Shot",
                Description = "Shooting doesn't costs meds anymore (including Super Shot)." + Environment.NewLine + Environment.NewLine + "This costs 30 meds.",
                Cost = 30,
                NeededFeature = "shot"
            });

        res.Add(
            new Feature()
            {
                Name = "strongshot",
                LabelName = "Strong Shot",
                Description = "Shooting affects more damage." + Environment.NewLine + Environment.NewLine + "This costs 10 meds.",
                Cost = 10,
                NeededFeature = "shot"
            });

        res.Add(
            new Feature()
            {
                Name = "supershot",
                LabelName = "Super Shot",
                Description = "Shoot in every side on RMB. Costs 10 meds by shot." + Environment.NewLine + Environment.NewLine + "This costs 15 meds.",
                Cost = 15,
                NeededFeature = "strongshot"
            });

        res.Add(
            new Feature()
            {
                Name = "weighting",
                LabelName = "Weighting",
                Description = "Press or hold F to add weight." + Environment.NewLine + Environment.NewLine + "This costs 15 meds.",
                Cost = 15
            });

        res.Add(
            new Feature()
            {
                Name = "growth",
                LabelName = "Growth",
                Description = "Press R to grow and press again to back to normal size." + Environment.NewLine + Environment.NewLine + "This costs 20 meds.",
                Cost = 20,
                NeededFeature = "weighting"
            });

        res.Add(
            new Feature()
            {
                Name = "reduction",
                LabelName = "Reduction",
                Description = "Press C to became small and press again to back to normal size." + Environment.NewLine + Environment.NewLine + "This costs 20 meds.",
                Cost = 20,
                NeededFeature = "weighting"
            });
        #endregion

        #region CubeFeatures
        res.Add(
            new Feature()
            {
                Name = "cube",
                LabelName = "Cube",
                Description = "Press 1 to summon Cube." + Environment.NewLine + Environment.NewLine + "",
                Cost = 0,
                NeededWorld = 2
            });

        res.Add(
            new Feature()
            {
                Name = "cube_weight",
                LabelName = "Weight",
                Description = "Cube affects damage when falling." + Environment.NewLine + Environment.NewLine + "This costs 10 meds.",
                Cost = 10,
                NeededWorld = 2,
                CanBeChosen = true
            });

        res.Add(
            new Feature()
            {
                Name = "cube_explosion",
                LabelName = "Explosion",
                Description = "Cube affects explosion that bring damage to all in the explosion zone (except you) when falling." + Environment.NewLine + Environment.NewLine + "This costs 15 meds.",
                Cost = 15,
                NeededFeature = "cube_weight",
                NeededWorld = 2,
                CanBeChosen = true
            });

        res.Add(
            new Feature()
            {
                Name = "cube_destruction",
                LabelName = "Destruction",
                Description = "Cube`s explosions bring x2 damage." + Environment.NewLine + Environment.NewLine + "This costs 20 meds.",
                Cost = 20,
                NeededFeature = "cube_explosion",
                NeededWorld = 2,
                CanBeChosen = true
            });

        res.Add(
            new Feature()
            {
                Name = "cube_weightlessness",
                LabelName = "Weightlessness",
                Description = "Cube hovers in the air instead of falling." + Environment.NewLine + Environment.NewLine + "This costs 15 meds.",
                Cost = 15,
                NeededWorld = 2,
                CanBeChosen = true
            });
        #endregion

        #region Pyramid Features
        res.Add(
            new Feature()
            {
                Name = "pyramid",
                LabelName = "Pyramid",
                Description = "Press 2 to summon Pyramid." + Environment.NewLine + Environment.NewLine + "",
                Cost = 0,
                NeededWorld = 3
            });

        res.Add(
            new Feature()
            {
                Name = "pyramid_weightlessness",
                LabelName = "Weightlessness",
                Description = "Pyramid hovers in the air instead of falling." + Environment.NewLine + Environment.NewLine + "This costs 15 meds.",
                Cost = 15,
                NeededWorld = 3,
                CanBeChosen = true
            });

        res.Add(
            new Feature()
            {
                Name = "pyramid_transformation",
                LabelName = "Transformation",
                Description = "Pyramid transforms input rays into rays of choosen type." + Environment.NewLine + Environment.NewLine + "This costs 5 meds.",
                Cost = 5,
                NeededWorld = 3
            });

        res.Add(
            new Feature()
            {
                Name = "pyramid_paralyzing",
                LabelName = "Paralyzing",
                Description = "Transform all rays to paralyzing rays." + Environment.NewLine + Environment.NewLine + "This costs 5 meds.",
                Cost = 5,
                NeededFeature = "pyramid_transformation",
                NeededWorld = 3,
                CanBeChosen = true,
                IsChosen = false
            });

        res.Add(
            new Feature()
            {
                Name = "pyramid_electric",
                LabelName = "Electric",
                Description = "Transform all rays to electric rays." + Environment.NewLine + Environment.NewLine + "This costs 5 meds.",
                Cost = 5,
                NeededFeature = "pyramid_transformation",
                NeededWorld = 3,
                CanBeChosen = true,
                IsChosen = false
            });

        res.Add(
            new Feature()
            {
                Name = "pyramid_fire",
                LabelName = "Fire",
                Description = "Transform all rays to fire rays." + Environment.NewLine + Environment.NewLine + "This costs 5 meds.",
                Cost = 5,
                NeededFeature = "pyramid_transformation",
                NeededWorld = 3,
                CanBeChosen = true,
                IsChosen = false
            });

        res.Add(
            new Feature()
            {
                Name = "pyramid_ice",
                LabelName = "Ice",
                Description = "Transform all rays to ice rays." + Environment.NewLine + Environment.NewLine + "This costs 5 meds.",
                Cost = 5,
                NeededFeature = "pyramid_transformation",
                NeededWorld = 3,
                CanBeChosen = true,
                IsChosen = false
            });

        res.Add(
            new Feature()
            {
                Name = "pyramid_lazer",
                LabelName = "Lazer",
                Description = "Transform all rays to lazer rays." + Environment.NewLine + Environment.NewLine + "This costs 5 meds.",
                Cost = 5,
                NeededFeature = "pyramid_transformation",
                NeededWorld = 3,
                CanBeChosen = true,
                IsChosen = false
            });
        #endregion

        #region Torus Features
        res.Add(
            new Feature()
            {
                Name = "torus",
                LabelName = "Torus",
                Description = "Press 3 to summon Torus." + Environment.NewLine + Environment.NewLine + "",
                Cost = 0,
                NeededWorld = 4
            });

        res.Add(
            new Feature()
            {
                Name = "torus_without",
                LabelName = "Without Me",
                Description = "Torus power don`t affects on you if this feature is choosen." + Environment.NewLine + Environment.NewLine + "This costs 20 meds.",
                Cost = 20,
                NeededWorld = 4,
                CanBeChosen = true
            });

        res.Add(
            new Feature()
            {
                Name = "torus_blowout",
                LabelName = "Blowout",
                Description = "Torus blows away all that was attracted when unsummoming." + Environment.NewLine + Environment.NewLine + "This costs 15 meds.",
                Cost = 15,
                NeededWorld = 4,
                CanBeChosen = true
            });

        res.Add(
            new Feature()
            {
                Name = "torus_hole",
                LabelName = "Blach Hole",
                Description = "Press 3 double time when summoning and torus will devour everything that attracts." + Environment.NewLine + Environment.NewLine + "This costs 15 meds.",
                Cost = 15,
                NeededFeature = "torus_without",
                NeededWorld = 4,
                CanBeChosen = true
            });
        #endregion

        #region TimeFeatures
        res.Add(
            new Feature()
            {
                Name = "time_freeze",
                LabelName = "Time Freeze",
                Description = "Press 4 to freeze the time to 20 seconds." + Environment.NewLine + Environment.NewLine + "",
                Cost = 0,
                NeededWorld = 5
            });
        #endregion


        return res;
    }
}

public static class FeatureNames
{
    public const string jump = "jump";
    public const string dbljump = "dbljump";
    public const string combojump = "combojump";
    public const string snatch = "snatch";
    public const string shift = "shift";
    public const string shot = "shot";
    public const string freeshot = "freeshot";
    public const string strongshot = "strongshot";
    public const string supershot = "supershot";
    public const string weighting = "weighting";
    public const string growth = "growth";
    public const string reduction = "reduction";
    public const string cube = "cube";
    public const string cube_weight = "cube_weight";
    public const string cube_explosion = "cube_explosion";
    public const string cube_destruction = "cube_destruction";
    public const string cube_weightlessness = "cube_weightlessness";
    public const string pyramid = "pyramid";
    public const string pyramid_weightlessness = "pyramid_weightlessness";
    public const string pyramid_transformation = "pyramid_transformation";
    public const string pyramid_paralyzing = "pyramid_paralyzing";
    public const string pyramid_electric = "pyramid_electric";
    public const string pyramid_fire = "pyramid_fire";
    public const string pyramid_ice = "pyramid_ice";
    public const string pyramid_lazer = "pyramid_lazer";
    public const string torus = "torus";
    public const string torus_without = "torus_without";
    public const string torus_blowout = "torus_blowout";
    public const string torus_hole = "torus_hole";
    public const string time_freeze = "time_freeze";
}
