
namespace Rover.Settings
{
    public static class GameSettings
    {
        public const float MESSAGE_TIMEOUT = 10f;
        public const float INPUT_TIMER_DELAY = 0.05f;
        public const string INPUT_BUNDLE_NAME = "InputRead";
        public const string OUTPUT_BUNDLE_NAME = "OutputSend";
        public const float PROXIMITY_CHECK_DELAY = 0.1f;
        public const int GAME_RES_X = 800;
        public const int GAME_RES_Y = 600;
        public const float PHOTO_LOAD_TIME = 10f;
        public const float PHOTO_VIEWER_LOAD_TIME = 2f;
        public const int PROXIMITY_LAYER_INDEX = 6;
    }
}

namespace Rover.Can
{
    public static class CanIDs
    {
        public const ushort SYSTEM_CAM = 0x2000;
    }
}

