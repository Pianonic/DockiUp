﻿using DockiUp.Application.Dtos;
using DockiUp.Application.Models;
using DockiUp.Infrastructure;
using MediatR;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace DockiUp.Application.Commands
{
    public class CreateContainerCommand : IRequest
    {
        public CreateContainerCommand(CreateContainerDto createContainerDto)
        {
            CreateContainerDto = createContainerDto;
        }

        public CreateContainerDto CreateContainerDto { get; set; }
    }

    public class CreateContainerCommandHandler : IRequestHandler<CreateContainerCommand>
    {
        public readonly DockiUpDbContext _DbContext;
        private readonly SystemPaths _systemPaths;

        public CreateContainerCommandHandler(DockiUpDbContext dbContext, IOptions<SystemPaths> systemPaths)
        {
            _DbContext = dbContext;
            _systemPaths = systemPaths.Value;
        }

        public async Task Handle(CreateContainerCommand request, CancellationToken cancellationToken)
        {
            string projectsPath = _systemPaths.ProjectsPath;
            string repoName = request.CreateContainerDto.ContainerName;
            string clonePath = Path.Combine(projectsPath, repoName);

            if (Directory.Exists(clonePath))
            {
                throw new ArgumentException("Container already exists.");
            }

            // Ensure the projects directory exists
            Directory.CreateDirectory(projectsPath);

            // Run git clone command
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"clone {request.CreateContainerDto.GitUrl} \"{clonePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.Start();

                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync();

                if (process.ExitCode == 0)
                {
                    Console.WriteLine($"Repository cloned successfully to {clonePath}");
                }
                else
                {
                    throw new ArgumentException($"Failed to clone repository. Error: {error}");
                }
            }
        }
    }
}
