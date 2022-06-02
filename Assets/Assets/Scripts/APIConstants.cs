namespace Assets.Scripts
{
    public struct APIConstants
    {
        private const string APIBase = "https://www.vr-teaching.net/";
        public const string AuthTokenAddress = APIBase + "auth-token/";
        public const string JoinGameAddress = APIBase + "api/games/join_game/";
        public const string WaitingRoomAddress = APIBase + "api/games/check_can_start/";
        public const string OrderAddress = APIBase + "api/games/place_order/";
        public const string NextRoundAddress = APIBase + "api/games/next_round/";
    }
}