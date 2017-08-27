using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    public partial class IssuesService
    {
        public async Task<Changes> GetChanges(string issueId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/{issueId}/changes");

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<Changes>(await response.Content.ReadAsStringAsync());
        }
    }
}