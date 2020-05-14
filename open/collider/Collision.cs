using OpenTK;
using System;
using System.Linq;

namespace Cgame
{
    /// <summary>
    /// Класс для проверки коллизии между объектами и хранении информации о ней.
    /// </summary>
    class Collision
    {
        /// <summary>
        /// Указывает вектор нормали к какой-то грани. Вдоль этого вектора нужно перемещать объекты.
        /// </summary>
        public Vector2 Mtv { get; }
        /// <summary>
        /// Указывает растояние на которое суммарно нужно раздвинуть объекты.
        /// </summary>
        public float MtvLength { get; }
        /// <summary>
        /// Указывает роизошла ли коллизия.
        /// </summary>
        public bool Collide { get; }

        public Collision(Collider first, Collider second)
        {
            CheckResult result = Check(first, second);

            if (result == null)
            {
                Collide = false;
                return;
            }

            Collide = true;
            Mtv = result.Mtv;
            MtvLength = Math.Abs(result.MtvLength);
        }

        private CheckResult Check(Collider firstCollider, Collider secondCollider)
        {
            if (firstCollider.Radius + secondCollider.Radius <= Vector2.Distance(firstCollider.Position, secondCollider.Position))
                return null;
            if (firstCollider.Vertices.Count == 0 && secondCollider.Vertices.Count == 0)
            {
                return new CheckResult(
                    (firstCollider.Position - secondCollider.Position).Normalized(),
                    firstCollider.Radius + secondCollider.Radius - Vector2.Distance(firstCollider.Position, secondCollider.Position));
            }
            return CollisionPolygons(firstCollider, secondCollider);
        }

        private CheckResult CollisionPolygons(Collider firstCollider, Collider secondCollider)
        {
            var mtv = default(Vector2);
            var minMTVLength = 0f;
            var first = true;

            foreach (var normal in firstCollider.GetNornals().Concat(secondCollider.GetNornals()))
            {
                Vector2 firstProjection = GetProjection(normal, firstCollider);
                Vector2 secondProjection = GetProjection(normal, secondCollider);

                if (firstProjection.X < secondProjection.Y || secondProjection.X < firstProjection.Y)
                    return null;

                if (first)
                {
                    first = false;
                    mtv = normal.Normalized();
                    minMTVLength = GetIntersectionLength(firstProjection, secondProjection);
                    continue;
                }

                float mtvLength = GetIntersectionLength(firstProjection, secondProjection);
                if (Math.Abs(mtvLength) < Math.Abs(minMTVLength))
                {
                    mtv = normal.Normalized();
                    minMTVLength = mtvLength;
                }
            }

            return new CheckResult(mtv, minMTVLength);
        }

        private Vector2 GetProjection(Vector2 vector, Collider collider)
        {
            var vertices = collider.Vertices;
            Vector2 result = default;

            if (vertices.Count == 0)
            {
                var projection = Vector2.Dot(vector, collider.Position);
                return new Vector2(projection + collider.Radius, projection - collider.Radius);
            }

            for (var i = 0; i < vertices.Count; i++)
            {
                var projection = Vector2.Dot(vector, vertices[i]);

                if (i == 0)
                    result = new Vector2(projection, projection);

                if (projection > result.X)
                    result.X = projection;

                if (projection < result.Y)
                    result.Y = projection;
            }

            return result;
        }

        private float GetIntersectionLength(Vector2 firstProjection, Vector2 secondProjection)
        {
            return secondProjection.Y - firstProjection.X > 0
              ? secondProjection.Y - firstProjection.X
              : firstProjection.Y - secondProjection.X;
        }

        private class CheckResult
        {
            public Vector2 Mtv { get; }
            public float MtvLength { get; }

            public CheckResult(Vector2 mtv, float mtvLength)
            {
                Mtv = mtv;
                MtvLength = mtvLength;
            }
        }
    }
}
