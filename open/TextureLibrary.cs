using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Cgame
{
    class TextureLibrary
    {
        private readonly Dictionary<string, int> descriptors = new Dictionary<string, int>();
        private readonly Shader shader;
        public int IndicesLength => indices.Length;

        private readonly float[] vertices =
        {
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f,
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f
        };

        private readonly uint[] indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        public TextureLibrary(Shader shader)
        {
            this.shader = shader;
            AddTexture("base", "textures/image.bmp");
        }

        public void AddTexture(string name, string path)
        {
            var localVertices = new float[vertices.Length];
            vertices.CopyTo(localVertices, 0);

            //texture
            var texture = new Texture(path);
            texture.Use();

            for (var i = 0; i < 4; i++)
            {
                localVertices[i * 5] = vertices[i * 5] * texture.Width;
                localVertices[i * 5 + 1] = vertices[i * 5 + 1] * texture.Height;
            }

            //VBO
            var vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, localVertices.Length * sizeof(float),
                localVertices, BufferUsageHint.StaticDraw);

            //EBO
            var elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);

            GL.BufferData(BufferTarget.ElementArrayBuffer,
                indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            //VAO
            var vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);

            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float,
                false, 5 * sizeof(float), 0);

            var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float,
                false, 5 * sizeof(float), 3 * sizeof(float));

            descriptors[name] = vertexArrayObject;
        }

        public int Get(string name)
        {
            return descriptors.ContainsKey(name) ? descriptors[name] : descriptors["base"];
        }
    }
}
