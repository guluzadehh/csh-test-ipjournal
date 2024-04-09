using System.Net;
using IpJournal.Core.Filter;
using IpJournal.Core.Models;

namespace IpJournal.Tests
{
    public class RequestLogsFilterTests
    {
        [Fact]
        public void Filter_NoFilters_ReturnsAllRequestLogs()
        {
            var filter = new RequestLogsFilter();
            var requestLogs = new List<RequestLog>
            {
                new RequestLog(IPAddress.Parse("192.168.1.1"), DateTimeOffset.Now),
                new RequestLog(IPAddress.Parse("192.168.1.2"), DateTimeOffset.Now),
                new RequestLog(IPAddress.Parse("192.168.1.3"), DateTimeOffset.Now)
            };

            var filteredLogs = filter.Filter(requestLogs, new FilterOptions(DateTimeOffset.MinValue, DateTimeOffset.MaxValue));

            Assert.Equal(requestLogs.Count, filteredLogs.Count());
        }

        [Fact]
        public void Filter_WithIpAddressFilter_ReturnsFilteredRequestLogs()
        {
            var ipAddressToFilter = IPAddress.Parse("192.168.1.1");
            var filter = new RequestLogsFilter((log, options) => log.IpAddress.Equals(ipAddressToFilter));
            var requestLogs = new List<RequestLog>
            {
                new RequestLog(IPAddress.Parse("192.168.1.1"), DateTimeOffset.Now),
                new RequestLog(IPAddress.Parse("192.168.1.2"), DateTimeOffset.Now),
                new RequestLog(IPAddress.Parse("192.168.1.3"), DateTimeOffset.Now)
            };

            var filteredLogs = filter.Filter(requestLogs, new FilterOptions(DateTimeOffset.MinValue, DateTimeOffset.MaxValue));

            Assert.Single(filteredLogs);
            Assert.Equal(ipAddressToFilter, filteredLogs.First().IpAddress);
        }

        [Fact]
        public void Filter_WithDateRangeFilter_ReturnsFilteredRequestLogs()
        {
            var startDate = DateTimeOffset.Parse("2024-01-01");
            var endDate = DateTimeOffset.Parse("2024-01-10");
            var filter = new RequestLogsFilter((log, options) => log.Date >= options.StartDate && log.Date <= options.EndDate);
            var requestLogs = new List<RequestLog>
            {
                new RequestLog(IPAddress.Parse("192.168.1.1"), DateTimeOffset.Parse("2024-01-05")),
                new RequestLog(IPAddress.Parse("192.168.1.2"), DateTimeOffset.Parse("2024-01-09")),
                new RequestLog(IPAddress.Parse("192.168.1.3"), DateTimeOffset.Parse("2024-01-15"))
            };

            var filteredLogs = filter.Filter(requestLogs, new FilterOptions(startDate, endDate));

            Assert.Equal(2, filteredLogs.Count());
            Assert.All(filteredLogs, log => Assert.True(log.Date >= startDate && log.Date <= endDate));
        }

        [Fact]
        public void Register_SingleCallback_AddsCallbackToList()
        {
            var filter = new RequestLogsFilter();
            FilterCb callback = (log, options) => true;

            filter.Register(callback);

            Assert.Contains(callback, filter.FilterCbs);
        }

        [Fact]
        public void Register_MultipleCallbacks_AddsAllCallbacksToList()
        {
            var filter = new RequestLogsFilter();
            var callbacks = new List<FilterCb>
            {
                (log, options) => true,
                (log, options) => false
            };

            filter.Register(callbacks);

            Assert.Equal(callbacks.Count, filter.FilterCbs.Count());
            Assert.All(callbacks, callback => Assert.Contains(callback, filter.FilterCbs));
        }

        [Fact]
        public void Filter_WithNoMatchingFilters_ReturnsEmptyList()
        {
            var filter = new RequestLogsFilter((log, options) => log.IpAddress.Equals(IPAddress.Parse("192.168.1.2")) && log.Date > DateTimeOffset.Parse("2024-01-01"));
            var requestLogs = new List<RequestLog>
            {
                new RequestLog(IPAddress.Parse("192.168.1.1"), DateTimeOffset.Parse("2024-01-05")),
                new RequestLog(IPAddress.Parse("192.168.1.1"), DateTimeOffset.Parse("2024-01-09")),
                new RequestLog(IPAddress.Parse("192.168.1.1"), DateTimeOffset.Parse("2024-01-15"))
            };

            var filteredLogs = filter.Filter(requestLogs, new FilterOptions(DateTimeOffset.MinValue, DateTimeOffset.MaxValue));

            Assert.Empty(filteredLogs);
        }

        [Fact]
        public void Filter_NullRequestLogs_ThrowsArgumentNullException()
        {
            var filter = new RequestLogsFilter();

            Assert.Throws<ArgumentNullException>(() => filter.Filter(null, new FilterOptions(DateTimeOffset.MinValue, DateTimeOffset.MaxValue)));
        }

        [Fact]
        public void Filter_NullFilterOptions_ThrowsArgumentNullException()
        {
            var filter = new RequestLogsFilter();
            var requestLogs = new List<RequestLog>();

            Assert.Throws<ArgumentNullException>(() => filter.Filter(requestLogs, null));
        }
    }
}
