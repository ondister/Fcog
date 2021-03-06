﻿using System;

namespace Fcog.Core.Recognition
{
    public class CharacterStatistics
    {
        public CharacterStatistics(Character character, double frequency, int count)
        {
            Character = character;
            Frequency = frequency;
            Count = count;
        }

        public Character Character { get; }
        public double Frequency { get; }
        public int Count { get; }

        public override string ToString()
        {
            return $"{Character.TextView.Text}: {Localization.CoreUI.CharacterFrequencySign}: {Math.Round(Frequency, 3)}, {Localization.CoreUI.CharacterCountSign}: {Count}";
        }
    }
}