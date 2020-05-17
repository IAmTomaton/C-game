using OpenTK.Input;

namespace Cgame.Core
{
    /// <summary>
    /// Статический класс для управления пространством.
    /// </summary>
    static class GameContext
    {
        public static bool IsInitialized { get; private set; }
        /// <summary>
        /// Игровое пространство.
        /// </summary>
        public static ISpaceContext Space => s;
        /// <summary>
        /// Промежуток времени прошедший с последнего обновления.
        /// </summary>
        public static float DelayTime => Space.DelayTime;
        public static KeyboardState Keyboard => Space.Keyboard;
        public static MouseState Mouse => Space.Mouse;

        private static Space s;

        public static void Init(Space space)
        {
            s = space;

            IsInitialized = true;
        }
    }
}
