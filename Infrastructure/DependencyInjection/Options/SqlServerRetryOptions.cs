﻿using System.ComponentModel.DataAnnotations;

public record SqlServerRetryOptions
{
    [Required, Range(5, 20)] public int MaxRetryCount { get; set; }
    [Required, Timestamp] public TimeSpan MaxRetryDelay { get; set; }
    public int[]? ErrorNumbersToAdd { get; set; }
}
