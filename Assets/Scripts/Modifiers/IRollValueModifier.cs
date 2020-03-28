using System;

// Modifiers affecting the values of the roll after it has been rolled
public interface IRollValueModifier
{
    Tuple<int, int> apply(int playerRoll, int enemyRoll);
}
