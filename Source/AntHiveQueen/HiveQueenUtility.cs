﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;


namespace AntiniumHiveQueen
{
    public static class HiveQueenUtility
    {
        // general sensitivity to Q effects, 0-1
        public static float GetPawnHQFactor(Pawn pawn, bool antsOnly = false, bool reqForAnts = true)
        {
            float factor = 0f;

            if (antsOnly && !(pawn.kindDef.race.defName == "Ant_AntiniumRace"))
            {
                return 0f;
            }

            
            // default increase for ants
            if (pawn.kindDef.race.defName == "Ant_AntiniumRace")
            {
                factor = .5f;
            }


            // psychic sensitivity
            float hyper = pawn.GetStatValue(StatDefOf.PsychicSensitivity, true);

            factor += (hyper - 1) * 0.5f;


            //if (hyper <= .3)
            //{
            //    factor -= .35f;
            //}

            //else if (hyper <= .6)
            //{
            //    factor -= .15f;
            //}

            //if (hyper >= 1.7)
            //{
            //    factor += .35f;
            //}

            //else if (hyper >= 1.3)
            //{
            //    factor += .15f;
            //}

            // Min 5 pct for ants, if req.
            if (reqForAnts && pawn.kindDef.race.defName == "Ant_AntiniumRace")
            {
                factor = Math.Max(factor, 0.05f);
            }

            return factor;
        }



        // general sensitivity to queen effects, 0-4
        public static int GetPawnHQScore(Pawn pawn, bool antsOnly = false, bool reqForAnts = true)
        {
            int factor = 0;

            if (antsOnly && !(pawn.kindDef.race.defName == "Ant_AntiniumRace"))
            {
                return 0;
            }


            // default increase for ants
            if (pawn.kindDef.race.defName == "Ant_AntiniumRace")
            {
                factor = 2;
            }

            // psychic sensitivity
            float hyper = pawn.GetStatValue(StatDefOf.PsychicSensitivity, true);


            if (hyper <= .3)
            {
                factor -= 2;
            }

            else if (hyper <= .6)
            {
                factor -= 1;
            }

            if (hyper >= 1.7)
            {
                factor += 2;
            }

            else if (hyper >= 1.3)
            {
                factor += 1;
            }

            // Min 1 pt for ants, if req.
            if (reqForAnts && pawn.kindDef.race.defName == "Ant_AntiniumRace")
            {
                factor = Math.Max(factor, 1);
            }

            return factor;
        }




        public static bool QueenExistsOnMap(Map map)
        {

            if( map.mapPawns.PawnsInFaction(Faction.OfPlayer).Any(p => p.TryGetComp<CompHQPresence>() != null))
            {
                // doesn't check if she's downed, in cryptosleep, etc
                return true;
            }

            return false;
        }


        //returns max if there are multiple queens
        public static int QueenMaturityLevel(Map map)
        {
            int level = 0;


            List<Pawn> queens = map.mapPawns.PawnsInFaction(Faction.OfPlayer).Where(p => p.TryGetComp<CompHQPresence>() != null).ToList();

            foreach(Pawn queen in queens)
            {
                CompHQPresence pres = queen.TryGetComp<CompHQPresence>();
                if (pres != null && pres.QueenMaturity > level)
                {
                    level = pres.QueenMaturity;
                }
            }

            return level;
        }


        public static Pawn TryGetQueen(Map map)
        {
            Pawn queen = null;

            List<Pawn> queens = map.mapPawns.PawnsInFaction(Faction.OfPlayer).Where(p => p.TryGetComp<CompHQPresence>() != null).ToList();

            if (queens.Count > 0)
            {
                // there should never be more than one
                queen = queens.FirstOrDefault();
            }

            return queen;
        }




    }
}
