using FluentAssertions;
using Microsoft.Extensions.Configuration;
using TaskManager.Api.Entities;
using TaskManager.Api.Repositories;
using Xunit;
using TaskStatus = TaskManager.Api.Entities.TaskStatus;

namespace TaskManager.Tests.Repositories;

public class JsonTaskRepositoryTests : IDisposable
{
    private readonly string _testDataDirectory;
    private readonly JsonTaskRepository _repository;

    public JsonTaskRepositoryTests()
    {
        // Criar diretório temporário para testes
        _testDataDirectory = Path.Combine(Path.GetTempPath(), $"TaskManagerTests_{Guid.NewGuid()}");
        Directory.CreateDirectory(_testDataDirectory);

        // Configurar repository com diretório de teste
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DataDirectory"] = _testDataDirectory
            })
            .Build();

        _repository = new JsonTaskRepository(configuration);
    }

    public void Dispose()
    {
        // Limpar arquivos de teste
        if (Directory.Exists(_testDataDirectory))
        {
            Directory.Delete(_testDataDirectory, true);
        }
    }

    [Fact]
    public async Task AddAsync_ShouldAddTaskAndGenerateId()
    {
        // Arrange
        var task = new TaskItem
        {
            Title = "Test Task",
            Description = "Test Description",
            Status = TaskStatus.Pending
        };

        // Act
        var result = await _repository.AddAsync(task);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Title.Should().Be("Test Task");
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        result.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTasks()
    {
        // Arrange
        var task1 = new TaskItem { Title = "Task 1", Description = "Desc 1", Status = TaskStatus.Pending };
        var task2 = new TaskItem { Title = "Task 2", Description = "Desc 2", Status = TaskStatus.InProgress };
        
        await _repository.AddAsync(task1);
        await _repository.AddAsync(task2);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTask_WhenTaskExists()
    {
        // Arrange
        var task = new TaskItem { Title = "Test Task", Description = "Test Desc", Status = TaskStatus.Pending };
        var addedTask = await _repository.AddAsync(task);

        // Act
        var result = await _repository.GetByIdAsync(addedTask.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(addedTask.Id);
        result.Title.Should().Be("Test Task");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenTaskDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTask_WhenTaskExists()
    {
        // Arrange
        var task = new TaskItem { Title = "Original", Description = "Original Desc", Status = TaskStatus.Pending };
        var addedTask = await _repository.AddAsync(task);

        addedTask.Title = "Updated";
        addedTask.Description = "Updated Desc";
        addedTask.Status = TaskStatus.Completed;

        // Act
        var result = await _repository.UpdateAsync(addedTask);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Updated");
        result.Description.Should().Be("Updated Desc");
        result.Status.Should().Be(TaskStatus.Completed);
        result.UpdatedAt.Should().NotBeNull();
        result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenTaskDoesNotExist()
    {
        // Arrange
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Non-existent",
            Description = "Desc",
            Status = TaskStatus.Pending
        };

        // Act
        var result = await _repository.UpdateAsync(task);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteTask_WhenTaskExists()
    {
        // Arrange
        var task = new TaskItem { Title = "To Delete", Description = "Desc", Status = TaskStatus.Pending };
        var addedTask = await _repository.AddAsync(task);

        // Act
        var result = await _repository.DeleteAsync(addedTask.Id);

        // Assert
        result.Should().BeTrue();
        
        var allTasks = await _repository.GetAllAsync();
        allTasks.Should().NotContain(t => t.Id == addedTask.Id);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenTaskDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.DeleteAsync(nonExistentId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Repository_ShouldPersistDataBetweenInstances()
    {
        // Arrange - Criar task com primeira instância
        var task = new TaskItem { Title = "Persistent Task", Description = "Desc", Status = TaskStatus.Pending };
        var addedTask = await _repository.AddAsync(task);

        // Act - Criar nova instância do repository
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DataDirectory"] = _testDataDirectory
            })
            .Build();
        var newRepository = new JsonTaskRepository(configuration);
        
        var result = await newRepository.GetByIdAsync(addedTask.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Persistent Task");
    }

    [Fact]
    public async Task AddAsync_ShouldHandleMultipleTasks()
    {
        // Arrange & Act
        var tasks = new List<TaskItem>();
        for (int i = 1; i <= 5; i++)
        {
            var task = new TaskItem
            {
                Title = $"Task {i}",
                Description = $"Description {i}",
                Status = TaskStatus.Pending
            };
            var added = await _repository.AddAsync(task);
            tasks.Add(added);
        }

        // Assert
        var allTasks = await _repository.GetAllAsync();
        allTasks.Should().HaveCount(5);
        
        // Verificar que todos têm IDs únicos
        var ids = allTasks.Select(t => t.Id).ToList();
        ids.Should().OnlyHaveUniqueItems();
    }
}