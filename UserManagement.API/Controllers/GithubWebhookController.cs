using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using UserManagement.Application.DTOs;
using UserManagement.Application.Interfaces;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("api/github")]
    public class GithubWebhookController : ControllerBase
    {
        private readonly IUserService _userService;

        public GithubWebhookController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> ReceiveWebhook([FromBody] GitHubPushPayload payload)
        {
            try
            {
                var githubEvent = Request.Headers["X-GitHub-Event"].ToString();

                if (githubEvent != "push")
                {
                    return Ok(new { message = "Ignored. Only push event is handled." });
                }

                if (payload == null)
                {
                    return BadRequest(new { message = "Payload is null." });
                }

                var userDto = new UserDto
                {
                    Name = payload.Pusher?.Name ?? payload.Sender?.Login ?? "Unknown User",
                    Username = payload.Sender?.Login ?? payload.Pusher?.Name ?? "unknown",
                    Email = payload.Pusher?.Email ?? $"{payload.Sender?.Login ?? "unknown"}@github.local"
                };

                await _userService.CreateUserAsync(userDto);

                Console.WriteLine("Webhook received");
                Console.WriteLine($"Event: {githubEvent}");
                Console.WriteLine($"Sender: {payload.Sender?.Login}");
                Console.WriteLine($"Repo: {payload.Repository?.FullName}");
                Console.WriteLine($"Commit: {payload.HeadCommit?.Message}");

                return Ok(new
                {
                    message = "Push webhook received and user created successfully.",
                    createdUser = userDto,
                    repository = payload.Repository?.FullName,
                    branch = payload.Ref,
                    commitMessage = payload.HeadCommit?.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while processing the webhook.",
                    error = ex.Message
                });
            }
        }
    }

    public class GitHubPushPayload
    {
        [JsonPropertyName("ref")]
        public string? Ref { get; set; }

        [JsonPropertyName("repository")]
        public GitHubRepository? Repository { get; set; }

        [JsonPropertyName("pusher")]
        public GitHubPusher? Pusher { get; set; }

        [JsonPropertyName("sender")]
        public GitHubSender? Sender { get; set; }

        [JsonPropertyName("head_commit")]
        public GitHubCommit? HeadCommit { get; set; }
    }

    public class GitHubRepository
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("full_name")]
        public string? FullName { get; set; }
    }

    public class GitHubPusher
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }

    public class GitHubSender
    {
        [JsonPropertyName("login")]
        public string? Login { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }
    }

    public class GitHubCommit
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}