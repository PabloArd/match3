using Match3.ScriptableObjects;

namespace Match3.Services.Config
{
    public interface IConfigService
    {
        public MatchGameSettings MatchGameSettings { get; }
        public MatchInfo MatchInfo { get; }
    }
}
