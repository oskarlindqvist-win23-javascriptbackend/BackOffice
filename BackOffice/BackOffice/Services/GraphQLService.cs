using Infrastructure.Models;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BackOffice.Services
{
    public class GraphQLService
    {
        private readonly HttpClient _httpClient;

        public GraphQLService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Course> CreateCourseAsync(object mutation)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://courseprovider-oskarl.azurewebsites.net/api/graphql?code=xC5zjH68LnD9xsWJLfzI7LCQYKV6SRJ8IFDD1tvdrEmEAzFuk9XzNw%3D%3D", mutation);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<GraphQLResponse<Course>>();
                if (result != null && result.Data != null)
                {
                    return result.Data;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("GraphQL error content: " + errorContent);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            return null!;
        }

        public async Task<Course> UpdateCourseAsync(Course input)
        {
            try
            {
                var mutation = new
                {
                    query = @"mutation ($input: CourseUpdateInput!) {
                updateCourse(input: $input) {
                    id
                    title
                }
            }",
                    variables = new { input }
                };

                var response = await _httpClient.PostAsJsonAsync("graphql", mutation);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<GraphQLResponse<Course>>();
                if (result != null && result.Data != null)
                {
                    return result.Data;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null!;
        }

        public async Task<bool> DeleteCourseAsync(string id)
        {
            try
            {
                var mutation = new
                {
                    query = @"mutation ($id: ID!) {
                deleteCourse(id: $id)
            }",
                    variables = new { id }
                };

                var response = await _httpClient.PostAsJsonAsync("graphql", mutation);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<GraphQLResponse<bool>>();
                if (result != null)
                {
                    return result.Data;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }
    }

    public class GraphQLResponse<T>
    {
        public T? Data { get; set; }
    }
}
