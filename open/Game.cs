using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Cgame
{
    class Game : GameWindow
    {
        private Camera camera;
        private Shader shader;
        private TextureLibrary textureLibrary;
        private Space space;

        public Game(int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            //shader
            shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
            shader.Use();

            textureLibrary = new TextureLibrary(shader);
            space = new Space(textureLibrary);

            camera = new Camera(Vector3.UnitZ * 3);
            //camera = new Camera(Vector3.UnitZ * 3, Width / (float)Height);

            //CursorVisible = false;

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.Use();

            foreach (var sprite in space.GetSprites())
                RenderSprite(sprite);

            SwapBuffers();

            base.OnRenderFrame(e);
        }

        private void RenderSprite(Sprite sprite)
        {
            GL.BindVertexArray(textureLibrary.Get(sprite.Texture));

            var model = Matrix4.Identity;
            model *= Matrix4.CreateRotationZ(sprite.Angle);
            model *= Matrix4.CreateTranslation(sprite.Position);

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            GL.DrawElements(PrimitiveType.Triangles, textureLibrary.IndicesLength,
                DrawElementsType.UnsignedInt, 0);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            space.Update(e.Time);

            if (!Focused)
            {
                return;
            }

            var input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            const float cameraSpeed = 100f;

            if (input.IsKeyDown(Key.W))
            {
                camera.Position += camera.Up * cameraSpeed * (float)e.Time;
            }
            if (input.IsKeyDown(Key.S))
            {
                camera.Position -= camera.Up * cameraSpeed * (float)e.Time;
            }
            if (input.IsKeyDown(Key.A))
            {
                camera.Position -= camera.Right * cameraSpeed * (float)e.Time;
            }
            if (input.IsKeyDown(Key.D))
            {
                camera.Position += camera.Right * cameraSpeed * (float)e.Time;
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            camera.Weidth = Width;
            camera.Height = Height;
            base.OnResize(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteProgram(shader.Handle);
            base.OnUnload(e);
        }
    }
}
