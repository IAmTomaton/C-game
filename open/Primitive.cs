using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    class Primitive
    {
        public int Handle { get; }

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

        public Primitive(int width, int height, Shader shader)
        {
            var localVertices = new float[vertices.Length];
            vertices.CopyTo(localVertices, 0);

            for (var i = 0; i < 4; i++)
            {
                localVertices[i * 5] = vertices[i * 5] * width;
                localVertices[i * 5 + 1] = vertices[i * 5 + 1] * height;
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

            Handle = vertexArrayObject;
        }

        public void Use()
        {
            GL.BindVertexArray(Handle);
        }
    }
}
