using Cgame.Core.Graphic;
using Cgame.Core.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace Cgame.Core
{
    /// <summary>
    /// Главный класс.
    /// Если не нужно разбираться с графикой, то сюда лучше не лезть.
    /// </summary>
    class Game : GameWindow
    {
        private Shader shader;
        private TextureLibrary textureLibrary;
        private Space space;
        private Camera admCamera;
        private bool adm;

        private bool firstMove = true;
        private Vector2 lastPos;

        private readonly Dictionary<Key, bool> pressedButton = new Dictionary<Key, bool> {
            { Key.T, false }
        };

        public Game(int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            //shader
            shader = new Shader("core/shaders/shader.vert", "core/shaders/shader.frag");
            shader.Use();

            textureLibrary = TextureLidraryLoader.LoadTextureLibrary(shader);
            admCamera = new Camera(Vector3.UnitZ * 500, Width, Height);
            var camera = new Camera(Vector3.UnitZ * 500, Width, Height);
            space = new Space(camera);
            GameContext.Init(space);

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            shader.Use();
            var camera = adm ? admCamera : space.Camera;

            foreach (var sprite in space.GetSprites())
                RenderSprite(sprite, camera);

            SwapBuffers();

            base.OnRenderFrame(e);
        }

        private void RenderSprite(Sprite sprite, Camera camera)
        {
            textureLibrary.GetTexture(sprite.Texture).Use();
            textureLibrary.GetPrimitive(sprite.Texture).Use();

            var model = Matrix4.Identity;
            model *= Matrix4.CreateRotationZ(sprite.Angle);
            model *= Matrix4.CreateTranslation(sprite.Position);

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            GL.DrawElements(PrimitiveType.Triangles, textureLibrary.IndicesLength,
                DrawElementsType.UnsignedInt, 0);

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindVertexArray(0);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            var input = Keyboard.GetState();
            var mouse = Mouse.GetState();

            if (!adm)
                space.Update((float)e.Time, input, mouse);

            if (!Focused)
            {
                firstMove = true;
                return;
            }


            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            if (input.IsKeyDown(Key.T) && !pressedButton[Key.T])
            {
                adm = !adm;
            }
            pressedButton[Key.T] = input.IsKeyDown(Key.T);

            if (input.IsKeyDown(Key.C) && !pressedButton[Key.C])
            {
                CursorVisible = !CursorVisible;
            }
            pressedButton[Key.C] = input.IsKeyDown(Key.C);

            const float cameraSpeed = 200f;
            const float sensitivity = 0.2f;

            if (adm)
            {
                if (input.IsKeyDown(Key.Space))
                {
                    admCamera.Position += admCamera.Up * cameraSpeed * (float)e.Time;
                }
                if (input.IsKeyDown(Key.LControl))
                {
                    admCamera.Position -= admCamera.Up * cameraSpeed * (float)e.Time;
                }
                if (input.IsKeyDown(Key.A))
                {
                    admCamera.Position -= admCamera.Right * cameraSpeed * (float)e.Time;
                }
                if (input.IsKeyDown(Key.D))
                {
                    admCamera.Position += admCamera.Right * cameraSpeed * (float)e.Time;
                }
                if (input.IsKeyDown(Key.S))
                {
                    admCamera.Position -= admCamera.Front * cameraSpeed * (float)e.Time;
                }
                if (input.IsKeyDown(Key.W))
                {
                    admCamera.Position += admCamera.Front * cameraSpeed * (float)e.Time;
                }

                if (!CursorVisible)
                {
                    if (firstMove)
                    {
                        lastPos = new Vector2(mouse.X, mouse.Y);
                        firstMove = false;
                    }
                    else
                    {
                        var deltaX = mouse.X - lastPos.X;
                        var deltaY = mouse.Y - lastPos.Y;
                        lastPos = new Vector2(mouse.X, mouse.Y);

                        admCamera.Yaw += deltaX * sensitivity;
                        admCamera.Pitch -= deltaY * sensitivity;
                    }
                }
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (Focused && !CursorVisible)
            {
                Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);
            }

            base.OnMouseMove(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            admCamera.Width = Width;
            admCamera.Height = Height;
            space.Camera.Width = Width;
            space.Camera.Height = Height;
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
