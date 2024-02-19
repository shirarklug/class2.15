using System.Data.SqlClient;

namespace HW2._15.Models
{
    public class ToDoItem
    {
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ItemId { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class ToDoManager
    {
        private string _connectionString;

        public ToDoManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ToDoItem> GetToDoItems()
        {
            var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM ToDoItems td
                                JOIN Categories c
                                ON c.CategoryId = td.CategoryId
                                 WHERE CompletedDate IS NULL";
            connection.Open();

            List<ToDoItem> toDoItems = new();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                toDoItems.Add(new ToDoItem
                {
                    Title = (string)reader["Title"],
                    DueDate = (DateTime)reader["DueDate"],
                    CompletedDate = Extensions.GetOrNull<DateTime?>(reader, "CompletedDate"),
                    CategoryId = (int)reader["CategoryId"],
                    CategoryName = (string)reader["CategoryName"],
                    ItemId = (int)reader["ItemId"]
                });
            }
            return toDoItems;
        }

        public List<ToDoItem> GetCompletedToDoItems()
        {
            var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM ToDoItems td
                                JOIN Categories c
                                ON c.CategoryId = td.CategoryId
                                WHERE CompletedDate IS NOT NULL";
            connection.Open();

            List<ToDoItem> toDoItems = new();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                toDoItems.Add(new ToDoItem
                {
                    Title = (string)reader["Title"],
                    DueDate = (DateTime)reader["DueDate"],
                    CompletedDate = Extensions.GetOrNull<DateTime?>(reader, "CompletedDate"),
                    CategoryId = (int)reader["CategoryId"],
                    CategoryName = (string)reader["CategoryName"]
                });
            }
            return toDoItems;
        }

        public List<Category> GetCategories()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Categories";
            connection.Open();

            List<Category> categories = new();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                categories.Add(new Category
                {
                    CategoryName = (string)reader["CategoryName"],
                    CategoryId = (int)reader["CategoryId"]
                });
            }
            return categories;
        }

        public void AddCategory(string categoryName)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Categories VALUES(@categoryName)";
            cmd.Parameters.AddWithValue("@categoryName", categoryName);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public void MarkAsCompleted(int ItemId)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = " UPDATE ToDoItems SET CompletedDate = (@today) WHERE ItemId = (@ItemId)";
            cmd.Parameters.AddWithValue("@today", DateTime.Today);
            cmd.Parameters.AddWithValue("ItemId", ItemId);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public void AddItem(string title, DateTime dueDate, int categoryId)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO ToDoItems (Title, DueDate, CategoryID) VALUES(@title, @dueDate, @categoryId)";
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@dueDate", dueDate);
            cmd.Parameters.AddWithValue("@categoryId", categoryId);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public static class Extensions
    {
        public static T GetOrNull<T>(this SqlDataReader reader, string columnName)
        {
            var value = reader[columnName];
            if (value == DBNull.Value)
            {
                return default(T);
            }

            return (T)value;
        }
    }

    public class ToDoItemsViewModel
    {
        public List<ToDoItem> ToDoItems { get; set; }
    }

    public class CategoriesViewModel
    {
        public List<Category> Categories { get; set; }
    }
}
