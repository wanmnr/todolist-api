// UnitTests/Services/TodoServiceTests.cs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using todolist_api.Data;
using todolist_api.Services;
using Xunit;

namespace UnitTests.Services
{
    public class TodoServiceTests : IDisposable
    {
        private readonly TodoService _service;
        private readonly TodoDbContext _context;
        private readonly Mock<IMemoryCache> _mockCache;

        public TodoServiceTests()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new TodoDbContext(options);
            _mockCache = new Mock<IMemoryCache>();

            _service = new TodoService(_context, _mockCache.Object);

            // Seed the database
            _context.Todos.Add(new Todo { Id = 1, Title = "Test Todo", Complete = false });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetTodoByIdAsync_ReturnsTodo_WhenTodoExists()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _service.GetTodoByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetTodoByIdAsync_ThrowsException_WhenTodoDoesNotExist()
        {
            // Arrange
            var id = 99;

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetTodoByIdAsync(id));
        }

        [Fact]
        public async Task AddTodoAsync_AddsTodoToDatabase()
        {
            // Arrange
            var todo = new Todo { Id = 2, Title = "New Todo", Complete = false };

            // Act
            var result = await _service.AddTodoAsync(todo);
            var savedTodo = await _context.Todos.FindAsync(todo.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(todo.Title, result.Title);
            Assert.Equal(todo.Id, savedTodo?.Id);
        }

        [Fact]
        public async Task UpdateTodoAsync_UpdatesTodoInDatabase()
        {
            // Arrange
            var existingTodo = await _context.Todos.FindAsync(1);
            existingTodo.Title = "Updated Todo";

            // Act
            var result = await _service.UpdateTodoAsync(existingTodo);
            var updatedTodo = await _context.Todos.FindAsync(1);

            // Assert
            Assert.Equal("Updated Todo", updatedTodo?.Title);
        }

        [Fact]
        public async Task DeleteTodoAsync_DeletesTodoFromDatabase()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _service.DeleteTodoAsync(id);
            var deletedTodo = await _context.Todos.FindAsync(id);

            // Assert
            Assert.True(result);
            Assert.Null(deletedTodo);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}