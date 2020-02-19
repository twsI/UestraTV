using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UestraTV.Data;

namespace UestraTV.Services
{
    internal interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }

    internal class ScopedProcessingService : IScopedProcessingService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;
        private readonly IHubContext<UpdateHub, IUpdate> _updateHub;
        private readonly BroadcastService _broadcastService;
        private ServiceOptions _options;
        private List<Broadcast> _lastResult;

        public ScopedProcessingService(BroadcastService broadcastService, IOptionsMonitor<ServiceOptions> optionsAccessor, IHubContext<UpdateHub, IUpdate> updateHub, ILogger<ScopedProcessingService> logger)
        {
            _broadcastService = broadcastService;
            _options = optionsAccessor.CurrentValue;
            _updateHub = updateHub;
            _logger = logger;
            _lastResult = new List<Broadcast>();
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                executionCount++;

                _logger.LogInformation(
                    "Scoped Processing Service is working. Count: {Count}", executionCount);

                var broadcasts = await _broadcastService.GetBroadcastAsync(ProgramWeekday.Today);

                if (broadcasts.Count != _lastResult.Count)
                {
                    broadcasts.RemoveRange(0, executionCount);
                    await _updateHub.Clients.All.Update(DateTime.Now, true, broadcasts, $"{broadcasts.Count} slides");
                }
                else
                {
                    await _updateHub.Clients.All.Update(DateTime.Now, false, null, string.Empty);
                }
                _lastResult = broadcasts;

                await Task.Delay(_options.UpdateInterval, stoppingToken);
            }
        }
    }
}
