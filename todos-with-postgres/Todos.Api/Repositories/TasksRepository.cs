using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Todos.Api.Repositories
{
    public class TasksRepository
    {
        private readonly NpgsqlDataSource _dataSource;

        public TasksRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string is required", nameof(connectionString));
            }

            _dataSource = NpgsqlDataSource.Create(connectionString);
        }

        public async Task<IEnumerable<Models.Task>> GetAllTasksAsync()
        {
            using var db = _dataSource.CreateConnection();

            await db.OpenAsync();

            return await db.QueryAsync<Models.Task>("SELECT * FROM tasks");
        }

        public async Task<int> SaveAsync(Models.Task task)
        {

            using var db = _dataSource.CreateConnection();

            await db.OpenAsync();

            if (task.Id.HasValue)
            {
                string update = @"UPDATE tasks SET description = @Description, completed = @Completed WHERE id = @Id";
                return await db.ExecuteAsync(update, new { task.Description, task.Completed, task.Id });
            }
            else
            {
                string insert = @"INSERT INTO tasks (description) VALUES (@Description)";
                return await db.ExecuteAsync(insert, new { task.Description });
            }
        }

        public async Task<int> DeleteAsync(int taskId)
        {
            using var db = _dataSource.CreateConnection();

            await db.OpenAsync();

            string delete = @"DELETE FROM tasks WHERE id = @Id";

            return await db.ExecuteAsync(delete, new { Id = taskId });
        }
    }
}