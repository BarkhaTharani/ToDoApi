using System;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data.Entities;

namespace ToDoApi.Data {

    public class ToDoContext : DbContext {

        public ToDoContext (DbContextOptions options) : base (options) { }

        public DbSet<ToDoItem> ToDoItems { get; set; }

    }

}