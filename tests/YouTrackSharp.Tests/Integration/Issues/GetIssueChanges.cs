using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Xunit.Sdk;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class GetIssueChanges
        {
            //https://ytsharp.myjetbrains.com/youtrack/rest/issue/DP1-1/changes
            //https://yt.skbkontur.ru/rest/issue
            //https://yt.skbkontur.ru/rest/issue/FMS-956
            //https://yt.skbkontur.ru/rest/issue/FMS-956/changes
            //Authorization: Bearer perm:ZGVtbzE=.WW91VHJhY2tTaGFycA==.AX3uf8RYk3y2bupWA1xyd9BhAHoAxc
            //Accept: application/json
            
            [Fact]
            public async Task Valid_Connection_Returns_Correct_Changes()
            {
                File.Delete("result.txt");
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateIssuesService();

                // Act
                var result = await service.GetChanges("DP1-1");

                // Assert
                Assert.NotNull(result);
                Assert.Equal("DP1-1", result.Issue.Id);
                
                //Assert.Equal(10, result.ChangesCollection.Count());
                
                /*foreach (var change in result.ChangesCollection)
                {
                    File.AppendAllText("result.txt", $"{change.UpdaterName} {change.Updated} {change.ChangedFields.Count()} {change.ChangedFields.First().NewValueField.Name}:{change.ChangedFields.First().NewValueField.Value}\n");
                }*/
                File.AppendAllText("result.txt", "\n" + JsonConvert.SerializeObject(result));
                
                
            }
        }
    }
}