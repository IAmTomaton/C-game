using System;

namespace Cgame.Core
{
    /// <summary>
    /// Класс определяет правила столкновения объектов на основании их слоя.
    /// </summary>
    static class LayerSettings
    {
        private static readonly bool[,] collisionRules;

        static LayerSettings()
        {
            var layersCount = Enum.GetNames(typeof(Layers)).Length;
            collisionRules = new bool[layersCount, layersCount];
            InitRules();
        }

        /// <summary>
        /// В данном методе нужно добавлять правила столкновений в соответствии с шаблоном:
        /// AddCollisionRule(Layers.НазваниеПервогоСлоя, Layers.НазваниеВторгоСлоя)
        /// Прорядок указания слоёв не важен.
        /// </summary>
        private static void InitRules()
        {
            AddCollisionRule(Layers.Player, Layers.Platform);
            AddCollisionRule(Layers.Player, Layers.Object);
            AddCollisionRule(Layers.Object, Layers.Platform);
            //AddCollisionRule(Layers.Object, Layers.Object);
        }

        /// <summary>
        /// Проверяет должны ли объекты из указанных слоёв сталкиваться.
        /// </summary>
        /// <param name="firstLayer"></param>
        /// <param name="secondLayer"></param>
        /// <returns></returns>
        public static bool CheckCollision(Layers firstLayer, Layers secondLayer)
        {
            return collisionRules[(int)secondLayer, (int)firstLayer];
        }


        /// <summary>
        /// Добавляет правило столкновения указанных слоёв.
        /// </summary>
        /// <param name="firstLayer"></param>
        /// <param name="secondLayer"></param>
        /// <param name="collision">Указывает должны ли объекты сталкиваться.</param>
        public static void AddCollisionRule(Layers firstLayer, Layers secondLayer, bool collision = true)
        {
            collisionRules[(int)firstLayer, (int)secondLayer] = collision;
            collisionRules[(int)secondLayer, (int)firstLayer] = collision;
        }
    }
}
