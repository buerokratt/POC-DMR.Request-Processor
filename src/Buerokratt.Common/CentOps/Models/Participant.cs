﻿namespace Buerokratt.Common.CentOps.Models
{
    public class Participant
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Host { get; set; }

        public ParticipantType Type { get; set; }
    }
}
