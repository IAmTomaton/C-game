using System;

namespace Cgame
{
    static class LayerSettings
    {
        private static readonly bool[,] collisionRules;

        static LayerSettings()
        {
            var layersCount = Enum.GetNames(typeof(Layers)).Length;
            collisionRules = new bool[layersCount, layersCount];

            AddCollisionRule(Layers.Player, Layers.Platform);
            AddCollisionRule(Layers.Player, Layers.Object);
            AddCollisionRule(Layers.Object, Layers.Platform);
        }

        public static bool CheckCollision(Layers firstLayer, Layers secondLayer)
        {
            return collisionRules[(int)secondLayer, (int)firstLayer];
        }

        public static void AddCollisionRule(Layers firstLayer, Layers secondLayer, bool collision = true)
        {
            collisionRules[(int)firstLayer, (int)secondLayer] = collision;
            collisionRules[(int)secondLayer, (int)firstLayer] = collision;
        }
    }
}
