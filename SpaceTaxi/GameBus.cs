using DIKUArcade.EventBus;

namespace SpaceTaxi_1 {
    public static class GameBus {
        private static GameEventBus<object> eventBus;

        public static GameEventBus<object> GetBus() {
            return GameBus.eventBus ?? (GameBus.eventBus =
                       new GameEventBus<object>());
        }
    }
}