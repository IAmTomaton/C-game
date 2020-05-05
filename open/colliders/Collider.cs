using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace Cgame
{
    /// <summary>
    /// Содержит информации для просчёта столкновений объекта.
    /// </summary>
    class Collider
    {
        /// <summary>
        /// Указывает должел ли данный коллайдер участвовать в столкновениях.
        /// </summary>
        public bool IsColliding { get; set; } = true;
        /// <summary>
        /// Угол поворока коллайдера, относительно его центра.
        /// </summary>
        public float Angle { get; private set; }
        /// <summary>
        /// Позиция центра коллайдера в глобальной системе координат.
        /// </summary>
        public Vector2 Position => (new Vector3(position) * RotateObject + gameObject.Position).Xy;
        /// <summary>
        /// Радиус коллайдера. Указывает минимальный радиус круга с центром в центре коллайдера и содержащий все его точки.
        /// </summary>
        public float Radius { get; }
        /// <summary>
        /// Указывает является ли коллайдер триггером. Триггеры не участвуют в столкновениях, но вызов метода Collision происходит.
        /// </summary>
        public bool IsTrigger { get; }
        /// <summary>
        /// Список всех вершин коллайдера в глобальной системе координат.
        /// </summary>
        public List<Vector2> Vertices => vertices
                    .Select(vert => vert * RotateCollider)
                    .Select(vert => vert + new Vector3(Position.X, Position.Y, 0))
                    .Select(vert => vert.Xy)
                    .ToList();

        private Matrix3 RotateCollider => Matrix3.CreateRotationZ(MathHelper.DegreesToRadians(Angle));
        private Matrix3 RotateObject => Matrix3.CreateRotationZ(MathHelper.DegreesToRadians(gameObject.Angle));
        private GameObject gameObject;
        private Vector2 position;
        private readonly List<Vector3> vertices = new List<Vector3>();

        /// <summary>
        /// Создаёт круглый коллайдер с указанным радиусом.
        /// </summary>
        /// <param name="gameObject">Обект-родитель коллайдера.</param>
        /// <param name="radius">Радиус коллайдера.</param>
        /// <param name="position">Позиция центра коллайдера в системе координат объекта.</param>
        public Collider(GameObject gameObject, float radius, Vector2 position = default)
        {
            Init(gameObject, position, 0);
            Radius = radius;
        }

        /// <summary>
        /// Создаёт примоугольный коллайдер с указанными высотой и шириной.
        /// </summary>
        /// <param name="gameObject">Обект-родитель коллайдера.</param>
        /// <param name="height">Высота.</param>
        /// <param name="width">Ширина.</param>
        /// <param name="position">Позиция центра коллайдера в системе координат объекта.</param>
        /// <param name="angle">Угол поворотаколлайдера относительно его центра.</param>
        public Collider(GameObject gameObject, float height, float width, Vector2 position = default, float angle = 0)
        {
            Init(gameObject, position, angle);
            vertices = new List<Vector3>
            {
                new Vector3(-width / 2, height / 2, 0),
                new Vector3(width / 2, height / 2, 0),
                new Vector3(width / 2, -height / 2, 0),
                new Vector3(-width / 2, -height / 2, 0)
            };
            Radius = vertices[0].Length;
        }

        /// <summary>
        /// Создаёт коллайдер с указанным набором вершин.
        /// </summary>
        /// <param name="gameObject">Обект-родитель коллайдера.</param>
        /// <param name="vertices">Список вершин. Перечисление по часовой стрелке.</param>
        /// <param name="position">Позиция центра коллайдера в системе координат объекта.</param>
        /// <param name="angle">Угол поворотаколлайдера относительно его центра.</param>
        public Collider(GameObject gameObject, List<Vector2> vertices, Vector2 position = default, float angle = 0)
        {
            Init(gameObject, position, angle);
            this.vertices = vertices
                .Select(vert => new Vector3(vert.X, vert.Y, 0))
                .ToList();
            Radius = vertices
                .Select(vert => vert.Length)
                .Max();
        }

        /// <summary>
        /// Возвращает список нормалей ко всем граням коллайдера.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Vector2> GetNornals()
        {
            var vertices = Vertices;
            for (var i = 0; i < vertices.Count; i++)
            {
                yield return (vertices[i] - vertices[(i + 1) % vertices.Count]).PerpendicularLeft.Normalized();
            }
        }

        private void Init(GameObject gameObject, Vector2 position, float angle)
        {
            this.gameObject = gameObject;
            this.position = position;
            Angle = angle;
        }
    }
}
