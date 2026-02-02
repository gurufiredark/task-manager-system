using FluentAssertions;
using Moq;
using TaskManager.Api.DTOs;
using TaskManager.Api.Entities;
using TaskManager.Api.Repositories;
using TaskManager.Api.Services;
using Xunit;
using TaskStatus = TaskManager.Api.Entities.TaskStatus;

namespace TaskManager.Tests.Services;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _repositoryMock;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _repositoryMock = new Mock<ITaskRepository>();
        _service = new TaskService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnAllTasks_WhenNoFilterIsProvided()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            new() { Id = Guid.NewGuid(), Title = "Task 1", Description = "Desc 1", Status = TaskStatus.Pending, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Title = "Task 2", Description = "Desc 2", Status = TaskStatus.InProgress, CreatedAt = DateTime.UtcNow }
        };
        
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

        // Act
        var result = await _service.GetAllTasksAsync();

        // Assert
        result.Should().HaveCount(2);
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnFilteredTasks_WhenStatusFilterIsProvided()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            new() { Id = Guid.NewGuid(), Title = "Task 1", Description = "Desc 1", Status = TaskStatus.Pending, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Title = "Task 2", Description = "Desc 2", Status = TaskStatus.InProgress, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Title = "Task 3", Description = "Desc 3", Status = TaskStatus.Pending, CreatedAt = DateTime.UtcNow }
        };
        
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

        // Act
        var result = await _service.GetAllTasksAsync(status: TaskStatus.Pending);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(t => t.Status == "Pending");
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnFilteredTasks_WhenCreatedAfterFilterIsProvided()
    {
        // Arrange
        var baseDate = new DateTime(2026, 2, 1);
        var tasks = new List<TaskItem>
        {
            new() { Id = Guid.NewGuid(), Title = "Old Task", Description = "Desc", Status = TaskStatus.Pending, CreatedAt = baseDate.AddDays(-5) },
            new() { Id = Guid.NewGuid(), Title = "Recent Task 1", Description = "Desc", Status = TaskStatus.Pending, CreatedAt = baseDate.AddDays(1) },
            new() { Id = Guid.NewGuid(), Title = "Recent Task 2", Description = "Desc", Status = TaskStatus.Pending, CreatedAt = baseDate.AddDays(2) }
        };
        
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

        // Act
        var result = await _service.GetAllTasksAsync(createdAfter: baseDate);

        // Assert
        result.Should().HaveCount(2);
        result.Select(t => t.Title).Should().Contain(new[] { "Recent Task 1", "Recent Task 2" });
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnFilteredTasks_WhenCreatedBeforeFilterIsProvided()
    {
        // Arrange
        var baseDate = new DateTime(2026, 2, 10);
        var tasks = new List<TaskItem>
        {
            new() { Id = Guid.NewGuid(), Title = "Old Task 1", Description = "Desc", Status = TaskStatus.Pending, CreatedAt = baseDate.AddDays(-5) },
            new() { Id = Guid.NewGuid(), Title = "Old Task 2", Description = "Desc", Status = TaskStatus.Pending, CreatedAt = baseDate.AddDays(-2) },
            new() { Id = Guid.NewGuid(), Title = "New Task", Description = "Desc", Status = TaskStatus.Pending, CreatedAt = baseDate.AddDays(2) }
        };
        
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

        // Act
        var result = await _service.GetAllTasksAsync(createdBefore: baseDate);

        // Assert
        result.Should().HaveCount(2);
        result.Select(t => t.Title).Should().Contain(new[] { "Old Task 1", "Old Task 2" });
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnFilteredTasks_WhenBothDateFiltersAreProvided()
    {
        // Arrange
        var startDate = new DateTime(2026, 2, 1);
        var endDate = new DateTime(2026, 2, 10);
        var tasks = new List<TaskItem>
        {
            new() { Id = Guid.NewGuid(), Title = "Before", Description = "Desc", Status = TaskStatus.Pending, CreatedAt = startDate.AddDays(-5) },
            new() { Id = Guid.NewGuid(), Title = "In Range 1", Description = "Desc", Status = TaskStatus.Pending, CreatedAt = startDate.AddDays(2) },
            new() { Id = Guid.NewGuid(), Title = "In Range 2", Description = "Desc", Status = TaskStatus.Pending, CreatedAt = startDate.AddDays(5) },
            new() { Id = Guid.NewGuid(), Title = "After", Description = "Desc", Status = TaskStatus.Pending, CreatedAt = endDate.AddDays(5) }
        };
        
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

        // Act
        var result = await _service.GetAllTasksAsync(createdAfter: startDate, createdBefore: endDate);

        // Assert
        result.Should().HaveCount(2);
        result.Select(t => t.Title).Should().Contain(new[] { "In Range 1", "In Range 2" });
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldCombineAllFilters()
    {
        // Arrange
        var baseDate = new DateTime(2026, 2, 1);
        var tasks = new List<TaskItem>
        {
            new() { Id = Guid.NewGuid(), Title = "Match", Description = "Desc", Status = TaskStatus.Pending, CreatedAt = baseDate.AddDays(1) },
            new() { Id = Guid.NewGuid(), Title = "Wrong Status", Description = "Desc", Status = TaskStatus.Completed, CreatedAt = baseDate.AddDays(1) },
            new() { Id = Guid.NewGuid(), Title = "Wrong Date", Description = "Desc", Status = TaskStatus.Pending, CreatedAt = baseDate.AddDays(-5) }
        };
        
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

        // Act
        var result = await _service.GetAllTasksAsync(
            status: TaskStatus.Pending,
            createdAfter: baseDate);

        // Assert
        result.Should().HaveCount(1);
        result.First().Title.Should().Be("Match");
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnOrderedTasks_WhenOrderByIsProvided()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            new() { Id = Guid.NewGuid(), Title = "Zebra", Description = "Desc 1", Status = TaskStatus.Pending, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Title = "Apple", Description = "Desc 2", Status = TaskStatus.InProgress, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Title = "Banana", Description = "Desc 3", Status = TaskStatus.Completed, CreatedAt = DateTime.UtcNow }
        };
        
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

        // Act
        var result = await _service.GetAllTasksAsync(orderBy: "title", orderDirection: "asc");

        // Assert
        var resultList = result.ToList();
        resultList.Should().HaveCount(3);
        resultList[0].Title.Should().Be("Apple");
        resultList[1].Title.Should().Be("Banana");
        resultList[2].Title.Should().Be("Zebra");
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnOrderedTasksDescending_WhenOrderDirectionIsDesc()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            new() { Id = Guid.NewGuid(), Title = "Apple", Description = "Desc", Status = TaskStatus.Pending, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Title = "Banana", Description = "Desc", Status = TaskStatus.Pending, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Title = "Zebra", Description = "Desc", Status = TaskStatus.Pending, CreatedAt = DateTime.UtcNow }
        };
        
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

        // Act
        var result = await _service.GetAllTasksAsync(orderBy: "title", orderDirection: "desc");

        // Assert
        var resultList = result.ToList();
        resultList[0].Title.Should().Be("Zebra");
        resultList[1].Title.Should().Be("Banana");
        resultList[2].Title.Should().Be("Apple");
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExists()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new TaskItem
        {
            Id = taskId,
            Title = "Test Task",
            Description = "Test Description",
            Status = TaskStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        
        _repositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

        // Act
        var result = await _service.GetTaskByIdAsync(taskId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(taskId);
        result.Title.Should().Be("Test Task");
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnNull_WhenTaskDoesNotExist()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync((TaskItem?)null);

        // Act
        var result = await _service.GetTaskByIdAsync(taskId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldCreateAndReturnTask()
    {
        // Arrange
        var dto = new CreateTaskDto
        {
            Title = "New Task",
            Description = "New Description",
            Status = TaskStatus.Pending
        };

        var createdTask = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            CreatedAt = DateTime.UtcNow
        };

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<TaskItem>())).ReturnsAsync(createdTask);

        // Act
        var result = await _service.CreateTaskAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(dto.Title);
        result.Description.Should().Be(dto.Description);
        result.Status.Should().Be(dto.Status.ToString());
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldUpdateAndReturnTask_WhenTaskExists()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var existingTask = new TaskItem
        {
            Id = taskId,
            Title = "Old Title",
            Description = "Old Description",
            Status = TaskStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var updateDto = new UpdateTaskDto
        {
            Title = "Updated Title",
            Description = "Updated Description",
            Status = TaskStatus.InProgress
        };

        var updatedTask = new TaskItem
        {
            Id = taskId,
            Title = updateDto.Title,
            Description = updateDto.Description,
            Status = updateDto.Status,
            CreatedAt = existingTask.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(existingTask);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>())).ReturnsAsync(updatedTask);

        // Act
        var result = await _service.UpdateTaskAsync(taskId, updateDto);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be(updateDto.Title);
        result.Description.Should().Be(updateDto.Description);
        result.Status.Should().Be(updateDto.Status.ToString());
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldReturnNull_WhenTaskDoesNotExist()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var updateDto = new UpdateTaskDto
        {
            Title = "Updated Title",
            Description = "Updated Description",
            Status = TaskStatus.InProgress
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync((TaskItem?)null);

        // Act
        var result = await _service.UpdateTaskAsync(taskId, updateDto);

        // Assert
        result.Should().BeNull();
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldReturnTrue_WhenTaskIsDeleted()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.DeleteAsync(taskId)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteTaskAsync(taskId);

        // Assert
        result.Should().BeTrue();
        _repositoryMock.Verify(r => r.DeleteAsync(taskId), Times.Once);
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldReturnFalse_WhenTaskDoesNotExist()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.DeleteAsync(taskId)).ReturnsAsync(false);

        // Act
        var result = await _service.DeleteTaskAsync(taskId);

        // Assert
        result.Should().BeFalse();
    }
}