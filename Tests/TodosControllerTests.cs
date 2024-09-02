using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todolist_api.Controllers;
using todolist_api.Models;
using Xunit;

namespace Tests
{
    public class TodosControllerTests
    {
        private readonly TodosController _controller;
        private readonly TodoDbContext _context;

        public TodosControllerTests()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new TodoDbContext(options);
            _controller = new TodosController(_context);

            // Seed the in-memory database with test data
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            if (!_context.Todos.Any()) // Ensure the database is only seeded if it's empty
            {
                var todos = new List<Todo>
                {
                    new Todo { Id = 1, Title = "Test Todo 1" },
                    new Todo { Id = 2, Title = "Test Todo 2" }
                };
                _context.Todos.AddRange(todos);
                _context.SaveChanges();
            }
        }

        [Fact]
        public async Task GetTodos_ReturnsAllTodos()
        {
            // Act
            var result = await _controller.GetTodos();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Todo>>>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Todo>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task GetTodo_ReturnsNotFound_WhenTodoNotExists()
        {
            // Act
            var result = await _controller.GetTodo(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetTodo_ReturnsTodo_WhenTodoExists()
        {
            // Act
            var result = await _controller.GetTodo(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Todo>>(result);
            var returnValue = Assert.IsType<Todo>(actionResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Todo 1", returnValue.Title);
        }

        // Similar tests can be added for PostTodo, PutTodo, DeleteTodo, etc.


        // Clean up the in-memory database after each test
        [Fact]
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
