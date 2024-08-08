﻿using System.Xml;
using LogFileReaderLibrary.Models;

namespace LogFileReaderLibrary.Services;

/// <summary>
/// This service analyses the contents of a log file.
/// </summary>
public class LogFileAnalyserService
{
    public int UniqueIpCount(List<HttpRequestLogEntry> logContent)
    {
        var ips = logContent.Select(x => x.IpAddress);
        var distinctIps = ips.Distinct();
        return distinctIps.Count();
    }

    public IDictionary<string, int> MostVisitedUrls(List<HttpRequestLogEntry> logContent, int top)
    {
        var dict = logContent
            .GroupBy(entry => entry.Resource)
            .Select(group => new 
            { 
                Url = group.Key, 
                Count = group.Count(), 
                LatestTimestamp = group.Max(entry => entry.Timestamp),
                LogEntries = group
            })
            .OrderByDescending(x => x.Count)
            .ThenByDescending(x => x.LatestTimestamp)
            .Take(top)
            .ToDictionary(group => group.Url, group => group.Count);
        
        return dict;
    }

    public IDictionary<string, int> MostActiveIps(List<HttpRequestLogEntry> logContent, int top)
    {
        var dict = logContent
            .GroupBy(entry => entry.IpAddress)
            .Select(group => new 
            { 
                IpAddress = group.Key, 
                Count = group.Count(), 
                LatestTimestamp = group.Max(entry => entry.Timestamp),
                LogEntries = group
            })
            .OrderByDescending(x => x.Count)
            .ThenByDescending(x => x.LatestTimestamp)
            .Take(top)
            .ToDictionary(group => group.IpAddress, group => group.Count);
        
        return dict;
    }
}