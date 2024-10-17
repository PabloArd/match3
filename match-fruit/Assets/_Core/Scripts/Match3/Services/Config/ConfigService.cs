using Match3.ScriptableObjects;
using System;
using Zenject;

namespace Match3.Services.Config
{
    public class ConfigService : IConfigService, IDisposable
    {
        public MatchGameSettings MatchGameSettings => _matchGameSettings;
        public MatchInfo MatchInfo => _matchInfo;

        private MatchGameSettings _matchGameSettings;
        private MatchInfo _matchInfo;

        public ConfigService(MatchGameSettings matchGameSettings)
        {
            _matchGameSettings = matchGameSettings;
            _matchInfo = new MatchInfo();
            _matchInfo.Initialize(_matchGameSettings.Pieces);
        }

        public void Dispose()
        {
            if (_matchInfo != null)
            {
                _matchInfo.Dispose();
                _matchInfo = null;
            }
        }
    }
}

