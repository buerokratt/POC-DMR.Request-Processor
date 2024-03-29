﻿using Buerokratt.Common.CentOps.Extensions;
using Buerokratt.Common.CentOps.Interfaces;
using Buerokratt.Common.CentOps.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Buerokratt.Common.CentOps
{
    public class ParticipantPoller : BackgroundService
    {
        private const string PublicParticipantsEndpoint = "public/participants";
        private const string ApiKeyHeaderName = "X-Api-Key";
        private readonly CentOpsServiceSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ConcurrentDictionary<string, Participant> _participants;
        private readonly ILogger<ParticipantPoller> _logger;

        public ParticipantPoller(
            IHttpClientFactory httpClientFactory,
            CentOpsServiceSettings settings,
            ConcurrentDictionary<string, Participant> participants,
            ILogger<ParticipantPoller> logger)
        {
            _settings = settings;
            _httpClientFactory = httpClientFactory;
            _participants = participants;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var httpClient = _httpClientFactory.CreateClient("CentOpsClient");
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, _settings.CentOpsApiKey);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await RefreshCache(httpClient, stoppingToken).ConfigureAwait(false);
                    await Task
                        .Delay(
                            _settings.ParticipantCacheRefreshIntervalMs,
                            stoppingToken)
                        .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        private async Task RefreshCache(HttpClient httpClient, CancellationToken cancellationToken)
        {
            try
            {
                var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                options.Converters.Add(new JsonStringEnumConverter());

                var centOpsParticipantsUri = new Uri(_settings.CentOpsUri!, PublicParticipantsEndpoint);
                var participantList = await httpClient!.GetFromJsonAsync<IEnumerable<Participant>>(centOpsParticipantsUri, options, cancellationToken).ConfigureAwait(false);
                if (participantList != null)
                {
                    _logger.RefreshingParticipantCache();

                    // Add or update active participants.
                    foreach (var participant in participantList.Where(p => !string.IsNullOrEmpty(p.Name)))
                    {
                        _ = _participants.AddOrUpdate(
                            participant.Name!,
                            participant,
                            (key, old) => participant);
                    }

                    // Remove items which are no longer availiable.
                    var participantsToRemove = _participants.Keys.Except(participantList.Select(p => p.Name));
                    foreach (var participantToRemove in participantsToRemove)
                    {
                        _ = _participants.Remove(participantToRemove!, out _);
                    }

                    if (participantList.Any() || participantsToRemove.Any())
                    {
                        _logger.ParticipantCacheRefreshed(participantList.Count(), participantsToRemove.Count());
                    }
                }
            }
            catch (HttpRequestException httpReqException)
            {
                _logger.ParticipantCacheRefreshFailure(httpReqException);
            }
            catch (JsonException jsonException)
            {
                _logger.ParticipantCacheRefreshFailure(jsonException);
            }
        }
    }
}
