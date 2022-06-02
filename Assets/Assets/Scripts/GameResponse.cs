namespace Assets.Scripts
{
    [System.Serializable]
    public class GameResponse
    {
        public Game game_data;
        public Player player_data;
        public Period period_data;
    }

    [System.Serializable]
    public class Game
    {
        public int pk;
        public bool can_start;
        public string created_at;
        public string finished_at;
        public string game_status;
        public bool is_joinable;
        public int num_periods;
        public int num_players;

        // public Game(int pk, bool canStart, string createdAt, string finishedAt, string gameStatus, bool isJoinable, int numPeriods, int numPlayers)
        // {
        //     this.pk = pk;
        //     this.canStart = canStart;
        //     this.createdAt = createdAt;
        //     this.finishedAt = finishedAt;
        //     this.gameStatus = gameStatus;
        //     this.isJoinable = isJoinable;
        //     this.numPeriods = numPeriods;
        //     this.numPlayers = numPlayers;
        // }
    }
    
    [System.Serializable]
    public class Player
    {
        public int pk;
        public bool ordered;
        public int game;
        public int role;
        public int user;

        // public Player(int pk, bool ordered, int game, int role, int user)
        // {
        //     this.pk = pk;
        //     this.ordered = ordered;
        //     this.game = game;
        //     this.role = role;
        //     this.user = user;
        // }
    }
    
    [System.Serializable]
    public class Period
    {
        public int pk;
        public int backorders;
        public int costs;
        public int current_period;
        public bool everyone_ordered;
        public int game;
        public int incoming_goods;
        public int inventory;
        public int orders_processing;
        public int outgoing_goods;
        public int placed_orders;
        public int player;
        public int raw_materials;
        public int incoming_orders;

        // public Period(int pk, int backorders, int costs, int currentPeriod, bool everyoneOrdered, int game, int incomingGoods, int incomingOrders, int inventory, int ordersProcessing, int outgoingGoods, int placedOrders, int player, int rawMaterials)
        // {
        //     this.pk = pk;
        //     this.backorders = backorders;
        //     this.costs = costs;
        //     this.currentPeriod = currentPeriod;
        //     this.everyoneOrdered = everyoneOrdered;
        //     this.game = game;
        //     this.incomingGoods = incomingGoods;
        //     this.inventory = inventory;
        //     this.ordersProcessing = ordersProcessing;
        //     this.outgoingGoods = outgoingGoods;
        //     this.placedOrders = placedOrders;
        //     this.player = player;
        //     this.rawMaterials = rawMaterials;
        //     this.incomingOrders = incomingOrders;
        // }
    }
}