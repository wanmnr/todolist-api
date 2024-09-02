using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using todolist_api.Data;

namespace todolist_api.Services;

public class TodoService
{
    private readonly TodoDbContext _dbContext;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

    public TodoService(TodoDbContext dbContext, IMemoryCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    // Get a Todo by ID, with caching
    public async Task<Todo> GetTodoByIdAsync(int id)
    {
        // Try to get the Todo from the cache
        if (!_cache.TryGetValue(id, out Todo? todo))
        {
            // If not in cache, retrieve it from the database
            todo = await _dbContext.Todos.FindAsync(id);

            // Cache the Todo entity with an expiration time
            if (todo != null)
            {
                _cache.Set(id, todo, _cacheDuration);
            }
        }

        // If todo is null, throw an exception
        if (todo == null)
        {
            throw new KeyNotFoundException($"Todo with id {id} not found.");
        }

        return todo;
    }

    // Get all Todos (no caching for list retrieval)
    public async Task<List<Todo>> GetAllTodosAsync()
    {
        return await _dbContext.Todos.ToListAsync();
    }

    // Add a new Todo
    public async Task<Todo> AddTodoAsync(Todo todo)
    {
        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();

        // Cache the new Todo
        _cache.Set(todo.Id, todo, _cacheDuration);

        return todo;
    }

    // Update an existing Todo
    public async Task<Todo> UpdateTodoAsync(Todo todo)
    {
        // Update the Todo in the database
        _dbContext.Entry(todo).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        // Update the cache
        _cache.Set(todo.Id, todo, _cacheDuration);

        return todo;
    }

    // Delete a Todo by ID
    public async Task<bool> DeleteTodoAsync(int id)
    {
        var todo = await _dbContext.Todos.FindAsync(id);

        if (todo == null)
        {
            return false;
        }

        // Remove from the database
        _dbContext.Todos.Remove(todo);
        await _dbContext.SaveChangesAsync();

        // Remove from cache
        _cache.Remove(id);

        return true;
    }

}