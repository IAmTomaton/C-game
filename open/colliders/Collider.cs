using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    class Collider
    {
        public float Angle { get; private set; }
        public Vector2 Position => (new Vector3(position) * RotateObject + gameObject.Position).Xy;
        public float Radius { get; }
        public bool IsTrigger { get; }
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

        public Collider(GameObject gameObject, float radius, Vector2 position = default, float angle = 0)
        {
            Init(gameObject, position, angle);
            Radius = radius;
        }

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
